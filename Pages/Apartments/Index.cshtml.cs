using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using RentWebService.Models;

namespace RentWebService.Pages.Apartments
{
    public class Index : PageModel
    {
        private IConnectionFactory _connectionFactory;

        public Index(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }


        public IEnumerable<Apartment> Apartments { get; private set; }
        
        public async Task OnGetAsync()
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                Apartments = await connection.QueryAsync<Apartment>("select * from apartments");
            }
        }
        
    }
}