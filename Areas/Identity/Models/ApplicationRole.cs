using Microsoft.AspNetCore.Identity;

namespace Standus_5_0.Areas.Identity.Models    
{
    public class ApplicationRole : IdentityRole
    {
        // Add custom properties here
        public string? Description { get; set; }
    }
}