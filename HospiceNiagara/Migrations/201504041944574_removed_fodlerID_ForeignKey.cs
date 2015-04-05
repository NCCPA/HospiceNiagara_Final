namespace HospiceNiagara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removed_fodlerID_ForeignKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Files", "FolderID", "dbo.Folders");
            DropIndex("dbo.Files", new[] { "FolderID" });
            RenameColumn(table: "dbo.Files", name: "FolderID", newName: "Folder_ID");
            AlterColumn("dbo.Files", "Folder_ID", c => c.Int());
            CreateIndex("dbo.Files", "Folder_ID");
            AddForeignKey("dbo.Files", "Folder_ID", "dbo.Folders", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "Folder_ID", "dbo.Folders");
            DropIndex("dbo.Files", new[] { "Folder_ID" });
            AlterColumn("dbo.Files", "Folder_ID", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Files", name: "Folder_ID", newName: "FolderID");
            CreateIndex("dbo.Files", "FolderID");
            AddForeignKey("dbo.Files", "FolderID", "dbo.Folders", "ID", cascadeDelete: true);
        }
    }
}
