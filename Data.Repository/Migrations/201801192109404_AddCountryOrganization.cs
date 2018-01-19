namespace Data.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCountryOrganization : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Organization",
                c => new
                    {
                        OrganizationId = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 300),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.OrganizationId);
            
            CreateTable(
                "dbo.OrganizationCountry",
                c => new
                    {
                        Organization_OrganizationId = c.Long(nullable: false),
                        Country_CountryId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Organization_OrganizationId, t.Country_CountryId })
                .ForeignKey("dbo.Organization", t => t.Organization_OrganizationId, cascadeDelete: true)
                .ForeignKey("dbo.Country", t => t.Country_CountryId, cascadeDelete: true)
                .Index(t => t.Organization_OrganizationId)
                .Index(t => t.Country_CountryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrganizationCountry", "Country_CountryId", "dbo.Country");
            DropForeignKey("dbo.OrganizationCountry", "Organization_OrganizationId", "dbo.Organization");
            DropIndex("dbo.OrganizationCountry", new[] { "Country_CountryId" });
            DropIndex("dbo.OrganizationCountry", new[] { "Organization_OrganizationId" });
            DropTable("dbo.OrganizationCountry");
            DropTable("dbo.Organization");
        }
    }
}
