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
    
    public partial class DEPO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DEPO()
        {
            this.DEPO_ESLESTIRME = new HashSet<DEPO_ESLESTIRME>();
        }
    
        public int DEPO_ID { get; set; }
        public string DEPO_ADI { get; set; }
        public bool STATU { get; set; }
        public int OLUSTURAN_KULLANICI { get; set; }
        public System.DateTime OLUSTURMA_TARIHI { get; set; }
        public Nullable<int> GUNCELLEYEN_KULLANICI { get; set; }
        public Nullable<System.DateTime> GUNCELLEME_TARIHI { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DEPO_ESLESTIRME> DEPO_ESLESTIRME { get; set; }
        public virtual KULLANICI KULLANICI { get; set; }
        public virtual KULLANICI KULLANICI1 { get; set; }
    }
}
