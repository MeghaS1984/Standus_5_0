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
    [Table("ReportsExcelCustomFields")]
    public class ExcelCustomFields
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("Report")]
        public int ReportID { get; set; }
        public string FieldType { get; set; }
        public string FieldName { get; set; }
        public virtual Reports Report { get; set; }

        public virtual ICollection<ExcelCustomValues> CustomValues { get; set; }
    }

}
