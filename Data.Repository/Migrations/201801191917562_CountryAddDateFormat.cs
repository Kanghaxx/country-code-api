namespace Data.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CountryAddDateFormat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Country", "DateFormat", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Country", "DateFormat");
        }
    }
}
