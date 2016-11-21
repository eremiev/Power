namespace PowerJump.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProjectLocales : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectLocales",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Locale = c.String(),
                        Title = c.String(nullable: false, maxLength: 60),
                        Description = c.String(nullable: false),
                        Project_GalleryId = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Projects", t => t.Project_GalleryId)
                .Index(t => t.Project_GalleryId);
            
            DropColumn("dbo.Projects", "Title");
            DropColumn("dbo.Projects", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projects", "Description", c => c.String(nullable: false));
            AddColumn("dbo.Projects", "Title", c => c.String(nullable: false, maxLength: 60));
            DropForeignKey("dbo.ProjectLocales", "Project_GalleryId", "dbo.Projects");
            DropIndex("dbo.ProjectLocales", new[] { "Project_GalleryId" });
            DropTable("dbo.ProjectLocales");
        }
    }
}
