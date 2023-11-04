using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace stockProject.Models
{
    [MetadataType(typeof(ALT_DEPO_METADATA))]
    public partial class ALT_DEPO
    {

    }
    public class ALT_DEPO_METADATA
    {
        public int ALT_DEPO_ID { get; set; }

        [DisplayName("Substore Name")]
        [Required(ErrorMessage = "Please enter a Substore name!")]
        [StringLength(50, ErrorMessage = "Substore name cannot contain more than 50 characters!")]
        public string ALT_DEPO_ADI { get; set; }

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