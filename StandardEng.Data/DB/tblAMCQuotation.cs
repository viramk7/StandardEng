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
    
    public partial class tblAMCQuotation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblAMCQuotation()
        {
            this.tblAMCQDetail = new HashSet<tblAMCQDetail>();
            this.tblAMCQNote = new HashSet<tblAMCQNote>();
        }
    
        public int AMCQId { get; set; }
        public Nullable<int> CommissioningId { get; set; }
        public string AMCQuotationNo { get; set; }
        public System.DateTime QuotationDate { get; set; }
        public int CustomerId { get; set; }
        public int CustomerContactPId { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public int RegionId { get; set; }
        public string Addressline1 { get; set; }
        public string Addressline2 { get; set; }
        public string Addressline3 { get; set; }
        public string PinCode { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string CustomerGST { get; set; }
        public Nullable<int> AMCBy { get; set; }
        public string Remarks { get; set; }
        public Nullable<decimal> TotalDetailAmount { get; set; }
        public Nullable<int> GSTPercentageId { get; set; }
        public Nullable<decimal> GSTAmount { get; set; }
        public Nullable<decimal> TotalGSTAmount { get; set; }
        public Nullable<decimal> FinalAmount { get; set; }
        public string FinalAmountInWords { get; set; }
        public Nullable<int> SequenceNo { get; set; }
        public bool IsConvertedIntoAMC { get; set; }
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
        public string QuotationYear { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblAMCQDetail> tblAMCQDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblAMCQNote> tblAMCQNote { get; set; }
        public virtual tblCustomer tblCustomer { get; set; }
        public virtual tblCustomerContactPersons tblCustomerContactPersons { get; set; }
    }
}
