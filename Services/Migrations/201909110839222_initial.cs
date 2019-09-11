namespace Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Author",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LastName = c.String(nullable: false, maxLength: 50),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        DateOfBirth = c.DateTime(),
                        DateOfDeath = c.DateTime(),
                        AboutAuthor = c.String(maxLength: 2000),
                        FileModelId = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.FileModel", t => t.FileModelId)
                .Index(t => t.FileModelId);
            
            CreateTable(
                "dbo.FileModel",
                c => new
                    {
                        FileModelId = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        FileType = c.String(),
                        FileContent = c.String(),
                        DateAdded = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.FileModelId);
            
            CreateTable(
                "dbo.Book",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false, maxLength: 1000),
                        SerialNumber = c.Int(nullable: false),
                        ReleaseDate = c.DateTime(nullable: false),
                        AuthorID = c.Int(nullable: false),
                        GenreID = c.Int(nullable: false),
                        Publisher = c.String(),
                        NumberInStock = c.Int(nullable: false),
                        RentedBooks = c.Int(nullable: false),
                        FileModelId = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Author", t => t.AuthorID, cascadeDelete: true)
                .ForeignKey("dbo.FileModel", t => t.FileModelId)
                .ForeignKey("dbo.Genre", t => t.GenreID, cascadeDelete: true)
                .Index(t => t.AuthorID)
                .Index(t => t.GenreID)
                .Index(t => t.FileModelId);
            
            CreateTable(
                "dbo.Genre",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountNumber = c.Int(nullable: false),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Address = c.String(nullable: false, maxLength: 200),
                        PhoneNumber = c.String(nullable: false, maxLength: 10),
                        RentedBooks = c.Int(nullable: false),
                        CustomerType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Rental",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BookId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        DateRented = c.DateTime(nullable: false),
                        DateReturned = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Book", t => t.BookId, cascadeDelete: true)
                .ForeignKey("dbo.Customer", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.BookId)
                .Index(t => t.CustomerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rental", "CustomerId", "dbo.Customer");
            DropForeignKey("dbo.Rental", "BookId", "dbo.Book");
            DropForeignKey("dbo.Book", "GenreID", "dbo.Genre");
            DropForeignKey("dbo.Book", "FileModelId", "dbo.FileModel");
            DropForeignKey("dbo.Book", "AuthorID", "dbo.Author");
            DropForeignKey("dbo.Author", "FileModelId", "dbo.FileModel");
            DropIndex("dbo.Rental", new[] { "CustomerId" });
            DropIndex("dbo.Rental", new[] { "BookId" });
            DropIndex("dbo.Book", new[] { "FileModelId" });
            DropIndex("dbo.Book", new[] { "GenreID" });
            DropIndex("dbo.Book", new[] { "AuthorID" });
            DropIndex("dbo.Author", new[] { "FileModelId" });
            DropTable("dbo.Rental");
            DropTable("dbo.Customer");
            DropTable("dbo.Genre");
            DropTable("dbo.Book");
            DropTable("dbo.FileModel");
            DropTable("dbo.Author");
        }
    }
}
