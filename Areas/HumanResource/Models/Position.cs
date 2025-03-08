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
using Standus_5_0.Areas.AppSetup.Models;

namespace Standus_5_0.Areas.HumanResource.Models
{

    [Table("positions")]
    public class Position
    {
        [Key]
        public int PositionID { get; set; }

        [Required]
        [StringLength(100)]
        public string? PositionName { get; set; }

        [StringLength(250)]
        public string? Description { get; set; }

        // Foreign Key
        public int? DepartmentID { get; set; }

        // Navigation Properties
        [ForeignKey("DepartmentID")]
        public virtual Department? Department { get; set; }

        public virtual ICollection<Employee>? Employees { get; set; }
    }

}
