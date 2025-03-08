using Microsoft.EntityFrameworkCore;
using Standus_5_0.Areas.AppSetup.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.HumanResource.Models
{
    [Table("SlabCategory")]
    public class SlabCategory
    {
        public int SlabID { get; set; } // Primary Key or ID field
        public int CategoryID { get; set; } // Foreign Key or Related Field
        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }
    }

}
