using BarRating.Data;
using BarRating.Data.Entities;
using BarRating.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BarRating.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(ILogger<HomeController> logger , ApplicationDbContext context , UserManager<AppUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var dict = new Dictionary<string, int>();
            ViewData["counts"] = dict;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return View();
            }
            var user = await _userManager.FindByIdAsync(userId);
            bool check = await _userManager.IsInRoleAsync(user, "Administrator");
            
            if (check == true)
            {
                var users = _context.Users.ToList().Count();
                var bars = _context.Bars.ToList().Count();
                var reviews = _context.Reviews.ToList().Count();
               
                dict.Add("Users", users);
                dict.Add("Bars", bars);
                dict.Add("Reviews" , reviews);
                ViewData["counts"] = dict;
                return View();
            }
            
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}