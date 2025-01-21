using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models
{
    public class Organization
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public ICollection<RegularUser> Users { get; set; }

        public ICollection<Project> Projects { get; set; }

        [ForeignKey("OwnerId")]
        public string OwnerId { get; set; }

        public Administrator Owner { get; set; }
    }
}
