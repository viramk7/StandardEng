//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StandardEng.Data.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblChargeble
    {
        public int ChargebleId { get; set; }
        public int ChargebleQId { get; set; }
        public int ChargebleQDetailId { get; set; }
        public System.DateTime ChargebleDate { get; set; }
        public string QuotationNo { get; set; }
        public int CustomerId { get; set; }
        public int CustomerContactPId { get; set; }
        public string ContactNo { get; set; }
        public string EmailAddress { get; set; }
        public string Remarks { get; set; }
        public int MachineTypeId { get; set; }
        public int MachineModelId { get; set; }
        public string MachineSerialNo { get; set; }
        public decimal Amount { get; set; }
        public int GSTPercentageId { get; set; }
        public decimal GSTAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public Nullable<bool> IsDifferentShipAddress { get; set; }
        public string ShipCompanyName { get; set; }
        public string ShipAddressline1 { get; set; }
        public string ShipAddressline2 { get; set; }
        public string ShipAddressline3 { get; set; }
        public string ShipGSTNo { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string MachineDescription { get; set; }
        public string FinalAmountInWords { get; set; }
    }
}
