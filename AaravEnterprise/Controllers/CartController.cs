using AaravEnterprise.DataAccess;
using AaravEnterprise.Models;
using AaravEnterprise.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace AaravEnterprise.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private Cart cart;
        public CartController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IActionResult Index()
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (userId == null)
            {
                return NotFound();
            }
            else
            {
                IQueryable<CartViewModel> query = from C in _dbContext.Cart
                                                  join S in _dbContext.Services on C.ServiceId equals S.Id
                                                  join P in _dbContext.Package on C.PackageId equals P.Id
                                                  where C.ApplicationUserId == userId
                                                  select new CartViewModel
                                                  {
                                                      CartId = C.CartId,
                                                      ServiceTitle = S.ServiceTitle,
                                                      PackageTitle = P.PackageTitle,
                                                      Amount = C.Amount
                                                  };
                System.Collections.Generic.List<CartViewModel> cart = query.ToList();
                int total = query.Sum(p => p.Amount);
                ViewBag.Total = total;
                ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
                return View(cart);
            }
        }

        public IActionResult RemoveCartItem(int CartId)
        {
            Cart rowToRemove = _dbContext.Cart.Find(CartId);
            if (rowToRemove != null)
            {
                _dbContext.Cart.Remove(rowToRemove);
                _dbContext.SaveChanges();
                TempData["success"] = "Service removed from the Cart";
            }
            return RedirectToAction("Index");
        }

        public IActionResult AddToCart(int ServiceId, int PackageId, int Amount)
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            Services objectService = _dbContext.Services.FirstOrDefault(c => c.Id == ServiceId);
            Package objectPackage = _dbContext.Package.FirstOrDefault(c => c.Id == PackageId);
            cart = new Cart
            {
                ApplicationUserId = userId,
                Amount = Amount,
                Package = objectPackage,
                Service = objectService,
                PackageId = PackageId,
                ServiceId = ServiceId
            };
            _dbContext.Cart.Add(cart);
            _dbContext.SaveChanges();
            TempData["success"] = "Service added in the Cart";
            return RedirectToAction("Index");

        }
    }
}
