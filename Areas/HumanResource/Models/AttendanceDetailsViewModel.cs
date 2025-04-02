namespace Standus_5_0.Areas.HumanResource.Models
{
    public class AttendanceDetailsViewModel
    {
        public int AttendanceID { get; set; }
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public DateOnly Date { get; set; }
        public DateTime InTime { get; set; }
        public DateTime OutTime { get; set; }
        public string Head1Name { get; set; }
        public string Head2Name { get; set; }
        public string ShiftName { get; set; }
    }

}
