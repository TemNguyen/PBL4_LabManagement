namespace PC_Heal_ServerService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActiveTime : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Computer Information", newName: "CI");
            AddColumn("dbo.CI", "ActiveTime", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CI", "ActiveTime");
            RenameTable(name: "dbo.CI", newName: "Computer Information");
        }
    }
}
