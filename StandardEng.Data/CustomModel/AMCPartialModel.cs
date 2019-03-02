using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandardEng.Data.CustomModel
{
    public class AMCPartialModel
    {
        public int AMCQuotationId { get; set; }
        public string Remarks { get; set; }
    }

    public class AMCServiceStatusPartialModel
    {
        public int AMCServiceId { get; set; }
        public string Remarks { get; set; }
        public bool IsServiceDone { get; set; }
    }
}
