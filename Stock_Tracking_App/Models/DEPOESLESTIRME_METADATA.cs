using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace stockProject.Models
{
    [MetadataType(typeof(DEPOESLESTIRME_METADATA))]
    public partial class DEPO_ESLESTIRME
    {

    }
    public class DEPOESLESTIRME_METADATA
    {
        public int DEPO_ESLESTIRME_ID { get; set; }

        [DisplayName("Store Name")]
        [Required(ErrorMessage = "Please enter a store name!")]
        public int DEPO_ID { get; set; }

        [DisplayName("SubStore Name")]
        [Required(ErrorMessage = "Please enter a store name!")]
        public int ALT_DEPO_ID { get; set; }
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