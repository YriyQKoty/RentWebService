using System.Data;
using Microsoft.AspNetCore.Connections.Features;

namespace RentWebService
{
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}