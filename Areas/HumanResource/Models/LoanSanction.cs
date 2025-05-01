using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.HumanResource.Models
{
    [Table("LoanSanction")]
    public class LoanSanction
    {
        public int RequestID { get; set; }        // Primary key (int type)
        [DataType("Date")]
        public DateTime Date { get; set; }        // Date of the loan deduction request (DateTime type)
        public decimal Amount { get; set; }       // The amount of the loan deduction (decimal type)
        public decimal Interest { get; set; }     // Interest charged on the loan (decimal type)
        public string? DeductionType { get; set; } // Type of the deduction (string type)
        public decimal Installment { get; set; }  // Installment amount (decimal type)
        public int InstallmentNo { get; set; }    // Installment number (int type)
        [DataType("Date")]
        public DateTime InstallmentDate { get; set; } // Date when the installment is due (DateTime type)
        [Key]
        public int SanctionId { get; set; }       // Foreign key for sanction (int type)
        public int DeductionId { get; set; }      // Foreign key for deduction (int type)

        // Navigation Properties (optional, if you want to include related data)
        [ForeignKey("RequestID")]
        public virtual LoanRequest Request { get; set; }   // Navigation property to Sanction
        
        public Deduction Deduction { get; set; }  // Navigation property to Deduction
        
    }

}
