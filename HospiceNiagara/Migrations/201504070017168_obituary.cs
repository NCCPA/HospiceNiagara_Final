namespace HospiceNiagara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class obituary : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Deaths", "Obituary", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Deaths", "Obituary");
        }
    }
}
