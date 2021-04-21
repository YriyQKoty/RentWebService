using System;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentWebService.Models;

namespace RentWebService.Pages
{
    public class Edit : PageModel
    {
        private IConnectionFactory _connectionFactory;

        public Edit(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        [BindProperty] public Apartment Apartment { get; set; }

        public void OnGet(int apartmentId)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                Apartment = connection.QuerySingleOrDefault<Apartment>("select * from apartments where id = @apartment_id",
                    new {apartment_id = apartmentId});
                if ( Apartment == null)
                {
                    throw new Exception("Such model was not found!");
                }
            }
        }

        public async Task<IActionResult> OnPost()
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                var book = connection.QuerySingleOrDefault<Apartment>("select * from apartments where id = @apartment_id",
                    new {apartment_id = Apartment.Id});
                if (book == null)
                {
                    return BadRequest();
                }

                await connection.ExecuteAsync("update apartments set Square = @Square, Address = @Address, Description = @Description where id = @Id", new
                {
                    Apartment.Square,
                    Apartment.Address,
                    Apartment.Description,
                    Apartment.Id
                });
            }

            return RedirectToPage("Index");
        }
    }
}