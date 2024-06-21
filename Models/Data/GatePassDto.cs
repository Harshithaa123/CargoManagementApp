namespace Cargo_FinalApplication.Models.Data
{
    public class GatePassDto
    {
        public int GatePassId { get; set; }
        public int OrderId { get; set; }
        public DateTime DispatchDate { get; set; }
        public int CreatedBy { get; set; }
    }
}
