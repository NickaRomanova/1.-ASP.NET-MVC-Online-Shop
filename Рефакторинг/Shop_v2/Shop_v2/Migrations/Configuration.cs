namespace Shop_v2.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Shop_v2.Context.Shop_version_2_Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Shop_v2.Context.Shop_version_2_Context";
        }

        protected override void Seed(Shop_v2.Context.Shop_version_2_Context context)
        { 
        }
    }
}
