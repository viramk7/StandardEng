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
    public class AMCQuotationController : BaseController
    {
        #region private variables
        private readonly GenericRepository<tblAMCQuotation> _dbRepository;
        private readonly GenericRepository<tblAMCQDetail> _dbRepositoryQDetail;
        private readonly GenericRepository<tblAMCQNote> _dbRepositoryQNote;
        private readonly GenericRepository<tblNote> _dBNote;
        #endregion

        #region Constructor
        public AMCQuotationController()
        {
            _dbRepository = new GenericRepository<tblAMCQuotation>();
            _dbRepositoryQDetail = new GenericRepository<tblAMCQDetail>();
            _dbRepositoryQNote = new GenericRepository<tblAMCQNote>();
            _dBNote = new GenericRepository<tblNote>();
        }
        #endregion

        #region Public Methods
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.CustomerList = SelectionList.CustomerList().Select(m => new { m.CustomerId, m.CustomerName });
            ViewBag.ContactPersonsList = SelectionList.ContactPersonsList().Select(m => new { m.ContactPersonId, m.ContactPersonName });
            return View();
        }

        public ActionResult KendoRead([DataSourceRequest] DataSourceRequest request)
        {
            if (!request.Sorts.Any())
            {
                request.Sorts.Add(new SortDescriptor("AMCQId", ListSortDirection.Descending));
            }

            return Json(_dbRepository.GetEntities().ToDataSourceResult(request));
        }

        //public ActionResult KendoReadQuotationHistory([DataSourceRequest] DataSourceRequest request , int Id , int CustomerId , int MachineModelId)
        //{
        //    if (!request.Sorts.Any())
        //    {
        //        request.Sorts.Add(new SortDescriptor("Id", ListSortDirection.Ascending));
        //    }

        //    return Json(_dbRepository.GetEntities().Where(m=>m.CustomerId == CustomerId && m.MachineModelId == MachineModelId && m.Id != Id).ToDataSourceResult(request));
        //}

        public ActionResult Create()
        {
            ViewBag.MachineTypeList = SelectionList.MachineTypeList().Select(m => new { m.MachineTypeId, m.MachineTypeName });
            ViewBag.MachineModelList = SelectionList.MachineModelsList().Select(m => new { m.MachineModelId, m.MachineName });
            return View(new tblAMCQuotation { QuotationDate = DateTime.Now.Date });
        }

        public ActionResult Edit(int id)
        {
            ViewBag.MachineTypeList = SelectionList.MachineTypeList().Select(m => new { m.MachineTypeId, m.MachineTypeName });
            ViewBag.MachineModelList = SelectionList.MachineModelsList().Select(m => new { m.MachineModelId, m.MachineName });
            return View("Create", _dbRepository.SelectById(id));
        }

        public ActionResult SaveModelData(tblAMCQuotation model,string create = null)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.MachineTypeList = SelectionList.MachineTypeList().Select(m => new { m.MachineTypeId, m.MachineTypeName });
                ViewBag.MachineModelList = SelectionList.MachineModelsList().Select(m => new { m.MachineModelId, m.MachineName });
                return View("Create", model);
            }

            string message = string.Empty;

            try
            {
                
                if (model.AMCQId > 0)
                {
                    if(model.FinalAmount.HasValue)
                    {
                        model.FinalAmountInWords = CurrencyHelper.changeCurrencyToWords(model.FinalAmount.Value);
                    }
                    model.ModifiedBy = SessionHelper.UserId;
                    model.ModifiedDate = DateTime.Now;
                    message = _dbRepository.Update(model);
                }
                else
                {
                    model.CreatedBy = SessionHelper.UserId;
                    model.CreatedDate = DateTime.Now;
                    message = _dbRepository.Insert(model);
                    if (string.IsNullOrEmpty(message))
                    {
                        bool result = InsertAllNotes(model.AMCQId);
                    }
                }

                if (model.AMCQId > 0)
                {
                    if (create == "Save & Continue")
                    {
                        return RedirectToAction("Edit", new { id = model.AMCQId });
                    }
                }
            }
            catch (Exception ex)
            {
                message = CommonHelper.GetErrorMessage(ex);
            }

            return RedirectToAction("Index");
        }

        public ActionResult KendoDestroy([DataSourceRequest] DataSourceRequest request, tblAMCQuotation model)
        {
            string deleteMessage = _dbRepository.Delete(model.AMCQId);

            ModelState.Clear();
            if (!string.IsNullOrEmpty(deleteMessage))
            {
                ModelState.AddModelError(deleteMessage, deleteMessage);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        //public ActionResult GenerateAMCPartial(int AMCQuotationId)
        //{
        //    AMCPartialModel obj = new AMCPartialModel();
        //    obj.AMCQuotationId = AMCQuotationId;
        //    return PartialView("_AMCPartial", obj);
        //}

        //public string GenerateAMC(AMCPartialModel model)
        //{
        //    if(model.AMCQuotationId != 0)
        //    {
        //        tblAMCQuotation quotation = _dbRepository.SelectById(model.AMCQuotationId);
        //        if(quotation != null)
        //        {
        //            tblAMC amcObj = new tblAMC();
        //            amcObj.AMCQuotationId = quotation.Id;
        //            amcObj.CustomerId = quotation.CustomerId;
        //            amcObj.MachineModelId = quotation.MachineModelId;
        //            amcObj.MachineTypeId = quotation.MachineTypeId;
        //            amcObj.MachineSerialNo = quotation.MachineSerialNo;
        //            amcObj.AMCQuotationNo = quotation.AMCQuotationNo;
        //            amcObj.ActualAmount = quotation.ActualAmount;
        //            amcObj.GSTPercentage = quotation.GSTPercentage;
        //            amcObj.GSTAmount = quotation.GSTAmount;
        //            amcObj.TotalAmount = quotation.TotalAmount;
        //            amcObj.Remarks = model.Remarks;
        //            amcObj.AMCStartDate = DateTime.Now.Date;
        //            amcObj.AMCEndDate = DateTime.Now.Date.AddYears(1);
        //            amcObj.IsAMCExpired = false;
        //            amcObj.CreatedBy = SessionHelper.UserId;
        //            amcObj.CreatedDate = DateTime.Now.Date;
        //            string result = _dbRepositoryAMC.Insert(amcObj);

        //            if (string.IsNullOrEmpty(result))
        //            {
        //                quotation.IsApproved = true;
        //                //quotation.IsConvertedIntoAMC = true;
        //                _dbRepository.Update(quotation);

        //                List<tblAMCQuotation> quotationList = new List<tblAMCQuotation>();
        //                if(quotation.CommissioningId != 0)
        //                {
        //                    quotationList = _dbRepository.GetEntities().Where(m => m.CommissioningId == quotation.CommissioningId).ToList();
        //                    foreach (tblAMCQuotation obj in quotationList)
        //                    {
        //                        obj.IsConvertedIntoAMC = true;
        //                        _dbRepository.Update(obj);
        //                    }
        //                }
        //                else
        //                {
        //                    quotationList = _dbRepository.GetEntities().Where(m => m.CustomerId == quotation.CustomerId && m.MachineTypeId == quotation.MachineTypeId
        //                                                                             && m.MachineModelId == quotation.MachineModelId
        //                                                                             && m.MachineSerialNo == quotation.MachineSerialNo).ToList();
        //                    foreach (tblAMCQuotation obj in quotationList)
        //                    {
        //                        obj.IsConvertedIntoAMC = true;
        //                        _dbRepository.Update(obj);
        //                    }
        //                }

        //                if(quotation.CommissioningId != null && quotation.CommissioningId != 0)
        //                {
        //                    tblCommissioning commsioningObj = _dbRepositoryCommissioning.SelectById(quotation.CommissioningId);
        //                    commsioningObj.IsConvertedToAMC = true;
        //                    _dbRepositoryCommissioning.Update(commsioningObj);
        //                }


        //                tblAMCServices amcServie1 = new tblAMCServices();
        //                amcServie1.AMCId = amcObj.AMCId;
        //                amcServie1.ServiceDate = DateTime.Now.Date.AddMonths(3);
        //                amcServie1.IsServiceDone = false;
        //                amcServie1.CreatedBy = SessionHelper.UserId;
        //                amcServie1.CreatedDate = DateTime.Now.Date;
        //                _dbRepositoryAMCService.Insert(amcServie1);

        //                tblAMCServices amcServie2 = new tblAMCServices();
        //                amcServie2.AMCId = amcObj.AMCId;
        //                amcServie2.ServiceDate = DateTime.Now.Date.AddMonths(6);
        //                amcServie2.IsServiceDone = false;
        //                amcServie2.CreatedBy = SessionHelper.UserId;
        //                amcServie2.CreatedDate = DateTime.Now.Date;
        //                _dbRepositoryAMCService.Insert(amcServie2);

        //                tblAMCServices amcServie3 = new tblAMCServices();
        //                amcServie3.AMCId = amcObj.AMCId;
        //                amcServie3.ServiceDate = DateTime.Now.Date.AddMonths(9);
        //                amcServie3.IsServiceDone = false;
        //                amcServie3.CreatedBy = SessionHelper.UserId;
        //                amcServie3.CreatedDate = DateTime.Now.Date;
        //                _dbRepositoryAMCService.Insert(amcServie3);

        //                tblAMCServices amcServie4 = new tblAMCServices();
        //                amcServie4.AMCId = amcObj.AMCId;
        //                amcServie4.ServiceDate = DateTime.Now.Date.AddDays(1);
        //                amcServie4.IsServiceDone = false;
        //                amcServie4.CreatedBy = SessionHelper.UserId;
        //                amcServie4.CreatedDate = DateTime.Now.Date;
        //                _dbRepositoryAMCService.Insert(amcServie4);

        //                return amcObj.AMCId.ToString();
        //            }

        //        }
        //    }
        //    return String.Empty;
        //}

        //public ActionResult AMCQuotationReport(int AMCQuotationId , string Reportname)
        //{
        //    ViewBag.AMCQuotationId = AMCQuotationId;
        //    ViewBag.Reportname = Reportname;
        //    return PartialView("_AMCQuotationReport");
        //}
        #endregion

        #region AMC Quotation Notes 
        public ActionResult KendoReadAMCNotes([DataSourceRequest] DataSourceRequest request,int AMCQId)
        {
            if (!request.Sorts.Any())
            {
                request.Sorts.Add(new SortDescriptor("AMCQId", ListSortDirection.Ascending));
            }

            return Json(_dbRepositoryQNote.GetEntities().Where(m=>m.AMCQId == AMCQId).ToDataSourceResult(request));
        }

        public ActionResult KendoSaveAMCNotes([DataSourceRequest] DataSourceRequest request, tblAMCQNote model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return Json(new[] { model }.ToDataSourceResult(request, ModelState));
            }

            string message = string.Empty;

            message = model.AMCQNoteId > 0 ? _dbRepositoryQNote.Update(model) : _dbRepositoryQNote.Insert(model);

            ModelState.Clear();
            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError("NoteId", message);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult KendoDestroyAMCNotes([DataSourceRequest] DataSourceRequest request, tblAMCQNote model)
        {
            string deleteMessage = _dbRepositoryQNote.Delete(model.AMCQNoteId);

            ModelState.Clear();
            if (!string.IsNullOrEmpty(deleteMessage))
            {
                ModelState.AddModelError(deleteMessage, deleteMessage);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region Private Methods
        private bool InsertAllNotes(int AMCQId)
        {
            bool result = false;
            string message = string.Empty;
            List<tblNote> notelist = _dBNote.GetEntities().ToList();
            if(notelist.Count > 0)
            {
                foreach(tblNote obj in notelist)
                {
                    tblAMCQNote amcnoteobj = new tblAMCQNote();
                    amcnoteobj.NoteText = obj.NoteText;
                    amcnoteobj.AMCQId = AMCQId;
                    message = _dbRepositoryQNote.Insert(amcnoteobj);
                }
                if (string.IsNullOrEmpty(message))
                {
                    return true;
                }
            }
            return result;
        }
        #endregion
    }
}