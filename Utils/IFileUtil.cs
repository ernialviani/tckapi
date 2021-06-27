using Microsoft.AspNetCore.Http;

namespace TicketingApi.Utils
{
    public interface IFileUtil
    {
        string AvatarUpload(IFormFile file, string Dir);

        bool Remove(string uploadDirectory);
    }
}