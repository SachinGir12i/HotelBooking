using Microsoft.AspNetCore.Mvc;
using HotelBooking.Infrastructure.Data;
using HotelBooking.Domain.Entities;


namespace HotelBooking.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _db;
        public VillaController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var villas = _db.Villas.ToList();
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
                _db.Villas.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Villa created successfully";
                return RedirectToAction("Index");
            }
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
