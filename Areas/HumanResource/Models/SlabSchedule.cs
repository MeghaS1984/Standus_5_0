using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.HumanResource.Models
{
    [Table("SlabSchedule")]
    public class SlabSchedule
    {
        public int? DeductionID { get; set; }  // Represents SlabID
        public int? AllowanceID { get; set; }  // Represents DetailsID
        public string Month { get; set; }  // Represents Month (e.g., "January", "February", etc.)
    }

}
