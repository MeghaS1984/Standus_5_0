using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Standus_5_0.Areas.Report.Models
{
    public class ReportPreview : ReportComponent
    {
        public List<ReportsSubquery> subquery { get; set; }
        public List<ExcelColumns> excelcolumns { get; set; }

        public List<String> tables { get; set; }

        public ReportPreview()
        {
            List<String> tables = new List<String>();
        }
    }
}
