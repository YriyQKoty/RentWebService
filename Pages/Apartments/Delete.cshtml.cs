using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentWebService.Models;

namespace RentWebService.Pages
{
    public class Delete : PageModel
    {
        private IConnectionFactory _connectionFactory;

        public Delete(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IActionResult> OnGet(int apartmentId)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                var apartment = connection.QuerySingleOrDefault<Apartment>("select * from apartments where id = @apartment_id",
                    new {apartment_id = apartmentId});
                if (apartment == null)
                {
                    return BadRequest();
                }

                await connection.ExecuteAsync("delete from apartments where id = @apartment_id", new {apartment_id = apartmentId});
            }

            return RedirectToPage("Index");
        }
    }
}