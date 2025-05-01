using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Standus_5_0.Areas.HumanResource.Models
{
		[Table("SlabDeductionExclude")]
		public class StandardDeductionExclude	{					
			public int EmployeeID { get; set; }					
			public int DeductionID { get; set; }
			public bool  Exclude { get; set; }
			public bool Include { get; set; }

			// Optional: Navigation properties (if you're using EF Core with relationships)			
			public virtual Employee Employee { get; set; }			
			public virtual Deduction Deduction { get; set; }
		}
	}


