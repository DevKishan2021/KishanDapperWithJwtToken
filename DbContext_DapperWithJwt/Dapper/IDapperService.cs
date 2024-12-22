using Dapper;
using System.Data;

namespace DbContext_DapperWithJwt.Dapper
{
    public interface IDapperService : IDisposable
    {
        T Get<T>(string sp, DynamicParameters dynamicParameters, CommandType commandType = CommandType.StoredProcedure);
        List<T> GetAll<T>(string sp, DynamicParameters dynamicParameters, CommandType commandType = CommandType.StoredProcedure);
        int ExecuteScaler<T>(string sp, DynamicParameters dynamicParameters, CommandType commandType = CommandType.StoredProcedure);
    }
}
