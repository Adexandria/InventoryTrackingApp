using InventoryTrackingApp.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryTrackingApp.Services
{
    public class TrackingRepository : ITracking
    {
        readonly DbService _db;
        readonly ConcurrencyService concurrencyService;
        public TrackingRepository(DbService _db,ConcurrencyService concurrencyService)
        {
            this._db = _db;
            this.concurrencyService = concurrencyService;
        }

        //Inventory repository

        // Get all inventories in the database using sql command
        public IEnumerable<Inventory> GetInventories 
        {
            get
            {
                return _db.Inventories.FromSqlRaw("SELECT * FROM dbo.Inventories").Include(s => s.Warehouse).AsNoTracking().OrderBy(s => s.TrackingId);
            }
        }

        //Add inventory into the table using sql command
        public async Task<Guid> AddInventory(Inventory inventory)
        {
            if (inventory == null)
            {
                throw new NullReferenceException(nameof(inventory));
            }
            inventory.TrackingId = Guid.NewGuid();
            //Raw sql command to insert inventory into the inventory table
            string commandText = "INSERT Inventories (Size,Name,WarehouseId,Destination,TrackingId) VALUES (@Size,@Name,@WarehouseId,@Destination,@TrackingId)";

            List<SqlParameter> sqlParameters = GetSqlParameters(inventory);
            IEnumerable<object> parameters = sqlParameters;

            await _db.Database.ExecuteSqlRawAsync(commandText, parameters);
            await Save();
            return inventory.TrackingId;
        }

      
        //Update existing inventory using sql command 
        public async Task<int> UpdateInventory(Inventory inventory)
        {
            if (inventory == null)
            {
                throw new NullReferenceException(nameof(inventory));
            }
            Inventory currentInventory = await GetInventoryByTrackingId(inventory.TrackingId);
            if(currentInventory == null)
            {
                throw new NullReferenceException(nameof(currentInventory));
            }
            _db.Entry(currentInventory).CurrentValues.SetValues(inventory);

            string commandText = "UPDATE Inventories SET WarehouseId = @WarehouseId,Name = @Name, @Destination = Destination,Size = @Size WHERE TrackingId = @TrackingId";
            List<SqlParameter> sqlParameters = GetSqlParameters(currentInventory);
            int noOfRowUpdated = await _db.Database.ExecuteSqlRawAsync(commandText, sqlParameters);
            //to prevent concurrency conflict
            concurrencyService.DbConcurreny<Inventory>();

            return noOfRowUpdated;
        }


        //Delete Inventory from the inventory

        public async Task<int> DeleteInventory(Guid trackingId)
        {
            if (trackingId == null)
            {
                throw new NullReferenceException(nameof(trackingId));
            }

            //Delete inventory by id and return 1(true) or 0(false)
            int noOfRowDeleted = await _db.Database.ExecuteSqlInterpolatedAsync($"Delete from Inventories WHERE TrackingId = {trackingId}");
            await Save();
            return noOfRowDeleted;
        }



        //Warehouse repository

        //Add warehouse into the database using sql command

        public async Task<int> AddWarehouse(Warehouse warehouse)
        {
            if(warehouse == null)
            {

                throw new NullReferenceException(nameof(warehouse));
            }
            warehouse.WarehouseId = Guid.NewGuid();
            //Raw sql command to insert warehouse into the warehouse table
            string commandText = "INSERT Warehouses (Location,Name,WarehouseId) VALUES (@Location,@Name,@WarehouseId)";

            List<SqlParameter> sqlParameters = GetSqlParameters(warehouse);
            IEnumerable<object> parameters = sqlParameters;

            int noOfRowInserted = await _db.Database.ExecuteSqlRawAsync(commandText, parameters);
            await Save();
            return noOfRowInserted;
        }

        //Get warehouse by name by sql command
        public async Task<Warehouse> GetWarehouseByName(string warehouse)
        {
               return await _db.Warehouses.FromSqlInterpolated($"SELECT * FROM dbo.Warehouses").Where(s=>s.Name.Contains(warehouse)).AsNoTracking().FirstOrDefaultAsync();
        }






        //Get Inventory by tracking id

        private async Task<Inventory> GetInventoryByTrackingId(Guid trackingId)
        {
            return await _db.Inventories.FromSqlInterpolated($"SELECT * FROM dbo.Inventories WHERE TrackingId = {trackingId}").AsNoTracking().FirstOrDefaultAsync();
        }

        //Save all changes made in the database
        private async Task Save()
        {
            await _db.SaveChangesAsync();
        }

        //Get sql parameter for inventory
        private List<SqlParameter> GetSqlParameters(Inventory inventory)
        {
            SqlParameter trackingId = new SqlParameter("@TrackingId",inventory.TrackingId);
            SqlParameter destination = new SqlParameter("@Destination", inventory.Destination);
            SqlParameter warehouseId = new SqlParameter("@WarehouseId", inventory.WarehouseId);
            SqlParameter name = new SqlParameter("@Name", inventory.Name);
            SqlParameter size = new SqlParameter("@Size", inventory.Size);
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                size,
                warehouseId,
                destination,
                trackingId,
                name,
            };
            return sqlParameters;
        }

        //get sql parameters for warehouse
        private List<SqlParameter> GetSqlParameters(Warehouse warehouse)
        {
            SqlParameter warehouseId = new SqlParameter("@WarehouseId", warehouse.WarehouseId);
            SqlParameter name = new SqlParameter("@Name", warehouse.Name);
            SqlParameter location = new SqlParameter("@Location", warehouse.Location);
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                location,
                warehouseId,
                name,
            };
            return sqlParameters;
        }

       
    }
}
