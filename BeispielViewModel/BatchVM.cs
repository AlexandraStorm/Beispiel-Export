using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
namespace BeispielViewModel
{
    public class BatchVM 
    {
        public bool Selected { get; set; }
        public string name { get; set; }
        public Color backcolor { get; set; }
        public string batchType { get; set; }
        public List<SampleVM> samples { get; set; }

    }

    public class SampleVM
    {
        public string name { get; set; }
        public bool Selected { get; set; }
    }

    public class LMXExportData
    {
        public string batchName { get; set; }
        public string sampleID { get; set; }
        public string patientName { get; set; }
        public string drawDate { get; set; }
        public string classiresult { get; set; }
        public string classiiresult { get; set; }
    }
    public class LSAExportData
    {
        public string batchName { get; set; }
        public string sampleID { get; set; }
        public string patientName { get; set; }
        public string drawDate { get; set; }
        public string pra { get; set; }
        public string assignedAntigen { get; set; }
    }
    public class DNAExportData
    {
        public string batchName { get; set; }
        public string sampleID { get; set; }
        public string patientName { get; set; }
        public string drawDate { get; set; }
        public string loci { get; set; }
        public string Ag1 { get; set; }
        public string Ag2 { get; set; }
    }
    public class PAKExportData
    {
        public string batchName { get; set; }
        public string sampleID { get; set; }
        public string patientName { get; set; }
        public string drawDate { get; set; }
        public string GPIVResult { get; set; }
        public string HLAResult { get; set; }
        public string GPIIbIIIaResult { get; set; }
        public string GPIBIXResult { get; set; }
        public string GPIaGPIIaResult { get; set; }
        public string UserComments { get; set; }
    }
}
