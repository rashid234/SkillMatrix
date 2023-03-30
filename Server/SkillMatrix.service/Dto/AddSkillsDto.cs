using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillMatrix.service.Dto
{
    public class AddSkillsDto
    {
        public string SkillName { get; set; }

        public string? SkillDescription { get; set; }

    }
}
