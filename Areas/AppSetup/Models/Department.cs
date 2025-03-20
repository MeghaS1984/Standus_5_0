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
using Microsoft.EntityFrameworkCore;
namespace Standus_5_0.Areas.AppSetup.Models
{


    [Table("Department")]
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }
        [Column("Department")]
        [Required]
        public string? DepartmentName { get; set; }
        [Required]
        public string? Description { get; set; }
        public bool InActive { get; set; }
    }

    /* public class DepartmentContext : DbContext
     {
         public DepartmentContext(string connString) : base(connString)
         {
         }

         public DbSet<Department> Departments { get; set; }
     }*/

}
