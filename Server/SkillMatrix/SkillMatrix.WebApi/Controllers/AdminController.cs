using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SkillMatrix.service.Dto;
using SkillMatrix.service.Services;

namespace SkillMatrix.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly SkillServices _skillServices;

        public AdminController(UserService userService,
                                SkillServices skillServices)
        {
            _userService = userService;
            _skillServices = skillServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetSkillsListForApproval()
        {
            var res = _userService.GetSkillsListForApprove();
            return Ok(res.Result);
        }

        [HttpPatch]
        public async Task<IActionResult> ApproveSkill(int id)
        {
            var res = _skillServices.ApproveSkill(id);
            return Ok(res);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSkill(int id)
        {
            var res = _skillServices.DeleteSkill(id);
            return Ok(res);
        }

        [HttpDelete("DeleteAdditionalSkill")]
        public async Task<IActionResult> DeleteAdditionalSkill(int id)
        {
            var res = _userService.DeleteAdditionalSkill(id);
            return Ok(res);
        }

        [HttpDelete("Approved")]
        public async Task<IActionResult> DeleteApprovedSkill(int id)
        {
            var res = _skillServices.DeleteApproveSkill(id);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> AddSkillByAdmin(AddSkillsDto dto)
        {
            var res = await _skillServices.AddSkillByAdmin(dto);
            return Ok(res);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSkillByAdmin(int id, AddSkillsDto dto)
        {
            var res = _userService.UpdateSkillByAdmin(id, dto);            
            return Ok(res);
            
        }

        [HttpGet("GetSkillById")]
        public async Task<IActionResult> GetSkillById(int id)
        {
            var res = _userService.GetSkillById(id);
            return Ok(res);

        }
    }
}
