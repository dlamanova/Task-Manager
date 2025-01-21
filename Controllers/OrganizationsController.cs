using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    public class OrganizationsController : Controller
    {
        private readonly TaskManagerContext _context;

        public OrganizationsController(TaskManagerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var organizations = await _context.Organizations.Include(o => o.Users).ToListAsync();
            return View(organizations);
        }

        public async Task<IActionResult> Details(int id)
        {
            var organization = await _context.Organizations.Include(o => o.Projects).Include(o => o.Users).FirstOrDefaultAsync(o => o.Id == id);

            if (organization == null)
            {
                return NotFound();
            }

            return View(organization);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Organization organization)
        {
            if (ModelState.IsValid)
            {
                _context.Organizations.Add(organization);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(organization);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(int organizationId, string userId)
        {
            var organization = await _context.Organizations.FindAsync(organizationId);
            var user = await _context.Users.FindAsync(userId);

            if (organization == null || user == null)
            {
                return NotFound();
            }

            (user as RegularUser).Organization = organization;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = organizationId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUser(int organizationId, string userId)
        {
            var organization = await _context.Organizations.FindAsync(organizationId);
            var user = await _context.Users.FindAsync(userId);

            if (organization == null || user == null)
            {
                return NotFound();
            }

            (user as RegularUser).Organization = null;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = organizationId });
        }
    }
}
