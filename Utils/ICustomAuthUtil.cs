        using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace TicketingApi.Utils
{
    public interface ICustomAuthUtil
    {
            bool AuthorizationFreeToken(string token);
    }
}