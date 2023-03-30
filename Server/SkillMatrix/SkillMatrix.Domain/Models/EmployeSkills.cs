using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillMatrix.Domain.Models
{
    public class EmployeSkills
    {
        public int id { get; set; }

        public int UsersId { get; set;}

        public Users Users { get; set; }

        public int SkillsMasterId { get; set; }

        public SkillsMaster SkillsMaster { get; set; }

        [StringLength(50)]
        public int SkillType { get; set; }

        public int SkillRating { get; set; }
    }
}
