namespace PC_Heal_ServerService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_account_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        Username = c.String(nullable: false, maxLength: 128),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Username);
            
            AddColumn("dbo.Computer Information", "Chipset", c => c.String());
            AddColumn("dbo.Computer Information", "RAM_Size", c => c.String());
            AddColumn("dbo.Computer Information", "Max_Clock_Speed", c => c.String());
            AddColumn("dbo.Computer Information", "Num_Thread", c => c.String());
            AddColumn("dbo.Computer Information", "Num_Process", c => c.String());
            AddColumn("dbo.Computer Information", "Disk_Usage", c => c.String());
            AddColumn("dbo.Computer Information", "GPU_Usage", c => c.String());
            DropColumn("dbo.Computer Information", "NumberOfProcessors");
            DropColumn("dbo.Computer Information", "ProcessorName");
            DropColumn("dbo.Computer Information", "C_DiskFree");
            DropColumn("dbo.Computer Information", "D_DiskFree");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Computer Information", "D_DiskFree", c => c.String());
            AddColumn("dbo.Computer Information", "C_DiskFree", c => c.String());
            AddColumn("dbo.Computer Information", "ProcessorName", c => c.String());
            AddColumn("dbo.Computer Information", "NumberOfProcessors", c => c.String());
            DropColumn("dbo.Computer Information", "GPU_Usage");
            DropColumn("dbo.Computer Information", "Disk_Usage");
            DropColumn("dbo.Computer Information", "Num_Process");
            DropColumn("dbo.Computer Information", "Num_Thread");
            DropColumn("dbo.Computer Information", "Max_Clock_Speed");
            DropColumn("dbo.Computer Information", "RAM_Size");
            DropColumn("dbo.Computer Information", "Chipset");
            DropTable("dbo.Account");
        }
    }
}
