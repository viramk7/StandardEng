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
    
    public partial class tblMachineType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblMachineType()
        {
            this.tblAMCQDetail = new HashSet<tblAMCQDetail>();
            this.tblChargebleQDetail = new HashSet<tblChargebleQDetail>();
            this.tblCommissioning = new HashSet<tblCommissioning>();
            this.tblMachineModels = new HashSet<tblMachineModels>();
            this.tblMachineParts = new HashSet<tblMachineParts>();
            this.tblMachinePartsQuotationDetail = new HashSet<tblMachinePartsQuotationDetail>();
            this.tblPerformaInvoiceDetail = new HashSet<tblPerformaInvoiceDetail>();
            this.tblPreCommissioningMachine = new HashSet<tblPreCommissioningMachine>();
        }
    
        public int MachineTypeId { get; set; }
        public string MachineTypeName { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblAMCQDetail> tblAMCQDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblChargebleQDetail> tblChargebleQDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblCommissioning> tblCommissioning { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblMachineModels> tblMachineModels { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblMachineParts> tblMachineParts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblMachinePartsQuotationDetail> tblMachinePartsQuotationDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblPerformaInvoiceDetail> tblPerformaInvoiceDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblPreCommissioningMachine> tblPreCommissioningMachine { get; set; }
    }
}
