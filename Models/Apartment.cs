
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Http;

namespace RentWebService.Models
{
    public class Apartment
    {
        public int Id { get; set; }
  
        public string Address { get; set; }
        
        public string Description { get; set; }

        public int Square { get; set; }
        
        public byte[] Image { get; set; }
    
    }
}