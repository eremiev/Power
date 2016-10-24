namespace PowerJump.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_Photo_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Path = c.String(),
                        GalleryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Galleries", t => t.GalleryId, cascadeDelete: true)
                .Index(t => t.GalleryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Photos", "GalleryId", "dbo.Galleries");
            DropIndex("dbo.Photos", new[] { "GalleryId" });
            DropTable("dbo.Photos");
        }
    }
}
