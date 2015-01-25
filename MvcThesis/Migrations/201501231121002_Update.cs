namespace MvcThesis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Topic", "UsualScore", c => c.Int(nullable: false));
            AddColumn("dbo.Topic", "ReviewScore", c => c.Int(nullable: false));
            AddColumn("dbo.Topic", "CommentScore", c => c.Int(nullable: false));
            AddColumn("dbo.Topic", "AnswerScore", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Topic", "AnswerScore");
            DropColumn("dbo.Topic", "CommentScore");
            DropColumn("dbo.Topic", "ReviewScore");
            DropColumn("dbo.Topic", "UsualScore");
        }
    }
}
