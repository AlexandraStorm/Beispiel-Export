using System;
using System.Data;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Serilog.Sinks.File;


namespace BeispielExportModel
{
    public interface IAllele
    {
        string Name { get; set; }
        int Location { get; set; }
        string ProbeValue { get; set; }
    }
    public class HPAAllele : IAllele
    {
        private string _name;
        private int _location;
        private string _probeValue;
        private string _glycoprotein;
        private int _sortHelper;
        public int SortHelper
        {
            get
            {
                return _sortHelper;
            }
            set
            {
                _sortHelper = value;
            }
        }
        public int Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public string ProbeValue
        {
            get
            {
                return _probeValue;
            }
            set
            {
                _probeValue = value;
            }
        }
        public string Glycoprotein
        {
            get
            {
                return _glycoprotein;
            }
            set
            {
                _glycoprotein = value;
            }
        }
    }
    public interface IProbe
    {
        string Name { get; set; }
        int Location { get; set; }
        List<IAllele> AlleleList { get; set; }
        double Con1Baf { get; set; }
        double Con2Baf { get; set; }
        double Con3Baf { get; set; }
    }
    public class HPAProbe : IProbe, IDisposable
    {
        private List<IAllele> _alleleList;
        private int _location;
        private string _name;
        private string _glycoProteinGroup;

        public List<IAllele> AlleleList
        {
            get
            {
                return _alleleList;
            }
            set
            {
                _alleleList = value;
            }
        }
        public int Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public string ProteinGroup
        {
            get
            {
                return _glycoProteinGroup;
            }
            set
            {
                _glycoProteinGroup = value;
            }
        }

        public double Con1Baf
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        public double Con2Baf
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        public double Con3Baf
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        private bool disposedValue = false;        // To detect redundant calls

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    // TODO: free other state (managed objects).
                    _alleleList.Clear();
                    _alleleList = null;
                }
            }
            this.disposedValue = true;
        }

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
    public class Probe : IProbe
    {
        private string _probeName;
        private double _con1Baf;
        private double _con2Baf;
        private double _con3Baf;


        public string Name
        {
            get
            {
                return _probeName;
            }
            set
            {
                _probeName = value;
            }
        }
        public double Con1Baf
        {
            get
            {
                return _con1Baf;
            }
            set
            {
                _con1Baf = value;
            }
        }
        public double Con2Baf
        {
            get
            {
                return _con2Baf;
            }
            set
            {
                _con2Baf = value;
            }
        }
        public double Con3Baf
        {
            get
            {
                return _con3Baf;
            }
            set
            {
                _con3Baf = value;
            }
        }

        // used currently only for HPAProbe 
        public int Location
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }
        public System.Collections.Generic.List<IAllele> AlleleList
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public Probe(string pName, double con1, double con2, double con3)
        {
            _probeName = pName;
            _con1Baf = con1;
            _con2Baf = con2;
            _con3Baf = con3;
        }
        public Probe()
        {
            _probeName = string.Empty;
            _con1Baf = 0;
            _con2Baf = 0;
            _con3Baf = 0;
        }
    }

    public class PakLx
    {
        private ILogger logger;
        public string PakLxConnString { get; set; }
        public Dictionary<string, ConcurrentBag<PakLxResult>> results { get; set; }
        public List<IProbe> probelist;
        private DataSet probes;
        public PakLx() { }
        public PakLx(string batchName, List<string> sampleids)
        {
            results = new Dictionary<string, ConcurrentBag<PakLxResult>>();
            logger = new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .CreateLogger();
                
            //load only the available sampleids
            probes = GetProbes(batchName);
            probelist = LoadProbes(batchName);
            results.Add(batchName, null);
            results[batchName] = new ConcurrentBag<PakLxResult>();
            Parallel.ForEach(sampleids, (sampleid) =>
            {
                AntibodyBO abo = new AntibodyBO();
                abo.SampleID = sampleid;
                abo.SessionID = batchName;
                DataSet data = GetPakLxData(batchName, sampleid);
                abo.LoadSample(data, 0);
                PakLxResult pakresult = GetResultsFromStats(probelist, abo);
                pakresult.patientName = data.Tables[3].Rows[0]["userDefinedName"].ToString();
                pakresult.drawdate = data.Tables[3].Rows[0]["drawDt"].ToString();
                pakresult.UserComments = data.Tables[6].Rows[0]["comments"].ToString();
                results[batchName].Add(pakresult);
            });
        }
        public DataSet GetProbes(string batch)
        {
            string sqlstmt = $"SELECT LotID, probeName, Con1, Con2, Con3 FROM tbAntibodyExpSet WHERE consensus = '0' and LOTID in (select distinct LotID from tbAntibodyMethod where sessionid='{batch}')";
            using (EvolutionDBFacade.EvolutionDBFacade.DBFacade dBFacade1 = new EvolutionDBFacade.EvolutionDBFacade.DBFacade($"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\LIFECODES\\External_MatchitPakLx.config", true, "appConnString"))
            {
                return dBFacade1.ExecuteSQL(sqlstmt);
            }
        }
        public DataSet GetPakLxData(string batch, string sampleid)
        {
            using (EvolutionDBFacade.EvolutionDBFacade.DBFacade dBFacade1 = new EvolutionDBFacade.EvolutionDBFacade.DBFacade($"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\LIFECODES\\External_MatchitPakLx.config", true, "appConnString"))
            {
                Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                keyValuePairs.Add("batchID", batch);
                keyValuePairs.Add("sampleID", sampleid);
                return dBFacade1.ExecuteProc("GetSampleForLoad", keyValuePairs);
            }
        }
        private DataSet RetrieveHPAAntigenListing(string logicName)
        {
            using (EvolutionDBFacade.EvolutionDBFacade.DBFacade dBFacade1 = new EvolutionDBFacade.EvolutionDBFacade.DBFacade($"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\LIFECODES\\External_MatchitPakLx.config", true, "appConnString"))
            {
                Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
                keyValuePairs.Add("logicName", logicName);
                return dBFacade1.ExecuteProc("GetPakLxProbeValues", keyValuePairs);
            }
        }
        private string GetLogicNameForBatch(string batchname)
        {
            string sqlstmt = $"select logicname from tblogic where logicid = (select distinct logicid from tbantibodymethod where sessionid = '{batchname}')";
            using (EvolutionDBFacade.EvolutionDBFacade.DBFacade dBFacade1 = new EvolutionDBFacade.EvolutionDBFacade.DBFacade($"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\LIFECODES\\External_MatchitPakLx.config", true, "appConnString"))
            {
                DataSet ds = dBFacade1.ExecuteSQL(sqlstmt);
                return ds.Tables[0].Rows[0][0].ToString();
            }
        }
        private List<IProbe> LoadProbes(string batchName)
        {
            //_ProbeAlleles = New List(Of IProbe)
            List<IProbe> _ProbeAlleles = new List<IProbe>();

            DataSet myAntigenDS = RetrieveHPAAntigenListing(GetLogicNameForBatch(batchName));
            foreach (DataRow dr in myAntigenDS.Tables[1].Rows)
            {
                string filter = string.Format("probename='{0}'", dr["probeName"].ToString());
                var myProbe = new HPAProbe();
                myProbe.Name = dr["probeName"].ToString();
                myProbe.Location = int.Parse(dr["logicalPosition"].ToString());
                myProbe.ProteinGroup = dr["nmdpname"].ToString();
                var myAlleleList = new List<IAllele>();
                foreach (DataRow dr2 in myAntigenDS.Tables[0].Select(filter))
                {
                    var myAllele = new HPAAllele();
                    myAllele.Name = dr2["allele"].ToString();
                    myAllele.Glycoprotein = dr2["nmdpname"].ToString();
                    myAllele.Location = int.Parse(dr2["alleleLogicalPosition"].ToString());
                    if (dr2["positiveFlag"].ToString() == "1")
                    {
                        myAllele.ProbeValue = "1";
                    }

                    if (myAllele.ProbeValue != "1")
                    {
                        string alleleFilter = "allele = '" + myAllele.Name + "' and nmdpname = '" + myProbe.ProteinGroup + "' and positiveFlag=1";
                        DataRow[] mydrRows;
                        mydrRows = myAntigenDS.Tables[0].Select(alleleFilter);
                        if (mydrRows.Any())
                        {
                            myAllele.ProbeValue = "-1";
                        }
                        else
                        {
                            myAllele.ProbeValue = "0";
                        }
                    }

                    if (_ProbeAlleles.Count == 0)
                    {
                        myAllele.SortHelper = 1;
                    }
                    else
                    {
                        HPAProbe myOldProbe = (HPAProbe)_ProbeAlleles[_ProbeAlleles.Count - 1];
                        HPAAllele myOldAllele = (HPAAllele)myOldProbe.AlleleList[1];
                        if (myAllele.Glycoprotein != myOldAllele.Glycoprotein)
                        {
                            myAllele.SortHelper = myOldAllele.SortHelper + 1;
                        }
                        else
                        {
                            myAllele.SortHelper = myOldAllele.SortHelper;
                        }
                    }

                    myAlleleList.Add(myAllele);
                }

                myProbe.AlleleList = myAlleleList;
                _ProbeAlleles.Add(myProbe);
            }
            return _ProbeAlleles;
        }

        private bool isValidSample(AntibodyBO sample)
        {
            bool returnValue = true;
            foreach (AntibodyProbesBO probe in sample.Probes)
            {
                if(probe.Assignment == "Bead Failure")
                {
                    returnValue = false;
                    break;
                }
            }
            return returnValue;
        }
        private PakLxResult GetResultsFromStats(List<IProbe> alleleProbes, AntibodyBO sample)
        {
            PakLxResult result = new PakLxResult();
            result.sampleid = sample.SampleID;
            string currentGroup = "";
            List<Dictionary<string, List<string>>> listOfGroups = new List<Dictionary<string, List<string>>>();
            Dictionary<string, List<IAllele>> proteinGrouplist = new Dictionary<string, List<IAllele>>();
            if (isValidSample(sample))
            {
                foreach (HPAProbe item in alleleProbes)
                {
                    if ((currentGroup != item.ProteinGroup))
                    {
                        if ((proteinGrouplist.ContainsKey(item.ProteinGroup) == false))
                            proteinGrouplist.Add(item.ProteinGroup, item.AlleleList);
                    }
                    currentGroup = item.ProteinGroup;
                }
                foreach (string pGroup in proteinGrouplist.Keys)
                {
                    // get the list of beads that exist in this group.

                    List<HPAProbe> mySortingList = new List<HPAProbe>();
                    List<string> finalBeadList = new List<string>();
                    foreach (HPAProbe item in alleleProbes)
                        mySortingList.Add(item);
                    // object wellPosition = new object();
                    string tmpGroup = pGroup;
                    var wellPosition = from item in mySortingList
                                       where item.ProteinGroup.Trim() == tmpGroup
                                       select item;
                    Dictionary<string, List<IAllele>> AlleleList = new Dictionary<string, List<IAllele>>();
                    foreach (HPAProbe item in wellPosition)
                    {
                        finalBeadList.Add(item.Name);
                        AlleleList.Add(item.Name, item.AlleleList);
                    }
                    Dictionary<string, List<string>> myList = new Dictionary<string, List<string>>();
                    myList.Add(pGroup, finalBeadList);
                    listOfGroups.Add(myList);
                    int pCount = 0;
                    int bCount = 0;
                    string abead = "";

                    int aCount = 0;
                    int abCount = 0;
                    switch (pGroup.ToUpper())
                    {
                        case "HLA CLASS I":
                        case "GPIV":
                        case "GPIIB-IIIA":
                            {
                                foreach (AntibodyProbesBO bead in sample.Probes)
                                {
                                    if ((finalBeadList.Contains(bead.ProbeName)))
                                    {
                                        if (bead.Assignment.ToLower() == "positive")
                                            pCount = pCount + 1;
                                    }
                                }

                                break;
                            }

                        case "GPIB/IX":
                            {
                                foreach (var item in AlleleList.Keys)
                                {
                                    foreach (IAllele allele in AlleleList[item])
                                    {
                                        // this allele is on this probe
                                        if ((allele.Name == "2a" & allele.ProbeValue == "1"))
                                        {
                                            foreach (AntibodyProbesBO bead in sample.Probes)
                                            {
                                                if ((item == bead.ProbeName))
                                                {
                                                    if ((bead.Assignment.ToLower() == "positive"))
                                                    {
                                                        abead = item;
                                                        aCount = aCount + 1;
                                                    }
                                                }
                                            }
                                        }
                                        else if ((allele.Name == "2b" & allele.ProbeValue == "1"))
                                        {
                                            foreach (AntibodyProbesBO bead in sample.Probes)
                                            {
                                                if ((item == bead.ProbeName))
                                                {
                                                    if ((bead.Assignment.ToLower() == "positive"))
                                                    {
                                                        if ((item == abead))
                                                        {
                                                            aCount = aCount - 1;
                                                            abCount = abCount + 1;
                                                        }
                                                        else
                                                            bCount = bCount + 1;
                                                    }
                                                }
                                            }
                                        }
                                        else if ((allele.Name == "2ab" & allele.ProbeValue == "1"))
                                        {
                                            foreach (AntibodyProbesBO bead in sample.Probes)
                                            {
                                                if ((item == bead.ProbeName))
                                                {
                                                    if ((bead.Assignment.ToLower() == "positive"))
                                                        abCount = abCount + 1;
                                                }
                                            }
                                        }
                                    }
                                }

                                break;
                            }

                        case "GPIA-IIA":
                            {
                                foreach (var item in AlleleList.Keys)
                                {
                                    foreach (IAllele allele in AlleleList[item])
                                    {
                                        // this allele is on this probe
                                        if ((allele.Name == "5a" & allele.ProbeValue == "1"))
                                        {
                                            foreach (AntibodyProbesBO bead in sample.Probes)
                                            {
                                                if ((item == bead.ProbeName))
                                                {
                                                    if ((bead.Assignment.ToLower() == "positive"))
                                                    {
                                                        abead = item;
                                                        aCount = aCount + 1;
                                                    }
                                                }
                                            }
                                        }
                                        else if ((allele.Name == "5b" & allele.ProbeValue == "1"))
                                        {
                                            foreach (AntibodyProbesBO bead in sample.Probes)
                                            {
                                                if ((item == bead.ProbeName))
                                                {
                                                    if ((bead.Assignment.ToLower() == "positive"))
                                                    {
                                                        if ((item == abead))
                                                        {
                                                            aCount = aCount - 1;
                                                            abCount = abCount + 1;
                                                        }
                                                        else
                                                            bCount = bCount + 1;
                                                    }
                                                }
                                            }
                                        }
                                        else if ((allele.Name == "5ab" & allele.ProbeValue == "1"))
                                        {
                                            foreach (AntibodyProbesBO bead in sample.Probes)
                                            {
                                                if ((item == bead.ProbeName))
                                                {
                                                    if ((bead.Assignment.ToLower() == "positive"))
                                                        abCount = abCount + 1;
                                                }
                                            }
                                        }
                                    }
                                }

                                break;
                            }

                        default:
                            {
                                break;
                            }
                    }
                    switch (pGroup.ToUpper())
                    {
                        case "HLA CLASS I":
                            {
                                if ((pCount >= 1))
                                    result.HLAResult = "Pos";
                                else
                                    result.HLAResult = "Neg";
                                break;
                            }

                        case "GPIV":
                            {
                                if ((pCount >= 1))
                                    result.GPIVResult = "Pos";
                                else
                                    result.GPIVResult = "Neg";
                                break;
                            }

                        case "GPIIB-IIIA":
                            {
                                if ((pCount >= 1))
                                    result.GPIIbIIIaResult = "Reactive";
                                else
                                    result.GPIIbIIIaResult = "Neg";
                                break;
                            }

                        case "GPIB/IX":
                            {
                                if (((aCount == 0 & bCount == 0 & abCount == 0) | (aCount == 1 & bCount == 0 & abCount == 0) | (bCount == 1 & aCount == 0 & abCount == 0) | (abCount == 1 & aCount == 0 & bCount == 0) | (aCount == 1 & abCount == 1 & bCount == 0) | (bCount == 1 & abCount == 1 & aCount == 0)))
                                    result.GPIBIXResult = "Neg";
                                else if (((aCount == 2 & bCount == 0) | (bCount == 2 & aCount == 0)))
                                    result.GPIBIXResult = "Pos";
                                else if (((aCount == 2 & bCount == 2) | (aCount >= 1 & bCount >= 1) | (abCount == 1 & aCount == 2 & bCount == 2)))
                                    result.GPIBIXResult = "Indeterminate";
                                break;
                            }

                        case "GPIA-IIA":
                            {
                                if (((aCount == 0 & bCount == 0 & abCount == 0) | (aCount == 1 & bCount == 0 & abCount == 0) | (bCount == 1 & aCount == 0 & abCount == 0) | (abCount == 1 & aCount == 0 & bCount == 0) | (aCount == 1 & abCount == 1 & bCount == 0) | (bCount == 1 & abCount == 1 & aCount == 0)))
                                    result.GPIaIIaResult = "Neg";
                                else if (((aCount == 2 & bCount == 0) | (bCount == 2 & aCount == 0)))
                                    result.GPIaIIaResult = "Pos";
                                else if (((aCount == 2 & bCount == 2) | (aCount >= 1 & bCount >= 1) | (abCount == 1 & aCount == 2 & bCount == 2)))
                                    result.GPIaIIaResult = "Indeterminate";
                                break;
                            }

                        default:
                            {
                                break;
                            }
                    }
                }
            }
            else
            {
                result.HLAResult = "INVALID";
                result.GPIVResult = "INVALID";
                result.GPIIbIIIaResult = "INVALID";
                result.GPIBIXResult = "INVALID";
                result.GPIaIIaResult = "INVALID";
            }
            return result;
        }
    }
    public interface ISampleBO
    {
        string SessionID { get; set; }
        string SampleID { get; set; }
        int AntibodyID { get; set; }
        DateTime RunDate { get; set; }
        int TestType { get; set; }
        string Comments { get; set; }
        bool IsDirty { get; set; }
        string ReviewedBy { get; set; }
        double Raw { get; set; }
        double RVal { get; set; }
        double PctPositive { get; set; }
        int AssignCutoff { get; set; }
        double AssignAdj1 { get; set; }
        double AssignAdj2 { get; set; }
        double AssignAdj3 { get; set; }
        bool IsCompleted { get; set; }
        bool IsApproved { get; set; }
        string WellPosition { get; set; }
        int WellIndex { get; set; }
        string UpdateBy { get; set; }
    }

    public class AntibodyProbesBO : ICloneable, IDisposable
    {
        private string _probeName;
        private double _rawVal;
        private double _adj1;
        private double _adj2;
        private double _adj3;
        private double _adjN;
        private string _assignment;
        private double _hiBgAdj1;
        private double _hiBgAdj2;
        private double _hiBgAdj3;
        private string _hiBgAssignment;
        private double _weakPct;
        private bool _overRidden;
        private int _beadCount;
        private DateTime _testDate;
        private string _consensus;
        private int _score;
        private int _hiBGScore;
        private string _eplets;
        public string Eplets
        {
            get
            {
                return _eplets;
            }
            set
            {
                _eplets = value;
            }
        }
        // Private _Alleles As List(Of SAAlleles)

        // Property Alleles() As List(Of SAAlleles)
        // Get
        // Return _Alleles
        // End Get
        // Set(ByVal value As List(Of SAAlleles))
        // _Alleles = value
        // End Set
        // End Property

        public string ProbeName
        {
            get
            {
                return _probeName;
            }
            set
            {
                _probeName = value;
            }
        }
        public double RawVal
        {
            get
            {
                return _rawVal;
            }
            set
            {
                _rawVal = value;
            }
        }
        public double Adj1
        {
            get
            {
                return _adj1;
            }
            set
            {
                _adj1 = value;
            }
        }
        public double Adj2
        {
            get
            {
                return _adj2;
            }
            set
            {
                _adj2 = value;
            }
        }
        public double Adj3
        {
            get
            {
                return _adj3;
            }
            set
            {
                _adj3 = value;
            }
        }
        public double AdjN
        {
            get
            {
                return _adjN;
            }
            set
            {
                _adjN = value;
            }
        }
        public string Assignment
        {
            get
            {
                return _assignment;
            }
            set
            {
                _assignment = value;
            }
        }
        public double HiBgAdj1
        {
            get
            {
                return _hiBgAdj1;
            }
            set
            {
                _hiBgAdj1 = value;
            }
        }
        public double HiBgAdj2
        {
            get
            {
                return _hiBgAdj2;
            }
            set
            {
                _hiBgAdj2 = value;
            }
        }
        public double HiBgAdj3
        {
            get
            {
                return _hiBgAdj3;
            }
            set
            {
                _hiBgAdj3 = value;
            }
        }
        public string HiBgAssignment
        {
            get
            {
                return _hiBgAssignment;
            }
            set
            {
                _hiBgAssignment = value;
            }
        }
        public double WeakPct
        {
            get
            {
                return _weakPct;
            }
            set
            {
                _weakPct = value;
            }
        }
        public bool OverRidden
        {
            get
            {
                return _overRidden;
            }
            set
            {
                _overRidden = value;
            }
        }
        public int BeadCount
        {
            get
            {
                return _beadCount;
            }
            set
            {
                _beadCount = value;
            }
        }
        public DateTime TestDate
        {
            get
            {
                return _testDate;
            }
            set
            {
                _testDate = value;
            }
        }
        public string Consensus
        {
            get
            {
                return _consensus;
            }
            set
            {
                _consensus = value;
            }
        }
        public int Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
            }
        }
        public int HiBGScore
        {
            get
            {
                return _hiBGScore;
            }
            set
            {
                _hiBGScore = value;
            }
        }


        /// <summary>
        ///     ''' Load the new probe with values from caller
        ///     ''' </summary>
        ///     ''' <param name="probeName"></param>
        ///     ''' <param name="rawValue"></param>
        ///     ''' <param name="adj1"></param>
        ///     ''' <param name="adj2"></param>
        ///     ''' <param name="adj3"></param>
        ///     ''' <param name="adjN"></param>
        ///     ''' <param name="assign"></param>
        ///     ''' <param name="hibgadj1"></param>
        ///     ''' <param name="hibgadj2"></param>
        ///     ''' <param name="hibgadj3"></param>
        ///     ''' <param name="hibgassign"></param>
        ///     ''' <param name="weakpct"></param>
        ///     ''' <param name="override"></param>
        ///     ''' <param name="beadcount"></param>
        ///     ''' <remarks></remarks>
        public AntibodyProbesBO(string probeName, double rawValue, double adj1, double adj2, double adj3, double adjN, string assign, double hibgadj1, double hibgadj2, double hibgadj3, string hibgassign, double weakpct, bool @override, int beadcount, string consensus, int score, int hiBGScore, string eplets)
        {
            _probeName = probeName;
            _rawVal = rawValue;
            _adj1 = Math.Round(adj1, 2);
            _adj2 = Math.Round(adj2, 2);
            _adj3 = Math.Round(adj3, 2);
            _adjN = Math.Round(adjN, 2);
            _assignment = assign; // IIf(assign.ToLower() = "positive", "POS", "NEG")
            _hiBgAdj1 = Math.Round(hibgadj1, 2);
            _hiBgAdj2 = Math.Round(hibgadj2, 2);
            _hiBgAdj3 = Math.Round(hibgadj3, 2);
            _hiBgAssignment = hibgassign;
            _weakPct = weakpct;
            _overRidden = @override;
            _beadCount = beadcount;
            _consensus = consensus;
            _score = score;
            _hiBGScore = hiBGScore;
            _eplets = eplets;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        private bool disposedValue = false;        // To detect redundant calls

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                }
            }
            this.disposedValue = true;
        }

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public class AntibodyStatsBO : ICloneable, IDisposable
    {
        private string _antigen;
        private int _posThis;
        private int _posOther;
        private int _negThis;
        private int _negOther;
        private double _chiSquare;
        private double _rValue;
        private double _percentPos;
        private double _tail;
        private string _traceType;
        private double _strength;
        private double _adjust1; // used solely for Single Antigen and the BCM value


        public double Adjust1
        {
            get
            {
                return _adjust1;
            }
            set
            {
                _adjust1 = value;
            }
        }
        public string Antigen
        {
            get
            {
                return _antigen;
            }
            set
            {
                _antigen = value;
            }
        }
        public int PosThis
        {
            get
            {
                return _posThis;
            }
            set
            {
                _posThis = value;
            }
        }
        public int PosOther
        {
            get
            {
                return _posOther;
            }
            set
            {
                _posOther = value;
            }
        }
        public int NegThis
        {
            get
            {
                return _negThis;
            }
            set
            {
                _negThis = value;
            }
        }
        public int NegOther
        {
            get
            {
                return _negOther;
            }
            set
            {
                _negOther = value;
            }
        }
        public double ChiSquare
        {
            get
            {
                return _chiSquare;
            }
            set
            {
                _chiSquare = value;
            }
        }
        public double RValue
        {
            get
            {
                return _rValue;
            }
            set
            {
                _rValue = value;
            }
        }
        public double PercentPos
        {
            get
            {
                return _percentPos;
            }
            set
            {
                _percentPos = value;
            }
        }
        public double Tail
        {
            get
            {
                return _tail;
            }
            set
            {
                _tail = value;
            }
        }
        public string TraceType
        {
            get
            {
                return _traceType;
            }
            set
            {
                _traceType = value;
            }
        }
        public double Strength
        {
            get
            {
                return _strength;
            }
            set
            {
                _strength = value;
            }
        }

        public AntibodyStatsBO(string antigen, int posThis, int posOther, int negThis, int negOther, double chiSquare, double rValue, double percentPos, double tail, string traceType, double strength)
        {
            _antigen = antigen;
            _posThis = posThis;
            _posOther = posOther;
            _negThis = negThis;
            _negOther = negOther;
            _chiSquare = chiSquare;
            _rValue = rValue;
            _percentPos = percentPos;
            _tail = tail;
            _traceType = traceType;
            _strength = strength;
        }
        public AntibodyStatsBO(string antigen, int posThis, int posOther, int negThis, int negOther, double chiSquare, double rValue, double percentPos, double tail, string traceType, double strength, double adjust1)
        {
            _antigen = antigen;
            _posThis = posThis;
            _posOther = posOther;
            _negThis = negThis;
            _negOther = negOther;
            _chiSquare = chiSquare;
            _rValue = rValue;
            _percentPos = percentPos;
            _tail = tail;
            _traceType = traceType;
            _strength = strength;
            _adjust1 = adjust1;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        private bool disposedValue = false;        // To detect redundant calls

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                }
            }
            this.disposedValue = true;
        }

        // This code added by Visual Basic to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public class PakLxResult
    {
        public string sampleid { get; set; }
        public string patientName { get; set; }
        public string drawdate { get; set; }
        public string GPIVResult { get; set; }
        public string HLAResult { get; set; }
        public string GPIIbIIIaResult { get; set; }
        public string GPIBIXResult { get; set; }
        public string GPIaIIaResult { get; set; }
        public string UserComments { get; set; }
    }
}
