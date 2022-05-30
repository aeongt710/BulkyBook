using BukbyBook.Models;
using BukbyBook.Models.ViewModels;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace BulkyBookWebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostingEnvironment _hostingEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IHostingEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
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
        public IActionResult Upsert(ProductVM productVm,IFormFile? formFile)
        {
            if (ModelState.IsValid)
            {
                string rootPath = _hostingEnvironment.WebRootPath;
                if(formFile != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(rootPath, @"img\products");
                    var extension = Path.GetExtension(formFile.FileName);

                    using (var fileStream = new FileStream (Path.Combine(uploads,filename+extension),FileMode.Create))
                    {
                        formFile.CopyTo(fileStream);
                    }
                    productVm.Product.ImgSrc = @"img\products\" + filename + extension;
                }

                _unitOfWork.IProductRepository.Update(productVm.Product);
                _unitOfWork.Save();
                TempData["warning"] = "Product Upserted Successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(productVm);
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
