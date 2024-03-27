using Factories.WebApi.BLL.Models;
using Factories.WebApi.BLL.Services;
using Factories.WebApi.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Factories.WebApi.BLL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(UserService userService, JwtService jwtService, UserManager<IdentityUser> userManager) : ControllerBase
    {
        private readonly UserService userService = userService ?? throw new ArgumentNullException(nameof(userService));
        private readonly JwtService jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        private readonly UserManager<IdentityUser> userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // Моя реализация аутентификации, в учебных целях
             var user = await userService.AuthenticateAsync(model.Login, model.Password);
           // var user = await userManager.FindByNameAsync(model.Login);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = jwtService.GenerateJwtToken(user);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }

        [HttpGet("current")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
            var currentUser = await userManager.GetUserAsync(User);

            if (currentUser == null)
                return NotFound();
            return Ok(new
            {
                currentUser.Id,
                currentUser.UserName,
                currentUser.Email,
            });
        }

        [HttpPost("password/update")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordModel model)
        {
            throw new NotImplementedException();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginModel model)
        {
            var result = await userService.RegisterAsync(model.Login, model.Password);

            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}