namespace HospiceNiagara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class subRoles_implementation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubRoles",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        RoleID = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.AspNetRoles", "SubRoles_ID", c => c.Int());
            CreateIndex("dbo.AspNetRoles", "SubRoles_ID");
            AddForeignKey("dbo.AspNetRoles", "SubRoles_ID", "dbo.SubRoles", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetRoles", "SubRoles_ID", "dbo.SubRoles");
            DropIndex("dbo.AspNetRoles", new[] { "SubRoles_ID" });
            DropColumn("dbo.AspNetRoles", "SubRoles_ID");
            DropTable("dbo.SubRoles");
        }
    }
}
