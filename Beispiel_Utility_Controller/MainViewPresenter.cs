using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using BeispielExportModel;
using BeispielViewModel;
using System.IO;
using Serilog;

namespace BeispielUtilityPresenter
{
    public class MainViewPresenter
    {
        ViewInterfaces.IMainView _mainView;
        public List<BatchVM> batchVM { get; set; }
        public List<LSAExportData> lsaVM { get; set; }
        public List<LMXExportData> lmxVM { get; set; }
        public List<DNAExportData> dnaVM { get; set; }
        public List<PAKExportData> pakVM { get; set; }

        private string startDate;
        private string endDate;
        public MainViewPresenter(ViewInterfaces.IMainView mainView)
        {
            _mainView = mainView;
            setEventHandlers();
            batchVM = new List<BatchVM>();
        }

        private void setEventHandlers()
        {
            _mainView.setStartDate += setStartDate;
            _mainView.setEndDate += setEndDate;
            _mainView.loadData += loadData;
            _mainView.getBatches += getBatches;
            _mainView.exportData += exportData;
        }

        private void setStartDate(object sender, EventArgs e)
        {
            startDate = (string)sender;
        }
        private void setEndDate(object sender, EventArgs e)
        {
            endDate = (string)sender;
        }
        private void loadData(object sender, EventArgs e)
        {
            List<BatchVM> batches = (List<BatchVM>)sender;
            Dictionary<string, LSAExportData> lsadata = new Dictionary<string, LSAExportData>();
            Dictionary<string, LMXExportData> lmxdata = new Dictionary<string, LMXExportData>();
            Dictionary<string, DNAExportData> dnadata = new Dictionary<string, DNAExportData>();
            Dictionary<string, PAKExportData> pakdata = new Dictionary<string, PAKExportData>();
            lsaVM = new List<LSAExportData>();
            dnaVM = new List<DNAExportData>();
            pakVM = new List<PAKExportData>();
            lmxVM = new List<LMXExportData>();
            foreach (BatchVM batch in batches)
            {
                switch (batch.batchType)
                {
                    case "LSA":
                        if (batch.Selected)
                        {
                            lsadata.Add(batch.name, null);
                        }
                        else
                        {
                            int countsamples = (from samp in batch.samples where samp.Selected == true select samp).Count();
                            if (countsamples > 0)
                            {
                                lsadata.Add(batch.name, null);
                            }
                        }
                        if (lsadata.ContainsKey(batch.name))
                        {
                            var query = (from sample in batch.samples where sample.Selected == true select sample).ToList<SampleVM>();
                            Batches b = new Batches();
                            foreach(SampleVM vM in query)
                            {
                                List<returnjson> data = b.LoadLSAExportDate(batch.name, vM.name);
                                LSAExportData lSAExportData = new LSAExportData();
                                if (data.Count > 0)
                                {
                                    lSAExportData.sampleID = data[0].bsample;
                                    lSAExportData.drawDate = data[0].bdrawdate;
                                    lSAExportData.pra = data[0].bpra.ToString();
                                    lSAExportData.patientName = data[0].bpatient;
                                    StringBuilder antigens = new StringBuilder();
                                    foreach (returnjson item in data)
                                    {
                                        if (item.bantigens.Length > 0)
                                        {
                                            antigens.Append(item.bantigens);
                                            antigens.Append($"({item.bsero})");
                                            antigens.Append(item.bmfi);
                                            antigens.Append("MFI,");
                                        }                                        
                                    }
                                    if (antigens.Length > 0)
                                    {
                                        lSAExportData.assignedAntigen = antigens.ToString().Remove(antigens.ToString().Length - 1, 1);
                                    }
                                    else
                                    {
                                        lSAExportData.assignedAntigen = string.Empty;
                                    }
                                    lsadata[batch.name] = lSAExportData;
                                    lSAExportData.batchName = batch.name;
                                    lsaVM.Add(lSAExportData);
                                }
                                else
                                {

                                }
                            }
                        }
                        break;
                    case "LMX":
                        if (batch.Selected)
                        {
                            lmxdata.Add(batch.name, null);
                        }
                        else
                        {
                            int countsamples = (from samp in batch.samples where samp.Selected == true select samp).Count();
                            if (countsamples > 0)
                            {
                                lmxdata.Add(batch.name, null);
                            }
                        }
                        if (lmxdata.ContainsKey(batch.name))
                        {
                            var query = (from sample in batch.samples where sample.Selected == true select sample).ToList<SampleVM>();
                            Batches b = new Batches();
                            foreach (SampleVM vM in query)
                            {
                                List<returnlmxjson> data = b.LoadMXExportDate(batch.name, vM.name);
                                LMXExportData lMXExportData = new LMXExportData();
                                if (data.Count > 0)
                                {
                                    lMXExportData.sampleID = data[0].bsample;
                                    lMXExportData.drawDate = data[0].bdrawdate;                                    
                                    lMXExportData.patientName = data[0].bpatient;
                                    lMXExportData.classiresult = data[0].bclassiresult;
                                    lMXExportData.classiiresult = data[0].bclassiiresult;
                                    lmxdata[batch.name] = lMXExportData;
                                    lMXExportData.batchName = batch.name;
                                    lmxVM.Add(lMXExportData);
                                }
                                else
                                {

                                }
                            }
                        }
                        break;
                    case "DNA":
                        if (batch.Selected)
                        {
                            dnadata.Add(batch.name, null);
                        }
                        else
                        {
                            int countsamples = (from samp in batch.samples where samp.Selected == true select samp).Count();
                            if (countsamples > 0)
                            {
                                dnadata.Add(batch.name, null);
                            }
                        }
                        if (dnadata.ContainsKey(batch.name))
                        {
                            var query = (from sample in batch.samples where sample.Selected == true select sample).ToList<SampleVM>();
                            Batches b = new Batches();
                            foreach (SampleVM vM in query)
                            {
                                List<returnDNAjson> data = b.LoadDNAExportData(batch.name, vM.name);
                                DNAExportData dnaExport = new DNAExportData();
                                
                                foreach (returnDNAjson item in data)
                                {
                                    dnaExport.sampleID = item.bsample;
                                    dnaExport.patientName = item.bpatient;
                                    dnaExport.drawDate = item.bdrawdate.HasValue ? item.bdrawdate.Value.ToString("MMM.dd.yy") : string.Empty;
                                    dnaExport.loci = item.bloci;
                                    dnaExport.Ag1 = item.bag1;
                                    if(item.bag1 != item.bag2)
                                    {
                                        dnaExport.Ag2 = item.bag2;
                                    }
                                }
                                dnadata[batch.name] = dnaExport;
                                dnaExport.batchName = batch.name;
                                dnaVM.Add(dnaExport);
                            }
                        }
                        break;
                    case "PAKLX":
                        if (batch.Selected)
                        {
                            pakdata.Add(batch.name, null);
                        }
                        else
                        {
                            int countsamples = (from samp in batch.samples where samp.Selected == true select samp).Count();
                            if (countsamples > 0)
                            {
                                pakdata.Add(batch.name, null);
                            }
                        }
                        if (pakdata.ContainsKey(batch.name))
                        {
                            var query = (from sample in batch.samples where sample.Selected == true select sample).ToList<SampleVM>();
                            Batches b = new Batches();
                            List<string> samplenames = new List<string>();
                            foreach(SampleVM vm in query)
                            {
                                samplenames.Add(vm.name);
                            }
                            PakLx pak = new PakLx(batch.name, samplenames);

                            foreach (PakLxResult result in pak.results[batch.name])
                            {
                                PAKExportData pAKExport = new PAKExportData();
                                pAKExport.sampleID = result.sampleid;
                                pAKExport.patientName = result.patientName;
                                pAKExport.GPIVResult = result.GPIVResult;
                                pAKExport.HLAResult = result.HLAResult;
                                pAKExport.GPIaGPIIaResult = result.GPIaIIaResult;
                                pAKExport.GPIBIXResult = result.GPIBIXResult;
                                pAKExport.GPIIbIIIaResult = result.GPIIbIIIaResult;
                                pAKExport.UserComments = result.UserComments;
                                pAKExport.batchName = batch.name;
                                pakVM.Add(pAKExport);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        private void getBatches(object sender, EventArgs e)
        {
            batchVM = new List<BatchVM>();
            Batches batches = new Batches(startDate, endDate);
            //create list of VM for frontend
            foreach(Batch b in batches.FoundBatches.Values)
            {
                BatchVM bVM = new BatchVM();
                bVM.name = b.BatchName;
                bVM.backcolor = b.backColor;
                bVM.batchType = b.type.ToString();
                bVM.samples = new List<SampleVM>();
                foreach(Sample s in b.Samples)
                {
                    SampleVM sVM = new SampleVM();
                    sVM.name = s.SampleID;
                    bVM.samples.Add(sVM);
                }
                batchVM.Add(bVM);
            }
        }
        private void exportData(object sender, EventArgs e)
        {
            string currentBatch = string.Empty;
            //lsa samples
            var listBatch = (from item in lsaVM select item.batchName).Distinct().ToList();
            foreach(string bname in listBatch)
            {
                var samples = lsaVM.Where(x => x.batchName == bname).ToList();
                StringBuilder lsaData = new StringBuilder();
                lsaData.AppendLine("sample ID;patient name;Draw Date;PRA;Assigned Antigens");
                foreach (LSAExportData data in samples)
                {
                    lsaData.AppendLine($"{data.sampleID};{data.patientName};{data.drawDate};{data.pra};{data.assignedAntigen}");
                }
                writeFile(bname, lsaData.ToString());
            }

            //lmx samples
            var lmxlistBatch = (from item in lmxVM select item.batchName).Distinct().ToList();
            foreach(string bname in lmxlistBatch)
            {
                var samples = lmxVM.Where(x => x.batchName == bname).ToList();
                StringBuilder lmxData = new StringBuilder();
                lmxData.AppendLine("sample ID;patient Name;Draw Date;Class I Results;Class II Results");
                foreach (LMXExportData data in samples)
                {
                    lmxData.AppendLine($"{data.sampleID};{data.patientName};{data.drawDate};{data.classiresult};{data.classiiresult}");
                }
                writeFile(bname, lmxData.ToString());
            }
            //paklx samples
            var pakBatchlist = (from item in pakVM select item.batchName).Distinct().ToList();
            foreach(string bname in pakBatchlist)
            {
                var samples = pakVM.Where(x => x.batchName == bname).ToList();
                StringBuilder pakData = new StringBuilder();
                pakData.AppendLine("sample ID;patient name;draw date;GPIV Result;HLA Result;GPIIb-IIIa(HPA-1-3-4) Result;GPIBIX(HPA-2) Result;GPIaIIa(HPA-5) Result;User Comments");
                foreach(PAKExportData data in samples)
                {
                    pakData.AppendLine($"{data.sampleID};{data.patientName};{data.drawDate};{data.GPIVResult};{data.HLAResult};{data.GPIIbIIIaResult};{data.GPIBIXResult};{data.GPIaGPIIaResult};{data.UserComments}");
                }
                writeFile(bname, pakData.ToString());
            }

            //dna samples
            var dnaBatchlist = (from item in dnaVM select item.batchName).Distinct().ToList();
            foreach(string bname in dnaBatchlist)
            {
                var samples = dnaVM.Where(x => x.batchName == bname).ToList();
                StringBuilder dnaData = new StringBuilder();
                dnaData.AppendLine("sample ID;patient name;Draw Date;Locus;AG1;AG2");
                foreach(DNAExportData data in samples)
                {
                    dnaData.AppendLine($"{data.sampleID};{data.patientName};{data.drawDate};{data.loci};{data.Ag1};{data.Ag2}");
                }
                writeFile(bname, dnaData.ToString());
            }
        }
        private void writeFile(string batchName, string data)
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter($"{_mainView.savepath}\\{batchName}.csv", false))
            {
                file.WriteLine(data);
            }
        }
    }
}
