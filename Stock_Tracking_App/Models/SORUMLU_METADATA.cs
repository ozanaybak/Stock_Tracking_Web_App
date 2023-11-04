using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace stockProject.Models
{
    [MetadataType(typeof(SORUMLU_METADATA))]
    public partial class SORUMLU
    {

    }
    public class SORUMLU_METADATA
    {
        public int SORUMLU_ID { get; set; }

        [DisplayName("Sorumlu Adı")]
        [Required(ErrorMessage = "Please enter an Sorumlu Adı!")]
        [StringLength(50, ErrorMessage = " Sorumlu name cannot contain more than 50 characters!")]
        public string SORUMLU_ADI { get; set; }

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