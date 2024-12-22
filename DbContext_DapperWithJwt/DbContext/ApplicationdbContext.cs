using DapperWithJwtBL;
using Microsoft.EntityFrameworkCore;

namespace DbContext_DapperWithJwt
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Account> Users { get; set; }

    }
}
