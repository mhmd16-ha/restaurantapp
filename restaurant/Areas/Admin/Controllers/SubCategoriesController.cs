using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using restaurant.Data;
using restaurant.Models;
using restaurant.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restaurant.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubCategoriesController : Controller
    {
        private readonly ApplicationDbContext db;
        [TempData]
        public string StatusMessage { get; set; }


        public SubCategoriesController(ApplicationDbContext db)
        {
            this.db = db;
        }
        [HttpGet]
        public async Task <IActionResult> Index()
        {
            var item = await db.SubCategories.Include(m=>m.Category).ToListAsync();
            return View(item);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            SubCategoryAndCategoryModelView model = new SubCategoryAndCategoryModelView()
            {
                CategoryList = await db.Categories.ToListAsync(),
                SubCategory= new Models.SubCategory(),
                SubCategoryList= await db.SubCategories.OrderBy(m=>m.Name).Select(m=>m.Name).Distinct().ToListAsync(),


            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCategoryAndCategoryModelView model)
        {

            if (ModelState.IsValid)
            {
                var doesExisting =await db.SubCategories.Include(m => m.Category).Where(m => m.Category.Id == model.SubCategory.CategoryId && m.Name == model.SubCategory.Name).ToListAsync();
                if (doesExisting.Count() > 0)
                {
                    StatusMessage = "Error : this is Sub Category Exist Before in "+ doesExisting.FirstOrDefault().Category.Name;
                }
                else
                {
                    db.SubCategories.Add(model.SubCategory);
                    await db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            SubCategoryAndCategoryModelView modelVm = new SubCategoryAndCategoryModelView()
            {
                CategoryList = await db.Categories.ToListAsync(),
                SubCategory = model.SubCategory,
                SubCategoryList = await db.SubCategories.OrderBy(m => m.Name).Select(m => m.Name).Distinct().ToListAsync(),
                StutusMessage=StatusMessage,

            };
            return View(modelVm);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories(int id)
        {
            List<SubCategory> subCategories = new List<SubCategory>();
            subCategories = await db.SubCategories.Where(m => m.CategoryId == id).ToListAsync();
            return Json(new SelectList(subCategories, "Id", "Name"));
        }
    }
}
