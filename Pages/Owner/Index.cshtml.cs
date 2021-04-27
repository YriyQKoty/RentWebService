using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentWebService.Models;

namespace RentWebService.Pages.Owners
{
    public class Index : PageModel
    {
        private IConnectionFactory _connectionFactory;

        public Index(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }


        public Models.Owner Owner { get; private set; }

        public void OnGet(int ownerId)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                Owner = connection.QuerySingleOrDefault<Models.Owner>("select * from owner where id = @ownerId", new {ownerId});
                if ( Owner == null)
                {
                    throw new Exception("Model with such ID was not found!");
                }
            }
        }
    }
}