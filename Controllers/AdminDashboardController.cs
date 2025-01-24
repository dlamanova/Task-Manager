using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Data;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : Controller
    {
        private readonly TaskManagerContext _context;
        private readonly UserManager<User> _userManager;

        public AdminDashboardController(TaskManagerContext context, UserManager<User> userManager)
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
            var projects = await _context.Projects.ToListAsync();

            var model = new AdminDashboardViewModel
            {
                Tasks = tasks,
                Categories = categories,
                Projects = projects
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskItem task)
        {
            if (task == null || string.IsNullOrWhiteSpace(task.Name))
            {
                return BadRequest("Task name cannot be empty.");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized("User not logged in.");
            }

            task.StatusId = 1;
            task.AssignedUserId = currentUser.Id;

            try
            {
                _context.TaskItems.Add(task);
                await _context.SaveChangesAsync();
                return Ok(new { task.Id, task.Name, task.Description, task.CategoryId, task.StatusId });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database update error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("api/tasks/{id}")]
        public async Task<IActionResult> GetTaskDetails(int id)
        {
            var task = await _context.TaskItems
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return NotFound("Task not found.");
            }

            return Ok(new
            {
                task.Id,
                task.Name,
                task.Description,
                task.CategoryId,
                Category = task.Category?.Name
            });
        }

        [HttpPut("api/tasks/{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItem updatedTask)
        {
            if (updatedTask == null || string.IsNullOrWhiteSpace(updatedTask.Name))
            {
                return BadRequest("Invalid task data.");
            }

            var task = await _context.TaskItems.FindAsync(id);
            if (task == null)
            {
                return NotFound("Task not found.");
            }

            task.Name = updatedTask.Name;
            task.Description = updatedTask.Description;
            task.CategoryId = updatedTask.CategoryId;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(task);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database update error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("api/tasks/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null)
            {
                return NotFound("Task not found.");
            }

            try
            {
                _context.TaskItems.Remove(task);
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Task deleted successfully." });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database update error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("api/tasks/update-statuses")]
        public async Task<IActionResult> UpdateTaskStatuses([FromBody] TaskStatusUpdate[] updates)
        {
            if (updates == null || !updates.Any())
            {
                return BadRequest("No updates provided.");
            }

            foreach (var update in updates)
            {
                var task = await _context.TaskItems.FindAsync(update.Id);
                if (task != null)
                {
                    task.StatusId = update.StatusId;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Task statuses updated successfully." });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Database update error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("api/projects/create")]
        public async Task<IActionResult> CreateProject([FromBody] Project project)
        {
            if (string.IsNullOrWhiteSpace(project.Name))
            {
                return BadRequest("Project name cannot be empty.");
            }

            if (project.TaskItems == null || !project.TaskItems.Any())
            {
                return BadRequest("At least one task must be included in the project.");
            }

            foreach (var task in project.TaskItems)
            {
                if (string.IsNullOrWhiteSpace(task.Name))
                {
                    return BadRequest("Each task must have a name.");
                }

                if (string.IsNullOrWhiteSpace(task.Description))
                {
                    return BadRequest("Each task must have a description.");
                }

                task.CategoryId = task.CategoryId == 0 ? 1 : task.CategoryId; // Default to "Home" category
                task.StatusId = 1; // Default to "TODO" status
            }

            try
            {
                var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(currentUserId))
                {
                    return Unauthorized("User is not logged in.");
                }

                project.OwnerId = currentUserId;

                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

                return Ok(new { project.Id, project.Name });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    public class TaskStatusUpdate
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
    }
}