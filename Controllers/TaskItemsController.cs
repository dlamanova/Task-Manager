using Microsoft.AspNetCore.Mvc;
using TaskManager.Data;
using TaskManager.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Data;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    public class TaskItemsController : Controller
    {
        private readonly TaskManagerContext _context;

        public TaskItemsController(TaskManagerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string category = null)
        {
            var tasks = _context.TaskItems.Include(t => t.AssignedUser).Include(t => t.Category).Include(t => t.Comments);

            if (!string.IsNullOrEmpty(category))
            {
                tasks = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<TaskItem, ICollection<Comment>>)tasks.Where(t => t.Category.Name == category);
            }

            var taskList = new
            {
                Todo = await tasks.Where(t => t.Deadline > DateTime.Now && t.AssignedUser == null).ToListAsync(),
                Current = await tasks.Where(t => t.Deadline > DateTime.Now && t.AssignedUser != null).ToListAsync(),
                Done = await tasks.Where(t => t.Deadline <= DateTime.Now).ToListAsync()
            };

            return View(taskList);
        }

        public async Task<IActionResult> Details(int id)
        {
            var task = await _context.TaskItems.Include(t => t.Comments).ThenInclude(c => c.Author).FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int taskId, string content)
        {
            var task = await _context.TaskItems.FindAsync(taskId);

            if (task == null)
            {
                return NotFound();
            }

            var comment = new Comment
            {
                Content = content,
                CreatedAt = DateTime.Now,
                Task = task,
                Author = await _context.Users.FindAsync(User.Identity.Name)
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = taskId });
        }
    }
}
