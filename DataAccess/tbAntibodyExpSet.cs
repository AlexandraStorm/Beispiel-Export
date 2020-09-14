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
    
    public partial class tbAntibodyExpSet
    {
        public string lotID { get; set; }
        public System.DateTime expirationdt { get; set; }
        public string probeName { get; set; }
        public int logicID { get; set; }
        public Nullable<int> probeLogicalPosition { get; set; }
        public Nullable<int> sortOrder { get; set; }
        public Nullable<decimal> Con1 { get; set; }
        public Nullable<decimal> Con2 { get; set; }
        public Nullable<decimal> Con3 { get; set; }
        public Nullable<decimal> Con4 { get; set; }
        public string consensus { get; set; }
        public string BeadType { get; set; }
        public string assayName { get; set; }
        public string assayVersion { get; set; }
        public string alleleDBVersion { get; set; }
        public Nullable<System.DateTime> createdt { get; set; }
        public Nullable<System.DateTime> updatedt { get; set; }
        public Nullable<int> updateby { get; set; }
        public Nullable<decimal> slope { get; set; }
        public decimal sabaf { get; set; }
        public Nullable<int> ID4DigitLogicID { get; set; }
        public Nullable<decimal> Cutoff { get; set; }
    }
}