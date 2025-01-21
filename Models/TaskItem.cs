using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        public User AssignedUser { get; set; }

        public Category Category { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
