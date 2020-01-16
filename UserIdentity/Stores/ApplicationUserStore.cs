using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankLibrary.UserIdentity.Models;

namespace TankLibrary.UserIdentity
{
    public class ApplicationUserStore :
        UserStore<ApplicationUser>, IUserStore<ApplicationUser>,
        IDisposable
    {
        public ApplicationUserStore() : this(new UserIdentityDbContext())
        {
            base.DisposeContext = true;
        }

        public ApplicationUserStore(DbContext context)
            : base(context)
        {
        }
    }
}
