using InventoryTrackingApp.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryTrackingApp.Services
{
    public interface ITracking
    {
        //Inventory
        IEnumerable<Inventory> GetInventories { get; }
        Task<Guid> AddInventory(Inventory inventory);
        Task<int> UpdateInventory(Inventory inventory);
        Task<int> DeleteInventory(Guid trackingId);

        //Warehouse
        Task<int> AddWarehouse(Warehouse warehouse);
        Task<Warehouse> GetWarehouseByName(string warehouse);
    }
}
