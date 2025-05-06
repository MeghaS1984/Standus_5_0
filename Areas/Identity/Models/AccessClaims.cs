using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.Identity.Models
{
    public class AccessClaims
    {
        [Key]
        public int Id { get; set; }
        public string UserId {  get; set; }
        public int ClaimId { get; set; }
        public int MenuId { get; set; }
        public int ReportId { get; set; }
    }
}
