using AaravEnterprise.DataAccess;
using AaravEnterprise.Models;
using AaravEnterprise.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace AaravEnterprise.Controllers
{
    public class PackagesAdminController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public PackagesAdminController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var query = from t1 in _dbContext.Package
                        join t2 in _dbContext.Services on t1.ServicesId equals t2.Id                       
                        select new PackagesAdminViewModel
                        {
                            Id = t2.Id,
                            ServiceTitle = t2.ServiceTitle,
                            ServiceDesciption = t2.ServiceDescription,
                            PackageId = t1.Id,
                            PackageTitle = t1.PackageTitle,
                            PackageDescription = t1.PackageDescription,
                            Price = t1.Price
                        };
            var result = query.ToList();
            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View(result);           
        }
        public IActionResult Create()
        {
            var services = _dbContext.Services.ToList();
            ViewBag.Services = new SelectList(services, "Id", "ServiceTitle");
            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View();
        }

        [HttpPost]
        public IActionResult Create(Package obj, int ServicesId)
        {
            var objectService = _dbContext.Services.FirstOrDefault(c => c.Id == ServicesId);
            obj.Services = objectService;
            obj.ServicesId = ServicesId;
            _dbContext.Package.Add(obj);
            _dbContext.SaveChanges();
            TempData["success"] = "Service created successfully";
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Package? serviceFromDb = _dbContext.Package.FirstOrDefault(s => s.Id == id);

            if (serviceFromDb == null)
            {
                return NotFound();
            }
            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View(serviceFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Package? objService = _dbContext.Package.Find(id);
            Package? objPackage = _dbContext.Package.FirstOrDefault(s => s.ServicesId == id);
            if (objService == null)
            {
                return NotFound();
            }

            if (objPackage != null)
            { _dbContext.Package.Remove(objPackage); }

            _dbContext.Package.Remove(objService);
            _dbContext.SaveChanges();
            TempData["success"] = "Package deleted successfully";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? PackageId)
        {
            if (PackageId == null || PackageId == 0)
            {
                return NotFound();
            }
            Package? packagesFromDb = _dbContext.Package.Find(PackageId);


            if (packagesFromDb == null)
            {
                return NotFound();
            }
            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View(packagesFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Package obj)
        {
            var objectService = _dbContext.Services.FirstOrDefault(c => c.Id == obj.ServicesId);
            obj.Services = objectService;
            _dbContext.Package.Update(obj);
            _dbContext.SaveChanges();
            TempData["success"] = "Service updated successfully";
            return RedirectToAction("Index");
        }
    }
}
