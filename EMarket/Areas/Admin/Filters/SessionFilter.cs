using EMarket.Areas.Admin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace EMarket.Areas.Admin.Filters
{
    public class SessionFilter: IActionFilter
    {
        private readonly EMarketContext _context;
        public SessionFilter(EMarketContext context)
        {
            _context = context;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var AdminUser = context.HttpContext.Session.GetString("User");
            if (AdminUser == null || AdminUser == "")
            { context.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    { "action" , "Index" },
                    { "controller",  "Login" },
                    { "area","Admin" }
                });
            }
            else
            {
                var account = _context.TaiKhoan.Where(p => p.UserName == AdminUser).FirstOrDefault();                
                if (account.LoaiTaiKhoan != false) context.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    { "action" , "Index" },
                    { "controller",  "Login" },
                    { "area","Admin" }
                });
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
           
        }
    }
}
