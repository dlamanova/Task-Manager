namespace TaskManager.Models
{
    public class AdminDashboardViewModel
    {
        public IEnumerable<TaskItem> Tasks { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Project> Projects { get; set; }
    }
}
