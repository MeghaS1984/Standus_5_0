using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Standus_5_0.Areas.Report.Models
{
    [Table("Groups")]
    public class Groups
    {
        [Key]
        public int GroupID { get; set; }
        public string Groupname { get; set; }
    }
}