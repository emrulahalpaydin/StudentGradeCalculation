namespace Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        NameSurname = c.String(),
                        Password = c.String(),
                        StudentNumber = c.Long(nullable: false),
                        Active = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        LastLoginDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Students");
        }
    }
}
