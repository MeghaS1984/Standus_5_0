using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.Report.Models
{
    [Table("dashboard")]
    public class Dashboard
    {
        [Key]
        public int ID { get; set; }
        public string? title { get; set; }
        public string? query { get; set; }
        public string? charttype { get; set; }
        public string? xvalue { get; set; }
        public string? yovalue { get; set; }
        public string? ytvalue { get; set; }
        public string? filterstring { get; set; }
        public virtual ICollection<DashboardFilter> filters { get; set; }
    }
}
