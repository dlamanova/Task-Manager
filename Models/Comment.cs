using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public TaskItem Task { get; set; }

        public User Author { get; set; }
    }
}
