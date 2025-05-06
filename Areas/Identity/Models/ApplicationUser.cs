using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.Identity.Models
{
    //[Table("AspNetUsers")]
    public class ApplicationUser : IdentityUser
    {
        public int EmployeeID { get; set; }
        public string? Report {  get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set;}
    }
}
