using BridgetSandalsAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BridgetSandalsAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration config;

        public AuthService(UserManager<IdentityUser> userManager, IConfiguration config)
        {
            this.userManager = userManager;
            this.config = config;
        }

        public async Task<bool> RegisterUser(User user)
        {
            var identityUser = new IdentityUser
            {
                UserName = user.Username,
                Email = user.Username
            };

            //Creates this user with the password so it can do hasing on it
            var result = await userManager.CreateAsync(identityUser, user.Password);

            return result.Succeeded;
        }

        public async Task<bool> Login(User user)
        {
            var identityUser = await userManager.FindByEmailAsync(user.Username);
            if (identityUser == null)
            {
                return false;
            }

            //Checks the password that is stored for the user
            return await userManager.CheckPasswordAsync(identityUser, user.Password);
        }

        public string GenerateToken(User user)
        {
            //Claims are pieces of information that can be included along with the token
            //such as the username and the Admin which is the role
            IEnumerable<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            //Used to create the signing credientials for the token and it helps to ensure
            //that the token is not hampered with
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWTConfig:Key").Value));

            var signingCreds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    issuer: config.GetSection("JWTConfig:Issuer").Value,
                    audience: config.GetSection("JWTConfig:Audience").Value,
                    signingCredentials: signingCreds
                );

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }
    }
}
