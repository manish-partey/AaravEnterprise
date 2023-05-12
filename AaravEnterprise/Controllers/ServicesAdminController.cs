using Microsoft.AspNetCore.Mvc;
using AaravEnterprise.DataAccess;
using AaravEnterprise.Models;
using System.Collections.Generic;
using System.Linq;

namespace AaravEnterprise.Controllers
{
    public class ServicesAdminController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public ServicesAdminController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            List<Services> objCategoryList = _dbContext.Services.ToList();
            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View();
        }

        [HttpPost]
        public IActionResult Create(Services obj)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Services.Add(obj);
                _dbContext.SaveChanges();
                TempData["success"] = "Service created successfully";
                return RedirectToAction("Index");
            }
            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Services? serviceFromDb = _dbContext.Services.FirstOrDefault(s => s.Id == id);

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
            Services? objService = _dbContext.Services.Find(id);
            Package? objPackage = _dbContext.Package.FirstOrDefault(s => s.ServicesId == id);
            if (objService == null)
            {
                return NotFound();
            }

            if (objPackage != null)
            { _dbContext.Package.Remove(objPackage); }

            _dbContext.Services.Remove(objService);
            _dbContext.SaveChanges();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Services? serviceFromDb = _dbContext.Services.Find(id);

            if (serviceFromDb == null)
            {
                return NotFound();
            }
            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View(serviceFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Services obj)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Services.Update(obj);
                _dbContext.SaveChanges();
                TempData["success"] = "Service updated successfully";
                return RedirectToAction("Index");
            }
            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View();

        }
    }
}
