using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using TicketingApi.Models.v1.Misc;


namespace TicketingApi.Utils
{

    public class FileUtil: IFileUtil
    {
        private readonly IWebHostEnvironment env;
        public FileUtil(IWebHostEnvironment env)
        {
            this.env = env;
        }
        public string AvatarUpload(IFormFile file, string Dir)
        {
            // var fileSplit = file.FileName.Split("/");
            // var endPath = fileSplit[0];
            // var fileNameOnly = fileSplit[1];
            var uploadPath = Path.Combine(env.ContentRootPath, "Medias/"+Dir);
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

        public Media FileUpload(IFormFile file, string Dir){
            var uploadPath = Path.Combine(env.ContentRootPath, "Medias/"+Dir);
            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            var filePath = Path.Combine(uploadPath, fileName);
            using (var image = Image.Load(file.OpenReadStream())){
                image.Save(filePath);
            }
            return new Media {
                FileName=fileName,
                FileType=System.IO.Path.GetExtension(file.FileName),
            };

        }

        public bool Remove(string uploadDirectory){
            var uploadPath = Path.Combine(env.ContentRootPath, "Medias/"+uploadDirectory);
          //  var filePath = Path.Combine(uploadPath, fileName);
            if (File.Exists(uploadPath)){
                File.Delete(uploadPath);
            }
            return (!File.Exists(uploadPath));
        }
    }
}