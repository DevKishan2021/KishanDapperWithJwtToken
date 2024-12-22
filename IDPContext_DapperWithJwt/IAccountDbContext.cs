
using DapperWithJwtBL;

namespace IDPContext_DapperWithJwt
{
    public interface IAccountDbContext
    {
        List<Account> GetAllUsers();
        Task<Account?> Login(LoginDTO loginInfo);
        Task<Account> ValidateUser(string token);
        Task<bool?> LogOut(string token);


    }
}
