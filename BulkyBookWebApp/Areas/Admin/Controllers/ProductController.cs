using BukbyBook.Models;
using BukbyBook.Models.ViewModels;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWebApp.Controllers
{
    public class ProductController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            //IEnumerable<Product> products = _unitOfWork.IProductRepository.GetAll();
            return View();
        }

        public IActionResult Upsert(int? Id)
        {

            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.ICategoryRepository.GetAll().Select(
                a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                }),
                CoverList = _unitOfWork.ICoverRepository.GetAll().Select(
                a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                })
        };

            if (Id == 0 || Id == null)
            {
                //Create Logic
                return View(productVM);
            }
            else
            {
                //Edit Logic
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Product product)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.IProductRepository.Update(product);
                _unitOfWork.Save();
                TempData["warning"] = "Cover Updated Successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        public IActionResult Delete(int? Id)
        {
            if (Id == 0 || Id == null)
                return NotFound("Invalid Product ID ");
            Product product = _unitOfWork.IProductRepository.GetFirstOrDefault(a => a.Id == Id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost, ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? Id)
        {
            if (Id == null)
                return NotFound();
            Product product = _unitOfWork.IProductRepository.GetFirstOrDefault(a => a.Id == Id);
            if (product != null)
            {
                _unitOfWork.IProductRepository.Remove(product);
                _unitOfWork.Save();
                TempData["success"] = "Product Deleted Successfully!";
                return RedirectToAction(nameof(Index), "Cover");
            }
            return NotFound();
        }
    }
}
