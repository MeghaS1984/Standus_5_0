using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.HumanResource.Models
{
   
    [Table("AttendanceHead")]
    public class AttendanceHead
    {
        [Key]
        [Column("HeadID")]
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string? HeadType { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public bool Paid { get; set; }

        public bool IsLeave { get; set; }

        public bool CarryForward { get; set; }

        public int MaxAllowed { get; set; }
        [Column("Reemberse")]
        public bool Reimburse { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }
        [Column("InActive")]
        public bool IsInactive { get; set; }

        public bool IsDefault { get; set; }

        public bool IsHoliday { get; set; }

        public bool IsHalfDay { get; set; }

        public bool IsHalfDayLeave { get; set; }

        public int Priority { get; set; }

        public bool IsEncashment { get; set; }
    }

}
