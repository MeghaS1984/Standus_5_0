using Microsoft.EntityFrameworkCore;
using Standus_5_0.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.HumanResource.Models
{
    [Table("PaySheetSummarySetting")]
    public class PaySheetSummarySetting
    {
        [Key]
        public int ID { get; set; }
        public string Query { get; set; }
        public string Design { get; set; }
    }
}
