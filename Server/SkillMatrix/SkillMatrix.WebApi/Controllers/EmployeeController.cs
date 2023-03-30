using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkillMatrix.service.Dto;
using SkillMatrix.service.Services;
using System.Security.Claims;

namespace SkillMatrix.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly SkillServices _skillServices;

        public EmployeeController(SkillServices skillServices)
        {
            _skillServices = skillServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserResult()
        {
            int id = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var res = _skillServices.GetUserSkills(id);
            return Ok(res);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateSkills(SkillUpdateDto dto)
        {
            int id = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var res = await _skillServices.UpdateSkills(id , dto);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> AddSkill(AddSkillsDto dto)
        {
            var res = await _skillServices.AddSkill(dto);
            return Ok(res);
        }
    }
}
