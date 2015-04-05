namespace HospiceNiagara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixedFiles_Folders : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Files", "Folder_ID", "dbo.Folders");
            DropIndex("dbo.Files", new[] { "Folder_ID" });
            RenameColumn(table: "dbo.Files", name: "Folder_ID", newName: "FolderID");
            AlterColumn("dbo.Files", "FolderID", c => c.Int(nullable: false));
            CreateIndex("dbo.Files", "FolderID");
            AddForeignKey("dbo.Files", "FolderID", "dbo.Folders", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "FolderID", "dbo.Folders");
            DropIndex("dbo.Files", new[] { "FolderID" });
            AlterColumn("dbo.Files", "FolderID", c => c.Int());
            RenameColumn(table: "dbo.Files", name: "FolderID", newName: "Folder_ID");
            CreateIndex("dbo.Files", "Folder_ID");
            AddForeignKey("dbo.Files", "Folder_ID", "dbo.Folders", "ID");
        }
    }
}
