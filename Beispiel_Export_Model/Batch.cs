using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace BeispielExportModel
{
    public class Batch
    {
        public string BatchName { get; set; }
        public List<Sample> Samples { get; set; }

        public Color backColor { get; set; }
        public BatchType type { get; set; }
        public bool isSelected { get; set; }
        public Batch() { }
        public Batch(string batchName)
        {
            BatchName = batchName;
            isSelected = false;
        }
    }

    public enum BatchType
    {
        DNA = 0,
        LSA = 1,
        PAKLX = 2,
        LMX = 3
    }

    public class Batches
    {
        public Dictionary<string, Batch> FoundBatches { get; }
        public string MIConnString { get; set; }
        public string PakLxConnString { get; set; }
        private readonly string _fromDate;
        private readonly string _toDate;
        public Batches() { }
        public Batches(string fDate, string tDate)
        {
            _fromDate = fDate;
            _toDate = tDate;
            FoundBatches = new Dictionary<string, Batch>();

            setconnectionstrings();
            LoadBatches().Wait();
        }
        private void setconnectionstrings()
        {
            EvolutionDBFacade.EvolutionDBFacade.DBFacade dBFacade = new EvolutionDBFacade.EvolutionDBFacade.DBFacade($"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\LIFECODES\\External_Matchit.config", true);
            MIConnString = GetMIEntityConnectionString(dBFacade.GetSqlConnectionString());
            dBFacade.Dispose();
            try
            {
                EvolutionDBFacade.EvolutionDBFacade.DBFacade dBFacade1 = new EvolutionDBFacade.EvolutionDBFacade.DBFacade($"{Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\LIFECODES\\External_MatchitPakLx.config", true);
                PakLxConnString = GetPLEntityConnectionString(dBFacade1.GetSqlConnectionString());
                dBFacade1.Dispose();
            }
            catch { }

        }
        private string GetMIEntityConnectionString(string provider)
        {
            var entityConnectionStringBuilder = new EntityConnectionStringBuilder();
            entityConnectionStringBuilder.Provider = "System.Data.SqlCLient";
            entityConnectionStringBuilder.ProviderConnectionString = provider;
            entityConnectionStringBuilder.Metadata = "res://*/MatchitModel.csdl|res://*/MatchitModel.ssdl|res://*/MatchitModel.msl";

            return entityConnectionStringBuilder.ToString();
        }
        private string GetPLEntityConnectionString(string provider)
        {
            var entityConnectionStringBuilder = new EntityConnectionStringBuilder();
            entityConnectionStringBuilder.Provider = "System.Data.SqlClient";
            entityConnectionStringBuilder.ProviderConnectionString = provider;
            entityConnectionStringBuilder.Metadata = "res://*/PakLxModel.csdl|res://*/PakLxModel.ssdl|res://*/PakLxModel.msl";

            return entityConnectionStringBuilder.ToString();
        }
        private async Task LoadBatches()
        {
            await LoadAntibodyBatchesAsync().ConfigureAwait(false);
            await LoadLMXAntibodyBatchesAsync().ConfigureAwait(false);
            await LoadDNABatchesAsync().ConfigureAwait(false);
            try
            {
                //paklx database may not exist
                await LoadPakLxBatchesAsync().ConfigureAwait(false);
            }
            catch { }
        }
        private async Task<int> LoadLMXAntibodyBatchesAsync()
        {
            DateTime from = DateTime.Parse(_fromDate);
            DateTime to = DateTime.Parse(_toDate);

            using (var db = new matchitEntities(MIConnString))
            {
                var listofBatch = await (db.tbAntibodyMethod
                                    .Where(item => (DateTime)item.createdt >= from &&
                                    (DateTime)item.createdt <= to).ToListAsync().ConfigureAwait(false));

                foreach (tbAntibodyMethod dr in listofBatch)
                {
                    if (dr.lotID.Split('-')[1].ToLower() == "lmx" || dr.lotID.Split('-')[1].ToLower() == "lmx")
                    {
                        if (!(FoundBatches.ContainsKey(dr.sessionID)))
                        {
                            Batch batch = new Batch(dr.sessionID);
                            batch.backColor = Color.LightGray;
                            batch.type = BatchType.LMX;
                            FoundBatches.Add(dr.sessionID, batch);
                        }

                        Batch batch1 = FoundBatches[dr.sessionID];
                        if (batch1.Samples == null)
                        {
                            batch1.Samples = new List<Sample>();
                        }
                        Sample sample = new Sample(dr.sampleID);
                        batch1.Samples.Add(sample);
                    }
                }
            }
            return 1;
        }
        private async Task<int> LoadAntibodyBatchesAsync()
        {
            DateTime from = DateTime.Parse(_fromDate);
            DateTime to = DateTime.Parse(_toDate);
            
            using (var db = new matchitEntities(MIConnString))
            {
                var listofBatch = await (db.tbAntibodyMethod
                                    .Where(item => (DateTime)item.createdt >= from &&
                                    (DateTime)item.createdt <= to).ToListAsync().ConfigureAwait(false));

                foreach(tbAntibodyMethod dr in listofBatch)
                {
                    if(dr.lotID.Split('-')[1].ToLower() == "sa1" || dr.lotID.Split('-')[1].ToLower() == "sa2")
                    {
                        if (!(FoundBatches.ContainsKey(dr.sessionID)))
                        {
                            Batch batch = new Batch(dr.sessionID);
                            batch.backColor = Color.LightSalmon;
                            batch.type = BatchType.LSA;
                            FoundBatches.Add(dr.sessionID, batch);
                        }

                        Batch batch1 = FoundBatches[dr.sessionID];
                        if (batch1.Samples == null)
                        {
                            batch1.Samples = new List<Sample>();
                        }
                        Sample sample = new Sample(dr.sampleID);
                        batch1.Samples.Add(sample);
                    }                   
                }
            }
            return 1;
        }

        private async Task<int> LoadDNABatchesAsync()
        {
            DateTime from = DateTime.Parse(_fromDate);
            DateTime to = DateTime.Parse(_toDate);

            using (var db = new matchitEntities(MIConnString))
            {
                var listofBatch = await (db.tbDNAMethod
                    .Where(item => (DateTime)item.createDt >= from &&
                                    (DateTime)item.createDt <= to)).ToListAsync().ConfigureAwait(false);
                foreach(tbDNAMethod dr in listofBatch)
                {
                    if (!FoundBatches.ContainsKey(dr.sessionID))
                    {
                        Batch batch = new Batch(dr.sessionID);
                        batch.backColor = Color.LightBlue;
                        batch.type = BatchType.DNA;
                        FoundBatches.Add(dr.sessionID, batch);
                    }
                    Batch batch1 = FoundBatches[dr.sessionID];
                    if(batch1.Samples == null)
                    {
                        batch1.Samples = new List<Sample>();
                    }
                    Sample sample = new Sample(dr.sampleID);
                    batch1.Samples.Add(sample);
                }
            }
            return 2;
        }
        
        private async Task<int> LoadPakLxBatchesAsync()
        {
            DateTime from = DateTime.Parse(_fromDate);
            DateTime to = DateTime.Parse(_toDate);

            using (var db = new matchitPlateletABEntities(PakLxConnString))
            {
                var listofBatch = await (db.tbAntibodyMethod
                    .Where(item => (DateTime)item.createdt >= from &&
                                    (DateTime)item.createdt <= to)).ToListAsync().ConfigureAwait(false);
                foreach (tbAntibodyMethod dr in listofBatch)
                {
                    if (!FoundBatches.ContainsKey(dr.sessionID))
                    {
                        Batch batch = new Batch(dr.sessionID);
                        batch.backColor = Color.LightCyan;
                        batch.type = BatchType.PAKLX;
                        FoundBatches.Add(dr.sessionID, batch);
                    }
                    Batch batch1 = FoundBatches[dr.sessionID];
                    if (batch1.Samples == null)
                    {
                        batch1.Samples = new List<Sample>();
                    }
                    Sample sample = new Sample(dr.sampleID);
                    batch1.Samples.Add(sample);
                }
            }
            return 3;
        }

        public  List<returnjson> LoadLSAExportDate(string batchname, string sampleid)
        {
            setconnectionstrings();
            using (var db = new matchitEntities(MIConnString))
            {
                var lsadata = (from items in db.SingleAntigenV2ExportView
                               where items.sessionID == batchname &&
                               items.Sample_ID == sampleid &&
                               items.Assignment == "Positive"
                               orderby items.Raw_Value descending
                               select new returnjson
                               {
                                   bname = items.Batch_Name,
                                   bsample = items.Sample_ID,
                                   bpatient = items.Patient_Name,
                                   bdrawdate = items.Draw_Date,
                                   bantigens = items.Allele,
                                   bmfi = items.Raw_Value,
                                   bpra = items.PRA,
                                   bsero = items.Serology
                               }).ToList();
                if(lsadata.Count == 0)
                {
                    var lsaolddata = (from items in db.SingleAntigenOncCONExportView
                                   where items.sessionID == batchname &&
                                   items.sampleID == sampleid &&
                                   items.assignment == "Positive"
                                   orderby items.rawValue descending
                                   select new returnjson
                                   {
                                       bname = items.Batch_Name,
                                       bsample = items.sampleID,
                                       bpatient = items.patientName,
                                       bdrawdate = items.drawDt,
                                       bantigens = items.allele,
                                       bmfi = items.rawValue,
                                       bpra = items.pra,
                                       bsero = items.Serology
                                   }).ToList();
                    if(lsaolddata.Count == 0)
                    {
                        var lsadataNeg = (from items in db.SingleAntigenV2ExportView
                                    where items.sessionID == batchname &&
                                    items.Sample_ID == sampleid 
                                    orderby items.Raw_Value descending
                                    select new returnjson
                                    {
                                        bname = items.Batch_Name,
                                        bsample = items.Sample_ID,
                                        bpatient = items.Patient_Name,
                                        bdrawdate = items.Draw_Date,
                                        bantigens = string.Empty,
                                        bmfi = 0,
                                        bpra = 0,
                                        bsero = string.Empty
                                    }).ToList();
                        if (lsadataNeg.Count == 0)
                        {
                            var lsaolddataNeg = (from items in db.SingleAntigenOncCONExportView
                                                 where items.sessionID == batchname &&
                                                 items.sampleID == sampleid
                                                 orderby items.rawValue descending
                                                 select new returnjson
                                                 {
                                                     bname = items.Batch_Name,
                                                     bsample = items.sampleID,
                                                     bpatient = items.patientName,
                                                     bdrawdate = items.drawDt,
                                                     bantigens = string.Empty,
                                                     bmfi = 0,
                                                     bpra = 0,
                                                     bsero = string.Empty
                                                 }).ToList();
                            return lsaolddataNeg;
                        }
                        else
                        {
                            return lsadataNeg;
                        }
                    }
                    else
                    {
                        return lsaolddata;
                    }                    
                }
                else
                {
                    return lsadata;
                }
            }
        }
        public List<returnlmxjson> LoadMXExportDate(string batchname, string sampleid)
        {
            setconnectionstrings();
            using (var db = new matchitEntities(MIConnString))
            {
                var useVBAF = (from item in db.tbAntibodyMethod
                               where item.sampleID == sampleid &&
                               item.sessionID == batchname
                               select item.useVBAF);
                if(useVBAF != null)
                {
                    if(useVBAF.First() == 1)
                    {
                        var lmxdata = (from items in db.LMXReportDataVBAFView
                                       where items.sessionID == batchname && items.sampleID == sampleid
                                       select new returnlmxjson
                                       {
                                           bname = items.sessionID,
                                           bsample = items.sampleID,
                                           bpatient = items.patientName,
                                           bdrawdate = items.Draw_Date,
                                           bclassiresult = items.ClassIResults,
                                           bclassiiresult = items.ClassIIResults
                                       }).ToList();
                        return lmxdata;
                    }
                    else
                    {
                        var lmxdata = (from items in db.LMXReportDataView
                                       where items.sessionID == batchname && items.sampleID == sampleid
                                       select new returnlmxjson
                                       {
                                           bname = items.sessionID,
                                           bsample = items.sampleID,
                                           bpatient = items.patientName,
                                           bdrawdate = items.Draw_Date,
                                           bclassiresult = items.ClassIResults,
                                           bclassiiresult = items.ClassIIResults
                                       }).ToList();
                        return lmxdata;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
        public List<returnDNAjson> LoadDNAExportData(string batchname, string sampleid)
        {
            setconnectionstrings();
            using (var db = new matchitEntities(MIConnString))
            {
                var dnadata = (from items in db.DNAViewReportFinalAssignments
                               join header in db.DNAViewReportHeader
                               on items.sampleID equals header.sampleID
                               
                               where items.Batch == batchname &&
                               items.sampleID == sampleid
                               select new returnDNAjson
                               {
                                   bname = items.Batch,
                                   bpatient = header.patientName,
                                   bsample = items.sampleID,
                                   bdrawdate = header.drawDt,
                                   bag1 = items.ag1,
                                   bag2 = items.ag2,
                                   bloci = items.Locusname
                               }).ToList();
                return dnadata;
            }
        }
    }

    public class returnlmxjson
    {
        public string bname { get; set; }
        public string bsample { get; set; }
        public string bpatient { get; set; }
        public string bdrawdate { get; set; }
        public string bclassiresult { get; set; }
        public string bclassiiresult { get; set; }
    }

    public class returnjson
    {
        public string bname { get; set; }
        public string bsample { get; set; }
        public string bpatient { get; set; }
        public string bdrawdate { get; set; }
        public string bantigens { get; set; }
        public decimal? bmfi { get; set; }
        public decimal? bpra { get; set; }
        public string bsero { get; set; }
    }
    public class returnDNAjson
    {
        public string bname { get; set; }
        public string bsample { get; set; }
        public string bpatient { get; set; }
        public DateTime? bdrawdate { get; set; }
        public string bloci { get; set; }
        public string bag1 { get; set; }
        public string bag2 { get; set; }
    }
    public class returnPAKjson
    {
        public string bsample { get; set; }
        public string bpatient { get; set; }
        public string bdrawdate { get; set; }
        public string bgvip { get; set; }
        public string bhla { get; set; }
        public string bgpiibiiia { get; set; }
        public string bgpibix { get; set; }
        public string bgpiaiia { get; set; }
        public string bcomments { get; set; }
    }    
}
