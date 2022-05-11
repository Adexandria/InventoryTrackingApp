using System;


namespace InventoryTrackingApp.Model.DTO
{
    public class InventoriesDTO
    {
        public Guid TrackingId { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public string Destination { get; set; }
        public WarehouseDTO Warehouse { get; set; }
    }
}
