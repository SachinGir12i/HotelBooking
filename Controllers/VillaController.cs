using Microsoft.AspNetCore.Mvc;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Domain.Entities;
using HotelBooking.Application.Common.Interfaces;


namespace HotelBooking.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaRepository _villaRepo;
        public VillaController(IVillaRepository villaRepo)
        {
            _villaRepo = villaRepo;
        }
        public IActionResult Index()
        {
            var villas = _villaRepo.GetAll();
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
            if (ModelState.IsValid)
            {
                _villaRepo.Add(obj);
                _villaRepo.Save();
                TempData["success"] = "Villa created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        
        public IActionResult Update(int id)
        {
            Villa? obj = _villaRepo.Get(u => u.Id == id);
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
                _villaRepo.Update(obj);
                _villaRepo.Save();
                TempData["success"] = "Villa updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }
        public IActionResult Delete(int id)
        {
            Villa? obj = _villaRepo.Get(u => u.Id == id);
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromDb = _villaRepo.Get(u => u.Id == obj.Id);

            if (objFromDb is not null)
            {
                _villaRepo.Remove(objFromDb);
                _villaRepo.Save();
                TempData["success"] = "Villa deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Error while deleting villa";
            return View();
        }
    }
}
