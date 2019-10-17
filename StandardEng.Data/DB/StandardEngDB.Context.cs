﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class StandardEngEntities : DbContext
    {
        public StandardEngEntities()
            : base("name=StandardEngEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tblAccessoriesType> tblAccessoriesType { get; set; }
        public virtual DbSet<tblCity> tblCity { get; set; }
        public virtual DbSet<tblCountry> tblCountry { get; set; }
        public virtual DbSet<tblCustomerContactPersons> tblCustomerContactPersons { get; set; }
        public virtual DbSet<tblMachineAccessories> tblMachineAccessories { get; set; }
        public virtual DbSet<tblMachineType> tblMachineType { get; set; }
        public virtual DbSet<tblMenu> tblMenu { get; set; }
        public virtual DbSet<tblRole> tblRole { get; set; }
        public virtual DbSet<tblRoleMenuMap> tblRoleMenuMap { get; set; }
        public virtual DbSet<tblState> tblState { get; set; }
        public virtual DbSet<tblUser> tblUser { get; set; }
        public virtual DbSet<tblWarrantyexpires> tblWarrantyexpires { get; set; }
        public virtual DbSet<tblMachineParts> tblMachineParts { get; set; }
        public virtual DbSet<tblMachineModels> tblMachineModels { get; set; }
        public virtual DbSet<tblGSTMaster> tblGSTMaster { get; set; }
        public virtual DbSet<tblCommissioning> tblCommissioning { get; set; }
        public virtual DbSet<tblPreCommissioning> tblPreCommissioning { get; set; }
        public virtual DbSet<tblPreCommissioningAccessories> tblPreCommissioningAccessories { get; set; }
        public virtual DbSet<tblPreCommissioningDetail> tblPreCommissioningDetail { get; set; }
        public virtual DbSet<tblPreCommissioningMachine> tblPreCommissioningMachine { get; set; }
        public virtual DbSet<tblMachinePartsQuotationDetail> tblMachinePartsQuotationDetail { get; set; }
        public virtual DbSet<tblPerformaInvoiceDetail> tblPerformaInvoiceDetail { get; set; }
        public virtual DbSet<tblRegion> tblRegion { get; set; }
        public virtual DbSet<tblPerformaInvoice> tblPerformaInvoice { get; set; }
        public virtual DbSet<tblMachinePartsQuotation> tblMachinePartsQuotation { get; set; }
        public virtual DbSet<tblAMCQDetail> tblAMCQDetail { get; set; }
        public virtual DbSet<tblAMCQNote> tblAMCQNote { get; set; }
        public virtual DbSet<tblNote> tblNote { get; set; }
        public virtual DbSet<tblAMC> tblAMC { get; set; }
        public virtual DbSet<tblAMCServices> tblAMCServices { get; set; }
        public virtual DbSet<tblAMCQuotation> tblAMCQuotation { get; set; }
        public virtual DbSet<tblChargeble> tblChargeble { get; set; }
        public virtual DbSet<tblChargebleQDetail> tblChargebleQDetail { get; set; }
        public virtual DbSet<tblChargebleQNote> tblChargebleQNote { get; set; }
        public virtual DbSet<tblChargebleQuotation> tblChargebleQuotation { get; set; }
        public virtual DbSet<tblChargebleNote> tblChargebleNote { get; set; }
        public virtual DbSet<tblCustomer> tblCustomer { get; set; }
    
        public virtual ObjectResult<AssignRoleList_Result> AssignRoleList(Nullable<int> roleId)
        {
            var roleIdParameter = roleId.HasValue ?
                new ObjectParameter("RoleId", roleId) :
                new ObjectParameter("RoleId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<AssignRoleList_Result>("AssignRoleList", roleIdParameter);
        }
    
        public virtual ObjectResult<Get_UserAccessPermissions_Result> Get_UserAccessPermissions(Nullable<int> roleId, Nullable<bool> isSuperAdmin)
        {
            var roleIdParameter = roleId.HasValue ?
                new ObjectParameter("RoleId", roleId) :
                new ObjectParameter("RoleId", typeof(int));
    
            var isSuperAdminParameter = isSuperAdmin.HasValue ?
                new ObjectParameter("IsSuperAdmin", isSuperAdmin) :
                new ObjectParameter("IsSuperAdmin", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Get_UserAccessPermissions_Result>("Get_UserAccessPermissions", roleIdParameter, isSuperAdminParameter);
        }
    
        public virtual ObjectResult<GetWarrantyExpiryCustomerList_Result> GetWarrantyExpiryCustomerList()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetWarrantyExpiryCustomerList_Result>("GetWarrantyExpiryCustomerList");
        }
    
        public virtual ObjectResult<GetPCAsseccoryListDD_Result> GetPCAsseccoryListDD(Nullable<int> preCommissioningId)
        {
            var preCommissioningIdParameter = preCommissioningId.HasValue ?
                new ObjectParameter("PreCommissioningId", preCommissioningId) :
                new ObjectParameter("PreCommissioningId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetPCAsseccoryListDD_Result>("GetPCAsseccoryListDD", preCommissioningIdParameter);
        }
    
        public virtual ObjectResult<GetPCMachineListDD_Result> GetPCMachineListDD(Nullable<int> preCommissioningId)
        {
            var preCommissioningIdParameter = preCommissioningId.HasValue ?
                new ObjectParameter("PreCommissioningId", preCommissioningId) :
                new ObjectParameter("PreCommissioningId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetPCMachineListDD_Result>("GetPCMachineListDD", preCommissioningIdParameter);
        }
    
        public virtual ObjectResult<GetPreCommisioningDetailData_Result> GetPreCommisioningDetailData(Nullable<int> preCommissioningId)
        {
            var preCommissioningIdParameter = preCommissioningId.HasValue ?
                new ObjectParameter("PreCommissioningId", preCommissioningId) :
                new ObjectParameter("PreCommissioningId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetPreCommisioningDetailData_Result>("GetPreCommisioningDetailData", preCommissioningIdParameter);
        }
    
        public virtual ObjectResult<GetPreCommissiningList_Result> GetPreCommissiningList()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetPreCommissiningList_Result>("GetPreCommissiningList");
        }
    
        public virtual ObjectResult<GetPreCommisioningListDetail_Result> GetPreCommisioningListDetail(Nullable<int> preCommissioningId)
        {
            var preCommissioningIdParameter = preCommissioningId.HasValue ?
                new ObjectParameter("PreCommissioningId", preCommissioningId) :
                new ObjectParameter("PreCommissioningId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetPreCommisioningListDetail_Result>("GetPreCommisioningListDetail", preCommissioningIdParameter);
        }
    
        public virtual ObjectResult<GetCommissioningList_Result> GetCommissioningList()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetCommissioningList_Result>("GetCommissioningList");
        }
    
        public virtual ObjectResult<GetPartsQuoatationList_Result> GetPartsQuoatationList()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetPartsQuoatationList_Result>("GetPartsQuoatationList");
        }
    
        public virtual ObjectResult<GetPerformaInvoieList_Result> GetPerformaInvoieList()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetPerformaInvoieList_Result>("GetPerformaInvoieList");
        }
    
        public virtual ObjectResult<GetAMCList_Result> GetAMCList()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetAMCList_Result>("GetAMCList");
        }
    
        public virtual ObjectResult<GetCommissioningListForPartsQuotation_Result> GetCommissioningListForPartsQuotation(Nullable<int> customerId)
        {
            var customerIdParameter = customerId.HasValue ?
                new ObjectParameter("CustomerId", customerId) :
                new ObjectParameter("CustomerId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetCommissioningListForPartsQuotation_Result>("GetCommissioningListForPartsQuotation", customerIdParameter);
        }
    
        public virtual ObjectResult<GetChargebleList_Result> GetChargebleList()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetChargebleList_Result>("GetChargebleList");
        }
    }
}
