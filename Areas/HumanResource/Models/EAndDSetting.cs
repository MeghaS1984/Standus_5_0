using Microsoft.EntityFrameworkCore;
using Standus_5_0.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.HumanResource.Models
{
    [Table("EAndDSetting")]
    public class EAndDSetting
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "Query is required.")]
        public string Query { get; set; }
        public int? AllowanceID { get; set; }
        public int? DeductionID { get; set; }

        public Allowance Allowance { get; set; }
        public Deduction Deduction { get; set; }
        public virtual ICollection<EAndDSettingParam> EAndDSettingParams { get; set; } = new List<EAndDSettingParam>();
    }

    [Table("EAndDSettingParam")]
    public class EAndDSettingParam {
        [Key]
        public int ID { get; set; }
        public int QueryId { get; set; }
        public string QueryParam { get; set; }
        [ForeignKey("QueryId")]
        public virtual Standus_5_0.Areas.HumanResource.Models.EAndDSetting? EAndDSetting { get; set; }
    }

    
}


