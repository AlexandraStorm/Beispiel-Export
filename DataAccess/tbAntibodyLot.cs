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
    
    public partial class tbAntibodyLot
    {
        public string LotID { get; set; }
        public int LogicID { get; set; }
        public Nullable<int> ID4DigitLogicID { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public string AssayName { get; set; }
        public string AssayVersion { get; set; }
        public string AssayDescription { get; set; }
        public Nullable<decimal> HiBGCalc1 { get; set; }
        public Nullable<decimal> HiBGCalc2 { get; set; }
        public Nullable<decimal> HiBGCalc3 { get; set; }
        public Nullable<int> Con1RangeUpper { get; set; }
        public Nullable<int> Con1RangeLower { get; set; }
        public Nullable<int> Con2RangeUpper { get; set; }
        public Nullable<int> Con2RangeLower { get; set; }
        public Nullable<int> Con3RangeUpper { get; set; }
        public Nullable<int> Con3RangeLower { get; set; }
        public bool Available { get; set; }
        public Nullable<byte> Version { get; set; }
        public Nullable<decimal> MFIThreshold { get; set; }
    }
}
