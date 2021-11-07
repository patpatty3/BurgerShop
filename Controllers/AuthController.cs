using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BurgerShop.Models;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using System;
using Microsoft.Extensions.Configuration;

namespace BurgerShop.Controllers {
    public class AuthController: ControllerBase {
        private UserManager<IdentityUser> userManager;
        private IConfiguration configuration;

        public AuthController(UserManager<IdentityUser> userMgr, IConfiguration config) {
            userManager = userMgr;
            configuration = config;
        }

        [HttpPost]
        [Route("api/register")]
        public async Task<IActionResult> Register([FromBody]LoginRegisterViewModel model)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            IdentityUser user = new IdentityUser{UserName = model.Email, Email = model.Email};
            IdentityResult result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded) {
                return Ok();
            }
            foreach (IdentityError err in result.Errors) {
                ModelState.AddModelError("", err.Description);
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("api/login")]
        public async Task<IActionResult> Login([FromBody]LoginRegisterViewModel model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            IdentityUser user = await userManager.FindByEmailAsync(model.Email);
            bool succeeded = await userManager.CheckPasswordAsync(user, model.Password);
            if (!succeeded) {
                return Unauthorized();
            }
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            byte[] secret = Encoding.ASCII.GetBytes(configuration["jwtSecret"]);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Email, model.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secret),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            SecurityToken token = handler.CreateToken(descriptor);
            return Ok(new {success = true, token = handler.WriteToken(token)});
        }
    }
}
