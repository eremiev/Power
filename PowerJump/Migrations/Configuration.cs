namespace PowerJump.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PowerJump.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PowerJump.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );

            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var role = new IdentityRole { Name = "Admin" };
                roleManager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Normal"))
            {
                var role = new IdentityRole { Name = "Normal" };
                roleManager.Create(role);
            }

            context.SaveChanges();

            if (!(context.Users.Any(u => u.UserName == "admin")))
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var userToInsert = new ApplicationUser
                {
                    Email = "admin@email.com",
                    UserName = "admin@email.com",
                    PhoneNumber = "0797697898"
                };
                userManager.Create(userToInsert, "Admin123!");
                userManager.AddToRole(userToInsert.Id, "Admin");
            }

            if (!(context.Users.Any(u => u.UserName == "normal")))
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var userToInsert = new ApplicationUser
                {
                    Email = "normal@email.com",
                    UserName = "normal@email.com",
                    PhoneNumber = "0797697898"
                };
                userManager.Create(userToInsert, "Normal123!");
                userManager.AddToRole(userToInsert.Id, "Normal");
            }

            context.SaveChanges();
        }
    }
}
