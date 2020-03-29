using StandardEng.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace StandardEng.Data.CustomModel
{
    public class AMCPartialModel
    {
        public int AMCQuotationId { get; set; }
        public string Remarks { get; set; }
    }

    public class AMCServiceCompltePartialModel
    {
        public int AMCServiceId { get; set; }

        [Display(ResourceType = typeof(CommonMessage), Name = "ServiceReportNo")]
        public string ServiceReportNo { get; set; }

        [Display(ResourceType = typeof(CommonMessage), Name = "ServiceEngineer")]
        public Nullable<int> ServiceEngineerId { get; set; }

        [Display(ResourceType = typeof(CommonMessage), Name = "ServiceRemarks")]
        public string ServiceRemarks { get; set; }
    }
    public class AMCServiceOverWritePartialModel
    {
        public int AMCServiceId { get; set; }
        public DateTime ServiceOverrideDate { get; set; }
        public string OverrideReason { get; set; }
    }
}
