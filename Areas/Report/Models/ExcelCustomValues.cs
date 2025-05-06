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

    [Table("ReportsExcelCustomValues")]
    public class ExcelCustomValues
    {
        [Key]
        public int ID { get; set; }
        [ForeignKey("CustomField")]
        public int CustomFieldID { get; set; }
        public string CustomValue { get; set; }
        public virtual ExcelCustomFields CustomField { get; set; }
    }

}
