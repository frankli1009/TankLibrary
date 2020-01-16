namespace TankLibrary.UserIdentity.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TankLibrary.UserIdentity.Managers;
    using TankLibrary.UserIdentity.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TankLibrary.UserIdentity.Models.UserIdentityDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "UserIdentity.Models.UserIdentityDbContext";
        }

        protected override void Seed(TankLibrary.UserIdentity.Models.UserIdentityDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            context.Set<IdentityRole>().AddOrUpdate(r => r.Name,
                new IdentityRole { Name = ConstValue.Role_Admin },
                new IdentityRole { Name = ConstValue.Role_User });

            if (context.Set<ApplicationUser>().Any(u => u.Email == ConstValue.User_Admin) 
                && context.Set<IdentityRole>().Any(r => r.Name == ConstValue.Role_Admin)
                && context.Set<IdentityRole>().Any(r => r.Name == ConstValue.Role_User))
            {
                ApplicationUser registerUser = context.Set<ApplicationUser>().Where(u => u.Email == ConstValue.User_Admin).FirstOrDefault();
                IdentityRole adminRole = context.Set<IdentityRole>().Where(r => r.Name == ConstValue.Role_Admin).FirstOrDefault();
                IdentityRole userRole = context.Set<IdentityRole>().Where(r => r.Name == ConstValue.Role_User).FirstOrDefault();
                context.Set<IdentityUserRole>().AddOrUpdate(ur => new { ur.UserId, ur.RoleId },
                new IdentityUserRole
                {
                    UserId = registerUser.Id,
                    RoleId = adminRole.Id
                },
                new IdentityUserRole
                {
                    UserId = registerUser.Id,
                    RoleId = userRole.Id
                });
            }

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
