using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
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

namespace Standus_5_0.Areas.Report.Models
{
    
[Table("ExcelColumns")]
    public class ExcelColumns
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("Report")]
        public int ReportID { get; set; }
        public string ColumnName { get; set; }
        public string Number { get; set; }
        public virtual Reports Report { get; set; }
    }

}
