namespace PowerJump.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class import_db_Gallery_Event_Project : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Galleries",
                c => new
                    {
                        GalleryId = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.GalleryId);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        GalleryId = c.Int(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.GalleryId)
                .ForeignKey("dbo.Galleries", t => t.GalleryId)
                .Index(t => t.GalleryId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        GalleryId = c.Int(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.GalleryId)
                .ForeignKey("dbo.Galleries", t => t.GalleryId)
                .Index(t => t.GalleryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "GalleryId", "dbo.Galleries");
            DropForeignKey("dbo.Events", "GalleryId", "dbo.Galleries");
            DropIndex("dbo.Projects", new[] { "GalleryId" });
            DropIndex("dbo.Events", new[] { "GalleryId" });
            DropTable("dbo.Projects");
            DropTable("dbo.Events");
            DropTable("dbo.Galleries");
        }
    }
}
