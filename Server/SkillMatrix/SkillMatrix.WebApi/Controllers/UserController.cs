using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkillMatrix.service.Dto;
using SkillMatrix.service.Services;

namespace SkillMatrix.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSkillsList()
        {
            var res = _userService.GetSkillsList();
            return Ok(res.Result);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var res = _userService.Login(dto);
            return Ok(res);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var res = await _userService.Register(dto);
            return Ok(res);
        }
    }
}
