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
    
    public partial class tbDNAAllele
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbDNAAllele()
        {
            this.tbDNASuggestedAssignments = new HashSet<tbDNASuggestedAssignments>();
            this.tbDNASuggestedAssignments1 = new HashSet<tbDNASuggestedAssignments>();
            this.tbBeadHits = new HashSet<tbBeadHits>();
        }
    
        public int AlleleID { get; set; }
        public string Name { get; set; }
        public int Sort { get; set; }
        public bool isCWD { get; set; }
        public string LastAlleleDBVersion { get; set; }
        public string Serology { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbDNASuggestedAssignments> tbDNASuggestedAssignments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbDNASuggestedAssignments> tbDNASuggestedAssignments1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbBeadHits> tbBeadHits { get; set; }
    }
}