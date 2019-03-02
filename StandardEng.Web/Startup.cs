using Hangfire;
using Hangfire.SqlServer;
using Owin;
using StandardEng.Web.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace StandardEng.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration
                .UseSqlServerStorage(ConfigurationManager.ConnectionStrings["StandardEngEntitiesSimple"].ConnectionString
                //, new SqlServerStorageOptions { CommandTimeout = TimeSpan.MaxValue }
                );

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            //RecurringJob.AddOrUpdate(() => WarrantyExpiryReminder.GetListOfWarrantyExpiry(), "00 08 * * *", TimeZoneInfo.Local); //Daily Morning 8AM
            RecurringJob.AddOrUpdate(() => WarrantyExpiryReminder.GetListOfWarrantyExpiry(), "* * * * *", TimeZoneInfo.Local);  //Every Minute
        }
    }
}