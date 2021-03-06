using System;
using System.Data.Entity;
using System.Linq;

namespace PC_Heal_ServerService.DAL
{
    public class CIDbContext : DbContext
    {
        // Your context has been configured to use a 'CIDbContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'PC_Heal_ServerService.DAL.CIDbContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'CIDbContext' 
        // connection string in the application configuration file.
        public CIDbContext()
            : base("name=CIDbContext")
        {
            Database.SetInitializer(new CIInitializer());
        }
        public DbSet<CI> CI { get; set; }
        public DbSet<Account> Account { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}