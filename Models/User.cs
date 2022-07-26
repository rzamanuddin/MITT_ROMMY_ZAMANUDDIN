using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MITT_ROMMY_ZAMANUDDIN.Models
{
    public class User
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
        public string tokenType { get; set; }
        public string token { get; set; }
        public string message { get; set; }
    }
}
