using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.HumanResource.Models
{
    [Table("PayRoll")]
    public class Payroll
    {
        [Key]
        public int PayID { get; set; }  // Primary key
        public string ForTheMonth { get; set; }  // For the month (e.g., "January", "February", etc.)
        public DateOnly Date { get; set; }  // The date of the payroll record
        public string ProcessNo { get; set; }  // Process number for the payroll process
        public bool Processed { get; set; }  // Flag to indicate if the payroll has been processed
        public string VoucherNo { get; set; }  // The voucher number associated with this payroll

        public List<PayrollDetails> PayrollDetails { get; set; }
    }

}
