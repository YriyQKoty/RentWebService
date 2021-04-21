using System.ComponentModel.DataAnnotations;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using RentWebService.DTOs;
using RentWebService.Extensions;
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
        [BindProperty] public ApartmentDTO Dto { get; set; }
        
        public async Task<IActionResult> OnPost()
        {
            if (Dto.FormFile != null)
            {
                
                Apartment.Image = Helper.GetBytesFromFile(Dto.FormFile);
            }
            
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync("insert into apartments (Address, Square,Description, Image) values (@Address, @Square, @Description, @Image)", 
                    new {Apartment.Address, Apartment.Square, Apartment.Description, Apartment.Image});
            }

            return RedirectToPage("Index");
        }
    }
}