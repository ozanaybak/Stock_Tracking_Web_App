using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace stockProject.Models
{
    [MetadataType(typeof(DEPO_METADATA))]
    public partial class DEPO
    {

    }
    public class DEPO_METADATA
    {
        public int DEPO_ID { get; set; }

        [DisplayName("Store Name")]
        [Required(ErrorMessage = "Please enter a store name!")]
        [StringLength(50, ErrorMessage = "store name cannot contain more than 50 characters!")]
        public string DEPO_ADI { get; set; }

        [DisplayName("Status")]
        public bool STATU { get; set; }

        [DisplayName("Creator User")]
        public int OLUSTURAN_KULLANICI { get; set; }

        [DisplayName("Creation Date")]
        public System.DateTime OLUSTURMA_TARIHI { get; set; }

        [DisplayName("Updator User")]
        public Nullable<int> GUNCELLEYEN_KULLANICI { get; set; }

        [DisplayName("Updation Date")]
        public Nullable<System.DateTime> GUNCELLEME_TARIHI { get; set; }
    }
}