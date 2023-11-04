using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace stockProject.Models
{
    [MetadataType(typeof(KULLANICI_TIP_METADATA))]
    public partial class KULLANICI_TIP
    {

    }
    public class KULLANICI_TIP_METADATA
    {
        public int KULTIP_ID { get; set; }


        [DisplayName("Kullanıcı Tip Adı")]
        public string KULTIP_ADI { get; set; }

        [DisplayName("Statü")]
        public bool STATU { get; set; }
    }
}