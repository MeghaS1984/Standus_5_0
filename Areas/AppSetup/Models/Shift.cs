using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.AppSetup.Models
{
    [Table("Shift")]
    public class Shift
    {
        [Key]
        public int ShiftID { get; set; }
        [DisplayName("Shift")]
        public string ShiftNo { get; set; }
        public string Description { get; set; }
        [DisplayName("Start Time")]
        public TimeSpan StartTime { get; set; }
        [DisplayName("End Time")]
        public TimeSpan EndTime { get; set; }
        [DisplayName("Break Start")]
        public TimeSpan BreakStartTime { get; set; }
        [DisplayName("Break End")]
        public TimeSpan BreakEndTime { get; set; }
        public bool InActive { get; set; }
    }

}
