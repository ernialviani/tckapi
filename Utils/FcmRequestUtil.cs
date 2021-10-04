using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using System.Collections.Generic;
using TicketingApi.Entities;
using TicketingApi.DBContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TicketingApi.Models.v1.Notifications;

namespace TicketingApi.Utils
{
    public class FcmRequestUtil :IFcmRequestUtil
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly AppDBContext  _context;
        
        public FcmRequestUtil(IHttpClientFactory clientFactory, AppDBContext context)
        {
            _clientFactory = clientFactory;
            _context = context;
        }

        public async Task SendMultipleNotify(NotifSetting notif)
        {
            try
            {
                 var registeredUser = _context.NotifRegisters.AsEnumerable().Where(w => notif.Users.Any(a => w.UserId != null && a.Id == w.UserId)).ToList();
                List<string> tokenList = new List<string>();
                List<Notif>  addNotify = new List<Notif>();
                foreach (var reg in registeredUser)
                {
                    addNotify.Add(new Notif{
                        NotifRegisterId = reg.Id,
                        Title = notif.Title,
                        Message = notif.Message,
                        Link = notif.LinkAction,
                        NtfType = notif.NotifType,
                        CreatedAt = DateTime.Now
                    });
                    tokenList.Add(reg.FcmToken);
                }
                _context.Notifs.AddRange(addNotify);
                _context.SaveChanges();
                //  Message single = new Message()
                //     {
                //         Notification = new Notification
                //         {
                //             Title = notif.Title,
                //             Body = notif.Message
                //         },
                //         Token = ""
                //     };
                // FirebaseMessaging messaging = FirebaseMessaging.DefaultInstance;
                // string result = await messaging.SendAsync(single);
                var message = new MulticastMessage()
                {
                    Tokens = tokenList,
                    Notification = new Notification
                    {
                        Title = notif.Title,
                        Body = notif.Message
                    },
                    Data = new Dictionary<string, string>()
                    {
                        { "ntfType", notif.NotifType },
                        { "actionLink", notif.LinkAction }
                    },
                    Webpush = new WebpushConfig
                    {
                    FcmOptions = new WebpushFcmOptions(){
                        // Link = notif.LinkAction
                    }  
                    }
                };
                var fcmResponse = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);    
            }
            catch (System.Exception e)
            {
                return;
            }
        
        }

        public async Task SendMultipleNotifyClient(NotifSetting notif)
        {

            var users = _context.NotifRegisters.AsNoTracking().Where(w => notif.Senders.Any(a => a.Id == w.UserId)).ToList();
            List<string> tokenList = new List<string>();
            foreach (var user in users)
            {
                tokenList.Add(user.FcmToken);
            }
            //  Message single = new Message()
            //     {
            //         Notification = new Notification
            //         {
            //             Title = notif.Title,
            //             Body = notif.Message
            //         },
            //         Token = ""
            //     };

            // FirebaseMessaging messaging = FirebaseMessaging.DefaultInstance;
            // string result = await messaging.SendAsync(single);
            var message = new MulticastMessage()
            {
                Tokens = tokenList,
                Notification = new Notification
                {
                    Title = notif.Title,
                    Body = notif.Message
                },
                Data = new Dictionary<string, string>()
                {
                    { "ntfType", notif.NotifType },
                    { "actionLink", notif.LinkAction }
                },
                
            };
            var fcmResponse = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
        }
        
    }
}