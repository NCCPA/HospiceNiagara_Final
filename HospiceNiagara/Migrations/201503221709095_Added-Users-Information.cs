namespace HospiceNiagara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUsersInformation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "PhoneExt", c => c.String());
            AddColumn("dbo.AspNetUsers", "IsContact", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "Position", c => c.String());
            AddColumn("dbo.AspNetUsers", "PositionDescription", c => c.String());
            AddColumn("dbo.AspNetUsers", "Bio", c => c.String(maxLength: 250));
            AddColumn("dbo.AspNetUsers", "ProfilePicture", c => c.Binary());
            AddColumn("dbo.AspNetUsers", "MimeType", c => c.String());
            AlterColumn("dbo.AspNetUsers", "Email", c => c.String(nullable: false, maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Email", c => c.String(maxLength: 256));
            DropColumn("dbo.AspNetUsers", "MimeType");
            DropColumn("dbo.AspNetUsers", "ProfilePicture");
            DropColumn("dbo.AspNetUsers", "Bio");
            DropColumn("dbo.AspNetUsers", "PositionDescription");
            DropColumn("dbo.AspNetUsers", "Position");
            DropColumn("dbo.AspNetUsers", "IsContact");
            DropColumn("dbo.AspNetUsers", "PhoneExt");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
        }
    }
}
