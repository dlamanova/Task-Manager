using Microsoft.AspNetCore.Identity;

namespace TaskManager.Models
{
    public class User : IdentityUser
    {
        public ICollection<TaskItem> Tasks { get; set; }
    }
}
