using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BarRating.Data;
using BarRating.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using BarRating.Models.BarModels;
using Microsoft.Extensions.Hosting;

namespace BarRating.Controllers
{
    public class BarsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public BarsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {

            _context = context;
            _environment = environment;
        }

        // GET: Bars        
        public async Task<IActionResult> Index()
        {
            
              return _context.Bars != null ? 
                          View(await _context.Bars.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Bars'  is null.");
        }

        // GET: Bars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bars == null)
            {
                return NotFound();
            }

            var bar = await _context.Bars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bar == null)
            {
                return NotFound();
            }

            return View(bar);
        }

        // GET: Bars/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(BarCreateModel barC)
        {
            if (ModelState.IsValid)
            {
                if (barC.Image != null && barC.Image.Length > 0)
                {
                    var newFileName = await UploadAsync(barC.Image, _environment.WebRootPath);
                    barC.ImagePath = newFileName;
                }

                Bar bar = new Bar()
                {
                    Id = barC.Id,
                    Name = barC.Name,
                    Description = barC.Description,
                    ImagePath = barC.ImagePath
                };

                _context.Add(bar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Bars/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bars == null)
            {
                return NotFound();
            }

            var bar = await _context.Bars.FindAsync(id);
            if (bar == null)
            {
                return NotFound();
            }
            BarCreateModel barE = new BarCreateModel()
            {
                Id = bar.Id,
                Name = bar.Name,
                Description = bar.Description,
                ImagePath = bar.ImagePath,
            };
            return View(barE);
        }

        // POST: Bars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, BarCreateModel barC)
        {
            if (id != barC.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (barC.Image != null && barC.Image.Length > 0)
                {
                    var newFileName = await UploadAsync(barC.Image, _environment.WebRootPath);
                    barC.ImagePath = newFileName;
                }

                Bar bar = new Bar()
                {
                    Id = barC.Id,
                    Name = barC.Name,
                    Description = barC.Description,
                    ImagePath = barC.ImagePath
                };

                try
                {
                    _context.Update(bar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BarExists(barC.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Bars/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bars == null)
            {
                return NotFound();
            }

            var bar = await _context.Bars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bar == null)
            {
                return NotFound();
            }

            return View(bar);
        }

        // POST: Bars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bars == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Bars'  is null.");
            }
            var bar = await _context.Bars.FindAsync(id);
            if (bar != null)
            {
                _context.Bars.Remove(bar);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Search(string name)
        {
            if (_context.Bars == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Bars'  is null.");
            }
            var bar = _context.Bars.Where(s => s.Name.Contains(name));
            
            return RedirectToRoute($"Details/{(bar.FirstOrDefault()).Id}");
            
            

            
            
        }

        private bool BarExists(int id)
        {
          return (_context.Bars?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task<string> UploadAsync(IFormFile picture, string root)
        {
            var extension = Path.GetExtension(picture.FileName);
            var name = Guid.NewGuid().ToString();
            var newFileName = $"{name}{extension}";
            var filePath = Path.Combine(root, "uploads", newFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await picture.CopyToAsync(stream);
            }
            return newFileName;
        }


    }
}
