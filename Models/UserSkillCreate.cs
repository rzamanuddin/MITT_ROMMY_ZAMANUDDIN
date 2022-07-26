using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MITT_ROMMY_ZAMANUDDIN.Models
{
    public class UserSkillCreate : Error
    {
        public string username { get; set; }
        public int skillID { get; set; }
        public int skillLevelID { get; set; }
        public string[] TAG { get; set; }
    }
}
