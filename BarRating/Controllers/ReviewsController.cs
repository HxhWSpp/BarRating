using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BarRating.Data;
using BarRating.Data.Entities;
using BarRating.Models.ReviewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace BarRating.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;      

        public ReviewsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reviews = _context.Reviews.Include(u => u.User).Where(r => r.User.Id == user);
            if (reviews != null)
            {
               return View(reviews.ToList());
            }
            else
            {
               return Problem("Entity set 'ApplicationDbContext.Reviews'  is null.");
            }
            
                                                  
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reviews == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.Include(b => b.Bar).Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Reviews/Create
        public IActionResult Create(int id)
        {
            if (id == null)
            {
                return RedirectToRoute("/Home");
            }
            ViewBag.Id = id;
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize] 
        public async Task<IActionResult> Create(ReviewCreateModel reviewC)
        {
            if (ModelState.IsValid)
            {
                var RBar = _context.Bars.FirstOrDefault(m => m.Id == reviewC.BarId);
                var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == reviewC.UserId);
                if (RBar != null && user != null)
                {
                    Review review = new Review()
                    {
                        Id = 0,
                        Text = reviewC.Text,
                        Bar = RBar,
                        User = user
                    };
                    _context.Add(review);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }                              
            }
            return View();
        }

        // GET: Reviews/Edit/5

        public async Task<IActionResult> Edit(int? id)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reviewt = await _context.Reviews.Include(u => u.User).FirstOrDefaultAsync(i => i.Id == id);
            if (user != reviewt.User.Id)
            {
                return RedirectToAction("Index");
            }
            if (id == null || _context.Reviews == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text")] Review review)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != review.User.Id)
            {
                return RedirectToAction("Index");
            }
            if (id != review.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.Id))
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
            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reviewt = await _context.Reviews.Include(u => u.User).FirstOrDefaultAsync(i => i.Id == id);
            if (userId != reviewt.User.Id)
            {
                return RedirectToAction("Index");
            }
            if (id == null || _context.Reviews == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reviewt = await _context.Reviews.Include(u => u.User).FirstOrDefaultAsync(i => i.Id == id);
            if (user != reviewt.User.Id)
            {
                return RedirectToAction("Index");
            }
            if (_context.Reviews == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Reviews'  is null.");
            }
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
          return (_context.Reviews?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
