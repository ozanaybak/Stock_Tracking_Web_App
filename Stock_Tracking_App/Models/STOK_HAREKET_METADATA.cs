using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace stockProject.Models
{
    [MetadataType(typeof(STOK_HAREKET_METADATA))]
    public partial class STOK_HAREKET
    {

    }
    public class STOK_HAREKET_METADATA
    {
        public int HAREKET_ID { get; set; }

        [DisplayName("Stock Name")]
        public int STOK_ID { get; set; }

        [DisplayName("Store / Substore")]
        public int DEPO_ESLESTIRME_ID { get; set; }

        [DisplayName("Sorumlu Adı")]
        public int SORUMLU_ID { get; set; }

        [DisplayName("Transfer Type")]
        public int HAREKET_TIP { get; set; }

        [DisplayName("Description")]
        public string ACIKLAMA { get; set; }

        [DisplayName("Transfer Amount")]
        public decimal HAREKET_MIKTAR { get; set; }

        [DisplayName("Transfer Date")]
        public System.DateTime HAREKET_TARIHI { get; set; }

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