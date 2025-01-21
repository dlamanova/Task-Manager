namespace TaskManager.Models
{
    public class Administrator : User
    {
        public ICollection<Organization> CreatedOrganizations { get; set; }
        public ICollection<Project> CreatedProjects { get; set; }
    }
}
