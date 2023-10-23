using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping2.Data2;
using Shopping2.Data2.Entities;

namespace Shopping2.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly DataContext2 _context2;

        public CategoriesController(DataContext2 context2)
        {
            _context2 = context2;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context2.Categories.ToListAsync());

        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context2.Add(category);
                    await _context2.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe una categoría con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return View(category);

        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context2.Countries == null)
            {
                return NotFound();
            }

            Category category = await _context2.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context2.Update(category);
                    await _context2.SaveChangesAsync();
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe una categoría con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }

            }

            return View(category);
        }
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context2.Countries == null)
            {
                return NotFound();
            }

            Category category = await _context2.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context2.Countries == null)
            {
                return NotFound();
            }

            Category category = await _context2.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Category category = await _context2.Categories.FindAsync(id);
            _context2.Categories.Remove(category);
            await _context2.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
