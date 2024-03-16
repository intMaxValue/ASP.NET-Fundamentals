using Microsoft.AspNetCore.Mvc;
using SoftUniBazar.Models;
using System.Diagnostics;

namespace SoftUniBazar.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User?.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("All", "Ad");
            }

            return View();
        }

        
    }
}