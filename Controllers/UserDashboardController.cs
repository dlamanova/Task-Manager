using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Data;
using TaskManager.Models;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Controllers
{
    public class UserDashboardController : Controller
    {
        private readonly TaskManagerContext _context;
        private readonly UserManager<User> _userManager;

        public UserDashboardController(TaskManagerContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var tasks = await _context.TaskItems
                .Where(t => t.AssignedUserId == currentUser.Id)
                .ToListAsync();

            var categories = await _context.Categories.ToListAsync();

            var model = new UserDashboardViewModel
            {
                Tasks = tasks,
                Categories = categories
            };

            return View(model);
        }
    }
}
