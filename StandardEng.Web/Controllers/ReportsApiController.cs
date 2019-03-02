using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Telerik.Reporting.Cache.File;
using Telerik.Reporting.Services;
using Telerik.Reporting.Services.WebApi;

namespace StandardEng.Web.Controllers
{
    public class ReportsApiController : ReportsControllerBase
    {
        public ReportsApiController()
        {
            this.ReportServiceConfiguration = new ReportServiceConfiguration
            {
                ReportResolver = new ReportFileResolver(Path.Combine(HttpContext.Current.Server.MapPath("~/"), "/Reports")).AddFallbackResolver(new ReportTypeResolver()),
                Storage = new FileStorage(),
            };
        }
    }
}
