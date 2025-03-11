using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourNamespace.Models
{
    [Table("AttendanceDetails")]
    public class AttendanceDetails
    {
        public int AttendanceID { get; set; }
        public TimeSpan Overtime { get; set; }
        public DateTime InTime { get; set; }
        public DateTime OutTime { get; set; }
        public string? Reason { get; set; }
        public DateTime Date { get; set; }
        public int HeadID { get; set; }
        public int EmployeeID { get; set; }
        public int ShiftID { get; set; }
        public decimal Half1 { get; set; }
        public decimal Half2 { get; set; }
        public string? Status { get; set; }
    }
}
