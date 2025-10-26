namespace FridgeManagementSystem.Models
{
    public class FaultReportDetail
    {
        public string CustomerName { get; set; }
        public string FridgeSerialNumber { get; set; }
        public string FaultDescription { get; set; }
        public string Status { get; set; }
        public DateTime ReportDate { get; set; }
        public DateTime? ResolutionDate { get; set; }
        public string Technician { get; set; }
    }
}
