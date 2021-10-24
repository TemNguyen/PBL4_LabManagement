namespace PC_Heal_ServerService.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PC_Heal_ServerService.DAL.CIDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PC_Heal_ServerService.DAL.CIDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            context.CIs.AddOrUpdate(p => p.ComputerName, new CI()
            {
                ComputerName = "test data",
                CPU_Usage = "100%"
            });
        }
    }
}
