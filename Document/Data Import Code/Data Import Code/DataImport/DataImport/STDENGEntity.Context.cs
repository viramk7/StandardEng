﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
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
        public virtual DbSet<tblAMC> tblAMC { get; set; }
        public virtual DbSet<tblAMCQuotation> tblAMCQuotation { get; set; }
        public virtual DbSet<tblAMCServices> tblAMCServices { get; set; }
        public virtual DbSet<tblCity> tblCity { get; set; }
        public virtual DbSet<tblCommissioning> tblCommissioning { get; set; }
        public virtual DbSet<tblCountry> tblCountry { get; set; }
        public virtual DbSet<tblCustomer> tblCustomer { get; set; }
        public virtual DbSet<tblCustomerContactPersons> tblCustomerContactPersons { get; set; }
        public virtual DbSet<tblGSTMaster> tblGSTMaster { get; set; }
        public virtual DbSet<tblMachineAccessories> tblMachineAccessories { get; set; }
        public virtual DbSet<tblMachineModels> tblMachineModels { get; set; }
        public virtual DbSet<tblMachineParts> tblMachineParts { get; set; }
        public virtual DbSet<tblMachinePartsQuotation> tblMachinePartsQuotation { get; set; }
        public virtual DbSet<tblMachinePartsQuotationDetail> tblMachinePartsQuotationDetail { get; set; }
        public virtual DbSet<tblMachineType> tblMachineType { get; set; }
        public virtual DbSet<tblMenu> tblMenu { get; set; }
        public virtual DbSet<tblPerformaInvoice> tblPerformaInvoice { get; set; }
        public virtual DbSet<tblPerformaInvoiceDetail> tblPerformaInvoiceDetail { get; set; }
        public virtual DbSet<tblPreCommissioning> tblPreCommissioning { get; set; }
        public virtual DbSet<tblPreCommissioningAccessories> tblPreCommissioningAccessories { get; set; }
        public virtual DbSet<tblPreCommissioningDetail> tblPreCommissioningDetail { get; set; }
        public virtual DbSet<tblPreCommissioningMachine> tblPreCommissioningMachine { get; set; }
        public virtual DbSet<tblRole> tblRole { get; set; }
        public virtual DbSet<tblRoleMenuMap> tblRoleMenuMap { get; set; }
        public virtual DbSet<tblSettings> tblSettings { get; set; }
        public virtual DbSet<tblState> tblState { get; set; }
        public virtual DbSet<tblUser> tblUser { get; set; }
        public virtual DbSet<tblWarrantyexpires> tblWarrantyexpires { get; set; }
        public virtual DbSet<AggregatedCounter> AggregatedCounter { get; set; }
        public virtual DbSet<Counter> Counter { get; set; }
        public virtual DbSet<Hash> Hash { get; set; }
        public virtual DbSet<Job> Job { get; set; }
        public virtual DbSet<JobParameter> JobParameter { get; set; }
        public virtual DbSet<JobQueue> JobQueue { get; set; }
        public virtual DbSet<List> List { get; set; }
        public virtual DbSet<Schema> Schema { get; set; }
        public virtual DbSet<Server> Server { get; set; }
        public virtual DbSet<Set> Set { get; set; }
        public virtual DbSet<State> State { get; set; }
    }
}
