using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.HumanResource.Models
{
    [Table("SlabAllowance")]
    public class SlabAllowance
    {
        public int EmployeeID { get; set; }
        public decimal FromAmount { get; set; }
        public decimal ToAmount { get; set; }
        public string Type { get; set; }
        public decimal Employee { get; set; }
        public decimal Employer { get; set; }
        public int AllowanceID { get; set; }
        public decimal Amount { get; set; }
        public decimal Fixed { get; set; }
        public int DetailsID { get; set; }

        // You can add navigation properties here if necessary, for example:
        // public virtual Allowance Allowance { get; set; }
    }

}
