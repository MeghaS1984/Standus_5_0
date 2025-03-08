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
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.HumanResource.Models
{


    [Table("EmployeeFamily")]
    public class Family
    {
        public int EmployeeID { get; set; }
        public string? Relation { get; set; }
        public string? Name { get; set; }
        public decimal Age { get; set; }

        [Key]
        [Column("SlNO")]
        public int ID { get; set; }

        [ForeignKey("EmployeeID")]
        public virtual Employee? Employee { get; set; }
    }

}
