using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace stockProject.Models
{
    
    [MetadataType(typeof(KULLANICI_METADATA))]
    public partial class KULLANICI
    {

    }
    public class KULLANICI_METADATA
    {
        [DisplayName("Name")]
        [Required(ErrorMessage = "Please enter a name!")]
        [StringLength(50, ErrorMessage = "name cannot contain more than 50 characters!")]
        public string KUL_AD { get; set; }
        public int KULLANICI_ID { get; set; }
        [DisplayName("Username")]
        [Required(ErrorMessage = "Please enter a username!")]
        [StringLength(50, ErrorMessage = "User name cannot contain more than 50 characters!")]
        public string KUL_USERNAME { get; set; }

        [DisplayName("Password")]
        [Required(ErrorMessage = "Please enter a password!")]
        [StringLength(50, ErrorMessage = "User name cannot contain more than 50 characters!")]
        public string KUL_SIFRE { get; set; }

        [Required(ErrorMessage = "Please enter a username!")]
        [StringLength(50, ErrorMessage = "Surname field cannot contain more than 50 characters!")]
        [DisplayName("Surname")]
        public string KUL_SOYAD { get; set; }

        [DisplayName("User Type")]
        [Required(ErrorMessage = "Please select an usertype!")]
        public int KUL_TIP { get; set; }

        [DisplayName("STATUS")]
        public bool STATU { get; set; }

        [DisplayName("Creator User")]
        public int OLUSTURAN_KULLANICI { get; set; }

        [DisplayName("Creation Date")]
        public System.DateTime OLUSTURMA_TARIHI { get; set; }

        [DisplayName("Updater User")]
        public Nullable<int> GUNCELLEYEN_KULLANICI { get; set; }

        [DisplayName("Updated Date")]
        public Nullable<System.DateTime> GUNCELLEME_TARIHI { get; set; }
    }
}