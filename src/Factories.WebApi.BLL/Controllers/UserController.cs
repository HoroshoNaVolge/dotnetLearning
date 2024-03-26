using Factories.WebApi.BLL.Authentification;
using Factories.WebApi.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    public class UserController(UserManager<User> userManager, SignInManager<User> signInManager) : ControllerBase
    {
        [HttpPost("auth")]
        [AllowAnonymous]
        //public async Task<IActionResult> Authenticate([FromBody] LoginModel model)
        //{
           
        //}

        [HttpGet("current")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUserInfo()
            {
                var user = await userManager.GetUserAsync(User);

                if (user == null)
                    return NotFound();

                return Ok(user);
            }

            [HttpPost("password/update")]
            [Authorize]
            public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordModel model)
            {
                var user = await userManager.FindByNameAsync(model.Login);

                if (user == null)
                    return NotFound();

                var result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                if (!result.Succeeded)
                    return BadRequest("Failed to update password.");

                return Ok("Password updated successfully.");
            }
        }
    }