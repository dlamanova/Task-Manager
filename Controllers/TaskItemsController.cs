//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using TaskManager.Data;
//using TaskManager.Models;

//namespace TaskManager.Controllers
//{
//    [ApiController]
//    [Route("api/tasks")]
//    public class TaskItemsController : ControllerBase
//    {
//        private readonly TaskManagerContext _context;

//        public TaskItemsController(TaskManagerContext context)
//        {
//            _context = context;
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetTaskDetails(int id)
//        {
//            var task = await _context.TaskItems
//                .Include(t => t.Category) // Include category if needed
//                .FirstOrDefaultAsync(t => t.Id == id);

//            if (task == null)
//            {
//                return NotFound("Task not found.");
//            }

//            return Ok(new
//            {
//                task.Id,
//                task.Name,
//                task.Description,
//                Category = task.Category?.Name,
//                task.CategoryId
//            });
//        }

//        [HttpPost("create")]
//        public async Task<IActionResult> CreateTask([FromBody] TaskItem task)
//        {
//            if (task == null || string.IsNullOrWhiteSpace(task.Name))
//            {
//                return BadRequest("Task name cannot be empty.");
//            }

//            // Set default values
//            task.StatusId = 1; // Default to "TODO"

//            // Simulate a logged-in user for testing purposes
//            task.AssignedUserId = "test-user-id"; // Replace with real user ID in production

//            try
//            {
//                _context.TaskItems.Add(task);
//                await _context.SaveChangesAsync();

//                return Ok(new
//                {
//                    task.Id,
//                    task.Name,
//                    task.Description,
//                    task.CategoryId,
//                    task.StatusId
//                });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Internal server error: {ex.Message}");
//            }
//        }



//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskItem updatedTask)
//        {
//            if (updatedTask == null)
//            {
//                return BadRequest("Invalid task data.");
//            }

//            var task = await _context.TaskItems.FindAsync(id);
//            if (task == null)
//            {
//                return NotFound("Task not found.");
//            }

//            task.Name = updatedTask.Name;
//            task.Description = updatedTask.Description;
//            task.CategoryId = updatedTask.CategoryId;

//            await _context.SaveChangesAsync();
//            return Ok(task);
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteTask(int id)
//        {
//            var task = await _context.TaskItems.FindAsync(id);
//            if (task == null)
//            {
//                return NotFound("Task not found.");
//            }

//            _context.TaskItems.Remove(task);
//            await _context.SaveChangesAsync();
//            return Ok();
//        }

//        [HttpPost("update-statuses")]
//        public async Task<IActionResult> UpdateTaskStatuses([FromBody] List<TaskStatusUpdate> taskStatusUpdates)
//        {
//            if (taskStatusUpdates == null || !taskStatusUpdates.Any())
//            {
//                return BadRequest("No task data received.");
//            }

//            foreach (var update in taskStatusUpdates)
//            {
//                var task = await _context.TaskItems.FindAsync(update.Id);
//                if (task != null)
//                {
//                    task.StatusId = update.StatusId;
//                }
//            }

//            await _context.SaveChangesAsync();
//            return Ok("Task statuses updated successfully.");
//        }

//        public class TaskStatusUpdate
//        {
//            public int Id { get; set; }
//            public int StatusId { get; set; }
//        }
//    }
//}
