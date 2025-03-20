namespace Standus_5_0.Areas.HumanResource.Models
{
    public class AllowanceDetails
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public bool Fixed { get; set; }
        public decimal FromAmount { get; set; }
        public decimal ToAmount { get; set; }
        public decimal Employee { get; set; }
        public decimal Employer { get; set; }
        public int DetailsID { get; set; }
    }
}
