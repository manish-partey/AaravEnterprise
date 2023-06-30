using AaravEnterprise.DataAccess;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AaravEnterprise.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public ServiceController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index(int? Id)
        {
            Models.Services objServices = _dbContext.Services.FirstOrDefault(s => s.Id == Id); ;
            ViewBag.Services = objServices;

            System.Collections.Generic.List<Models.Package> objPackages = _dbContext.Package.Where(s => s.ServicesId == Id).ToList();
            ViewBag.Packages = objPackages;
            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View();
        }
    }
}
