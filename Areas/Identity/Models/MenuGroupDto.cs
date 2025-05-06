namespace Standus_5_0.Areas.Identity.Models
{
    public class MenuGroupDto
    {
        public MenuGroupDto()
        {
            Menus = new List<MenuAccessDto>();
        }
        public string GroupName { get; set; }
        public List<MenuAccessDto> Menus { get; set; }
    }

}
