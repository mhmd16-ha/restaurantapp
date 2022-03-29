using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restaurant.Data;
using restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restaurant.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext db;

        public CategoriesController(ApplicationDbContext db)
        {
            this.db = db;
        }
        [HttpGet]
        public async Task <IActionResult> Index()
        {
            var p = await db.Categories.ToListAsync();
            return View(p);
        }
        [HttpGet]
        public IActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
               await db.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            return View(category);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var item = await db.Categories.FindAsync(id);
            if (item == null) {
                return NotFound();
            }
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Update(category);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            return View(category);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var item = await db.Categories.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Remove(category);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            return View(category);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var item = await db.Categories.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }
      

    }
}
