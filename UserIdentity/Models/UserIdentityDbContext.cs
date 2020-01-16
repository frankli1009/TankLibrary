using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankLibrary.UserIdentity.Migrations;

namespace TankLibrary.UserIdentity.Models
{
    public partial class UserIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public UserIdentityDbContext()
            : base("name=UserIdentityDbContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<UserIdentityDbContext, Configuration>());
        }

        public static UserIdentityDbContext Create(IdentityFactoryOptions<UserIdentityDbContext> options, IOwinContext context)
        {
            return new UserIdentityDbContext();
        }
    }
}