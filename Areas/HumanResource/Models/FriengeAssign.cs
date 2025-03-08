using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.HumanResource.Models
{
    [Table("FriengeAssign")]
    public class FriengeAssign
    {        
        public int FriengeID { get; set; }   // Primary key (int type)
        public int EmployeeID { get; set; }  // Foreign key to Employee (int type)
        public decimal Amount { get; set; } // The amount associated with the fringe (decimal type)

        // Navigation Property for Employee (if you want to include related Employee data)
        [ForeignKey("EmployeeID")]
        public virtual Employee? Employee { get; set; }
    }

}
