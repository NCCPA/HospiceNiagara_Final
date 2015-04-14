namespace HospiceNiagara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Users_SubRoles_ManyToMany : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubRolesApplicationUsers",
                c => new
                    {
                        SubRoles_ID = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.SubRoles_ID, t.ApplicationUser_Id })
                .ForeignKey("dbo.SubRoles", t => t.SubRoles_ID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.SubRoles_ID)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubRolesApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SubRolesApplicationUsers", "SubRoles_ID", "dbo.SubRoles");
            DropIndex("dbo.SubRolesApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.SubRolesApplicationUsers", new[] { "SubRoles_ID" });
            DropTable("dbo.SubRolesApplicationUsers");
        }
    }
}
