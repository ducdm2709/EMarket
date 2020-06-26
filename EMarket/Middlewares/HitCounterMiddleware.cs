using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EMarket.Middlewares
{
    public class HitCounterMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public HitCounterMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext context,ILogger<HitCounterMiddleware> logger, IHostingEnvironment env)
        {
            string cookieId = context.Request.Cookies["CookieId"];
            if (cookieId == null)
            {
                UpdateCounter(env);
                context.Response.Cookies.Append("CookieId", Guid.NewGuid().ToString(), new CookieOptions()
                {
                    Path = "/",
                    HttpOnly = true,
                    Secure = false,
                    Expires = DateTime.Now.AddMinutes(1),
                });
            }

            await _requestDelegate(context);
            logger.LogInformation("New Cookie was added in HitCounterMiddleware");
        }

        private void UpdateCounter(IHostingEnvironment env)
        {
            try
            {
                string visitedCountPath = Path.Combine(env.ContentRootPath, "Logs", "visitedcount.txt");

                if (!File.Exists(visitedCountPath))
                {
                    File.WriteAllText(visitedCountPath, "0");
                }
                else
                {
                    int visitedCount = Convert.ToInt32(File.ReadAllText(visitedCountPath));
                    visitedCount++;
                    File.WriteAllText(visitedCountPath, visitedCount.ToString());
                }
            }
            catch ( Exception exception)
            {
                throw exception;
            }
        }
    }
   
    public static class HitCounterMiddlewareExtensions
    {
        public static IApplicationBuilder UseHitCounter(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HitCounterMiddleware>();
        }
    }
}
