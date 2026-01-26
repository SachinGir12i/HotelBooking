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
                    ModelState.AddModelError("CustomError", "The Description cannot be the same as the Name.");
                }
            }
                if (ModelState.IsValid)
                {
                    _db.Villas.Add(obj);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(obj);
        }
    }
}
