using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using api_librarymanagment.Data;
using api_librarymanagment.Models;
using api_librarymanagment.Models.Library;
using api_librarymanagment.Models.Login;
using api_librarymanagment.Models.SignUp;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using api_librarymanagment.Services;
using api_librarymanagment.Models.ServiceEmail;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using NuGet.Common;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using api_librarymanagment.Migrations;

namespace api_librarymanagment.Controllers
{
   
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenlicationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ITokenHandle _tokenHandle;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IConfiguration _configuration;

        public AuthenlicationController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, 
            ApplicationDbContext context, IEmailService emailService, ITokenHandle tokenHandle, IRefreshTokenService refreshTokenService, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _emailService = emailService;
            _tokenHandle = tokenHandle;
            _refreshTokenService = refreshTokenService;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser, [FromQuery] string role)
        {
            // Kiểm tra xem người dùng đã tồn tại dựa trên email đã cung cấp
            var existingUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == registerUser.Email);
            if (existingUser != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                     new Response { Status = "Error", StatusMessage = "User already exists" });
            }

            // Tạo một đối tượng người dùng mới
            var newUser = new ApplicationUser
            {
                Name = registerUser.Name,
                Gender = registerUser.Gender,
                Email = registerUser.Email,
                UserName = registerUser.Email,
                Id = Guid.NewGuid().ToString(),

            };

            if (await _roleManager.RoleExistsAsync(role))   
            {
                // Tạo người dùng và băm mật khẩu        
                var result = await _userManager.CreateAsync(newUser, registerUser.Password);
                if (!result.Succeeded)
                {
                    StatusCode(StatusCodes.Status500InternalServerError,
                                      new Response { Status = "Error", StatusMessage = "User failed to create" });
                }
                //add role to user
                await _userManager.AddToRoleAsync(newUser, role);

                //add token to verify email
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authenlication", new { token, email = newUser.Email }, Request.Scheme);
                var message = new Messages(new string[]
                {
                   newUser.Email!
                }, "Confirm your email", confirmationLink);
                _emailService.SendEmail(message);

                return StatusCode(StatusCodes.Status200OK, new Response
                {
                    Status = "Success",
                    StatusMessage = "User created successfully"
                });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                    new Response { Status = "Error", StatusMessage = "Role does not exist" });
            }
        }

        /* [HttpGet]
         public async Task<IActionResult> TestEmail()
         {
             var message = new Messages(new string[]
             {
                 "htndiep0204@gmail.com"
             }, "Test", "ghdkvjdv");

             emailService.SendEmail(message);
             return StatusCode(StatusCodes.Status200OK, new Response
             {
                 Status = "Success", StatusMessage = "Emaul sent Successfully"
             });
         }*/

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByIdAsync(email);
            if (user != null)
            {   
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if(result.Succeeded)
                {
                    return Ok(new { message = "Email confirmed successfully" });
                }
                else
                {
                    return BadRequest("Email not confirmed");
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new Response
            {
                Status = "Error",
                StatusMessage = "User not found"

            });
          
        }



        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginUser loginUser)
        {
            var existingUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginUser.Email);

            if (existingUser != null && await _userManager.CheckPasswordAsync(existingUser, loginUser.Password))
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, existingUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var userRoles = await _userManager.GetRolesAsync(existingUser);
                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                var jwtToken = _tokenHandle.GetToken(authClaims); // access token
               // var refreshToken = _tokenHandle.GetRefreshToken();
               // var storedRefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken);
               // await _refreshTokenService.SaveRefreshTokenAsync(existingUser.Id, storedRefreshToken);
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    existingUser,
                    userRoles,
                    expession = jwtToken.ValidTo,
                   // refreshToken = storedRefreshToken
                });
            }

            return Unauthorized();
        }
        /*[HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginUser loginUser)
        {
            var existingUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginUser.Email);

            if (existingUser != null && await _userManager.CheckPasswordAsync(existingUser, loginUser.Password))
            {
                var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, existingUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

                var userRoles = await _userManager.GetRolesAsync(existingUser);
                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                var jwtToken = _tokenHandle.GetToken(authClaims);
                var refreshToken = _tokenHandle.GenerateRefreshToken();

                await _refreshTokenService.SaveRefreshTokenAsync(existingUser.Id, refreshToken);

                return Ok(new
                {
                    token = jwtToken,
                    refreshToken = refreshToken
                });
            }

            return Unauthorized();
        }*/

        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.RefreshToken))
            {
                return BadRequest("Invalid client request");
            }

            var principal = _tokenHandle.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            var username = principal.Identity.Name;
            var isValidRefreshToken = await _refreshTokenService.ValidateRefreshTokenAsync(username, request.RefreshToken);
            if (!isValidRefreshToken)
            {
                return BadRequest("Invalid refresh token");
            }

            var authClaims = principal.Claims.ToList();
            var newJwtToken = _tokenHandle.GetToken(authClaims);
            var newRefreshToken =  _tokenHandle.GetRefreshToken();
            var storedRefreshToken = new JwtSecurityTokenHandler().WriteToken(newRefreshToken);

            await _refreshTokenService.SaveRefreshTokenAsync(username, storedRefreshToken);

            return new ObjectResult(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(newJwtToken),
                refreshToken = newRefreshToken,

            });
        }


        [HttpGet]
        public IActionResult GetUserProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var user = _userManager.Users.FirstOrDefault(x => x.UserName == userName);
            return Ok(user);
            
        }
    }
}
