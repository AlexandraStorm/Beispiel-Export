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
    
    public partial class tbAntibodyStats
    {
        public int AntibodyID { get; set; }
        public string antigen { get; set; }
        public Nullable<int> positiveThis { get; set; }
        public Nullable<int> positiveOther { get; set; }
        public Nullable<int> negativeThis { get; set; }
        public Nullable<int> negativeOther { get; set; }
        public Nullable<decimal> chiSquare { get; set; }
        public Nullable<decimal> rValue { get; set; }
        public Nullable<decimal> pctPositive { get; set; }
        public Nullable<int> tail { get; set; }
        public string traceType { get; set; }
        public Nullable<decimal> strength { get; set; }
        public Nullable<System.DateTime> UpdateDt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    
        public virtual tbAntibodyMethod tbAntibodyMethod { get; set; }
    }
}
