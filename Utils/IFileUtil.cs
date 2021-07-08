using Microsoft.AspNetCore.Http;
using TicketingApi.Models.v1.Misc;

namespace TicketingApi.Utils
{
    public interface IFileUtil
    {
        string AvatarUpload(IFormFile file, string Dir);

        bool Remove(string uploadDirectory);

        Media FileUpload(IFormFile file, string Dir);
    }
}