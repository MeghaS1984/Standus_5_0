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
namespace Standus_5_0.Areas.HumanResource.Models
{   

    [Table("Holidays")]
    public class Holidays
    {
        [Key]
        [Column("HolidayID")]
        public int ID { get; set; }
        [Column("Date")]
        public string sDate { get; set; }
        public string Reason { get; set; }
        public string Reminder { get; set; }
        public bool Paid { get; set; }
        public int HeadID { get; set; }
    }

}
