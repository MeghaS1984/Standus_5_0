namespace Standus_5_0.Areas.Identity.Models
{
    public class MenuAccessDto
    {
        public MenuAccessDto()
        {
            Claims = new List<ClaimDto>();
        }
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string UrlPath { get; set; }
        public int? AccessClaimId { get; set; }
        public string AccessClaimUserId { get; set; }
        public List<ClaimDto> Claims { get; set; } = new();
    }

}
