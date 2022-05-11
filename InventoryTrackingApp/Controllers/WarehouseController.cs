using InventoryTrackingApp.Model;
using InventoryTrackingApp.Model.DTO;
using InventoryTrackingApp.Services;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace InventoryTrackingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        readonly ITracking _tracking;
        public WarehouseController(ITracking _tracking)
        {
            this._tracking = _tracking;

        }

        //Add warehouse
        [HttpPost]
        public async Task<IActionResult> AddWareHouse(CreateWarehouse createWarehouse)
        {
            try
            {
                //map from the createwarehouse object to warehouse object
                Warehouse warehouse = createWarehouse.Adapt<Warehouse>();

                int noOfRowInserted = await _tracking.AddWarehouse(warehouse);
                if(noOfRowInserted == 0)
                {
                    return BadRequest("Try again later");
                }

                return Ok("Warehouse created");
            }
            catch (Exception e)
            {
                // returns any exception
                return BadRequest(e.Message);
            }
        }
    }
}
