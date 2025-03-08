using Microsoft.CodeAnalysis.Elfie.Serialization;
using Standus_5_0.Areas.AppSetup.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.HumanResource.Models
{
    [Table("LeaveAllocation")]
    public class LeaveAllocation
    {
        [Key]
        [Column("AllocationID")]
        public int ID { get; set; }

        // Foreign Key for Category
        public int CategoryID { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        // Foreign Key for Head
        public int HeadID { get; set; }

        // Navigation Properties
        [ForeignKey("CategoryID")]
        public virtual Category? Category { get; set; }

        [ForeignKey("HeadID")]
        public virtual AttendanceHead? Head { get; set; }
    }

}
