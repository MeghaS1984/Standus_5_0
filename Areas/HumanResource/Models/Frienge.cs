using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.HumanResource.Models
{
    [Table("Frienge")]
    public class Frienge
    {
        [Key]
        [Column("FriengeID")]
        public int Id { get; set; }
        public string? FriengeType { get; set; } // Type of the fringe benefit (string type)
        public string? PayrollSlNO { get; set; } // Payroll slip number (string type, as it might contain alphanumeric characters)
        public bool InActive { get; set; } // Represents if it's inactive or not (bool type, assuming 0 for active, 1 for inactive)
    }

}
