using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TicketingApi.Entities;

namespace TicketingApi.Utils
{
    public interface IFcmRequestUtil
    {
           Task SendMultipleNotify(NotifSetting notif);
    }
}