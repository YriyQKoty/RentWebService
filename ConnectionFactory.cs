using System.Data;
using System.Data.SQLite;
using Microsoft.Extensions.Configuration;

namespace RentWebService
{
    public class ConnectionFactory : IConnectionFactory
    {
        private IConfiguration _configuration;

        public ConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public IDbConnection CreateConnection()
        {
            return new SQLiteConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        }
    }
}