namespace Contests.Data.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Web;
    using DAO;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using Newtonsoft.Json;

    public sealed class Configuration : DbMigrationsConfiguration<Contests.Data.ContestsDbContext>
    {
        private string categoriesImportPath;
        private string usersImportPath;

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "Contests.Data.ContestsDbContext";
        }

        protected override void Seed(ContestsDbContext context)
        {
            this.SeedRoles(context);
            this.SeedUsers(context);
            this.SeedCategories(context);
        }


        private void SeedRoles(ContestsDbContext context)
        {
            // makes an admin role if one doesn't exist
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };

                manager.Create(role);
            }

            // makes an user role if one doesn't exist
            if (!context.Roles.Any(r => r.Name == "User"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "User" };

                manager.Create(role);
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

        private void SeedUsers(ContestsDbContext context)
        {
            if (!context.Users.Any())
            {
                this.usersImportPath = HttpContext.Current.Server.MapPath(@"~\") + @"Seed\users.json";

                var json = File.ReadAllText(this.usersImportPath);

                var users = JsonConvert.DeserializeObject<IList<UserModel>>(json);

                var userStore = new UserStore<User>(context);
                var userManager = new UserManager<User>(userStore);

                foreach (var userModel in users)
                {
                    var user = new User
                    {
                        UserName = userModel.UserName,
                        Email = userModel.Email,
                        FullName = userModel.FullName,
                        PhoneNumber = userModel.PhoneNumber
                    };

                    var createUserResult = userManager.Create(user, userModel.Password);
                    if (createUserResult.Succeeded)
                    {
                        var addToRoleResult = userManager.AddToRole(user.Id, userModel.Role);
                        if (!addToRoleResult.Succeeded)
                        {
                            userManager.Delete(user);
                        }
                    }
                }
            }
        }
    }
}
