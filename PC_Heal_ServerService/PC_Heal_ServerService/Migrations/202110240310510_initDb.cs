namespace PC_Heal_ServerService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Computer Information",
                c => new
                    {
                        ComputerName = c.String(nullable: false, maxLength: 128),
                        GPUName = c.String(),
                        NumberOfProcessors = c.String(),
                        NumberOfLogicalProcessors = c.String(),
                        ProcessorName = c.String(),
                        IPAddress = c.String(),
                        MACAddress = c.String(),
                        C_DiskFree = c.String(),
                        D_DiskFree = c.String(),
                        CPU_Usage = c.String(),
                        RAM_Usage = c.String(),
                    })
                .PrimaryKey(t => t.ComputerName);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Computer Information");
        }
    }
}
