namespace HospiceNiagara.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class required_SubRole_Name_Added : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SubRoles", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SubRoles", "Name", c => c.String());
        }
    }
}
