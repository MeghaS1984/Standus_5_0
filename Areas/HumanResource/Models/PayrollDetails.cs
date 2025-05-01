using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Standus_5_0.Areas.HumanResource.Models
{
    [Table("PayrollDetails")]
    public class PayrollDetails
    {
        
        public int PayID { get; set; }

        [Required]
        public int EmployeeID { get; set; }
        [Column("Employee")]
        public decimal EmployeeAmount { get; set; }  // Optional: consider moving to Employee table

        public int? AllowanceID { get; set; }

        public int? DeductionID { get; set; }

        public decimal Employer { get; set; }

        public int? CategoryID { get; set; }

        public decimal Paid { get; set; }

       
        public decimal UnPaid { get; set; }

       
        public decimal FromAmount { get; set; }
       
        public decimal ToAmount { get; set; }
        public bool Fixed { get; set; }       
        public decimal Amount { get; set; }       
        public decimal EmployeeContribution { get; set; }       
        public decimal EmployerContribution { get; set; }
        public string Type { get; set; }

        [Key]
        public int ID { get; set; } // unclear what this is, could rename/remove

        // Relationships (if using EF Core)
         public virtual Employee Employee { get; set; }
         public virtual Allowance Allowance { get; set; }
         public virtual Deduction Deduction { get; set; }
         public virtual Standus_5_0.Areas.AppSetup.Models.Category Category { get; set; }
        //[ForeignKey("PayId")]
        public virtual Payroll Payroll { get; set; }
    }

}
