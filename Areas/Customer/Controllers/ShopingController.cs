using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookShop.Data;
using BookShop.Models;
using BookShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShopingController : Controller
    {
        private readonly ApplicationDbContext _context;


        public ShopingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShopCardVM cardObj = new ShopCardVM()
            {
                listCard = await _context.ShopingCard.Where(m => m.UserId == claim.Value).ToArrayAsync(),
                OrderHeader = new OrderHeader()
                
            };

            foreach(var item in cardObj.listCard)
            {
                item.MenuItem = await _context.MenuItem.FirstOrDefaultAsync(m => m.Id == item.MenuItemId);
                cardObj.OrderHeader.OrderTotalPrice = cardObj.OrderHeader.OrderTotalPrice + (item.MenuItem.Price * item.Count);
            }

            return View(cardObj);
        }
    }
}
