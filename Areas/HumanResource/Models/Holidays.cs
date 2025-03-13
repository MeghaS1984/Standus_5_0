using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
namespace Standus_5_0.Areas.HumanResource.Models
{

    [Table("Holidays")]
    public class Holidays
    {
        [Key]
        [Column("HolidayID")]
        public int ID { get; set; }
        [Required]
        [Column("Date")]
        [DisplayName("Date")]
        [DataType(DataType.Date)]
        public DateTime  sDate { get; set; }
        public string Reason { get; set; }
        [Required]
        [DisplayName("Date")]
        [DataType(DataType.Date)]
        public DateTime Reminder { get; set; }
        public bool Paid { get; set; }
        [DisplayName("Attendance Head")]
        public int HeadID { get; set; }

        public AttendanceHead Head { get; set; }
    }

}
