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
        private readonly GenericRepository<tblAMC> _dBAMC;
        private readonly GenericRepository<tblAMCServices> _dBAMCServices;
        private readonly GenericRepository<tblGSTMaster> _dBGST;
        #endregion

        #region Constructor
        public AMCQuotationController()
        {
            _dbRepository = new GenericRepository<tblAMCQuotation>();
            _dbRepositoryQDetail = new GenericRepository<tblAMCQDetail>();
            _dbRepositoryQNote = new GenericRepository<tblAMCQNote>();
            _dBNote = new GenericRepository<tblNote>();
            _dBAMC = new GenericRepository<tblAMC>();
            _dBAMCServices = new GenericRepository<tblAMCServices>();
            _dBGST = new GenericRepository<tblGSTMaster>();
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
            return View(new tblAMCQuotation { QuotationDate = DateTime.Now.Date , AMCBy = SessionHelper.UserId});
        }

        public ActionResult Edit(int id)
        {
            ViewBag.MachineTypeList = SelectionList.MachineTypeList().Select(m => new { m.MachineTypeId, m.MachineTypeName });
            ViewBag.MachineModelList = SelectionList.MachineModelsList().Select(m => new { m.MachineModelId, m.MachineName });
            return View("Create", _dbRepository.SelectById(id));
        }

        public ActionResult SaveModelData(tblAMCQuotation model, string create = null)
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
                    if (model.FinalAmount.HasValue)
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

        public ActionResult GenerateAMC(int QuotationId, string SelectedIds = null)
        {
            try
            {
                if (QuotationId > 0)
                {
                    tblAMCQuotation quotationObj = _dbRepository.SelectById(QuotationId);

                    if (quotationObj != null)
                    {
                        List<int> selectedIDs = new List<int>();
                        List<tblAMCQDetail> detailObj = new List<tblAMCQDetail>();
                        if (!string.IsNullOrEmpty(SelectedIds))
                        {
                            selectedIDs = SelectedIds.Split(',').Select(int.Parse).ToList();
                        }
                        else
                        {
                            selectedIDs = _dbRepositoryQDetail.GetEntities().Where(m => m.AMCQId == QuotationId).Select(m => m.AMCQDetailId).ToList();
                        }
                        if (selectedIDs.Count > 0)
                        {
                            foreach (int id in selectedIDs)
                            {
                                tblAMCQDetail obj = _dbRepositoryQDetail.SelectById(id);
                                detailObj.Add(obj);
                            }
                            if (detailObj.Count > 0)
                            {
                                string result = string.Empty;

                                foreach (tblAMCQDetail singleAMC in detailObj)
                                {
                                    tblAMC amcObj = new tblAMC();
                                    amcObj.AMCQuotationId = quotationObj.AMCQId;
                                    amcObj.AMCQDetailId = singleAMC.AMCQDetailId;
                                    amcObj.AMCDate = DateTime.Now.Date;
                                    //amcObj.AMCStartDate = DateTime.Now.Date;
                                    //amcObj.AMCEndDate = DateTime.Now.Date.AddYears(1);
                                    amcObj.QuotationNo = quotationObj.AMCQuotationNo;
                                    amcObj.CustomerId = quotationObj.CustomerId;
                                    amcObj.CustomerContactPId = quotationObj.CustomerContactPId;

                                    amcObj.CountryId = quotationObj.CountryId;
                                    amcObj.StateId = quotationObj.StateId;
                                    amcObj.CityId = quotationObj.CityId;
                                    amcObj.RegionId = quotationObj.RegionId;
                                    amcObj.Addressline1 = quotationObj.Addressline1;
                                    amcObj.Addressline2 = quotationObj.Addressline2;
                                    amcObj.Addressline3 = quotationObj.Addressline3;
                                    amcObj.PinCode = quotationObj.PinCode;

                                    amcObj.ContactNo = quotationObj.ContactNo;
                                    amcObj.EmailAddress = quotationObj.Email;
                                    amcObj.Remarks = quotationObj.Remarks;
                                    amcObj.MachineTypeId = singleAMC.MachineTypeId;
                                    amcObj.MachineModelId = singleAMC.MachineModelId;
                                    amcObj.MachineSerialNo = singleAMC.MachineSerialNo;
                                    amcObj.Description = singleAMC.Description;
                                    amcObj.Amount = singleAMC.Amount;
                                    amcObj.CreatedDate = DateTime.Now;
                                    amcObj.CreatedBy = SessionHelper.UserId;
                                    amcObj.IsDifferentShipAddress = quotationObj.IsDifferentShipAddress;
                                    amcObj.ShipCompanyName = quotationObj.ShipCompanyName;
                                    amcObj.ShipAddressline1 = quotationObj.ShipAddressline1;
                                    amcObj.ShipAddressline2 = quotationObj.ShipAddressline2;
                                    amcObj.ShipAddressline3 = quotationObj.ShipAddressline3;
                                    amcObj.ShipGSTNo = quotationObj.ShipGSTNo;

                                    decimal finalAmount = singleAMC.Amount;
                                    if (quotationObj.GSTPercentageId.HasValue)
                                    {
                                        amcObj.GSTPercentageId = quotationObj.GSTPercentageId.Value;
                                        decimal gstPercentage = _dBGST.SelectById(quotationObj.GSTPercentageId.Value).Percentage;

                                        decimal amount = (singleAMC.Amount * gstPercentage) / 100;

                                        amcObj.GSTAmount = amount;
                                        finalAmount = finalAmount + amount;
                                    }
                                    //if (quotationObj.GSTAmount.HasValue)
                                    //{
                                    //    finalAmount = quotationObj.GSTAmount.Value;
                                    //}
                                    amcObj.FinalAmount = finalAmount;
                                    amcObj.FinalAmountInWords = CurrencyHelper.changeCurrencyToWords(finalAmount);
                                    result = _dBAMC.Insert(amcObj);

                                    //if (string.IsNullOrEmpty(result))
                                    //{
                                    //    tblAMCServices amcServie1 = new tblAMCServices();
                                    //    amcServie1.AMCId = amcObj.AMCId;
                                    //    amcServie1.ServviceDate = DateTime.Now.Date.AddMonths(3);
                                    //    amcServie1.IsCompleted = false;
                                    //    amcServie1.IsSystemGenerated = true;
                                    //    result = _dBAMCServices.Insert(amcServie1);

                                    //    tblAMCServices amcServie2 = new tblAMCServices();
                                    //    amcServie2.AMCId = amcObj.AMCId;
                                    //    amcServie2.ServviceDate = DateTime.Now.Date.AddMonths(6);
                                    //    amcServie2.IsCompleted = false;
                                    //    amcServie2.IsSystemGenerated = true;
                                    //    result = _dBAMCServices.Insert(amcServie2);

                                    //    tblAMCServices amcServie3 = new tblAMCServices();
                                    //    amcServie3.AMCId = amcObj.AMCId;
                                    //    amcServie3.ServviceDate = DateTime.Now.Date.AddMonths(9);
                                    //    amcServie3.IsCompleted = false;
                                    //    amcServie3.IsSystemGenerated = true;
                                    //    result = _dBAMCServices.Insert(amcServie3);

                                    //    tblAMCServices amcServie4 = new tblAMCServices();
                                    //    amcServie4.AMCId = amcObj.AMCId;
                                    //    amcServie4.ServviceDate = DateTime.Now.Date.AddMonths(12);
                                    //    amcServie4.IsCompleted = false;
                                    //    amcServie4.IsSystemGenerated = true;
                                    //    result = _dBAMCServices.Insert(amcServie4);
                                    //}
                                }

                                if (string.IsNullOrEmpty(result))
                                {
                                    quotationObj.IsConvertedIntoAMC = true;
                                    _dbRepository.Update(quotationObj);
                                    return RedirectToAction("Index", "AMC");
                                }
                            }
                        }
                    }
                }
                return RedirectToAction("Edit", "AMCQuotation", new { id = QuotationId });
            }
            catch(Exception ex)
            {
                return RedirectToAction("Edit", "AMCQuotation", new { id = QuotationId });
            }
        }

        public ActionResult AMCQuotationReport(int AMCQuotationId, string Reportname)
        {
            ViewBag.AMCQuotationId = AMCQuotationId;
            ViewBag.Reportname = Reportname;
            return PartialView("_AMCQuotationReport");
        }
        #endregion

        #region AMC Quotation Notes 
        public ActionResult KendoReadAMCNotes([DataSourceRequest] DataSourceRequest request, int AMCQId)
        {
            if (!request.Sorts.Any())
            {
                request.Sorts.Add(new SortDescriptor("AMCQId", ListSortDirection.Ascending));
            }

            return Json(_dbRepositoryQNote.GetEntities().Where(m => m.AMCQId == AMCQId).ToDataSourceResult(request));
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
            if (notelist.Count > 0)
            {
                foreach (tblNote obj in notelist)
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