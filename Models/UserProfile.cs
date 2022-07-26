using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MITT_ROMMY_ZAMANUDDIN.Models
{
    public class UserProfile
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string address { get; set; }
        [Required]
        public DateTime birthdate { get; set; }
        [Required]
        public string email { get; set; }

        public int ERRORCODE { get; set; }
        public string ERRORDESC { get; set; }
        public string[] TAG { get; set; }
    }
}
