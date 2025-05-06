using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.Report.Models
{
    [Table("ReportFilters")]
    public class ReportFilters
    {
        [Key]
        public int Id { get; set; }
        // <ForeignKey("Report")>
        public int ReportID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string? Table { get; set; }
        public string? ValueField { get; set; }
        public string? DisplayField { get; set; }
        public string? DefaultValue { get; set; }
        public string? ShowAll { get; set; }
        public string? CustomFilter { get; set; }
        public string? LinkField { get; set; }
    }

}
