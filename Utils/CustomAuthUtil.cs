        using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TicketingApi.Entities;
using Newtonsoft.Json;


namespace TicketingApi.Utils
{
 
    public class CustomAuthUtil: ICustomAuthUtil
    {
        public bool AuthorizationFreeToken(string Token){
            var tokenPlain = AncDecUtil.DecryptString(Token.Replace("Bearer ", ""), "EPSYLONHOME2021$");
            IList<FreeToken> tokenJson = JsonConvert.DeserializeObject<IList<FreeToken>>(tokenPlain);
            if(Convert.ToDateTime(tokenJson[0].expiredAt) > DateTime.Now){
                return true;
            }
            else{
                return false;
            }
        }
    }
}