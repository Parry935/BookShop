using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Data;
using BookShop.Models;
using BookShop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.AdminUser)]
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext db)
        {
            _context = db;
        }


        //Get
        public async Task<IActionResult> Index()
        {
             
            return View( await _context.Category.ToListAsync());
        }


        //Get - Create
        public IActionResult Create()
        {
            return View();
        }

        //Post - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category c)
        {
            if(ModelState.IsValid)
            {
                _context.Category.Add(c);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(c);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context.Category.FindAsync(id);

            if (category == null)
                return NotFound();

            return View(category);

        }//Get - Edit
        

        //Post - Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category c)
        {
            if (ModelState.IsValid)
            {
                _context.Category.Update(c);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(c);

        }

        //Get - Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context.Category.FindAsync(id);

            if (category == null)
                return NotFound();

            return View(category);

        }

        //Get - Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var category = await _context.Category.FindAsync(id);

            if (category == null)
                return NotFound();

            return View(category);

        }

        //Post - Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var c = await _context.Category.FindAsync(id);

            if(c == null)
            {
                return NotFound();
            }

            _context.Category.Remove(c);
            await _context.SaveChangesAsync();

                return RedirectToAction("Index");
        }

    }
}
