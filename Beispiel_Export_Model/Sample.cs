using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeispielExportModel
{
    public class Sample
    {
        public string SampleID { get; set; }
        public string PatientName { get; set; }
        public string Comments { get; set; }
        public DateTime? DrawDate { get; set; }
        public bool isSelected { get; set; }

        public Sample(string sampleId)
        {
            SampleID = sampleId;
        }
    }
}
