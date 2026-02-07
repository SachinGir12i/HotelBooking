using Microsoft.AspNetCore.Mvc;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using HotelBooking.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _db;
        public VillaNumberController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var villaNumbers = _db.VillaNumbers.Include(u=>u.Villa).ToList();
            return View(villaNumbers);
        }

        public IActionResult Create()
        {
            VillaNumberVM villaNumberVM = new ()
            {
                
                VillaList = _db.Villas.ToList().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };  
            
            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            bool roomNumberExists = _db.VillaNumbers.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);
            ModelState.Remove("Villa");
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    var key = state.Key; // property name
                    var errors = state.Value.Errors;
                    foreach (var error in errors)
                    {
                        TempData["success"] = ($"Property: {key}, Error: {error.ErrorMessage}");
                    }
                }
            }

            if (ModelState.IsValid && !roomNumberExists)
            {
                _db.VillaNumbers.Add(obj.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "Villa Number created successfully";
                return RedirectToAction("Index");
            }
            if(roomNumberExists)
            {
                TempData["error"] = "The vills number alresdy exist.";
            }
            obj.VillaList = _db.Villas.ToList().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(obj);
        }

        
        public IActionResult Update(int id)
        {
            Villa? obj = _db.Villas.FirstOrDefault(u => u.Id == id);
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
                _db.Villas.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Villa updated successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public IActionResult Delete(int id)
        {
            Villa? obj = _db.Villas.FirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromDb = _db.Villas.FirstOrDefault(u => u.Id == obj.Id);

            if (objFromDb is not null)
            {
                _db.Villas.Remove(objFromDb);
                _db.SaveChanges();
                TempData["success"] = "Villa deleted successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Error while deleting villa";
            return View();
        }
    }
}
