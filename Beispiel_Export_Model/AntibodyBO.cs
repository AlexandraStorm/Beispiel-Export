using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
namespace BeispielExportModel
{
    /// ''' Defines an Antibody Sample business object used in Antibody testing
    /// ''' </summary>

    /// ''' <remarks></remarks>
    public class AntibodyBO : ISampleBO, IDisposable
    {
        #region "Properties"
        public int AntibodyID { get; set; }
        public string ReviewedBy { get; set; }
        public string WellPosition { get; set; }
        public int WellIndex { get; set; }
        public string SessionID { get; set; }
        public string SampleID { get; set; }
        public DateTime RunDate { get; set; }
        public int TestType { get; set; }
        public string Comments { get; set; }
        public bool IsDirty { get; set; }
        public double Raw { get; set; }
        public double RVal { get; set; }
        public double PctPositive { get; set; }
        public int AssignCutoff { get; set; }
        public double AssignAdj1 { get; set; }
        public double AssignAdj2 { get; set; }
        public double AssignAdj3 { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsApproved { get; set; }
        public string UpdateBy { get; set; }
        #endregion
        public List<AntibodyProbesBO> Probes { get; set; }
        private List<AntibodyStatsBO> _stats;
        public AntibodyBO()
        {
            Probes = new List<AntibodyProbesBO>();
            _stats = new List<AntibodyStatsBO>();
        }
        public AntibodyBO(string sampleID, string sessionID)
        {
            SampleID = sampleID;
            SessionID = sessionID;
            Probes = new List<AntibodyProbesBO>();
            _stats = new List<AntibodyStatsBO>();
        }

        /// <summary>
        ///     ''' If a dataset has already been filled then call this
        ///     ''' function to load this BO sample will filter on assigned
        ///     ''' sample ID
        ///     ''' </summary>
        ///     ''' <param name="myDS">prefilled dataset</param>
        ///     ''' <remarks></remarks>
        public void LoadSample(DataSet myDS, int rowIndex)
        {
            WellIndex = rowIndex;
            LoadMe(myDS);
        }
        /// <summary>
        ///     ''' Will load a local dataset with call to DAL using GetBatch call then filter out the
        ///     ''' necessary sample to fill this BO.
        ///     ''' </summary>
        ///     ''' <remarks></remarks>
        private void LoadMe(DataSet mySampleTable)
        {            
            LoadObjectHierarchy(mySampleTable);
        }
        public void LoadObjectHierarchy(DataSet myDS)
        {
            Probes.Clear();
            _stats.Clear();
            LoadProbes(myDS.Tables[0], myDS.Tables[5]);
            LoadStats(myDS.Tables[1]);
        }
        private void LoadProbes(DataTable myProbeTable, DataTable myAlleleTable)
        {
            string _myClause = string.Format("SampleID='{0}' AND SessionID = '{1}'", this.SampleID, this.SessionID);
            foreach (DataRow myRow in myProbeTable.Select(_myClause))
            {
                AntibodyProbesBO myProbe;
                string lbeadname = myRow["bead"].ToString();
                int lrval = (int)Math.Round(decimal.Parse(myRow["rawValue"].ToString()), 0, MidpointRounding.AwayFromZero);
                double ladj1 = Math.Round(System.Convert.ToDouble(myRow["adjust1"]), 2, MidpointRounding.AwayFromZero);
                double ladj2 = Math.Round(System.Convert.ToDouble(myRow["adjust2"]), 2, MidpointRounding.AwayFromZero);
                double ladj3 = Math.Round(System.Convert.ToDouble(myRow["adjust3"]), 2, MidpointRounding.AwayFromZero);
                double ladjn = System.Convert.ToDouble(myRow["adjustN"]);
                string lassgn = myRow["assignment"].ToString();
                double lhibg1 = System.Convert.ToDouble(myRow["hiBGAdjust1"]);
                double lhibg2 = System.Convert.ToDouble(myRow["hiBGAdjust2"]);
                double lhibg3 = System.Convert.ToDouble(myRow["hiBGAdjust3"]);
                string lhibgassign = myRow["hiBGAssignment"].ToString();
                string lconsensus = myRow["consensus"].ToString();
                int lScore = int.Parse(myRow["score"].ToString());
                double lweak;
                if ((myRow["weakPct"] == DBNull.Value))
                    lweak = System.Convert.ToDouble(0);
                else
                    lweak = System.Convert.ToDouble(myRow["weakPct"]);

                bool loverride;
                if ((myRow["overRide"] == DBNull.Value))
                    loverride = false;
                else
                    loverride = myRow["overRide"].ToString() == "0" ? false : true;

                int lbcount;
                if ((myRow["beadcount"] == DBNull.Value))
                    lbcount = 0;
                else
                    lbcount = int.Parse(myRow["beadcount"].ToString());
                int lhigbscore = int.Parse(myRow["hiBGScore"].ToString());
                myProbe = new AntibodyProbesBO(lbeadname, lrval, ladj1, ladj2, ladj3, ladjn, lassgn, lhibg1, lhibg2, lhibg3, lhibgassign, lweak, loverride, lbcount, lconsensus, lScore, lhigbscore, null);

                Probes.Add(myProbe);
            }
        }
        /// <summary>
        ///     ''' Loads the AntibodyStats BO
        ///     ''' </summary>
        ///     ''' <param name="myStatsTable"></param>
        ///     ''' <remarks></remarks>
        private void LoadStats(DataTable myStatsTable)
        {
            string _myClause;
            if ((_stats.Count > 0))
                _stats.Clear();
            _myClause = "SampleID='" + this.SampleID + "' AND SessionID = '" + this.SessionID + "'";
            foreach (DataRow myRow in myStatsTable.Select(_myClause))
            {
                AntibodyStatsBO myStats;
                string antigen = myRow["antigen"].ToString();
                int posThis = System.Convert.ToInt32(myRow["positiveThis"].ToString());
                int posOther = System.Convert.ToInt32(myRow["positiveOther"].ToString());
                int negThis = System.Convert.ToInt32(myRow["negativeThis"].ToString());
                int negOther = System.Convert.ToInt32(myRow["negativeOther"].ToString());
                double chiSquare = System.Convert.ToDouble(myRow["chiSquare"].ToString());
                double rValue = System.Convert.ToDouble(myRow["rValue"].ToString());
                int percentPos = System.Convert.ToInt32(Double.Parse(myRow["pctPositive"].ToString()));
                int Tail = System.Convert.ToInt32(myRow["tail"].ToString());
                string traceType = myRow["tracetype"].ToString();
                double strength = System.Convert.ToDouble((myRow["strength"] == DBNull.Value) ? "0" : myRow["strength"].ToString());
                double adjust1;

                try
                {
                    adjust1 = System.Convert.ToDouble((myRow["adjust1"] == DBNull.Value) ? "0" : myRow["adjust1"].ToString());
                }
                catch (Exception ex)
                {
                    adjust1 = 0.0;
                }
                myStats = new AntibodyStatsBO(antigen, posThis, posOther, negThis, negOther, chiSquare, rValue, percentPos, Tail, traceType, strength);
                _stats.Add(myStats);
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
                    foreach (AntibodyProbesBO item in Probes)
                        item.Dispose();
                    Probes.Clear();
                    Probes = null;
                    foreach (AntibodyStatsBO item in _stats)
                        item.Dispose();
                    _stats.Clear();
                    _stats = null;
                   // Stats = null;
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
}