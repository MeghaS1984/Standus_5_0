using System;
using System.Collections.Generic;
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
    public class ReportCharts
    {
        [Key]
        public int Id { get; set; }
        // <ForeignKey("Report")>
        public int ReportID { get; set; }
        public string ChartType { get; set; }
        public string XValue { get; set; }
        public string YOValue { get; set; }
        public string YTValue { get; set; }
    }

}
