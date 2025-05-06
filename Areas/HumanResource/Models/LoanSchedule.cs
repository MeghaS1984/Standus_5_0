using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.HumanResource.Models
{
    [Table("LoanSchedule")]
    public class LoanSchedule
    {
        [Key]
        public int ID { get; set; }
        public int SanctionID { get; set; }          // Primary key for the sanction (int type)
        public int Installment { get; set; }     // The installment amount (decimal type)
        public decimal Amount { get; set; }          // Total amount for the installment (decimal type)
        public decimal Paid { get; set; }            // Amount paid for this installment (decimal type)
        public bool Forward { get; set; }            // Flag to check if the installment is forward (bool type)
        [DataType("Date")]
        public DateTime Date { get; set; }           // The date when the installment is due (DateTime type)
        [DataType("Date")]
        public DateTime SalaryDate { get; set; }     // The date the salary will be paid (DateTime type)
        //public bool Skip { get; set; }               // Flag to check if this installment was skipped (bool type)
        [ForeignKey("SanctionID")]
        public virtual LoanSanction Sanction { get; set; }
    }

}
