//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataImport
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblPerformaInvoice
    {
        public int PerformaInvoiceId { get; set; }
        public Nullable<int> MPQuotationId { get; set; }
        public string QuotationNo { get; set; }
        public System.DateTime QuotationDate { get; set; }
        public int CustomerId { get; set; }
        public int CustomerContactPId { get; set; }
        public string CustomerContactPContactNo { get; set; }
        public string ReportServiceNo { get; set; }
        public string InquiryNo { get; set; }
        public Nullable<System.DateTime> InquiryDate { get; set; }
        public Nullable<int> PaymentDays { get; set; }
        public Nullable<int> DeliveryWeeks { get; set; }
        public string Insurance { get; set; }
        public Nullable<int> ValidityDays { get; set; }
        public string Email { get; set; }
        public Nullable<decimal> TotalFinalAmount { get; set; }
        public Nullable<decimal> FreightAmount { get; set; }
        public Nullable<decimal> QuotationAmount { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> SequenceNo { get; set; }
    
        public virtual tblCustomer tblCustomer { get; set; }
        public virtual tblCustomerContactPersons tblCustomerContactPersons { get; set; }
    }
}
