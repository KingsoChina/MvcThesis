namespace MvcThesis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false),
                        CommentTime = c.DateTime(nullable: false),
                        CommentLevel = c.Int(nullable: false),
                        CommentNode = c.Int(nullable: false),
                        UserProfile_UserId = c.Int(),
                        Topic_TopicId = c.Int(),
                        Student_UserId = c.Int(),
                        Teacher_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.UserProfile", t => t.UserProfile_UserId)
                .ForeignKey("dbo.Topic", t => t.Topic_TopicId)
                .ForeignKey("dbo.UserProfile", t => t.Student_UserId)
                .ForeignKey("dbo.UserProfile", t => t.Teacher_UserId)
                .Index(t => t.UserProfile_UserId)
                .Index(t => t.Topic_TopicId)
                .Index(t => t.Student_UserId)
                .Index(t => t.Teacher_UserId);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 50),
                        FullName = c.String(maxLength: 15),
                        Institute = c.String(maxLength: 50),
                        Major = c.String(maxLength: 50),
                        Class = c.String(maxLength: 50),
                        Phone = c.String(maxLength: 11),
                        ShortPhone = c.String(maxLength: 10),
                        Email = c.String(maxLength: 250),
                        QQ = c.String(maxLength: 20),
                        JobTitle = c.String(maxLength: 250),
                        MaxGuideNum = c.Int(nullable: false),
                        Direction = c.String(),
                        MyTopic_TopicId = c.Int(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Topic", t => t.MyTopic_TopicId)
                .Index(t => t.MyTopic_TopicId);
            
            CreateTable(
                "dbo.Topic",
                c => new
                    {
                        TopicId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Status = c.Int(nullable: false),
                        ApplyClass = c.String(maxLength: 50),
                        Source = c.String(maxLength: 50),
                        Type = c.String(nullable: false, maxLength: 50),
                        Condition = c.String(),
                        Elaboration = c.String(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        IsFromStudent = c.Boolean(nullable: false),
                        IsChosen = c.Boolean(nullable: false),
                        IsTeacherAgree = c.Int(nullable: false),
                        IsAssistAgree = c.Int(nullable: false),
                        IsDeanAgree = c.Int(nullable: false),
                        UsualScore = c.Int(nullable: false),
                        ReviewScore = c.Int(nullable: false),
                        CommentScore = c.Int(nullable: false),
                        AnswerScore = c.Int(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        AnswerTeacher_UserId = c.Int(),
                        CommentBook_DocumentId = c.Int(),
                        CommentTeacher_UserId = c.Int(),
                        MissionBook_DocumentId = c.Int(),
                        Report_DocumentId = c.Int(),
                        Student_UserId = c.Int(),
                        Teacher_UserId = c.Int(),
                        Thesis_DocumentId = c.Int(),
                        UserProfile_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.TopicId)
                .ForeignKey("dbo.UserProfile", t => t.AnswerTeacher_UserId)
                .ForeignKey("dbo.Document", t => t.CommentBook_DocumentId)
                .ForeignKey("dbo.UserProfile", t => t.CommentTeacher_UserId)
                .ForeignKey("dbo.Document", t => t.MissionBook_DocumentId)
                .ForeignKey("dbo.Document", t => t.Report_DocumentId)
                .ForeignKey("dbo.UserProfile", t => t.Student_UserId)
                .ForeignKey("dbo.UserProfile", t => t.Teacher_UserId)
                .ForeignKey("dbo.Document", t => t.Thesis_DocumentId)
                .ForeignKey("dbo.UserProfile", t => t.UserProfile_UserId)
                .Index(t => t.AnswerTeacher_UserId)
                .Index(t => t.CommentBook_DocumentId)
                .Index(t => t.CommentTeacher_UserId)
                .Index(t => t.MissionBook_DocumentId)
                .Index(t => t.Report_DocumentId)
                .Index(t => t.Student_UserId)
                .Index(t => t.Teacher_UserId)
                .Index(t => t.Thesis_DocumentId)
                .Index(t => t.UserProfile_UserId);
            
            CreateTable(
                "dbo.Document",
                c => new
                    {
                        DocumentId = c.Int(nullable: false, identity: true),
                        LastUploadTime = c.DateTime(nullable: false),
                        Path = c.String(),
                        Student_UserId = c.Int(),
                        Topic_TopicId = c.Int(),
                    })
                .PrimaryKey(t => t.DocumentId)
                .ForeignKey("dbo.UserProfile", t => t.Student_UserId)
                .ForeignKey("dbo.Topic", t => t.Topic_TopicId)
                .Index(t => t.Student_UserId)
                .Index(t => t.Topic_TopicId);
            
            CreateTable(
                "dbo.Majors",
                c => new
                    {
                        MajorId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.MajorId);
            
            CreateTable(
                "dbo.webpages_Membership",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        CreateDate = c.DateTime(),
                        ConfirmationToken = c.String(maxLength: 128),
                        IsConfirmed = c.Boolean(),
                        LastPasswordFailureDate = c.DateTime(),
                        PasswordFailuresSinceLastSuccess = c.Int(nullable: false),
                        Password = c.String(nullable: false, maxLength: 128),
                        PasswordChangedDate = c.DateTime(),
                        PasswordSalt = c.String(nullable: false, maxLength: 128),
                        PasswordVerificationToken = c.String(maxLength: 128),
                        PasswordVerificationTokenExpirationDate = c.DateTime(),
                        PasswordResetToken = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.webpages_Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.webpages_PermissionsInRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        PermissionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.webpages_Permission", t => t.PermissionId, cascadeDelete: true)
                .ForeignKey("dbo.webpages_Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.PermissionId);
            
            CreateTable(
                "dbo.webpages_Permission",
                c => new
                    {
                        PermissionId = c.Int(nullable: false, identity: true),
                        PermissionName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.PermissionId);
            
            CreateTable(
                "dbo.Setting",
                c => new
                    {
                        SettingId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Content = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.SettingId);
            
            CreateTable(
                "dbo.webpages_UsersInRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.webpages_Membership", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.webpages_Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.webpages_UsersInRoles", "RoleId", "dbo.webpages_Roles");
            DropForeignKey("dbo.webpages_UsersInRoles", "UserId", "dbo.webpages_Membership");
            DropForeignKey("dbo.webpages_PermissionsInRoles", "RoleId", "dbo.webpages_Roles");
            DropForeignKey("dbo.webpages_PermissionsInRoles", "PermissionId", "dbo.webpages_Permission");
            DropForeignKey("dbo.Comment", "Teacher_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Comment", "Student_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Topic", "UserProfile_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.UserProfile", "MyTopic_TopicId", "dbo.Topic");
            DropForeignKey("dbo.Topic", "Thesis_DocumentId", "dbo.Document");
            DropForeignKey("dbo.Topic", "Teacher_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Topic", "Student_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Topic", "Report_DocumentId", "dbo.Document");
            DropForeignKey("dbo.Topic", "MissionBook_DocumentId", "dbo.Document");
            DropForeignKey("dbo.Topic", "CommentTeacher_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Comment", "Topic_TopicId", "dbo.Topic");
            DropForeignKey("dbo.Topic", "CommentBook_DocumentId", "dbo.Document");
            DropForeignKey("dbo.Document", "Topic_TopicId", "dbo.Topic");
            DropForeignKey("dbo.Document", "Student_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Topic", "AnswerTeacher_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Comment", "UserProfile_UserId", "dbo.UserProfile");
            DropIndex("dbo.webpages_UsersInRoles", new[] { "RoleId" });
            DropIndex("dbo.webpages_UsersInRoles", new[] { "UserId" });
            DropIndex("dbo.webpages_PermissionsInRoles", new[] { "PermissionId" });
            DropIndex("dbo.webpages_PermissionsInRoles", new[] { "RoleId" });
            DropIndex("dbo.Document", new[] { "Topic_TopicId" });
            DropIndex("dbo.Document", new[] { "Student_UserId" });
            DropIndex("dbo.Topic", new[] { "UserProfile_UserId" });
            DropIndex("dbo.Topic", new[] { "Thesis_DocumentId" });
            DropIndex("dbo.Topic", new[] { "Teacher_UserId" });
            DropIndex("dbo.Topic", new[] { "Student_UserId" });
            DropIndex("dbo.Topic", new[] { "Report_DocumentId" });
            DropIndex("dbo.Topic", new[] { "MissionBook_DocumentId" });
            DropIndex("dbo.Topic", new[] { "CommentTeacher_UserId" });
            DropIndex("dbo.Topic", new[] { "CommentBook_DocumentId" });
            DropIndex("dbo.Topic", new[] { "AnswerTeacher_UserId" });
            DropIndex("dbo.UserProfile", new[] { "MyTopic_TopicId" });
            DropIndex("dbo.Comment", new[] { "Teacher_UserId" });
            DropIndex("dbo.Comment", new[] { "Student_UserId" });
            DropIndex("dbo.Comment", new[] { "Topic_TopicId" });
            DropIndex("dbo.Comment", new[] { "UserProfile_UserId" });
            DropTable("dbo.webpages_UsersInRoles");
            DropTable("dbo.Setting");
            DropTable("dbo.webpages_Permission");
            DropTable("dbo.webpages_PermissionsInRoles");
            DropTable("dbo.webpages_Roles");
            DropTable("dbo.webpages_Membership");
            DropTable("dbo.Majors");
            DropTable("dbo.Document");
            DropTable("dbo.Topic");
            DropTable("dbo.UserProfile");
            DropTable("dbo.Comment");
        }
    }
}
