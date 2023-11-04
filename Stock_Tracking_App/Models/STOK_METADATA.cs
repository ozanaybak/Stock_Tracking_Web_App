using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace stockProject.Models
{
    [MetadataType(typeof(STOK_METADATA))]
    public partial class STOK
    {

    }
    public class STOK_METADATA
    {
        public int STOK_ID { get; set; }

        [DisplayName("Stok Adı")]
        [Required(ErrorMessage = " Lütfen stok adı giriniz")]
        [StringLength(50, ErrorMessage = "50 karakterden fazla olamaz")]
        public string STOK_AD { get; set; }

        [DisplayName("Ölçü Birimi")]
        [Required(ErrorMessage = " Lütfen ölçü birimi seçiniz!")]
        public int STOK_OLCUBIRIM { get; set; }

        [DisplayName("Stok Marka")]
        [Required(ErrorMessage = " Lütfen stok marka giriniz!")]
        public string STOK_MARKA { get; set; }

        [DisplayName("Stok Detay")]
        public string STOK_DETAY { get; set; }

        [DisplayName("Kayıt Tarihi")]
        public bool KAYIT_TARIHI { get; set; }

        [DisplayName("Minimum Miktar")]
        public decimal MIN_MIKTAR { get; set; }

        [DisplayName("Statü")]
        public bool STATU { get; set; }

        [DisplayName("Oluşturan Kullanıcı")]
        public int OLUSTURAN_KULLANICI { get; set; }

        [DisplayName("Oluşturma Tarihi")]
        public System.DateTime OLUSTURMA_TARIHI { get; set; }

        [DisplayName("Güncelleyen Kullanıcı")]
        public Nullable<int> GUNCELLEYEN_KULLANICI { get; set; }

        [DisplayName("Güncelleme Tarihi")]
        public Nullable<System.DateTime> GUNCELLEME_TARIHI { get; set; }
    }
}