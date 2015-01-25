namespace MvcThesis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMajor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Majors",
                c => new
                    {
                        MajorId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.MajorId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Majors");
        }
    }
}
