
using BulkyBook.DataAccess;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulkyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> objCategoryList = await _db.Categories.ToListAsync();
            return View(objCategoryList);
        }
        
        public IActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category obj)
        {
            if(obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name","The DisplayOrder cannot exactly match the Name.");
            }
            if (ModelState.IsValid) { 
             await _db.Categories.AddAsync(obj);
             _db.SaveChanges();
                TempData["success"] = "Category created successfully!";
            return RedirectToAction("Index");
            }
            
            return View(obj);
            
        }

        public async Task<IActionResult> Edit(int? id)
        {
            
           if(id== null || id == 0)
            {
                return NotFound();
            }
            //var categoryFromDb = await _db.Categories.FindAsync(id);
            var categoryFromDb = _db.Categories.FirstOrDefault(c => c.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                await _db.SaveChangesAsync();
                TempData["success"] = "Category edited successfully!";
                return RedirectToAction("Index");
            }

            return View(obj);

        }

        //Metinin Çözümü
        //public IActionResult Delete(int? id)
        //{

        //    if (id != null || id != 0)
        //    {
        //        var categoryFromDb = _db.Categories.Find(id);

        //        if(categoryFromDb != null)
        //        {
        //            _db.Categories.Remove(categoryFromDb);
        //            _db.SaveChanges();
        //        }

        //    }
        //    return RedirectToAction("Index");
        //}

        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDb = await _db.Categories.FindAsync(id);
            //var categoryFromDbFirst = _db.Categories.FirstOrDefault(c => c.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }


        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            var obj = await _db.Categories.FindAsync(id);
            if(obj == null)
            {
                return NotFound();
            }

            _db.Categories.Remove(obj);
            await _db.SaveChangesAsync();
            TempData["success"] = "Category deleted successfully!";
            return RedirectToAction("Index");
        }

    }
}
