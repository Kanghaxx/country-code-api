namespace Data.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCountryCallingCode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Country", "CallingCode", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Country", "CallingCode");
        }
    }
}
