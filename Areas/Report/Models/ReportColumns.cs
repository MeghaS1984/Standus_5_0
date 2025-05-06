using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Standus_5_0.Areas.Report.Models
{
    [Table("ReportColumns")]
    public class ReportColumns
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Report")]
        public int ReportID { get; set; }
        public string ColumnName { get; set; }
        public decimal Width { get; set; }
        public string SummeryType { get; set; }
        public string? LinkTo { get; set; }
        public int ColumnNo { get; set; } = 0;
        public string GroupBy { get; set; }
        public int GroupLevel { get; set; } 
        public bool HideDuplicates { get; set; }
        public bool HideColumn { get; set; } 

        public virtual Reports Report { get; set; }
    }

}
