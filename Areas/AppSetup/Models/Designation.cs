using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Standus_5_0.Areas.AppSetup.Models
{
    [Table("Designation")]
    public class Designation
    {
        [Key]
        [Column("DesignationID")]
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string? DesignationName { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public int Level { get; set; }

        public decimal Min { get; set; }

        public decimal Max { get; set; }

        public int CategoryID { get; set; }

        public bool InActive { get; set; }
    }

}
