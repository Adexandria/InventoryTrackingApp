using System;
using System.ComponentModel.DataAnnotations;

namespace InventoryTrackingApp.Model.DTO
{
    public class UpdateInventory
    {
        [Required(ErrorMessage ="Enter Tracking Id")]
        public Guid TrackingId { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public string Destination { get; set; }
        public string WarehouseName { get; set; }
    }
}
