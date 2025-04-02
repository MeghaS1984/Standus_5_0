using Standus_5_0.Areas.HumanResource.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourNamespace.Models
{
    [Table("AttendanceDetails")]
    public class AttendanceDetails
    {
        public int AttendanceID { get; set; }
        public decimal Overtime { get; set; }
        public DateTime InTime { get; set; }
        public DateTime OutTime { get; set; }
        public string? Reason { get; set; }
        public DateOnly Date { get; set; }
        public int HeadID { get; set; }
        public int EmployeeID { get; set; }
        public int ShiftID { get; set; }
        public int Half1 { get; set; }
        public int Half2 { get; set; }
        public string? Status { get; set; }

        public Employee employee { get; set; }
        public AttendanceHead Head { get; set; }
    }
}
