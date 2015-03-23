namespace HospiceNiagara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDeathstoDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Deaths",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Date = c.DateTime(nullable: false),
                        Location = c.String(maxLength: 100),
                        Note = c.String(maxLength: 200),
                        Visible = c.Boolean(nullable: false),
                        CreatedByID = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Deaths");
        }
    }
}
