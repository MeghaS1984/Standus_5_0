using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.HumanResource.Models
{
    [Table("LoanRequest")]
    public class LoanRequest
    {
        [Key]
        public int RequestID { get; set; }  // Primary key (int type)
        public string RequestNo { get; set; }  // Request number (string type)
        public int EmployeeId { get; set; }  // Foreign key to Employee (int type)
        public DateTime Date { get; set; }  // Date of the request (DateTime type)
        public decimal Amount { get; set; }  // Amount requested (decimal type for monetary values)
        public string Reason { get; set; }  // Reason for the loan request (string type)
        public string LoanType { get; set; }  // Type of loan (string type)
        public string Bank { get; set; }  // Bank name (string type)
        public string Status { get; set; }  // Status of the loan request (e.g., Pending, Approved, Rejected)
        public string Comments { get; set; }  // Additional comments (string type)

        // Navigation property for Employee (if you have an Employee model)
        public virtual Employee Employee { get; set; }
    }

}
