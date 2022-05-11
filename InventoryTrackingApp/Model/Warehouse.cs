using System;

namespace InventoryTrackingApp.Model
{
    public class Warehouse
    {
        public Guid WarehouseId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
    }
}
