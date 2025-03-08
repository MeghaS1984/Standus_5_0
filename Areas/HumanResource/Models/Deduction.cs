using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.HumanResource.Models
{
    [Table("Deduction")]
    public class Deduction
    {
        [Key]
        [Column("DeductionID")]
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [StringLength(50)]
        public string? Period { get; set; }

        [Required]
        [StringLength(50)]
        public string? CutOffType { get; set; }

        public decimal? CutOff { get; set; }

        public string RoundOf { get; set; }

        public string Month { get; set; }

        public int Day { get; set; }

        public bool Variable { get; set; }

        public int? AccountID { get; set; }

        public bool OnYearlyIncome { get; set; }

        public int PayRollSlNo { get; set; }

        public int InActive { get; set; }

        public bool Fixed { get; set; }

        public int DebitTo { get; set; }

        public int CreditTo { get; set; }

        public int EmployerDebitTo { get; set; }

        // Navigation property for Account
        //[ForeignKey("AccountID")]
        //public virtual Account Account { get; set; }

        // Optionally, you can create navigation properties for DebitTo, CreditTo, and EmployerDebitTo if they refer to other entities.
    }

    [Table("SlabDeductionExclude")]
    public class SlabDeductionExclude { 
        public int EmployeeID { get; set; }
        public int DeductionID { get; set; }
        public bool Exclude {  get; set; }

        public Employee Employee { get; set; }
        public Deduction Deduction { get; set; }
    }
}
