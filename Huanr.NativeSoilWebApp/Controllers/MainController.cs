using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Huanr.NativeSoilWebApp.Controllers
{
    public class MainController : Controller
    {
        ZeroDbs.Interfaces.IDbService zeroService = null;
        public MainController(ZeroDbs.Interfaces.IDbService zeroService)
        {
            this.zeroService = zeroService;
        }
        public IActionResult Index()
        {
            

            ViewBag.areaStr = "";// s.ToString();

            return View();
        }
        
    }
}