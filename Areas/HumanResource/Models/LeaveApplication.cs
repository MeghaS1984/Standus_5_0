namespace Standus_5_0.Areas.HumanResource.Models
{
    using Microsoft.CodeAnalysis.Elfie.Serialization;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("LeaveApplication")]
    public class LeaveApplication
    {
        [Key]
        [Column("ApplicationID")]
        public int id { get; set; }

        // Foreign Key for Employee
        public int EmployeeID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public decimal Days { get; set; }

        public string? Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        // Foreign Key for Head
        public int HeadID { get; set; }

        public string? status { get; set; } 
        public string? comment { get; set; }
        public bool? cancel {  get; set; }

        public int ApproverID { get; set; }

        public string? PendingForApproval { get; set; }

        // Navigation Properties
        [ForeignKey("EmployeeID")]
        public virtual Employee? Employee { get; set; }

        [ForeignKey("HeadID")]
        public virtual AttendanceHead? Head { get; set; }
    }

}
