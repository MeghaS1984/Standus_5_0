using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.HumanResource.Models
{
    [Table("Slab")]
    public class Slab
    {
        [Key]
        public int SlabID { get; set; } // Primary Key or ID field
        public int DeductionID { get; set; } // Foreign Key or Related Field
        public int AllowanceID { get; set; } // Foreign Key or Related Field
    }

}
