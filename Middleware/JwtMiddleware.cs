using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketingApi.Models;
using TicketingApi.Entities;
using TicketingApi.DBContexts;
using Microsoft.EntityFrameworkCore;
using TicketingApi.Utils;
using Newtonsoft.Json;
using System.Security.Claims;

namespace TicketingApi.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;
        //private readonly AppDBContext  _context;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, AppDBContext dataContext)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await attachAccountToContext(context, dataContext, token);

            await _next(context);
        }

        private async Task attachAccountToContext(HttpContext context, AppDBContext dataContext, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var email = jwtToken.Claims.First(x => x.Type == ClaimTypes.Email).Value;

                // attach account to context on successful jwt validation
                var user = await dataContext.Users.Where(w => w.Email == email).AsNoTracking().FirstOrDefaultAsync();
                if(user != null){
                 context.Items["TokenType"] = "loged-user";
                 context.Items["Email"] = user.Email;
                }
                else{
                 var sender = await dataContext.Senders.Where(w => w.Email == email).AsNoTracking().FirstOrDefaultAsync();
                 context.Items["TokenType"] = "loged-sender";
                 context.Items["Email"] = sender.Email; 
                }
            }
            catch(Exception e)
            {
                try
                {
                    var tokenPlain = AncDecUtil.DecryptString(token.Replace("Bearer ", ""), "EPSYLONHOME2021$");
                    IList<FreeToken> tokenJson = JsonConvert.DeserializeObject<IList<FreeToken>>(tokenPlain);
                    if(tokenJson != null){
                        if(Convert.ToDateTime(tokenJson[0].expiredAt) > DateTime.Now && tokenJson[0].key == "epsylon$"){
                            context.Items["TokenType"] = "free-token";
                            context.Items["FreeToken"] = token;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                     // do nothing if jwt validation fails
                     // account is not attached to context so request won't have access to secure routes
                }
            }
        }
    }
}