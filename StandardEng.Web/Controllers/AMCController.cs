using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using StandardEng.Common;
using StandardEng.Data.CustomModel;
using StandardEng.Data.DB;
using StandardEng.Data.Repository;
using StandardEng.Web.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StandardEng.Web.Controllers
{
    public class AMCController : BaseController
    {
        //#region private variables
        //private readonly GenericRepository<tblAMC> _dbRepository;
        //private readonly GenericRepository<tblAMCServices> _dbRepositoryAMCService;
        //#endregion

        //#region Constructor
        //public AMCController()
        //{
        //    _dbRepository = new GenericRepository<tblAMC>();
        //    _dbRepositoryAMCService = new GenericRepository<tblAMCServices>();
        //}
        //#endregion

        //#region Public Methods
        //[HttpGet]
        //public ActionResult Index()
        //{
        //    ViewBag.CustomerList = SelectionList.CustomerList().Select(m => new { m.CustomerId, m.CustomerName });
        //    ViewBag.MachineTypeList = SelectionList.MachineTypeList().Select(m => new { m.MachineTypeId, m.MachineTypeName });
        //    ViewBag.MachineModelList = SelectionList.MachineModelsList().Select(m => new { m.MachineModelId, m.MachineName });
        //    return View();
        //}

        //public ActionResult KendoRead([DataSourceRequest] DataSourceRequest request)
        //{
        //    if (!request.Sorts.Any())
        //    {
        //        request.Sorts.Add(new SortDescriptor("AMCQuotationId", ListSortDirection.Ascending));
        //    }

        //    return Json(_dbRepository.GetEntities().ToDataSourceResult(request));
        //}

        //public ActionResult KendoReadAMCSeriveList([DataSourceRequest] DataSourceRequest request, int AMCId)
        //{
        //    if (!request.Sorts.Any())
        //    {
        //        request.Sorts.Add(new SortDescriptor("AMCServiceId", ListSortDirection.Ascending));
        //    }

        //    return Json(_dbRepositoryAMCService.GetEntities().Where(m=>m.AMCId == AMCId).ToDataSourceResult(request));
        //}

        //public ActionResult Create()
        //{
        //    return View(new tblAMCQuotation());
        //}

        //public ActionResult Edit(int id)
        //{
        //    ViewBag.CustomerList = SelectionList.CustomerList().Select(m => new { m.CustomerId, m.CustomerName });
        //    ViewBag.MachineTypeList = SelectionList.MachineTypeList().Select(m => new { m.MachineTypeId, m.MachineTypeName });
        //    ViewBag.MachineModelList = SelectionList.MachineModelsList().Select(m => new { m.MachineModelId, m.MachineName });
        //    return View("Create", _dbRepository.SelectById(id));
        //}

        //public ActionResult SaveModelData(tblAMC model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View("Create", model);
        //    }

        //    string message = string.Empty;

        //    try
        //    {
        //        if (model.AMCId > 0)
        //        {
        //            model.ModifiedBy = SessionHelper.UserId;
        //            model.ModifiedDate = DateTime.Now;
        //            message = _dbRepository.Update(model);
        //        }
        //        else
        //        {
        //            model.CreatedBy = SessionHelper.UserId;
        //            model.CreatedDate = DateTime.Now;
        //            message = _dbRepository.Insert(model);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        message = CommonHelper.GetErrorMessage(ex);
        //    }

        //    return RedirectToAction("Index");
        //}

        //public ActionResult KendoDestroy([DataSourceRequest] DataSourceRequest request, tblAMC model)
        //{
        //    string deleteMessage = _dbRepository.Delete(model.AMCId);

        //    ModelState.Clear();
        //    if (!string.IsNullOrEmpty(deleteMessage))
        //    {
        //        ModelState.AddModelError(deleteMessage, deleteMessage);
        //    }

        //    return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        //}

        //public ActionResult ChangeAMCServiceStatus(int AMCServiceId)
        //{
        //    AMCServiceStatusPartialModel obj = new AMCServiceStatusPartialModel();
        //    obj.AMCServiceId = AMCServiceId;
        //    return PartialView("_AMCServiceStatusPartial", obj);
        //}

        //public string SaveAMCServiceStatus(AMCServiceStatusPartialModel model)
        //{
        //    if(model.AMCServiceId != 0)
        //    {
        //        tblAMCServices serviceObj = _dbRepositoryAMCService.SelectById(model.AMCServiceId);
        //        if(serviceObj != null)
        //        {
        //            serviceObj.Remarks = model.Remarks;
        //            serviceObj.IsServiceDone = model.IsServiceDone;
        //            _dbRepositoryAMCService.Update(serviceObj);
        //            return model.AMCServiceId.ToString();
        //        }
        //    }
        //    return String.Empty;
        //}
        //#endregion
    }
}