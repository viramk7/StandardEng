using StandardEng.Data.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandardEng.Data.Repository
{
    public class BaseContext
    {
        public static StandardEngEntities GetDbContext()
        {
            StandardEngEntities context = new StandardEngEntities();
            context.Configuration.ProxyCreationEnabled = false;
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;
            objectContext.CommandTimeout = int.MaxValue;
            return context;
        }
    }

    public class CustomRepository
    {
        public static List<Get_UserAccessPermissions_Result> UserAccessPermissions(int? roleId = null, bool isSuperAdmin = false)
        {
            List<Get_UserAccessPermissions_Result> dataList;

            using (StandardEngEntities context = BaseContext.GetDbContext())
            {
                dataList = context.Get_UserAccessPermissions(roleId, isSuperAdmin).ToList();
            }

            return dataList.OrderBy(m => m.MenuName).ToList();
        }


        public static List<AssignRoleList_Result> GetUserRoleRights(int? roleId = null)
        {
            List<AssignRoleList_Result> dataList;

            using (StandardEngEntities context = BaseContext.GetDbContext())
            {
                dataList = context.AssignRoleList(roleId).ToList();
            }

            return dataList.OrderBy(m => m.MenuName).ToList();
        }

        public static bool CheckRoleExistsOrNot(string RoleName, int RoleId)
        {
            using (StandardEngEntities dbContext = BaseContext.GetDbContext())
            {
                bool result = dbContext.tblRole.Any(m => m.RoleName == RoleName && m.RoleId != RoleId);
                return result;
            }
        }

        public static bool IsUserEmailExists(string Email, int UserId)
        {
            using (StandardEngEntities context = BaseContext.GetDbContext())
            {
                bool result = context.tblUser.Any(m => m.UserEmail.ToLower().Trim() == Email.ToLower().Trim() && m.UserId != UserId);
                return result;
            }
        }

        public static bool IsUserNameExists(string Username, int UserId)
        {
            using (StandardEngEntities context = BaseContext.GetDbContext())
            {
                bool result = context.tblUser.Any(m => m.Username.ToLower().Trim() == Username.ToLower().Trim() && m.UserId != UserId);
                return result;
            }
        }

        public static bool IsUserCodeExists(string UserCode, int UserId)
        {
            using (StandardEngEntities context = BaseContext.GetDbContext())
            {
                bool result = context.tblUser.Any(m => m.UserCode.ToLower().Trim() == UserCode.ToLower().Trim() && m.UserId != UserId);
                return result;
            }
        }

        public static bool IsCountryNameExists(string CountryName, int CountryId)
        {
            using (StandardEngEntities context = BaseContext.GetDbContext())
            {
                bool result = context.tblCountry.Any(m => m.CountryName.ToLower().Trim() == CountryName.ToLower().Trim() && m.CountryId != CountryId);
                return result;
            }
        }

        public static bool IsStateNameExists(string StateName, int StateId)
        {
            using (StandardEngEntities context = BaseContext.GetDbContext())
            {
                bool result = context.tblState.Any(m => m.StateName.ToLower().Trim() == StateName.ToLower().Trim() && m.StateId != StateId);
                return result;
            }
        }

        public static bool IsCityNameExists(string CityName, int CityId)
        {
            using (StandardEngEntities context = BaseContext.GetDbContext())
            {
                bool result = context.tblCity.Any(m => m.CityName.ToLower().Trim() == CityName.ToLower().Trim() && m.CityId != CityId);
                return result;
            }
        }

        public static bool IsCustomerNameExists(string CustomerName, int CustomerId)
        {
            using (StandardEngEntities context = BaseContext.GetDbContext())
            {
                bool result = context.tblCustomer.Any(m => m.CustomerName.ToLower().Trim() == CustomerName.ToLower().Trim() && m.CustomerId != CustomerId);
                return result;
            }
        }

        public static List<GetWarrantyExpiryCustomerList_Result> GetWarrantyExpiryCustomerList(int? roleId = null, bool isSuperAdmin = false)
        {
            List<GetWarrantyExpiryCustomerList_Result> dataList;

            using (StandardEngEntities context = BaseContext.GetDbContext())
            {
                dataList = context.GetWarrantyExpiryCustomerList().ToList();
            }

            return dataList.OrderBy(m => m.CustomerName).ToList();
        }

        public static bool IsAMCQuotationWithSameAmountExists(int MachineTypeId, int MachineModelId, string MachineSerialNo, int CustomerId, decimal ActualAmount, decimal GSTPercentage,int Id)
        {
            using (StandardEngEntities context = BaseContext.GetDbContext())
            {
                bool result = context.tblAMCQuotation.Any(m => m.MachineTypeId == MachineTypeId && m.MachineModelId == MachineModelId
                                                           && m.MachineSerialNo.Trim() == MachineSerialNo.Trim()
                                                           && m.CustomerId == CustomerId && m.ActualAmount == ActualAmount && m.GSTPercentage == GSTPercentage
                                                           && m.Id != Id);
                return result;
            }
        }

        public static bool IsCommissioningReportServiceNoExists(int CommissioningId, string ReportServiceNo)
        {
            using (StandardEngEntities context = BaseContext.GetDbContext())
            {
                bool result = context.tblCommissioning.Any(m => m.ReportServiceNo == ReportServiceNo 
                                                           && m.CommissioningId != CommissioningId);
                return result;
            }
        }

        public static bool IsPCMachineSerialAlreadyExists(int PreCommissioningId,string MachineSerialNo, int PCMachined)
        {
            using (StandardEngEntities context = BaseContext.GetDbContext())
            {
                bool result = context.tblPreCommissioningMachine.Any(m => m.MachineSerialNo.Trim().ToLower() == MachineSerialNo.Trim().ToLower() && m.PreCommissioningId == PreCommissioningId && m.PCMachined != PCMachined);
                return result;
            }
        }

        public static bool IsPCAccessorySerialAlreadyExists(int PreCommissioningId, string AccessoriesSerialNo, int PCAccessoriesId)
        {
            using (StandardEngEntities context = BaseContext.GetDbContext())
            {
                bool result = context.tblPreCommissioningAccessories.Any(m => m.AccessoriesSerialNo.Trim().ToLower() == AccessoriesSerialNo.Trim().ToLower() && m.PreCommissioningId == PreCommissioningId && m.PCAccessoriesId != PCAccessoriesId);
                return result;
            }
        }
    }
}
