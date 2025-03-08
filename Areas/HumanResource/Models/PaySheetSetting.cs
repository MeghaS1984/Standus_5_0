using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Standus_5_0.Areas.HumanResource.Models;
using Standus_5_0.Data;
using System.ComponentModel.DataAnnotations;

namespace Standus_5_0.Areas.HumanResource.Models
{
    public class PaySheetSetting
    {
        [Key]
        public int ID { get; set; }
        public string Query {  get; set; }
        public string Design {  get; set; }
    }
}
