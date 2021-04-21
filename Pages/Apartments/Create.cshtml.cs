using System.ComponentModel.DataAnnotations;
using System.Data.SQLite;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using RentWebService.Models;

namespace RentWebService.Pages
{
    public class Create : PageModel
    {
        private IConnectionFactory _connectionFactory;

        public Create(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        [BindProperty] public Apartment Apartment { get; set; }

        public async Task<IActionResult> OnPost()
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync("insert into apartments (Address, Square,Description) values (@Address, @Square, @Description)", 
                    new {Apartment.Address, Apartment.Square, Apartment.Description});
            }

            return RedirectToPage("Index");
        }
    }
}