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
    
    public partial class tblMachineModels
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblMachineModels()
        {
            this.tblCommissioning = new HashSet<tblCommissioning>();
            this.tblPreCommissioningMachine = new HashSet<tblPreCommissioningMachine>();
            this.tblMachinePartsQuotationDetail = new HashSet<tblMachinePartsQuotationDetail>();
            this.tblPerformaInvoiceDetail = new HashSet<tblPerformaInvoiceDetail>();
        }
    
        public int MachineModelId { get; set; }
        public int MachineTypeId { get; set; }
        public string MachineName { get; set; }
        public int WarrantyPeriod { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual tblMachineType tblMachineType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblCommissioning> tblCommissioning { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblPreCommissioningMachine> tblPreCommissioningMachine { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblMachinePartsQuotationDetail> tblMachinePartsQuotationDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblPerformaInvoiceDetail> tblPerformaInvoiceDetail { get; set; }
    }
}
