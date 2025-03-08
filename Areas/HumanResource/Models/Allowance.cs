namespace Standus_5_0.Areas.HumanResource.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Allowance")]
    public class Allowance
    {
        [Key]
        [Column("AllowanceID")]
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [StringLength(50)]
        public string? CutOffType { get; set; }

        public string? Period { get; set; } // Period can be stored as string, modify type as necessary

        public Boolean? DaysPresent { get; set; }

        public Boolean? AuthorisedLeave { get; set; }

        public bool? GeneralHoliday { get; set; }

        public decimal? CutOff { get; set; }

        public String? RoundOf { get; set; }

        public int? AccountID { get; set; }

        public string? Month { get; set; }

        public int Day { get; set; }

        public int Variable { get; set; }

        public int PayrollSlNO { get; set; }

        public bool InActive { get; set; }

        public bool Fixed { get; set; }

        public int DebitTo { get; set; }

        public int CreditTo { get; set; }

        //// Navigation property for Account
        //[ForeignKey("AccountID")]
        //public virtual Account Account { get; set; }

        // Optionally, you can create navigation properties for DebitTo and CreditTo if they refer to other entities.
    }

    [Table("IncentiveSetting")]
    public class IncentiveSetting { 
        public int EmployeeID { get; set; } 
        public bool Incentive { get; set; }
        public bool AddnIncentive {  get; set; }

        [ForeignKey("EmployeeID")]
        public Employee? EmployeeDetails { get; set; }
    }

}
