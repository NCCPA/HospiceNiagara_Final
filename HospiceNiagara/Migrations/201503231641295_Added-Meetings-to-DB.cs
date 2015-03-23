namespace HospiceNiagara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMeetingstoDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Meetings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.String(maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 1000),
                        Date = c.DateTime(nullable: false),
                        Length = c.String(maxLength: 50),
                        Location = c.String(maxLength: 50),
                        Requirements = c.String(maxLength: 200),
                        isVisible = c.Boolean(nullable: false),
                        StartTime = c.String(nullable: false),
                        EndTime = c.String(nullable: false),
                        StaffLeadID = c.Int(nullable: false),
                        CreatedByID = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.AspNetUsers", "Meetings_ID", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Meetings_ID");
            AddForeignKey("dbo.AspNetUsers", "Meetings_ID", "dbo.Meetings", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Meetings_ID", "dbo.Meetings");
            DropIndex("dbo.AspNetUsers", new[] { "Meetings_ID" });
            DropColumn("dbo.AspNetUsers", "Meetings_ID");
            DropTable("dbo.Meetings");
        }
    }
}
