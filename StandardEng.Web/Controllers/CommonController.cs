using StandardEng.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StandardEng.Web.Controllers
{
    public class CommonController : Controller
    {
        public ActionResult GetCountryList()
        {
            using (var context = BaseContext.GetDbContext())
            {
                var list = context.tblCountry.Where(m => m.IsActive).Select(m => new { m.CountryId, m.CountryName }).OrderBy(m => new { m.CountryName }).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetStateList(int? CountryId = 0)
        {
            using (var context = BaseContext.GetDbContext())
            {
                if (CountryId == 0)
                {
                    var list = context.tblState.Where(m => m.IsActive).Select(m => new { m.StateId, m.StateName }).ToList();
                    return Json(list, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var list = context.tblState.Where(m => m.IsActive && m.CountryId == CountryId).Select(m => new { m.StateId, m.StateName }).ToList();
                    return Json(list, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult GetCityList(int? CountryId = 0, int? StateId = 0)
        {
            using (var context = BaseContext.GetDbContext())
            {
                    var list = context.tblCity.Where(m => m.IsActive && m.CountryId == CountryId && m.StateId == StateId).Select(m => new { m.CityId, m.CityName }).ToList();
                    return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetCustomerList()
        {
            using (var context = BaseContext.GetDbContext())
            {
                var list = context.tblCustomer.Select(m => new { m.CustomerId, m.CustomerName }).OrderBy(m => new { m.CustomerName }).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetCustomerContactPersonList(int? CustomerId = 0)
        {
            using (var context = BaseContext.GetDbContext())
            {
                if(CustomerId > 0)
                {
                    var list = context.tblCustomerContactPersons.Where(m => m.CustomerId == CustomerId).Select(m => new { m.ContactPersonId, m.ContactPersonName, m.ContactNo , m.ContactPersonEmail }).OrderBy(m => new { m.ContactPersonName }).ToList();
                    return Json(list, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var list = context.tblCustomerContactPersons.Select(m => new { m.ContactPersonId, m.ContactPersonName, m.ContactNo, m.ContactPersonEmail }).OrderBy(m => new { m.ContactPersonName }).ToList();
                    return Json(list, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult GetMachineTypeList()
        {
            using (var context = BaseContext.GetDbContext())
            {
                var list = context.tblMachineType.Select(m => new { m.MachineTypeId, m.MachineTypeName }).OrderBy(m => new { m.MachineTypeName }).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetMachineModelList(int? MachineTypeId = 0)
        {
            using (var context = BaseContext.GetDbContext())
            {
                if(MachineTypeId != 0)
                {
                    var list = context.tblMachineModels.Where(m => m.MachineTypeId == MachineTypeId).Select(m => new { m.MachineModelId, m.MachineName }).OrderBy(m => new { m.MachineModelId }).ToList();
                    return Json(list, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var list = context.tblMachineModels.Select(m => new { m.MachineModelId, m.MachineName }).OrderBy(m => new { m.MachineModelId }).ToList();
                    return Json(list, JsonRequestBehavior.AllowGet);
                }
                
            }
        }

        public ActionResult GetServiceEngineerList()
        {
            using (var context = BaseContext.GetDbContext())
            {
                var list = context.tblUser.Select(m => new { m.UserId, m.Name }).OrderBy(m => new { m.UserId }).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetAccessoriesTypeList()
        {
            using (var context = BaseContext.GetDbContext())
            {
                var list = context.tblAccessoriesType.Select(m => new { m.AccessoriesTypeId, m.AccessoriesTypeName }).OrderBy(m => new { m.AccessoriesTypeId }).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetMachineAccessoriesList(int? AccessoriesTypeId = 0)
        {
            using (var context = BaseContext.GetDbContext())
            {
                var list = context.tblMachineAccessories.Where(m => m.AccessoriesTypeId == AccessoriesTypeId).Select(m => new { m.MachineAccessoriesId, m.AccessoriesName }).OrderBy(m => new { m.MachineAccessoriesId }).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetGSTPercentageList()
        {
            using (var context = BaseContext.GetDbContext())
            {
                var list = context.tblGSTMaster.Select(m => new { m.Id, Percentage= m.Percentage.ToString() }).OrderBy(m => new { m.Id }).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetMachinePartList(int? MachineTypeId = 0)
        {
            using (var context = BaseContext.GetDbContext())
            {
                if(MachineTypeId > 0)
                {
                    var list = context.tblMachineParts.Where(m=>m.MachineTypeId == MachineTypeId).Select(m => new { m.MachinePartId, ProductValue = m.ProductValue + "-" + m.AlternateProductValue + "-" + m.AlternateProductValue1, m.Description, m.HSNCode ,m.IPL}).OrderBy(m => new { m.MachinePartId }).ToList();
                    return Json(list, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var list = context.tblMachineParts.Select(m => new { m.MachinePartId, ProductValue = m.ProductValue + "-" + m.AlternateProductValue + "-" + m.AlternateProductValue1, m.Description, m.HSNCode, m.IPL }).OrderBy(m => new { m.MachinePartId }).ToList();
                    return Json(list, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult GetPCMachineListDD(int PreCommissioningId)
        {
            using (var context = BaseContext.GetDbContext())
            {
                var list = context.GetPCMachineListDD(PreCommissioningId).Select(m => new { m.PCMachined, m.MachineName }).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetPCAsseccoryListDD(int PreCommissioningId)
        {
            using (var context = BaseContext.GetDbContext())
            {
                var list = context.GetPCAsseccoryListDD(PreCommissioningId).Select(m => new { m.PCAccessoriesId, m.AccessoriesName }).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetRegionList(int CityId)
        {
            using (var context = BaseContext.GetDbContext())
            {
                var list = context.tblRegion.Where(m=>m.CityId == CityId).Select(m => new { m.Id, m.Name}).OrderBy(m => new { m.Id }).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
        }
    }
}