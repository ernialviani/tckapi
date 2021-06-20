using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;


namespace TicketingApi.Utils
{

    public class FileUtil: IFileUtil
    {
        private readonly IWebHostEnvironment env;
        public FileUtil(IWebHostEnvironment env)
        {
            this.env = env;
        }
        public string Upload(IFormFile file, string uploadDirectory)
        {
            var uploadPath = Path.Combine(env.ContentRootPath, uploadDirectory);
            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadPath, fileName);
            using (var image = Image.Load(file.OpenReadStream())){
                 image.Mutate(x => x
                     .Resize(200, 200)
                     .Grayscale());
                image.Save(filePath);
            }
            return fileName;
        }
    }
}