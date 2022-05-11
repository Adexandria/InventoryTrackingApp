using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace InventoryTrackingApp.Model
{
    public class Inventory
    {
        [Key]
        public Guid TrackingId { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public string Destination { get; set; }
        [ForeignKey("Warehouse")]
        public Guid WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}
