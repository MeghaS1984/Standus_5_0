using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.HumanResource.Models
{
   
    public class BankDetails : Employee
    {
       
        [Required]
        public string BankName { get; set; }
        [Required]
        public string AccountNo { get; set;}
        [Required]
        public string IFSCCode { get; set;}

    }
}
