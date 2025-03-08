using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.HumanResource.Models
{
    [Table("StandardDeduction")]
    public class StandardDeduction
    {
        public int EmployeeID { get; set; }
        public string Type { get; set; }
        public string Employee { get; set; }
        public string Employer { get; set; }
        public int DeductionID { get; set; }

        // Navigation Properties
        public Employee EmployeeDetails { get; set; } // Assuming you have an Employee class
        public Deduction DeductionDetails { get; set; } // Assuming you have a Deduction class
    }

    [Table("StandardDeductionCalculation")]
    public class StandardDeductionCalculation { 
        public int DeductionID { get; set; }    
        public int AllowanceID { get; set; }
    }
}
