using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace stockProject.Models
{
    [MetadataType(typeof(HAREKET_TIP_METADATA))]
    public partial class HAREKET_TIP
    {

    }
    public class HAREKET_TIP_METADATA
    {
        public int HAREKET_TIP_ID { get; set; }

        [DisplayName("Transfer Type Name")]
        public string HAREKET_TIP_ADI { get; set; }
        [DisplayName("Process Indicator")]
        public bool ISLEM_GOSTERGESI { get; set; }
        [DisplayName("Status")]
        public bool STATU { get; set; }
    }
}