using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Standus_5_0.Areas.Report.Models
{
    [Table("Reports")]
    public class Reports
    {
        [Key]
        public int ReportID { get; set; }
        public string? ReportName { get; set; }
        public string? SqlQuery { get; set; }
        public int GroupID { get; set; }
        public string ReportType { get; set; }
        public string AutoMailTo { get; set; }
        public string PDFPageSize { get; set; }
        public bool IsExcel { get; set; }
        public string FilePath { get; set; }
        public string Ref_Table { get; set; }
        public DateTime? Date_Created { get; set; }
        public DateTime? Date_updated { get; set; }
        public bool Hide { get; set; }
        // Public Overridable Property Columns As New List(Of ReportColumns)
        public virtual Groups group { get; set; }

        public virtual ICollection<ReportColumns> Columns { get; set; }
        public virtual ICollection<ExcelColumns> ExcelColumns { get; set; }
        public virtual ICollection<ExcelCustomFields> CustomFields { get; set; }
    }

}
