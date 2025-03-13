using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.HumanResource.Models
{
    [Table("LeaveAllocationDetails")]
    public class LeaveAllocationDetails
    {
        public int EmployeeID { get; set; }  // Represents the Employee's ID
        public Decimal Days { get; set; }        // Represents the number of days
        public bool Closed { get; set; }     // Indicates if the task is closed (true or false)
        [Key]
        public int ID { get; set; }          // Unique task identifier
        public DateTime StartDate { get; set; }  // Start date of the task
        public int HeadID { get; set; }      // ID of the supervisor or manager
        public DateTime EndDate { get; set; }    // End date of the task

        public Employee Employee { get; set; }
        public AttendanceHead Head { get; set; }
    }

}
