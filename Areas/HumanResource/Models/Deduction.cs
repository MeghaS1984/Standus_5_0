using System;
using System.ComponentModel;
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
        public string Name { get; set; }

        public string Description { get; set; }

        
        public string? Period { get; set; }

        public string? CutOffType { get; set; }

        public decimal? CutOff { get; set; }

        [DisplayName("Round Off")]
        public string? RoundOff { get; set; }

        public string? Month { get; set; }

        public int? Day { get; set; }

        public bool? Variable { get; set; }

        public int? AccountID { get; set; }

        [DisplayName("On Yearly Income")]
        public bool OnYearlyIncome { get; set; }
        [DisplayName("Payroll Rank")]
        public int PayRollSlNo { get; set; }
        [DisplayName("Deduction Type")]
        public string? Type { get; set; }    

        [DisplayName("In Active")]
        public bool InActive { get; set; }

        public bool Fixed { get; set; }

        public int? DebitTo { get; set; }

        public int? CreditTo { get; set; }

        public int EmployerDebitTo { get; set; }

        public Slab slab { get; set; }
        // Navigation property for Account
        //[ForeignKey("AccountID")]
        //public virtual Account Account { get; set; }

        // Optionally, you can create navigation properties for DebitTo, CreditTo, and EmployerDebitTo if they refer to other entities.
    }
       
}
