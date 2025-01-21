using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models
{
    public class RegularUser : User
    {
        [ForeignKey("OrganizationId")]
        public Organization Organization { get; set; }
    }
}
