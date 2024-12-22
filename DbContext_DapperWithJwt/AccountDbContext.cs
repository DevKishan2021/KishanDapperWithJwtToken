using System.Security.Claims;
using Dapper;
using DapperWithJwtBL;
using DbContext_DapperWithJwt.Dapper;
using IDPContext_DapperWithJwt;
using Microsoft.EntityFrameworkCore;

namespace DbContext_DapperWithJwt
{
    public class AccountDbContext : IAccountDbContext
    {

        //private readonly IOtpDbcontext _otp;
        private readonly ICommonDbcontext _comm;
        private readonly ApplicationDbContext _context;
        private readonly IDapperService _dapper;

        public AccountDbContext(ICommonDbcontext comm, ApplicationDbContext context, IDapperService dapper)
        {
            _comm = comm;
            _context = context;
            _dapper = dapper;
        }

        public List<Account> GetAllUsers()
        {
            DynamicParameters parameters = new DynamicParameters();
            List<Account> users = _dapper.GetAll<Account>("getAllUsers", parameters);
            return users;
        }

        public async Task<Account?> Login(LoginDTO loginInfo)
        {
            var user = await _context.Users.Where(us => us.Email == loginInfo.Email).FirstOrDefaultAsync();
            if (user == null) throw new Exception("Look's Like you are not registered with us");
            var isPasswordmatched = user.VerifyPassword(loginInfo.Password);
            if (!isPasswordmatched) throw new Exception("Invaild Password");
            var token = _comm.GenerateJwtToken(user, TimeSpan.FromDays(30));
            user.Token = token;
            var activity = new UserActivity
            {
                Username = user.Username,
                Type = "Login",
                SessionToken = token,
                Details = "Login successfull",
                EventSource = "web",
                ActivityTime = DateTime.Now
            };

            //await _context.UserActivity.AddAsync(activity);
            await _context.SaveChangesAsync();
            return user;
        }
        
        public async Task<Account> ValidateUser(string token)
        {
            var verifyToken = _comm.ValidateToken(token);
            var claims = _comm.ReadToken(token);
            string userId = claims["userId"] ?? throw new Exception("Invalid Token");
            var user = _context.Users.Find(userId) ?? throw new Exception("Unauthorized access");
            var newToken = _comm.GenerateJwtToken(user, TimeSpan.FromDays(3));
            user.Token = newToken;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool?> LogOut(string token)
        {
            var claims = _comm.ReadToken(token);
            string email = claims[ClaimTypes.Email];
            if (email == null) throw new Exception("Invalid Token");

            var user = _context.Users.Where(us => us.Email == email).FirstOrDefault();
            if (user == null) throw new Exception("Unauthorized access");
            user.Token = null;

            var activity = new UserActivity
            {
                Username = user.Username,
                Type = "Logout",
                SessionToken = null,
                Details = "Logout successfull",
                EventSource = "web",
                ActivityTime = DateTime.Now
            };

            //await _context.UserActivity.AddAsync(activity);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
