using System.ComponentModel.DataAnnotations;

namespace InventoryTrackingApp.Model.DTO
{
    public class CreateInventory
    {
        [Required(ErrorMessage ="Enter inventory name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Enter inventory size")]
        public int Size { get; set; }
        [Required(ErrorMessage = "Enter Destination")]
        public string Destination { get; set; }
        [Required(ErrorMessage = "Enter WarehouseId")]
        public string WarehouseName { get; set; }
    }
}
