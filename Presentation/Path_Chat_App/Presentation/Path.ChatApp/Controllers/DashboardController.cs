using Microsoft.AspNetCore.Mvc;

namespace Path.ChatApp.Controllers
{
    public class DashboardController : BaseController
    {
        
        public IActionResult Index()
        {
            return View();
        }
    }
}
