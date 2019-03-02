using StandardEng.Data.DB;
using StandardEng.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StandardEng.Web.Common
{
    public class SelectionList
    {
        public static List<tblRole> RoleList()
        {
            using (StandardEngEntities dbContext = BaseContext.GetDbContext())
            {
                List<tblRole> result = dbContext.tblRole.OrderBy(m => m.RoleName).ToList();
                return result;
            }
        }

        public static List<tblCountry> CountryList()
        {
            using (StandardEngEntities dbContext = BaseContext.GetDbContext())
            {
                List<tblCountry> result = dbContext.tblCountry.OrderBy(m => m.CountryName).ToList();
                return result;
            }
        }

        public static List<tblState> StateList()
        {
            using (StandardEngEntities dbContext = BaseContext.GetDbContext())
            {
                List<tblState> result = dbContext.tblState.OrderBy(m => m.StateName).ToList();
                return result;
            }
        }

        public static List<tblCity> CityList()
        {
            using (StandardEngEntities dbContext = BaseContext.GetDbContext())
            {
                List<tblCity> result = dbContext.tblCity.OrderBy(m => m.CityName).ToList();
                return result;
            }
        }

        public static List<tblMachineType> MachineTypeList()
        {
            using (StandardEngEntities dbContext = BaseContext.GetDbContext())
            {
                List<tblMachineType> result = dbContext.tblMachineType.OrderBy(m => m.MachineTypeName).ToList();
                return result;
            }
        }

        public static List<tblCustomer> CustomerList()
        {
            using (StandardEngEntities dbContext = BaseContext.GetDbContext())
            {
                List<tblCustomer> result = dbContext.tblCustomer.OrderBy(m => m.CustomerName).ToList();
                return result;
            }
        }

        public static List<tblCustomerContactPersons> ContactPersonsList()
        {
            using (StandardEngEntities dbContext = BaseContext.GetDbContext())
            {
                List<tblCustomerContactPersons> result = dbContext.tblCustomerContactPersons.OrderBy(m => m.CustomerId).ToList();
                return result;
            }
        }

        public static List<tblMachineModels> MachineModelsList()
        {
            using (StandardEngEntities dbContext = BaseContext.GetDbContext())
            {
                List<tblMachineModels> result = dbContext.tblMachineModels.OrderBy(m => m.MachineModelId).ToList();
                return result;
            }
        }

        public static List<tblAccessoriesType> AccessoriesTypeList()
        {
            using (StandardEngEntities dbContext = BaseContext.GetDbContext())
            {
                List<tblAccessoriesType> result = dbContext.tblAccessoriesType.OrderBy(m => m.AccessoriesTypeId).ToList();
                return result;
            }
        }

        public static List<tblMachineAccessories> MachineAccessoriesList()
        {
            using (StandardEngEntities dbContext = BaseContext.GetDbContext())
            {
                List<tblMachineAccessories> result = dbContext.tblMachineAccessories.OrderBy(m => m.MachineAccessoriesId).ToList();
                return result;
            }
        }

        public static List<tblUser> ServiceEngineerList()
        {
            using (StandardEngEntities dbContext = BaseContext.GetDbContext())
            {
                List<tblUser> result = dbContext.tblUser.OrderBy(m => m.UserId).ToList();
                return result;
            }
        }

        public static List<tblMachineParts> MachinePartsList()
        {
            using (StandardEngEntities dbContext = BaseContext.GetDbContext())
            {
                List<tblMachineParts> result = dbContext.tblMachineParts.OrderBy(m => m.MachinePartId).ToList();
                return result;
            }
        }

        public static List<tblGSTMaster> GSTPercentageList()
        {
            using (StandardEngEntities dbContext = BaseContext.GetDbContext())
            {
                List<tblGSTMaster> result = dbContext.tblGSTMaster.OrderBy(m => m.Id).ToList();
                return result;
            }
        }

        public static List<tblPreCommissioningAccessories> PreCommissioningAccessoriesList(int? id = 0)
        {
            using (StandardEngEntities dbContext = BaseContext.GetDbContext())
            {
                if(id > 0)
                {
                    List<tblPreCommissioningAccessories> result = dbContext.tblPreCommissioningAccessories.Where(m => m.PreCommissioningId == id).ToList();
                    return result;
                }
                else
                {
                    List<tblPreCommissioningAccessories> result = dbContext.tblPreCommissioningAccessories.ToList();
                    return result;
                }
                
            }
        }

        public static List<tblPreCommissioningMachine> PreCommissioningMachineList(int? id = 0)
        {
            using (StandardEngEntities dbContext = BaseContext.GetDbContext())
            {
                if (id > 0)
                {
                    List<tblPreCommissioningMachine> result = dbContext.tblPreCommissioningMachine.Where(m => m.PreCommissioningId == id).ToList();
                    return result;
                }
                else
                {
                    List<tblPreCommissioningMachine> result = dbContext.tblPreCommissioningMachine.ToList();
                    return result;
                }

            }
        }
    }
}