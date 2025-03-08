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
//using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;

namespace Standus_5_0.Areas.HumanResource.Models
{


    [Table("Employee")]
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        [Column("PhoneNo")]
        public string Phone { get; set; }

        [StringLength(200)]
        [Column("CurrentAddress")]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column("DOB")]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column("DOJ")]
        public DateTime HireDate { get; set; }
        public double? Salary { get; set; }
        // Foreign Keys
        public int DepartmentID { get; set; }
        [Column("DesignationId")]
        public int PositionID { get; set; }

        // Navigation Properties
        //[ForeignKey("DepartmentID")]
        // public virtual Department Department { get; set; }
        public string? Discriminator { get; set; }

        [ForeignKey("PositionID")]
        public virtual Position? Position { get; set; }

        public virtual ICollection<EmploymentHistory>? EmploymentHistories { get; set; }
        public virtual ICollection<PerformanceEvaluation>? PerformanceEvaluations { get; set; }
        public virtual ICollection<Education>? Education { get; set; }
        public virtual ICollection<Family>? Dependants { get; set; }
        public virtual ICollection<Statutory>? Statutory { get; set; }
    }

}
