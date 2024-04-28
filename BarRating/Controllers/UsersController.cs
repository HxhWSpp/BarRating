using BarRating.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BarRating.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        public UsersController(UserManager<AppUser> userManager)
        {
              _userManager = userManager;
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {        
            var users = _userManager.Users.ToList();
            return View(users);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);           
            if (user != null)
            {
                await _userManager.DeleteAsync(user);            
            }
            
            return RedirectToAction(nameof(Index));
        }

       

        [HttpGet("Edit/{id}")]
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> Edit(string? id)
        {
            
            
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> EditPost(string id, [Bind("FirstName,LastName")] AppUser user)
        {                                    
            if (ModelState.IsValid)
            {
                var Auser = await _userManager.FindByIdAsync(id);
                Auser.FirstName = user.FirstName;
                Auser.LastName = user.LastName;
                
                  var result =  await _userManager.UpdateAsync(user);
                              
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
