using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rocky.Data;
using Rocky.Models;
using Rocky.Utility;
using Rocky.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db )
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            HomeVM HomeVM = new HomeVM()
            {
                Products = _db.Product.Include(u => u.Category).Include(u => u.Application),
                Categories = _db.Category

            };
            return View(HomeVM);

        }

        public IActionResult Details(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            //if the object exsists in session 
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            var DetailsVM = new DetailsVM()
            {
                Product = _db.Product.Include(u => u.Category).Include(u => u.Application).Where(u => u.Id == id).FirstOrDefault(),
                ExistsInCart = false
            };


            foreach (var item in shoppingCartList)
            {
                if (item.ProductId==id)
                {
                    DetailsVM.ExistsInCart = true; 
                }
            }
            return View(DetailsVM);
        }

        [HttpPost,ActionName("Details")]
        public IActionResult DetailsPost(int id)
        {
            List<ShoppingCart> shoppingCartlist = new List<ShoppingCart>();

            //if the object exsists in session 
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartlist = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            shoppingCartlist.Add(new ShoppingCart { ProductId = id });
            HttpContext.Session.Set<IEnumerable<ShoppingCart>>(WC.SessionCart, shoppingCartlist);

                                        //get the string version of a memeber , variable etc.    
            return RedirectToAction(nameof(Index));
        }


        public IActionResult RemoveFromCart(int id)
        {

            //Check if shopping 
            List<ShoppingCart> shoppingCartlist = new List<ShoppingCart>();

            //if the object exsists in session 
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                shoppingCartlist = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            //retrieve product form shopping cart 
            var itemFollower = shoppingCartlist.SingleOrDefault(r=>r.ProductId==id);

            //checkl if itemFollower exists. If it does, remove it 
            if (itemFollower!=null)
            {
                shoppingCartlist.Remove(itemFollower);
            }


            HttpContext.Session.Set<IEnumerable<ShoppingCart>>(WC.SessionCart, shoppingCartlist);

            //get the string version of a memeber , variable etc.    
            return RedirectToAction(nameof(Index));
        }





        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
