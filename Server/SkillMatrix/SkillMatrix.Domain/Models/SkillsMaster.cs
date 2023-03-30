using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillMatrix.Domain.Models
{
    public class SkillsMaster
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string SkillName { get; set; }

        [StringLength(500)]
        public string? SkillDescription { get; set;}

        public int? SkillStatus { get; set; }
    }
}
