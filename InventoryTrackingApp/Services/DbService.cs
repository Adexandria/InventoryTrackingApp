using InventoryTrackingApp.Model;
using Microsoft.EntityFrameworkCore;

namespace InventoryTrackingApp.Services
{
    public class DbService :DbContext
    {
        public DbService(DbContextOptions<DbService> options):base(options)
        {

        }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
    }
}
