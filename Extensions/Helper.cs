using System.IO;
using Microsoft.AspNetCore.Http;

namespace RentWebService.Extensions
{
    public class Helper
    {
        public static byte[] GetBytesFromFile(IFormFile file)
        {
            byte[] imageData = null;
            // read file into array
            using (var binaryReader = new BinaryReader(file.OpenReadStream()))
            {
                imageData = binaryReader.ReadBytes((int)file.Length);
            }

            return imageData;
        }
    }
}