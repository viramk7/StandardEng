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
    
    public partial class tblPreCommissioningDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblPreCommissioningDetail()
        {
            this.tblCommissioning = new HashSet<tblCommissioning>();
        }
    
        public int PCDetailId { get; set; }
        public int PreCommissionId { get; set; }
        public Nullable<int> PCMachineId { get; set; }
        public Nullable<int> PCAccesseriesId { get; set; }
        public System.DateTime PreCommisoningDate { get; set; }
        public bool IsCommisioning { get; set; }
        public int ServiceEngineerId { get; set; }
        public string PrecommisioningRemark { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblCommissioning> tblCommissioning { get; set; }
        public virtual tblPreCommissioning tblPreCommissioning { get; set; }
        public virtual tblUser tblUser { get; set; }
    }
}
