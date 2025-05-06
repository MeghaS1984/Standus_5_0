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
namespace Standus_5_0.Areas.Report.Models
{
    [Table("dashboardfilter")]
    public class DashboardFilter
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("Dashboard")]
        public int dahsid { get; set; }
        public string filtertype { get; set; }
        public string filtername { get; set; }
        public virtual Dashboard Dashboard { get; set; }
    }
}
