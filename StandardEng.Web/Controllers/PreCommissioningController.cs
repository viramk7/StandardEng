using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using StandardEng.Common;
using StandardEng.Data.DB;
using StandardEng.Data.Repository;
using StandardEng.Web.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace StandardEng.Web.Controllers
{
    public class PreCommissioningController : BaseController
    {
        #region private variables

        private readonly GenericRepository<tblPreCommissioning> _dbRepository;

        #endregion
        #region Constructor
        public PreCommissioningController()
        {
            _dbRepository = new GenericRepository<tblPreCommissioning>();
        }
        #endregion

        #region Public Methods
        [HttpGet]
        public ActionResult Index()
        {
            //ViewBag.CustomerList = SelectionList.CustomerList().Select(m => new { m.CustomerId, m.CustomerName });
            //ViewBag.ContactPersonsList = SelectionList.ContactPersonsList().Select(m => new { m.ContactPersonId, m.ContactPersonName });
            
            return View();
        }

        public ActionResult KendoRead([DataSourceRequest] DataSourceRequest request)
        {
            //if (!request.Sorts.Any())
            //{
            //    request.Sorts.Add(new SortDescriptor("PreCommissioningId", ListSortDirection.Ascending));
            //}

            return Json(CustomRepository.GetPreCommissiningList().ToDataSourceResult(request));
        }

        public ActionResult KendoReadDetailById([DataSourceRequest] DataSourceRequest request,int preCommisioningId)
        {
            //if (!request.Sorts.Any())
            //{
            //    request.Sorts.Add(new SortDescriptor("PreCommissioningId", ListSortDirection.Ascending));
            //}

            return Json(CustomRepository.GetPreCommisioningListDetail(preCommisioningId).ToDataSourceResult(request));
        }

        public ActionResult Create()
        {
            //ViewBag.ServicEngineerList = SelectionList.ServiceEngineerList().Select(m => new { m.UserId, m.Name });
            ViewBag.AccessoriesTypeList = SelectionList.AccessoriesTypeList().Select(m => new { m.AccessoriesTypeId, m.AccessoriesTypeName });
            ViewBag.MachineAccessoriesList = SelectionList.MachineAccessoriesList().Select(m => new { m.MachineAccessoriesId, m.AccessoriesName });
            ViewBag.MachineTypeList = SelectionList.MachineTypeList().Select(m => new { m.MachineTypeId, m.MachineTypeName });
            ViewBag.MachineModelList = SelectionList.MachineModelsList().Select(m => new { m.MachineModelId, m.MachineName });
            //ViewBag.PreCommissioningAccessoriesList = SelectionList.PreCommissioningAccessoriesList().Select(m => new { m.PCAccessoriesId, m.AccessoriesSerialNo });
            //ViewBag.PreCommissioningMachineList = SelectionList.PreCommissioningMachineList().Select(m => new { m.PCMachined, m.MachineSerialNo });
            return View(new tblPreCommissioning());
        }

        public ActionResult Edit(int id)
        {
            //ViewBag.ServicEngineerList = SelectionList.ServiceEngineerList().Select(m => new { m.UserId, m.Name });
            ViewBag.AccessoriesTypeList = SelectionList.AccessoriesTypeList().Select(m => new { m.AccessoriesTypeId, m.AccessoriesTypeName });
            ViewBag.MachineAccessoriesList = SelectionList.MachineAccessoriesList().Select(m => new { m.MachineAccessoriesId, m.AccessoriesName });
            ViewBag.MachineTypeList = SelectionList.MachineTypeList().Select(m => new { m.MachineTypeId, m.MachineTypeName });
            ViewBag.MachineModelList = SelectionList.MachineModelsList().Select(m => new { m.MachineModelId, m.MachineName });
            //ViewBag.PreCommissioningAccessoriesList = SelectionList.PreCommissioningAccessoriesList(id).Select(m => new { m.PCAccessoriesId, m.AccessoriesSerialNo });
            //ViewBag.PreCommissioningMachineList = SelectionList.PreCommissioningMachineList(id).Select(m => new { m.PCMachined, m.MachineSerialNo });
            return View("Create", _dbRepository.SelectById(id));
        }

        public ActionResult SaveModelData(tblPreCommissioning model, string create = null)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }

            string message = string.Empty;

            try
            {
                if(model.PreCommissioningId > 0)
                {
                    model.ModifiedBy = SessionHelper.UserId;
                    model.ModifiedDate = DateTime.Now;
                    message = _dbRepository.Update(model);
                }
                else
                {
                    model.CreatedBy = SessionHelper.UserId;
                    model.CreatedDate = DateTime.Now;
                    model.IsCommissioningDone = false;
                    message =  _dbRepository.Insert(model);
                }
            }
            catch (Exception ex)
            {
                message = CommonHelper.GetErrorMessage(ex);
            }

            if (model.PreCommissioningId > 0)
            {
                if (create == "Save & Continue")
                {
                    return RedirectToAction("Edit", new { id = model.PreCommissioningId });
                }
                else if (create == "Save & New")
                {
                    return RedirectToAction("Create");
                }
            }
            return RedirectToAction("Index");
        }

        public string KendoDestroy(int id)
        {
            string deleteMessage = string.Empty;

            try
            {
                if (id != 0)
                {
                    deleteMessage = _dbRepository.Delete(id);
                }
                else
                {
                    deleteMessage = "Something went wrong.";
                }
            }
            catch (Exception ex)
            {
                deleteMessage = CommonHelper.GetDeleteException(ex);
            }

            return deleteMessage;
        }

        #endregion
    }
}