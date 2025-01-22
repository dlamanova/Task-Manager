using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<TaskItem> TaskItems { get; set; }

        [ForeignKey("OwnerId")]
        public string OwnerId { get; set; }

        public Administrator Owner { get; set; }
    }
}
