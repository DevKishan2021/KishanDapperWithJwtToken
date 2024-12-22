using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DbContext_DapperWithJwt.Dapper
{
    public class DapperService : IDapperService
    {
        private readonly ApplicationDbContext _db;
        private static string connetionString = "";
        public DapperService(ApplicationDbContext db)
        {
            _db = db;
            connetionString = db.Database.GetDbConnection().ConnectionString;
        }


        public void Dispose()
        {
            _db.Dispose();
        }

        public int ExecuteScaler<T>(string sp, DynamicParameters dynamicParameters, CommandType commandType = CommandType.StoredProcedure)
        {
            using (SqlConnection sqlCon = new SqlConnection(connetionString))
            {
                sqlCon.Open();
                return sqlCon.Execute(sp, dynamicParameters, commandType: commandType);
            }
        }

        public T Get<T>(string sp, DynamicParameters dynamicParameters, CommandType commandType = CommandType.StoredProcedure)
        {
            using (SqlConnection sqlCon = new SqlConnection(connetionString))
            {
                sqlCon.Open();
                return sqlCon.Query<T>(sp, dynamicParameters, commandType: commandType).FirstOrDefault();
            }
        }

        public List<T> GetAll<T>(string sp, DynamicParameters dynamicParameters, CommandType commandType = CommandType.StoredProcedure)
        {
            using (SqlConnection sqlCon = new SqlConnection(connetionString))
            {
                sqlCon.Open();
                return sqlCon.Query<T>(sp, dynamicParameters, commandType: commandType).ToList();
            }
        }
    }
}
