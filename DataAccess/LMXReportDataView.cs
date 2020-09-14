//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class LMXReportDataView
    {
        public string Batch_Name { get; set; }
        public string wellLocation { get; set; }
        public string sampleID { get; set; }
        public string patientName { get; set; }
        public Nullable<System.DateTime> drawDt { get; set; }
        public string accession { get; set; }
        public string donorCenterNumber { get; set; }
        public string patientDOB { get; set; }
        public string userDefinedName { get; set; }
        public string donorNumber { get; set; }
        public string AssayName { get; set; }
        public string LotID { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public Nullable<decimal> CI_01_MFI { get; set; }
        public Nullable<decimal> CI_01_ADJ_1 { get; set; }
        public Nullable<decimal> CI_01_ADJ_2 { get; set; }
        public Nullable<decimal> CI_01_ADJ_3 { get; set; }
        public Nullable<int> CI_01_Score { get; set; }
        public Nullable<decimal> CI_02_MFI { get; set; }
        public Nullable<decimal> CI_02_ADJ_1 { get; set; }
        public Nullable<decimal> CI_02_ADJ_2 { get; set; }
        public Nullable<decimal> CI_02_ADJ_3 { get; set; }
        public Nullable<int> CI_02_Score { get; set; }
        public Nullable<decimal> CI_03_MFI { get; set; }
        public Nullable<decimal> CI_03_ADJ1 { get; set; }
        public Nullable<decimal> CI_03_ADJ2 { get; set; }
        public Nullable<decimal> CI_03_ADJ3 { get; set; }
        public Nullable<int> CI_03_SCORE { get; set; }
        public Nullable<decimal> CI_04_MFI { get; set; }
        public Nullable<decimal> CI_04_ADJ1 { get; set; }
        public Nullable<decimal> CI_04_ADJ2 { get; set; }
        public Nullable<decimal> CI_04_ADJ3 { get; set; }
        public Nullable<int> CI_04_SCORE { get; set; }
        public Nullable<decimal> CI_05_MFI { get; set; }
        public Nullable<decimal> CI_05_ADJ1 { get; set; }
        public Nullable<decimal> CI_05_ADJ2 { get; set; }
        public Nullable<decimal> CI_05_ADJ3 { get; set; }
        public Nullable<int> CI_05_SCORE { get; set; }
        public Nullable<decimal> CI_06_MFI { get; set; }
        public Nullable<decimal> CI_06_ADJ1 { get; set; }
        public Nullable<decimal> CI_06_ADJ2 { get; set; }
        public Nullable<decimal> CI_06_ADJ3 { get; set; }
        public Nullable<int> CI_06_SCORE { get; set; }
        public Nullable<decimal> CI_07_MFI { get; set; }
        public Nullable<decimal> CI_07_ADJ1 { get; set; }
        public Nullable<decimal> CI_07_ADJ2 { get; set; }
        public Nullable<decimal> CI_07_ADJ3 { get; set; }
        public Nullable<int> CI_07_SCORE { get; set; }
        public string ClassIResults { get; set; }
        public string ClassIOverride { get; set; }
        public Nullable<decimal> CII_01_MFI { get; set; }
        public Nullable<decimal> CII_01_ADJ1 { get; set; }
        public Nullable<decimal> CII_01_ADJ2 { get; set; }
        public Nullable<decimal> CII_01_ADJ3 { get; set; }
        public Nullable<int> CII_01_SCORE { get; set; }
        public Nullable<decimal> CII_02_MFI { get; set; }
        public Nullable<decimal> CII_02_ADJ1 { get; set; }
        public Nullable<decimal> CII_02_ADJ2 { get; set; }
        public Nullable<decimal> CII_02_ADJ3 { get; set; }
        public Nullable<int> CII_02_SCORE { get; set; }
        public Nullable<decimal> CII_03_MFI { get; set; }
        public Nullable<decimal> CII_03_ADJ1 { get; set; }
        public Nullable<decimal> CII_03_ADJ2 { get; set; }
        public Nullable<decimal> CII_03_ADJ3 { get; set; }
        public Nullable<int> CII_03_SCORE { get; set; }
        public Nullable<decimal> CII_04_MFI { get; set; }
        public Nullable<decimal> CII_04_ADJ1 { get; set; }
        public Nullable<decimal> CII_04_ADJ2 { get; set; }
        public Nullable<decimal> CII_04_ADJ3 { get; set; }
        public Nullable<int> CII_04_SCORE { get; set; }
        public Nullable<decimal> CII_05_MFI { get; set; }
        public Nullable<decimal> CII_05_ADJ1 { get; set; }
        public Nullable<decimal> CII_05_ADJ2 { get; set; }
        public Nullable<decimal> CII_05_ADJ3 { get; set; }
        public Nullable<int> CII_05_SCORE { get; set; }
        public string ClassIIResults { get; set; }
        public string ClassIIOverride { get; set; }
        public Nullable<decimal> CON1 { get; set; }
        public Nullable<decimal> CON2 { get; set; }
        public Nullable<decimal> CON3 { get; set; }
        public Nullable<decimal> C77 { get; set; }
        public Nullable<System.DateTime> runDate { get; set; }
        public string comments { get; set; }
        public int ctrlSampleFailed { get; set; }
        public Nullable<int> CI_01_BeadCount { get; set; }
        public Nullable<int> CI_02_BeadCount { get; set; }
        public Nullable<int> CI_03_BeadCount { get; set; }
        public Nullable<int> CI_04_BeadCount { get; set; }
        public Nullable<int> CI_05_BeadCount { get; set; }
        public Nullable<int> CI_06_BeadCount { get; set; }
        public Nullable<int> CI_07_BeadCount { get; set; }
        public Nullable<int> CII_01_BeadCount { get; set; }
        public Nullable<int> CII_02_BeadCount { get; set; }
        public Nullable<int> CII_03_BeadCount { get; set; }
        public Nullable<int> CII_04_BeadCount { get; set; }
        public Nullable<int> CII_05_BeadCount { get; set; }
        public bool useMFICutoff { get; set; }
        public Nullable<decimal> MFICutoffValue { get; set; }
        public string Exp_Date { get; set; }
        public string ReportDate { get; set; }
        public string Draw_Date { get; set; }
        public string Run_Date { get; set; }
        public string reviewer { get; set; }
        public string sessionID { get; set; }
        public string CompletedBy { get; set; }
        public Nullable<System.DateTime> CompletedDt { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedDt { get; set; }
        public int AntibodyID { get; set; }
    }
}