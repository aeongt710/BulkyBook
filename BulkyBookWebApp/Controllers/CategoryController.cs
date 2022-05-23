using BulkyBookWebApp.Data;
using BulkyBookWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWebApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IList<Category> categories = _db.Categories.ToList();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if(ModelState.IsValid)
            {
                _db.Add(category);
                _db.SaveChanges();
                TempData["success"] = "Category Edited Successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public IActionResult Edit(int? Id)
        {
            if(Id == null||Id==0)
            {
                return NotFound();
            }
            Category category = _db.Categories.Find(Id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(category);
                _db.SaveChanges();
                TempData["warning"] = "Category Edited Successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            Category category = _db.Categories.Find(Id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost,ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? Id)
        {
            if (Id == null)
                return NotFound();
            Category category = _db.Categories.Find(Id);
            if(category!=null)
            {
                _db.Categories.Remove(category);
                _db.SaveChanges();
                TempData["warning"] = "Category Deleted Successfully!";
                return RedirectToAction(nameof(Index), "Category");
            }
            return NotFound();
        }
    }
}
