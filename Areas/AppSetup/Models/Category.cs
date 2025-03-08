namespace Standus_5_0.Areas.AppSetup.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("Category")]
    public class Category
    {
        [Key]
        [Column("CategoryID")]
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        [Column("Category")]
        public string? CategoryName { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
        public string? Wages { get; set; }
        public bool OT { get; set; }

        public int AccountID { get; set; }

        public bool InActive { get; set; }

        // Navigation property
        //[ForeignKey("AccountID")]
        //public virtual Account Account { get; set; }
    }

}
