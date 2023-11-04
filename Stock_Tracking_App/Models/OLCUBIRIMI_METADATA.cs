using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace stockProject.Models
{
    [MetadataType(typeof(OLCUBIRIMI_METADATA))]
    public partial class OLCU_BIRIMI
    {

    }
    public class OLCUBIRIMI_METADATA
    {
        public int OLCUBIRIM_ID { get; set; }

        [DisplayName("Ölçü Birimi")]
        public string OLCUBIRIM_ADI { get; set; }
    }
}