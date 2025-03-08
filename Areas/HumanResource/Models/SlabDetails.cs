
using Standus_5_0.Areas.AppSetup.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.HumanResource.Models
{
    [Table("SlabDetails")]
    public class SlabDetails
    {
        public int SlabID { get; set; } // Primary key, usually an int type
        public decimal FromAmount { get; set; } // Starting amount for the slab (decimal type for money)
        public decimal ToAmount { get; set; } // Ending amount for the slab
        public string Type { get; set; } // Type of the slab (could be string, enum, etc.)
        public decimal Employee { get; set; } // Employee contribution amount (decimal type for monetary values)
        public decimal Employer { get; set; } // Employer contribution amount (decimal type for monetary values)
        [Key]
        [Column("DetailsID")]
        public int ID { get; set; } // Foreign key for related details
        public int CategoryID { get; set; } // Foreign key for the category (possibly another table)
        [ForeignKey("CategoryID")]
        public virtual Category? Category { get; set; } // Navigation property to Category
    }

}
