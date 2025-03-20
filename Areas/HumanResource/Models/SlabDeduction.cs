using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.HumanResource.Models
{
    [Table("SlabDeduction")]
    public class SlabDeduction
    {
        public int EmployeeID { get; set; }
        public decimal FromAmount { get; set; }
        public decimal ToAmount { get; set; }
        public string Type { get; set; }
        public decimal Employee { get; set; }
        public decimal Employer { get; set; }
        public int DeductionID { get; set; }
        public decimal Amount { get; set; }
        public decimal Fixed { get; set; }
        public int DetailsID { get; set; }

        // Optional navigation properties
        // public virtual Deduction Deduction { get; set; }
    }

}
