using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskManager.Data;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskApiController : ControllerBase
    {
        private readonly TaskManagerContext _context;

        public TaskApiController(TaskManagerContext context)
        {
            _context = context;
        }

        [HttpPost("update-status")]
        public async Task<IActionResult> UpdateStatus([FromBody] TaskStatusUpdateDto[] tasks)
        {
            foreach (var taskUpdate in tasks)
            {
                var task = await _context.TaskItems.FindAsync(taskUpdate.Id);
                if (task != null)
                {
                    task.StatusId = taskUpdate.StatusId;
                }
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }

    public class TaskStatusUpdateDto
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
    }
}
