using Microsoft.AspNetCore.Http;

namespace TicketingApi.Utils
{
    public interface IFileUtil
    {
        string Upload(IFormFile file, string uploadDirectory);
    }
}