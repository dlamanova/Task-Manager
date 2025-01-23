namespace TaskManager.Models
{
    public class UserDashboardViewModel
    {
        public IEnumerable<TaskItem> Tasks { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
