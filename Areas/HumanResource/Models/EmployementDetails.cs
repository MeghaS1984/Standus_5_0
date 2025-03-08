using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.HumanResource.Models
{
    
    public class EmployementDetails : Employee    {
        
        [Required]
        public string Code { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string MaritalStatus { get; set; }
        [Required]
        public string PermanantAddress { get; set; }
        
        public string Pincode1 { get; set; }
        public string? WeeklyHoliday { get; set; }
        public int GradeID { get; set; }
        public string? ShiftType{ get; set; }
        public string? WagesType { get; set;}
        
        public string? PaymentType { get; set; }        
}
}
