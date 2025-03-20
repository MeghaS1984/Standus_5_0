using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.HumanResource.Models
{
    [Table("SlabCalculation")]
    public class SlabCalculation
    {
        public int SlabID { get; set; }
        public int AllowanceID { get; set; }
        public int? DetailsID { get; set; }  // Assuming DetailsID can be nullable
        public int? DeductionID { get; set; }  // Assuming DeductionID can be nullable
        public String OnIncome { get; set; }

        // You may add navigation properties if you have related entities in your DbContext
        // public virtual Allowance Allowance { get; set; }
        // public virtual Deduction Deduction { get; set; }
    }

}
