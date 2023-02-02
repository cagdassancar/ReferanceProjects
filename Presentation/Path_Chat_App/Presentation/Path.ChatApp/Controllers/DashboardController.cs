using Microsoft.AspNetCore.Mvc;
using Path.Core.Dto;
using Path.Core.Interfaces;
using Path.Infrastructure.RedisManager;
using ServiceStack;

namespace Path.ChatApp.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly IRedisBase _redis;

        public DashboardController(IRedisBase redis)
        {
            _redis = redis;
        }

        public IActionResult Index()
        {
            _redis.SetLoginUser(new LoginUser() { SessionID = "11111", UserName = "Cagdas" });
            _redis.SetLoginUser(new LoginUser() { SessionID = "22222", UserName = "Samed" });
            _redis.SetLoginUser(new LoginUser() { SessionID = "33333", UserName = "Elcim" });

            IEnumerable<LoginUser> userList = _redis.GetAllLoginUser();
            

            return View();
        }
    }
}
