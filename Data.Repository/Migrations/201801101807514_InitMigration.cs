namespace Data.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        CountryId = c.Long(nullable: false, identity: true),
                        IsoCode = c.String(nullable: false, maxLength: 2),
                        Name = c.String(nullable: false, maxLength: 300),
                    })
                .PrimaryKey(t => t.CountryId)
                .Index(t => t.IsoCode, unique: true);
            
            CreateTable(
                "dbo.Currency",
                c => new
                    {
                        CurrencyId = c.Long(nullable: false, identity: true),
                        IsoCode = c.String(nullable: false, maxLength: 3),
                        Name = c.String(nullable: false, maxLength: 300),
                    })
                .PrimaryKey(t => t.CurrencyId)
                .Index(t => t.IsoCode, unique: true);
            
            CreateTable(
                "dbo.CurrencyCountry",
                c => new
                    {
                        Currency_CurrencyId = c.Long(nullable: false),
                        Country_CountryId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Currency_CurrencyId, t.Country_CountryId })
                .ForeignKey("dbo.Currency", t => t.Currency_CurrencyId, cascadeDelete: true)
                .ForeignKey("dbo.Country", t => t.Country_CountryId, cascadeDelete: true)
                .Index(t => t.Currency_CurrencyId)
                .Index(t => t.Country_CountryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CurrencyCountry", "Country_CountryId", "dbo.Country");
            DropForeignKey("dbo.CurrencyCountry", "Currency_CurrencyId", "dbo.Currency");
            DropIndex("dbo.CurrencyCountry", new[] { "Country_CountryId" });
            DropIndex("dbo.CurrencyCountry", new[] { "Currency_CurrencyId" });
            DropIndex("dbo.Currency", new[] { "IsoCode" });
            DropIndex("dbo.Country", new[] { "IsoCode" });
            DropTable("dbo.CurrencyCountry");
            DropTable("dbo.Currency");
            DropTable("dbo.Country");
        }
    }
}
