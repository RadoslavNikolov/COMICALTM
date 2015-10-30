namespace Contests.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;

    public sealed class Configuration : DbMigrationsConfiguration<Contests.Data.ContestsDbContext>
    {
        private string categoriesImportPath;

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "Contests.Data.ContestsDbContext";
        }

        protected override void Seed(ContestsDbContext context)
        {
            this.SeedCategories(context);

            // makes an admin role if one doesn't exist
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };

                manager.Create(role);
            }

            // if user doesn't exist, create one and add it to the admin role
            if (!context.Users.Any(u => u.UserName == "admin"))
            {
                var store = new UserStore<User>(context);
                var manager = new UserManager<User>(store);
                var user = new User { UserName = "admin", Email = "admin@admin.com" };

                manager.Create(user, "password");
                manager.AddToRole(user.Id, "Admin");

            }
        }


        private void SeedCategories(ContestsDbContext context)
        {
            if (!context.Categories.Any())
            {
                string line;

                this.categoriesImportPath = HttpContext.Current.Server.MapPath(@"~\") + @"Seed\categories.txt";

                StreamReader file = new StreamReader(this.categoriesImportPath);
                while ((line = file.ReadLine()) != null)
                {
                    var newCategory = new Category
                    {
                        Name = line,
                        IsActive = true
                    };

                    context.Categories.Add(newCategory);
                }
            }
        }
    }
}
