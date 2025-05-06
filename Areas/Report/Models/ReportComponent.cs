namespace Standus_5_0.Areas.Report.Models
{

    public class ReportComponent
    {
        public Reports Report { get; set; }
        public List<ReportFilters> Filter { get; set; }

        public List<ReportColumns> Column { get; set; }

        public List<ReportCharts> Chart { get; set; }
    }
}