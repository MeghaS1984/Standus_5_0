using System.ComponentModel.DataAnnotations;
namespace Standus_5_0.Areas.Identity.Models     
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
        public string? Description { get; set; }
    }
}