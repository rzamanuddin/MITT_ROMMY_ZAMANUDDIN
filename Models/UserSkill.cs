using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MITT_ROMMY_ZAMANUDDIN.Models
{
    public class UserSkill
    { 
    //{"UserSkillID": 5,
    //  "Username": "rommy",
    //  "SkillID": 1,
    //  "SkillName": "MATEMATIKA",
    //  "SkillLevelID": 2,
    //  "SkillLevelName": "JUNIOR"
        public int UserSkillID { get; set; }
        public string Usesrname { get; set; }
        public int SkillID { get; set; }
        public string SkillName { get; set; }
        public int SkillLevelID { get; set; }
        public string SkillLevelName { get; set; }
    }
}
