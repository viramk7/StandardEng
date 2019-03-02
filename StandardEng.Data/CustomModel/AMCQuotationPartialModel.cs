using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandardEng.Data.CustomModel
{
    public class AMCQuotationPartialModel
    {
        public int CommissioningId { get; set; }
        public decimal QuotationAmount { get; set; }
        public decimal GSTPercentage { get; set; }
        public decimal GSTPercentageText { get; set; }
        public decimal GSTAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
