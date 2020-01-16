using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using TankLibrary.UserIdentity.Exceptions;
using TankLibrary.UserIdentity.Models;
using TankLibrary.UserIdentity.Services;

namespace TankLibrary.UserIdentity.Managers
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            UserIdentityDbContext appContext = context.Get<UserIdentityDbContext>();
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(appContext));
            manager.Context = appContext;
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            return manager;
        }

        public UserIdentityDbContext Context { get; set; }

        public override async Task SendEmailAsync(string userId, string subject, string body)
        {
            AdminUser adminUser = GetAdminUserInfo("register@franklidev.com");
            if (adminUser == null) throw new AdminUserNotExistException();

            var emailInfo = Json.Decode<EmailInfo>(adminUser.ExtraInfo);

            var user = await FindByIdAsync(userId);
            System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                                    new System.Net.Mail.MailAddress(adminUser.UserName, emailInfo.Description),
                                    new System.Net.Mail.MailAddress(user.Email));
            m.Subject = subject;
            m.Body = body;
            m.IsBodyHtml = true;
            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(emailInfo.SmtpClient);
            smtp.Credentials = new System.Net.NetworkCredential(adminUser.UserName, adminUser.Password);
            smtp.EnableSsl = (emailInfo.SSL == 1);
            smtp.Send(m);
            //try
            //{
            //    smtp.Send(m);
            //}
            //catch (System.Net.Mail.SmtpFailedRecipientException)
            //{

            //}
            //catch (Exception)
            //{

            //}
        }

        private AdminUser GetAdminUserInfo(string userName)
        {
            // Prepare @adminUserInfo OUTPUT parameter
            string jsonAdminUserInfo = "";
            SqlParameter pJsonAdminUserInfo = new SqlParameter("@jsonAdminUserInfo", SqlDbType.VarChar, 512,
                ParameterDirection.Output, false, 0, 0, "", DataRowVersion.Current, jsonAdminUserInfo);
            Context.Database.ExecuteSqlCommand("usp_GetAdminUserInfo @adminUserName, @jsonAdminUserInfo OUT",
            new SqlParameter("@adminUserName", userName), pJsonAdminUserInfo);

            jsonAdminUserInfo = pJsonAdminUserInfo.Value.ToString(); // jsonRealDealCard.Length==0 means PASS when dealResult == 1
            if (jsonAdminUserInfo == null || jsonAdminUserInfo.Length == 0) return null;
            var adminUser = Json.Decode<AdminUser[]>(jsonAdminUserInfo)[0];
            return adminUser;
        }
    }
}
