namespace MyShop.DataAcess.MySQL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using MySql.Data.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<MyShop.DataAcess.MySQL.DataContext>
    {
        public Configuration()
        {
            SetSqlGenerator("MySql.Data.MySqlClient", new MySql.Data.EntityFramework.MySqlMigrationSqlGenerator());

            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MyShop.DataAcess.MySQL.DataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
