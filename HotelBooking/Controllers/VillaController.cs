using HotelBooking.Application.Common.Interfaces;
using HotelBooking.Domain.Entities;
using HotelBooking.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace HotelBooking.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var villas = _unitOfWork.Villa.GetAll();
            return View(villas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            if (obj.Name == obj.Description)
            {
                {
                    ModelState.AddModelError("name", "The Description cannot be the same as the Name.");
                }
            }
            var existingVilla = _unitOfWork.Villa
           .GetAll(v => v.Name.ToLower() == obj.Name.ToLower());

            if (existingVilla.Any())
            {
                ModelState.AddModelError("Name", "A villa with this name already exists.");
                return View(obj); // Razor view will show validation error
            }

            if (ModelState.IsValid)
            {
                if(obj.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    string ImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "VillaImage", fileName);
                    using var fileStream = new FileStream(ImagePath, FileMode.Create);
                    obj.Image.CopyTo(fileStream);
                  
                    obj.ImageUrl = @"\images\VillaImage\" + fileName;
                }
                else {
                    obj.ImageUrl = "https://placehold.co/600x400";
                } 
                
                _unitOfWork.Villa.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Villa created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        
        public IActionResult Update(int id)
        {
            Villa? obj = _unitOfWork.Villa.Get(u => u.Id == id);
            if (obj == null)
            {
                return RedirectToAction("Error","Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            
            if (ModelState.IsValid && obj.Id>0)
            {
                _unitOfWork.Villa.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Villa updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }
        public IActionResult Delete(int id)
        {
            Villa? obj = _unitOfWork.Villa.Get(u => u.Id == id);
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromDb = _unitOfWork.Villa.Get(u => u.Id == obj.Id);

            if (objFromDb is not null)
            {
                _unitOfWork.Villa.Remove(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "Villa deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Error while deleting villa";
            return View();
        }
    }
}
