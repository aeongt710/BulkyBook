using BukbyBook.Models;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWebApp.Controllers
{
    public class CoverController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;
        public CoverController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Cover> covers = _unitOfWork.ICoverRepository.GetAll();
            return View(covers);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Cover cover)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ICoverRepository.Add(cover);
                _unitOfWork.Save();
                TempData["success"] = "Cover Added Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public IActionResult Edit(int Id)
        {
            Cover cover = _unitOfWork.ICoverRepository.GetFirstOrDefault(a => a.Id == Id);
            if (cover == null)
                return NotFound();
            return View(cover);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Cover cover)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ICoverRepository.Update(cover);
                _unitOfWork.Save();
                TempData["warning"] = "Cover Updated Successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(cover);
        }
        public IActionResult Delete(int? Id)
        {
            if (Id == 0 || Id == null)
                return NotFound("Invalid Cover ID ");
            Cover cover = _unitOfWork.ICoverRepository.GetFirstOrDefault(a => a.Id == Id);
            if (cover == null)
                return NotFound();

            return View(cover);
        }

        [HttpPost, ActionName(nameof(Delete))]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? Id)
        {
            if (Id == null)
                return NotFound();
            Cover cover = _unitOfWork.ICoverRepository.GetFirstOrDefault(a => a.Id == Id);
            if (cover != null)
            {
                _unitOfWork.ICoverRepository.Remove(cover);
                _unitOfWork.Save();
                TempData["success"] = "Cover Deleted Successfully!";
                return RedirectToAction(nameof(Index), "Cover");
            }
            return NotFound();
        }
    }
}
