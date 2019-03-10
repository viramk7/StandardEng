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
    
    public partial class tblPerformaInvoiceDetail
    {
        public int PIDetailId { get; set; }
        public int PerformaInvoiceId { get; set; }
        public int MPQDetailId { get; set; }
        public int MachineTypeId { get; set; }
        public int MachineModelId { get; set; }
        public string MachineModelSerialNo { get; set; }
        public int MachinePartsId { get; set; }
        public string MachinePartsNo { get; set; }
        public string MachinePartDescription { get; set; }
        public string PartsHSNCode { get; set; }
        public int PartsQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public Nullable<decimal> PAndFPercentage { get; set; }
        public Nullable<decimal> ProfitMarginPercentage { get; set; }
        public Nullable<decimal> DiscountPercentage { get; set; }
        public decimal TaxablePrice { get; set; }
        public decimal GSTPercentage { get; set; }
        public decimal GSTAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual tblMachineModels tblMachineModels { get; set; }
        public virtual tblMachineParts tblMachineParts { get; set; }
        public virtual tblMachineType tblMachineType { get; set; }
    }
}
