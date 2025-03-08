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


    [Table("EmployeeEducation")]
    public class Education
    {
        public int EmployeeID { get; set; }
        [Column("Education")]
        public string? EducationType { get; set; }
        [Key]
        [Column("SlNO")]
        public int ID { get; set; }

        [ForeignKey("EmployeeID")]
        public virtual Employee? Employee { get; set; }
    }

}
