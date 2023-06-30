using AaravEnterprise.DataAccess;
using AaravEnterprise.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace AaravEnterprise.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "Home";
            return View();
        }

        public IActionResult About()
        {
            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View();
        }

        public IActionResult Contact()
        {
            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View();
        }

        public IActionResult Service()
        {
            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "Home";
            return View();
        }

        public IActionResult Shipping()
        {
            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View();
        }
        public IActionResult Privacy()
        {
            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View();
        }
        public IActionResult Refund()
        {
            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View();
        }
        public IActionResult TermsAndCondition()
        {
            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}