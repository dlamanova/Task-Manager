using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [ForeignKey("AssignedUserId")]
        public User? AssignedUser { get; set; }
        public string? AssignedUserId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public int CategoryId { get; set; }

        [ForeignKey("StatusId")]
        public Status Status { get; set; }
        public int StatusId { get; set; }
    }
}
