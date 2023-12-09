using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SaminExamination.Context;
using SaminExamination.Dto;
using SaminExamination.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SaminExamination.Controllers
{
    [ApiController]
    [Route("/api/[Controller]")]
    public class AuthController:Controller
    {
        public DataContext _context;
        public IMapper _mapper;
        public  IConfiguration _configuration;


        public AuthController(DataContext context , IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
            _context = context;
            _configuration = configuration;
        }
        [HttpPost("AddedUser")]
        [ProducesResponseType(200)]
        public IActionResult Register([FromBody] UserDto requestUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            var user = _mapper.Map<User>(requestUser);
            _context.Add(user);
            _context.SaveChanges();
            return Ok("عملیات با موفقیت انجام شد");
        }
        [HttpPost("login")]
        public IActionResult login([FromBody] UserDto Loginrequest) {
            var requestLogin = _mapper.Map<User>(Loginrequest);
            var user = _context.Users.Where(u => u.userName == requestLogin.userName && u.password == requestLogin.password).FirstOrDefault();
            if(user == null)
            {
                return NotFound("هیچ کاربری با این مشخصات یافت نشد");
            }
            else
            {
                string token = GenerateToken(user);
                return Ok(token);
            }
        }
        [HttpGet("Users")]
        public IActionResult GetUsers() { 
            var users = _context.Users.ToList();
            return Ok(users);
        }
        private string createToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, user.Role)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value
                ));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.userName),
                new Claim(ClaimTypes.Role,user.Role)
            };
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
