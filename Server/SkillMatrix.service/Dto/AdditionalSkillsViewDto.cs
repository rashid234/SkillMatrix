using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillMatrix.service.Dto
{
    public class AdditionalSkillsViewDto
    {
        public int? SelectSkillId { get; set; }

        public int? SelectSkill { get; set; }

        public int? SelectRating { get; set; }

        public string? SkillName { get; set; }
    }
}
