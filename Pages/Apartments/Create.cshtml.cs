using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        
        public IEnumerable<Models.Owner> Owners { get; private set; }

        public void OnGet()
        {
            if (Owners != null)
            {
                return;
            }
            
            using (var connection = _connectionFactory.CreateConnection())
            {
                Owners = connection.Query<Models.Owner>("select * from owner");
            }
        }
        
        public async Task<IActionResult> OnPost()
        {
            if (Dto.FormFile != null)
            {
                Apartment.Image = Helper.GetBytesFromFile(Dto.FormFile);
            }
            else
            {
                await using (var stream = System.IO.File.OpenRead("wwwroot/images/empty.png"))
                {
                    var file = new FormFile(stream, 0, stream.Length, String.Empty, Path.GetFileName(stream.Name))
                    {
                        Headers = new HeaderDictionary(),
                        ContentType = "multipart/form-data"
                    };
                    
                    Apartment.Image = Helper.GetBytesFromFile(file);
                }
               
            }
            
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync("insert into apartments (Address, Square, OwnerId, Description, Image) values (@Address, @Square, @OwnerId, @Description, @Image)", 
                        new {Apartment.Address, Apartment.Square, Apartment.OwnerId, Apartment.Description, Apartment.Image});
            }

            return RedirectToPage("Index");
        }
    }
}