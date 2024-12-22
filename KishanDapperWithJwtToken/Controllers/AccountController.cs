using Microsoft.AspNetCore.Authorization;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using IDPContext_DapperWithJwt;
using DbContext_DapperWithJwt;
using DapperWithJwtBL;

namespace KishanDapperWithJwtToken.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/users")]
    public class AccountController : Controller
    {
        private readonly IAccountDbContext _userDbContext;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AccountController(IAccountDbContext userDbContext, ApplicationDbContext context, IWebHostEnvironment env)
        {
            _userDbContext = userDbContext;
            _context = context;
            _env = env;
        }

        [HttpGet("get-all")]
        public IActionResult GetAllUsers()
        {
            var users = _userDbContext.GetAllUsers();
            return Ok(new ApiResponse("Success", HttpStatusCode.OK, true, users));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginInfo)
        {
            var res = await _userDbContext.Login(loginInfo);
            return Ok(new ApiResponse("Login Successfull", HttpStatusCode.OK, true, res));
        }

        [HttpGet("auth")]
        public async Task<IActionResult> ValidateUser()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing.");
            }
            var res = await _userDbContext.ValidateUser(token);
            return Ok(new ApiResponse("Valid session", HttpStatusCode.OK, true, res));
        }

        [HttpDelete("logout")]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing.");
            }
            var res = await _userDbContext.LogOut(token);
            return Ok(new ApiResponse("Logout Sucessfull", HttpStatusCode.OK, true, res));
        }

    }
}
