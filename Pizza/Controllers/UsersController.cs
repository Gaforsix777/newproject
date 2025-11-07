using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Pizza.Models.DTTOs;
using Pizza.Services;


namespace Pizza.Controllers
{

    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class UsersController: ControllerBase
    {
        private readonly UserService _userService;
        public UsersController(UserService userService)
        {
            _userService = userService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegiserUserDto dto)
        {
            throw new NotImplementedException();
            try
            {
                var user = await _userService.RegisterUserAsync(dto);
                return Ok(new { message = "Usurair creado", user.Id, user.UserName });

            }
            catch (Exception ex)
            {
                return BadRequest(new {error=ex.Message});
            }
        }
    }
}
