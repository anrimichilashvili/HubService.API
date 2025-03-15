using HubService.Application.Dtos;
using HubService.Application.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HubService.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<ResultDTO> RegisterUser(UserModelDto user)
        {
            var idemtityUser = new IdentityUser
            {
                UserName = user.UserName,
                Email = user.UserName + "@gmail.com"
            };

            var result = await _userManager.CreateAsync(idemtityUser, user.Password);
            if(result.Succeeded)
            return new ResultDTO { Success=true, Data = idemtityUser };
            else return new ResultDTO { Success = false, Data = result.Errors };
        }

        public async Task<ResultDTO> Login(UserModelDto user)
        {
            var identityUser = await _userManager.FindByNameAsync(user.UserName);
            if (identityUser == null) { return new ResultDTO { Success=false}; }
            var result =  await _userManager.CheckPasswordAsync(identityUser, user.Password);
            return new ResultDTO { Success=true,Data= identityUser };
        }

        public string GenerateToeknString(UserModelDto user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Role, "Admin"),
            };


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection
                ("Jwt:Key").Value));



            SigningCredentials signingCredentials = new SigningCredentials(securityKey,
                SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                issuer: _configuration.GetSection("Jwt:Issuer").Value,
                audience: _configuration.GetSection("Jwt:Audience").Value,
                signingCredentials: signingCredentials
                );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return tokenString;
        }
    }
}
