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


    public class PerformanceEvaluation
    {
        [Key]
        public int EvaluationID { get; set; }

        // Foreign Key
        public int EmployeeID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EvaluationDate { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(1000)]
        public string Comments { get; set; }

        // Navigation Property
        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }
    }

}
