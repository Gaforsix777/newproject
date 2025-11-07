// Controllers/UsersController.cs
using Microsoft.AspNetCore.Mvc;
using WebAPI1.Models.DTOs;
using WebAPI1.Services;

namespace WebAPI1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        public UsersController(UserService userService) => _userService = userService;

        // GET /api/Users/obtener
        [HttpGet("obtener")]
        public async Task<IActionResult> Obtener()
        {
            var users = await _userService.GetAllUserAsync();
            return Ok(users);
        }

        // POST /api/Users/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            var user = await _userService.RegisterUserAsync(dto);
            return CreatedAtAction(nameof(Obtener), new { id = user.Id }, new { message = "Usuario creado", user.Id, user.UserName });
        }

        // PUT /api/Users/update
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserDto dto)
        {
            var user = await _userService.UpdateUserAsync(dto);
            return Ok(new { message = "Usuario actualizado", user.Id, user.UserName });
        }

        // PUT /api/Users/updateemail
        [HttpPut("updateemail")]
        public async Task<IActionResult> UpdateEmail([FromBody] UpdateEmailDto dto)
        {
            var user = await _userService.UpdateEmailAsync(dto);
            return Ok(new { message = "Email actualizado", user.Id, user.Email });
        }

        // DELETE /api/Users/delete
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteUserDto dto)
        {
            await _userService.DeleteUserAsync(dto);
            return Ok(new { message = "Usuario eliminado", dto.Id });
        }
    }
}
