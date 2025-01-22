namespace TaskManager.Models
{
    public class Administrator : User
    {
        public ICollection<Project> CreatedProjects { get; set; }
    }
}
