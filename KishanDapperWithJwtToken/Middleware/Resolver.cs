

using DbContext_DapperWithJwt;
using DbContext_DapperWithJwt.Dapper;
using IDPContext_DapperWithJwt;

namespace SetuMantra.Middleware
{
    public static class Resolver
    {
        public static void AddResolvers(this IServiceCollection services)
        {
            services.AddScoped<IDapperService, DapperService>();
            services.AddScoped<ICommonDbcontext, CommonDbContext>();
            services.AddScoped<IAccountDbContext, AccountDbContext>();
          
        }
    }
}
