using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentWebService.DTOs;
using RentWebService.Extensions;
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
        [BindProperty] public ApartmentDTO Dto { get; set; }
        
        public IEnumerable<Models.Owner> Owners { get; private set; }
        
        public void OnGet(int apartmentId)
        {
            if (Owners == null) {
                using (var connection = _connectionFactory.CreateConnection())
                {
                    Owners = connection.Query<Models.Owner>("select * from owner");
                }
            }
            
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
                var apartment = connection.QuerySingleOrDefault<Apartment>("select * from apartments where id = @apartment_id",
                    new {apartment_id = Apartment.Id});
                if (apartment == null)
                {
                    return BadRequest();
                }
                
                if (Dto.FormFile != null)
                {
                    Apartment.Image = Helper.GetBytesFromFile(Dto.FormFile);
                    
                    await connection.ExecuteAsync("update apartments set Square = @Square, OwnerId = @OwnerId, Address = @Address, Image = @Image, Description = @Description where id = @Id", new
                    {
                        Apartment.Square,
                        Apartment.OwnerId,
                        Apartment.Address,
                        Apartment.Image,
                        Apartment.Description,
                        Apartment.Id
                    });
                }
                else
                {
                    await connection.ExecuteAsync("update apartments set Square = @Square, OwnerId = @OwnerId, Address = @Address, Description = @Description where id = @Id", new
                    {
                        Apartment.Square,
                        Apartment.OwnerId,
                        Apartment.Address,
                        Apartment.Description,
                        Apartment.Id
                    });
                }

                
            }

            return RedirectToPage("Index");
        }
    }
}