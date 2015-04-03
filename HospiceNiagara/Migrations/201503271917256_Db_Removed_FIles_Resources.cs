namespace HospiceNiagara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Db_Removed_FIles_Resources : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Files", "FolderID", "dbo.Folders");
            DropIndex("dbo.Files", new[] { "FolderID" });
            DropTable("dbo.Files");
            DropTable("dbo.Folders");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Folders",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FolderName = c.String(nullable: false, maxLength: 100),
                        FolderDescription = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FolderID = c.Int(nullable: false),
                        FileContent = c.Binary(nullable: false),
                        MimeType = c.String(nullable: false, maxLength: 256),
                        FileName = c.String(nullable: false, maxLength: 100),
                        FileDescription = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateIndex("dbo.Files", "FolderID");
            AddForeignKey("dbo.Files", "FolderID", "dbo.Folders", "ID", cascadeDelete: true);
        }
    }
}
