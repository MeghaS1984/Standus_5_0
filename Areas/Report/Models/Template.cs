using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Standus_5_0.Areas.Report.Models
{

    [Table("Template")]
    public class Template
    {
        [Key]
        [Column("TemplateID")]
        public int ID { get; set; }
        public string Header { get; set; }
        public string Footer { get; set; }
        public string Details { get; set; }
        public string Query { get; set; }
        public string Name { get; set; }
        public string Screen { get; set; }
        public string FilterString { get; set; }
    }

}
