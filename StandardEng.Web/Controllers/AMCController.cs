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
        #region private variables
        private readonly GenericRepository<tblAMC> _dbRepository;
        private readonly GenericRepository<tblAMCServices> _dbRepositoryAMCService;
        private readonly GenericRepository<tblAMCQuotation> _dbRepositoryAMCQuotation;
        private readonly GenericRepository<tblAMCStart> _dbRepositoryAMCStart;

        #endregion

        #region Constructor
        public AMCController()
        {
            _dbRepository = new GenericRepository<tblAMC>();
            _dbRepositoryAMCService = new GenericRepository<tblAMCServices>();
            _dbRepositoryAMCQuotation = new GenericRepository<tblAMCQuotation>();
            _dbRepositoryAMCStart = new GenericRepository<tblAMCStart>();
        }
        #endregion

        #region Public Methods
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult KendoRead([DataSourceRequest] DataSourceRequest request)
        {
            if (!request.Sorts.Any())
            {
                request.Sorts.Add(new SortDescriptor("AMCId", ListSortDirection.Descending));
            }

            return Json(CustomRepository.GetAMCList().ToDataSourceResult(request));
        }

        public ActionResult KendoReadAMCSeriveList([DataSourceRequest] DataSourceRequest request, int AMCId)
        {
            if (!request.Sorts.Any())
            {
                request.Sorts.Add(new SortDescriptor("AMCServiceId", ListSortDirection.Descending));
            }

            return Json(_dbRepositoryAMCService.GetEntities().Where(m => m.AMCId == AMCId).ToDataSourceResult(request));
        }

        //public ActionResult Create()
        //{
        //    return View(new tblAMC());
        //}

        public ActionResult Edit(int id)
        {
            return View("Create", _dbRepository.SelectById(id));
        }

        public ActionResult SaveModelData(tblAMC model)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }

            string message = string.Empty;

            try
            {
                if (model.AMCId > 0)
                {
                    model.ModifiedBy = SessionHelper.UserId;
                    model.ModifiedDate = DateTime.Now;
                    message = _dbRepository.Update(model);
                }
                else
                {
                    model.CreatedBy = SessionHelper.UserId;
                    model.CreatedDate = DateTime.Now;
                    message = _dbRepository.Insert(model);
                }
            }
            catch (Exception ex)
            {
                message = CommonHelper.GetErrorMessage(ex);
            }

            return RedirectToAction("Index");
        }

        public ActionResult KendoDestroy([DataSourceRequest] DataSourceRequest request, tblAMC model)
        {

            //tblAMCQuotation quotationObj = _dbRepositoryAMCQuotation.SelectById(model.AMCQuotationId);
            //quotationObj.IsConvertedIntoAMC = false;
            //_dbRepositoryAMCQuotation.Update(quotationObj);

            string deleteMessage = _dbRepository.Delete(model.AMCId);

            ModelState.Clear();
            if (!string.IsNullOrEmpty(deleteMessage))
            {
                ModelState.AddModelError(deleteMessage, deleteMessage);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult StartAMCService(int AMCId)
        {
            string result = string.Empty;
            if (AMCId > 0)
            {
                //Add AMC start refrence
                tblAMCStart obj = new tblAMCStart();
                obj.AMCId = AMCId;
                obj.AMCStartDate = DateTime.Now.Date;
                obj.AMCEndDate = DateTime.Now.Date.AddYears(1);
                obj.IsActive = true;
                obj.CreatedBy = SessionHelper.UserId;
                obj.CreatedDate = DateTime.Now.Date;
                _dbRepositoryAMCStart.Insert(obj);
                
                //Add services for AMC 
                tblAMCServices amcServie1 = new tblAMCServices();
                amcServie1.AMCId = AMCId;
                amcServie1.ServviceDate = DateTime.Now.Date.AddMonths(3);
                amcServie1.IsCompleted = false;
                amcServie1.IsSystemGenerated = true;
                result = _dbRepositoryAMCService.Insert(amcServie1);

                tblAMCServices amcServie2 = new tblAMCServices();
                amcServie2.AMCId = AMCId;
                amcServie2.ServviceDate = DateTime.Now.Date.AddMonths(6);
                amcServie2.IsCompleted = false;
                amcServie2.IsSystemGenerated = true;
                result = _dbRepositoryAMCService.Insert(amcServie2);

                tblAMCServices amcServie3 = new tblAMCServices();
                amcServie3.AMCId = AMCId;
                amcServie3.ServviceDate = DateTime.Now.Date.AddMonths(9);
                amcServie3.IsCompleted = false;
                amcServie3.IsSystemGenerated = true;
                result = _dbRepositoryAMCService.Insert(amcServie3);

                tblAMCServices amcServie4 = new tblAMCServices();
                amcServie4.AMCId = AMCId;
                amcServie4.ServviceDate = DateTime.Now.Date.AddMonths(12);
                amcServie4.IsCompleted = false;
                amcServie4.IsSystemGenerated = true;
                result = _dbRepositoryAMCService.Insert(amcServie4);

                if (string.IsNullOrEmpty(result))
                {
                    tblAMC amc = _dbRepository.SelectById(AMCId);
                    amc.IsServiceStarted = true;
                    _dbRepository.Update(amc);
                }

                return RedirectToAction("Index", "AMCServices");
            }
            return RedirectToAction("Edit", "AMC", new { id = AMCId });
        }
        #endregion

        #region AMC Service Methods
        public ActionResult CompleteAMCServiceStatus(int AMCServiceId)
        {
            AMCServiceCompltePartialModel obj = new AMCServiceCompltePartialModel();
            obj.AMCServiceId = AMCServiceId;
            return PartialView("_AMCServiceCompletePartial", obj);
        }

        public string SaveCompletedAMCService(AMCServiceCompltePartialModel model)
        {
            if (model.AMCServiceId != 0)
            {
                tblAMCServices serviceObj = _dbRepositoryAMCService.SelectById(model.AMCServiceId);
                if (serviceObj != null)
                {
                    serviceObj.ServiceRemarks = model.ServiceRemarks;
                    serviceObj.IsCompleted = true;
                    serviceObj.ServiceEngineerId = model.ServiceEngineerId;
                    serviceObj.ServiceReportNo = model.ServiceReportNo;
                    string msg = _dbRepositoryAMCService.Update(serviceObj);
                    return model.AMCServiceId.ToString();
                }
            }
            return String.Empty;
        }

        public ActionResult OverWriteAMCService(int AMCServiceId)
        {
            AMCServiceOverWritePartialModel obj = new AMCServiceOverWritePartialModel();
            obj.AMCServiceId = AMCServiceId;
            return PartialView("_AMCServiceOverWritePartial", obj);
        }

        public string SaveOverWriteAMCService(AMCServiceOverWritePartialModel model)
        {
            if (model.AMCServiceId != 0)
            {
                tblAMCServices serviceObj = _dbRepositoryAMCService.SelectById(model.AMCServiceId);
                if (serviceObj != null)
                {
                    serviceObj.OverrideReason = model.OverrideReason;
                    serviceObj.ServiceOverrideDate = model.ServiceOverrideDate;
                    _dbRepositoryAMCService.Update(serviceObj);
                    return model.AMCServiceId.ToString();
                }
            }
            return String.Empty;
        }


        public ActionResult KendoDestroyService([DataSourceRequest] DataSourceRequest request, tblAMCServices model)
        {
            string deleteMessage = _dbRepositoryAMCService.Delete(model.AMCServiceId);

            ModelState.Clear();
            if (!string.IsNullOrEmpty(deleteMessage))
            {
                ModelState.AddModelError(deleteMessage, deleteMessage);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult KendoSaveService([DataSourceRequest] DataSourceRequest request, tblAMCServices model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return Json(new[] { model }.ToDataSourceResult(request, ModelState));
            }

            string message = string.Empty;

            model.IsCompleted = false;
            model.IsSystemGenerated = false;

            message = _dbRepositoryAMCService.Insert(model);

            ModelState.Clear();
            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError("ServviceDate", message);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }


        #endregion



    }
}