using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EMarket.Areas.Admin.Controllers
{
    [Area("Admin")]
    
    public class BaseController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "HangHoa");
        }
    }
}