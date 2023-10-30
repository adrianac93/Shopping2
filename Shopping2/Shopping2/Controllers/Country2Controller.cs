#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping2.Data2;
using Shopping2.Data2.Entities;
using Shopping2.Models;

namespace Shopping2.Controllers
{
    public class Country2Controller : Controller
    {
        private readonly DataContext2 _context2;

        public Country2Controller(DataContext2 context)
        {
            _context2 = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context2.Countries
                 .Include(c => c.States)
                .ToListAsync());
                     
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Country2 country2 = await _context2.Countries
                .Include(c => c.States)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country2 == null)
            {
                return NotFound();
            }

            return View(country2);
        }

        public IActionResult Create()
        {
            Country2 country2 = new() { States = new List<State>() };
            return View(country2);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Country2 country2)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context2.Add(country2);
                    await _context2.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un país con el mismo nombre.");
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
            
            return View(country2);

        }
        public async Task<IActionResult> AddState(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Country2 country2 = await _context2.Countries.FindAsync(id);
            if (country2 == null)
            {
                return NotFound();
            }

            StateViewModel model = new()
            {
                CountryId = country2.Id,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddState(StateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    State state = new()
                    {
                        Cities = new List<City>(),
                        Country = await _context2.Countries.FindAsync(model.CountryId),
                        Name = model.Name,
                    };
                    _context2.Add(state);
                    await _context2.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { Id = model.CountryId  } );
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un departamento/estado con el mismo nombre en este país.");
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

            return View(model);

        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context2.Countries == null)
            {
                return NotFound();
            }

            Country2 country2 = await _context2.Countries.FindAsync(id);
            if (country2 == null)
            {
                return NotFound();
            }

            return View(country2);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Country2 country2)
        {
            if (id != country2.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context2.Update(country2);
                    await _context2.SaveChangesAsync();
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un país con el mismo nombre.");
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

            return View(country2);
        }
        public async Task<IActionResult> EditState(int? id)
        {
            if (id == null || _context2.Countries == null)
            {
                return NotFound();
            }

            State state = await _context2.States
                .Include(s => s.Country)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (state == null)
            {
                return NotFound();
            }

            StateViewModel model = new()
            {
                CountryId = state.Country.Id,
                Id = state.Id, 
                Name = state.Name,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditState(int id, StateViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    State state = new()
                    {
                        Id = model.Id,
                        Name = model.Name,
                    };
                    _context2.Update(state);
                    await _context2.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { Id = model.CountryId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un departamento/estado con el mismo nombre en este país.");
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

            return View(model);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context2.Countries == null)
            {
                return NotFound();
            }

           Country2 country2 = await _context2.Countries
                .Include(c => c.States)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (country2 == null)
            {
                return NotFound();
            }

            return View(country2);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
        
            Country2 country2 = await _context2.Countries.FindAsync(id);
            _context2.Countries.Remove(country2);
            await _context2.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
