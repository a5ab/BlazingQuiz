using BlazingQuiz.Api.Data;
using BlazingQuiz.Api.Data.Entities;
using BlazingQuiz.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using System.Text;

namespace BlazingQuiz.Api.Services
{
    public class AuthService
    {
        private readonly QuizContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration configuration;

        public AuthService(QuizContext context, IPasswordHasher<User> passwordHasher,IConfiguration configuration)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            this.configuration = configuration;
        }



        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user =await _context.Users.FirstOrDefaultAsync(u=>u.Email== dto.UserName);

            if (user == null)
            {
                return new AuthResponseDto(default,ErrorMessage: "Invalid UserName or Password");
            }

            //Get the hashed password and compare it with the password in the dto
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return new AuthResponseDto(default, ErrorMessage: "Invalid UserName or Password");

            }

            //Generate a token
            var token = GenerateToken(user, configuration);


            return new AuthResponseDto(token);

            

        }




        private static string GenerateToken(User user, IConfiguration configuration)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim(ClaimTypes.Email,user.Email),
            new Claim(ClaimTypes.Role,user.Role)
        };

            var secret = configuration.GetValue<string>("JWT:secret");
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secret));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtsecuritytoken = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("JWT:issuer"),
                audience: configuration.GetValue<string>("JWT:audience"),
                claims: claims,
                expires: DateTime.Now.AddMinutes(configuration.GetValue<int>("JWT:ExpirationInMinutes")),
                signingCredentials: creds
            );
            var token = new JwtSecurityTokenHandler().WriteToken(jwtsecuritytoken);
            return token;
        }

    }


}
