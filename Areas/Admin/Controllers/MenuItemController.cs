using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Data;
using BookShop.Utility;
using BookShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.AdminUser)]
    [Area("Admin")]
    public class MenuItemController : Controller
    {
        private ApplicationDbContext _context;
        private IWebHostEnvironment _hosting;
        
        public MenuItemController(ApplicationDbContext db , IWebHostEnvironment env )
        {
            _context = db;
            _hosting = env;
            MenuItemVM = new MenuItemVM()
            {
                Categories = _context.Category,
                MenuItem = new Models.MenuItem()
            };

        }

        [BindProperty]
        public MenuItemVM MenuItemVM { get; set; }


        public async Task<IActionResult> Index()
        {
            var menuItems = await _context.MenuItem.Include(m=>m.Category).ToListAsync();
            return View(menuItems);
        }


        //Get - Create
        public IActionResult Create()
        {    
            return View(MenuItemVM);
        }

        //Post - Create
        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost()
        {
            if(!ModelState.IsValid)
            {
                return View(MenuItemVM);
            }

            _context.MenuItem.Add(MenuItemVM.MenuItem);
            await _context.SaveChangesAsync();

            string webRootPath = _hosting.WebRootPath;
            var files = HttpContext.Request.Form.Files;


            var menuItemFromDB = await _context.MenuItem.FindAsync(MenuItemVM.MenuItem.Id);

            if(files.Count>0)
            {
                var uploads = Path.Combine(webRootPath, "img");
                var extension = Path.GetExtension(files[0].FileName);

                using (var flieStream = new FileStream(Path.Combine(uploads, MenuItemVM.MenuItem.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(flieStream);
                }

                 menuItemFromDB.Image = @"\img\" + MenuItemVM.MenuItem.Id + extension;
            }
            else
            {
                var uploads = Path.Combine(webRootPath, @"img\" + SD.DefaultImg);
                System.IO.File.Copy(uploads, webRootPath + @"\img\" + MenuItemVM.MenuItem.Id + ".png");
                menuItemFromDB.Image = @"\img\" + MenuItemVM.MenuItem.Id + ".png";
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //Get - Edti
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MenuItemVM.MenuItem = await _context.MenuItem.Include(m=>m.Category).SingleOrDefaultAsync(m=>m.Id==id);

            if (MenuItemVM.MenuItem == null)
            {
                return NotFound();
            }


            return View(MenuItemVM);
        }

        //Post - Edit
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {

            if(id == null)
            {
                return NotFound();
            }    

            if (!ModelState.IsValid)
            {
                return View(MenuItemVM);
            }

            string webRootPath = _hosting.WebRootPath;
            var files = HttpContext.Request.Form.Files;


            var menuItemFromDB = await _context.MenuItem.FindAsync(id);

            if (files.Count > 0)
            {
                var uploads = Path.Combine(webRootPath, "img");
                var extension_new = Path.GetExtension(files[0].FileName);

                var imgToDel = Path.Combine(webRootPath, menuItemFromDB.Image.TrimStart('\\'));

                if(System.IO.File.Exists(imgToDel))
                {
                    System.IO.File.Delete(imgToDel);
                }

                using (var flieStream = new FileStream(Path.Combine(uploads, MenuItemVM.MenuItem.Id + extension_new), FileMode.Create))
                {
                    files[0].CopyTo(flieStream);
                }

                menuItemFromDB.Image = @"\img\" + MenuItemVM.MenuItem.Id + extension_new;
            }

            menuItemFromDB.Name = MenuItemVM.MenuItem.Name;
            menuItemFromDB.Description = MenuItemVM.MenuItem.Description;
            menuItemFromDB.Price = MenuItemVM.MenuItem.Price;
            menuItemFromDB.CategoryId = MenuItemVM.MenuItem.CategoryId;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //Get - Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            MenuItemVM.MenuItem = await _context.MenuItem.Include(m=>m.Category).SingleOrDefaultAsync(m=>m.Id==id);

            if (MenuItemVM.MenuItem == null)
                return NotFound();

            return View(MenuItemVM);

        }

        //Get - Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            MenuItemVM.MenuItem = await _context.MenuItem.Include(m => m.Category).SingleOrDefaultAsync(m => m.Id == id);

            if (MenuItemVM.MenuItem == null)
                return NotFound();

            return View(MenuItemVM);

        }

        //Post - Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var MIfromDB = await _context.MenuItem.FindAsync(id);

            if (MIfromDB == null)
            {
                return NotFound();
            }


            string webRootPath = _hosting.WebRootPath;

            var imgToDel = Path.Combine(webRootPath, MIfromDB.Image.TrimStart('\\'));

            if (System.IO.File.Exists(imgToDel))
            {
                System.IO.File.Delete(imgToDel);
            }

            _context.MenuItem.Remove(MIfromDB);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
