using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace stockProject.Models
{
    public class LogIn
    {
        [Required(ErrorMessage = "Please enter an Username")]
        public string USERNAME { get; set; }
        [Required(ErrorMessage = "Please enter a Pasword")]
        public string PASSWORD { get; set; }
    }
}