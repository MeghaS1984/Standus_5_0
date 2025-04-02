using System.ComponentModel.DataAnnotations;

namespace Standus_5_0.Areas.HumanResource.Models
{
    public class AllowanceCalculation
    {
        [Key]
        public int SlabCalculationID { get; set; }
        public int AllowanceID { get; set; }
        public string AllowanceName { get; set; }
        public int SlabID { get; set; }
        public int DetailsID { get; set; }
        public string OnIncome { get; set; } // Assuming OnIncome is a decimal and can be null
    }

}
