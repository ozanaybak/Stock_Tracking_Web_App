//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace stockProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OLCU_BIRIMI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OLCU_BIRIMI()
        {
            this.STOK = new HashSet<STOK>();
        }
    
        public int OLCUBIRIM_ID { get; set; }
        public string OLCUBIRIM_ADI { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<STOK> STOK { get; set; }
    }
}
