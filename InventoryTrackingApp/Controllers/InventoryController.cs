using InventoryTrackingApp.Model;
using InventoryTrackingApp.Model.DTO;
using InventoryTrackingApp.Services;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryTrackingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        readonly ITracking _tracking;
        public InventoryController(ITracking _tracking)
        {
            this._tracking = _tracking;
        }

        [HttpGet]
        public ActionResult<IEnumerable<InventoriesDTO>> GetInventories()
        {
            try
            {
                IEnumerable<Inventory> inventories = _tracking.GetInventories;
                IEnumerable<InventoriesDTO> inventoriesDTO = inventories.Adapt<IEnumerable<InventoriesDTO>>();
                return Ok(inventoriesDTO);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddInventory(CreateInventory newInventory)
        {
            try
            {
                Warehouse warehouse = await  _tracking.GetWarehouseByName(newInventory.WarehouseName);
                if(warehouse == null)
                {
                    return NotFound();
                }
                Inventory inventory = newInventory.Adapt<Inventory>();
                inventory.WarehouseId = warehouse.WarehouseId.Adapt<Guid>();
                Guid trackingId = await  _tracking.AddInventory(inventory);
                return this.StatusCode(StatusCodes.Status201Created, $"Inventory created successfully, tracking id: {trackingId}");
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateInventory(UpdateInventory updateInventory)
        {
            try
            {
                Warehouse warehouse = await _tracking.GetWarehouseByName(updateInventory.WarehouseName);
                if (warehouse == null)
                {
                    return NotFound();
                }
                Inventory inventory = updateInventory.Adapt<Inventory>();
                inventory.WarehouseId = warehouse.WarehouseId.Adapt<Guid>();
                int noOfRowUpdated = await _tracking.UpdateInventory(inventory);
                if(noOfRowUpdated == 0)
                {
                    return BadRequest("Try again later");
                }
                return Ok("Inventory updated");
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
        [HttpDelete("{trackingId}")]
        public async Task<IActionResult> DeleteInventory(Guid trackingId)
        {
            try
            {
                int noOfRowDeleted = await _tracking.DeleteInventory(trackingId);
                if(noOfRowDeleted == 0)
                {
                    return BadRequest("Try again later");
                }
                return this.StatusCode(StatusCodes.Status204NoContent, "Inventory deleted");
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }
            
    }
}
