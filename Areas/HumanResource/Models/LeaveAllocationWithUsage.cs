using Standus_5_0.Areas.HumanResource.Models;

namespace Standus_5_0.Areas.HumanResource.Models
{
    public class LeaveAllocationWithUsage
    {
        public LeaveAllocationDetails Allocation { get; set; }
        public decimal Used { get; set; }
        public decimal Balance { get; set; }
    }
}