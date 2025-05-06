namespace Standus_5_0.Areas.Identity.Models
{
    public class ClaimDto
    {
        public int ClaimId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public bool IsSelected { get; set; }
    }

}
