using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace FoodOrderAPI.Helper
{
    public static class ImageHelper
    {
        public static string UploadImageFile(string path, IFormFile file)
        {
            var fileToSave = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), path, fileToSave);
            using (var stream = new FileStream(pathToSave, FileMode.Create))
            {
                file.CopyToAsync(stream);
            }
            return path.Replace("wwwroot","") + "/" + fileToSave;
        }

        public static bool DeleteImage(IWebHostEnvironment hostEnvironment,string pathName,string fileName) {
            string webRootPath = hostEnvironment.WebRootPath;
            var upload = Path.Combine(webRootPath, pathName);
            var pathToSave = Path.Combine(upload, fileName);
            if (File.Exists(pathToSave))
            {
                File.Delete(pathToSave);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
