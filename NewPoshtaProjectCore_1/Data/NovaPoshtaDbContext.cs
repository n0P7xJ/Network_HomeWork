using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NovaPoshtaProject.Constant;
using NovaPoshtaProjectCore_1.Models;


namespace NovaPoshtaProject.Data
{
    public class NovaPoshtaDbContext : DbContext
    {
        public DbSet<Area> Areas { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(AppDataBase.ConnetctioString);
        }
    }
}