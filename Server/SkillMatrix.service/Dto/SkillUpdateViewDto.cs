using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillMatrix.service.Dto
{
    public class SkillUpdateViewDto
    {
        public int? PrimarySkill { get; set; }

        public int? PrimarySkillId { get; set; }

        public int? PrimaryRating { get; set; }

        public int? SecondarySkill { get; set; }

        public int? SecondarySkillId { get; set; }

        public int? SecondaryRating { get; set; }

        public List<AdditionalSkillsViewDto>? AdditionalSkills { get; set; }
    }
}
