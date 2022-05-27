
using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWebApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork db)
        {
            _unitOfWork = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> categories = _unitOfWork.ICategoryRepository.GetAll();
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
            if (ModelState.IsValid)
            {
                _unitOfWork.ICategoryRepository.Add(category);
                _unitOfWork.Save();
                TempData["success"] = "Category Edited Successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            Category category = _unitOfWork.ICategoryRepository.GetFirstOrDefault(a => a.Id == Id);
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
                _unitOfWork.ICategoryRepository.Update(category);
                _unitOfWork.Save();
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
            Category category = _unitOfWork.ICategoryRepository.GetFirstOrDefault(a => a.Id == Id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost, ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? Id)
        {
            if (Id == null)
                return NotFound();
            Category category = _unitOfWork.ICategoryRepository.GetFirstOrDefault(a => a.Id == Id);
            if (category != null)
            {
                _unitOfWork.ICategoryRepository.Remove(category);
                _unitOfWork.Save();
                TempData["warning"] = "Category Deleted Successfully!";
                return RedirectToAction(nameof(Index), "Category");
            }
            return NotFound();
        }
    }
}
