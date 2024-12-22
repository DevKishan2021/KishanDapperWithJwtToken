using DapperWithJwtBL;
using System.Security.Claims;

namespace IDPContext_DapperWithJwt
{
    public interface ICommonDbcontext
    {
        public string GenerateJwtToken(Account user, TimeSpan duration);
        public string GenerateCode(string requestType);
        public ClaimsPrincipal ValidateToken(string token);
        public Dictionary<string, string> ReadToken(string token);


        //public Task<GoogleUser> AuthenticateGoogletoken(string token);
        //public Task<bool?> SendMail(SendMailInfo mailInfo);
        //public Task<Emails> GetEmailById(int id, string? stringVals = null);
        //public Task<string> UploadFile(string rootPath, IFormFile file, string folderName = "mis");
    }
}