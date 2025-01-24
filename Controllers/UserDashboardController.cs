using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Data;
using TaskManager.Models;

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

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskItem task)
        {
            if (task == null || string.IsNullOrWhiteSpace(task.Name))
            {
                return BadRequest("Task name cannot be empty.");
            }

            task.StatusId = 1; // Default to "TODO"
            task.AssignedUserId = (await _userManager.GetUserAsync(User))?.Id; // Assign to current user

            if (string.IsNullOrEmpty(task.AssignedUserId))
            {
                return Unauthorized("User not logged in.");
            }

            try
            {
                _context.TaskItems.Add(task);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    task.Id,
                    task.Name,
                    task.Description,
                    task.CategoryId,
                    task.StatusId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("api/tasks/{id}")]
        public async Task<IActionResult> GetTaskDetails(int id)
        {
            var task = await _context.TaskItems
                .Include(t => t.Category) // Include category
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


        [HttpPost]
        public async Task<IActionResult> UpdateTaskStatuses([FromBody] List<TaskStatusUpdate> taskStatusUpdates)
        {
            if (taskStatusUpdates == null || !taskStatusUpdates.Any())
            {
                return BadRequest("No task data received.");
            }

            foreach (var update in taskStatusUpdates)
            {
                var task = await _context.TaskItems.FindAsync(update.Id);
                if (task != null)
                {
                    task.StatusId = update.StatusId;
                }
            }

            await _context.SaveChangesAsync();
            return Ok("Task statuses updated successfully.");
        }

        [HttpPut]
        [Route("api/tasks/{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItem updatedTask)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null)
            {
                return NotFound("Task not found.");
            }

            task.Name = updatedTask.Name;
            task.Description = updatedTask.Description;
            task.CategoryId = updatedTask.CategoryId;

            await _context.SaveChangesAsync();
            return Ok(task);
        }


        [HttpDelete]
        [Route("api/tasks/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null)
            {
                return NotFound("Task not found.");
            }

            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Task deleted successfully." });
        }


        public class TaskStatusUpdate
        {
            public int Id { get; set; }
            public int StatusId { get; set; }
        }
    }
}
