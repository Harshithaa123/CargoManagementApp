namespace Cargo_FinalApplication.Models.Data
{
    public class WarehouseDto
    {
        public int WarehouseId { get; set; }

        public string WarehouseName { get; set; } = null!;

        public string Location { get; set; } = null!;

        public int Capacity { get; set; }

        public string MobileNum { get; set; } = null!;
    }
}
