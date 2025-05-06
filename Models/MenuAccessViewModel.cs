using Standus_5_0.Areas.Identity.Models;

namespace Standus_5_0.Models
{
    public class MenuAccessViewModel
    {
        public string GroupName { get; set; }
        public List<MenuAccessDto> Menus { get; set; }
        public AccessClaims AccessClaim { get; set; }
    }
}
