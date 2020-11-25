using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BookShop.Models;
using BookShop.ViewModels;
using BookShop.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using BookShop.Utility;

namespace BookShop.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IndexPageVM indexVM = new IndexPageVM()
            {
                MenuItems = await _context.MenuItem.Include(m=>m.Category).ToListAsync(),
                Categories = await _context.Category.ToListAsync()
            };

            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                var count = _context.ShopingCard.Where(u => u.UserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.SessionCard, count);
            }

            return View(indexVM);
        }


        [Authorize]
        public async Task<IActionResult> Buy(int id)
        {
            var menuItemDb = await _context.MenuItem.Include(m => m.Category).Where(m => m.Id == id).FirstOrDefaultAsync();

            ShopingCard SCard = new ShopingCard()
            {
                MenuItem = menuItemDb,
                MenuItemId = menuItemDb.Id
            };

            return View(SCard);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy(ShopingCard SCard)
        {
            SCard.Id = 0;
            if(ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                SCard.UserId = claim.Value;

                ShopingCard SCfromDB = await _context.ShopingCard.Where(m => m.MenuItemId == SCard.MenuItemId && m.UserId == SCard.UserId).FirstOrDefaultAsync();

                if (SCfromDB == null)
                    await _context.ShopingCard.AddAsync(SCard);

                else
                    SCfromDB.Count += SCard.Count;

                await _context.SaveChangesAsync();

                var cout = _context.ShopingCard.Where(m => m.UserId == SCard.UserId).ToList().Count;

                HttpContext.Session.SetInt32(SD.SessionCard,cout);
            }

            else
            {
                var MenuItemFromDB = await _context.MenuItem.Include(m=>m.Category).Where(m => m.Id == SCard.MenuItemId).FirstOrDefaultAsync();

                ShopingCard sc = new ShopingCard()
                 {
                     MenuItem = MenuItemFromDB,
                     MenuItemId = MenuItemFromDB.Id
                 };

                return View(sc);
            }

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
