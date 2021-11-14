namespace PC_Heal_ServerService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsOnline : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CI", "IsOnline", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CI", "IsOnline");
        }
    }
}
