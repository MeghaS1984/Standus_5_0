using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.HumanResource.Models
{
    [Table("LoanForward")]
    public class LoanForward
    {
        [Key]
        public int ForwardId { get; set; }         // Primary key for the forward sanction (int type)
        public int SanctionID { get; set; }        // Foreign key referencing the Sanction table (int type)
        public decimal Installment { get; set; }   // Installment amount (decimal type)
        public string Type { get; set; }           // Type of the forward (e.g., "Loan", "Bonus") (string type)
        public string Reason { get; set; }         // Reason for the forward sanction (string type)
        public DateTime Date { get; set; }         // Date when the forward sanction was applied (DateTime type)

        // Navigation property (optional, if you want to include related data from the Sanction table)
        [ForeignKey("SanctionID")]
        public virtual LoanSanction? Sanction { get; set; }  // Navigation property to Sanction (if it exists)
    }

}
