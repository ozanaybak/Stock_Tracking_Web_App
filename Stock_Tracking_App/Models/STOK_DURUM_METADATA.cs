using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace stockProject.Models
{
    [MetadataType(typeof(STOK_DURUM_METADATA))]
    public partial class STOK_DURUM
    {

    }
    public class STOK_DURUM_METADATA
    {
        public int DURUM_ID { get; set; }

        [DisplayName("Stock Name")]
        public int STOK_ID { get; set; }

        [DisplayName("Store / Substore")]
        public int DEPO_ESLESTIRME_ID { get; set; }

        [DisplayName("Durum Miktar")]
        public decimal DURUM_MIKTAR { get; set; }

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