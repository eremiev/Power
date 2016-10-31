namespace PowerJump.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_validation_to_Project_model : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Projects", "Title", c => c.String(nullable: false, maxLength: 60));
            AlterColumn("dbo.Projects", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Projects", "Description", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Projects", "Title", c => c.String(maxLength: 60));
        }
    }
}
