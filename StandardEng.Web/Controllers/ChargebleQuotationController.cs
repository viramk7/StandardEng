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

namespace StandardEng.Web.Controllers
{
    public class ChargebleQuotationController : BaseController
    {
        #region private variables
        private readonly GenericRepository<tblChargebleQuotation> _dbRepository;
        private readonly GenericRepository<tblChargebleQDetail> _dbRepositoryQDetail;
        private readonly GenericRepository<tblChargebleQNote> _dbRepositoryQNote;
        private readonly GenericRepository<tblChargeble> _dBChargeble;
        private readonly GenericRepository<tblGSTMaster> _dBGST;
        private readonly GenericRepository<tblChargebleNote> _dBNote;
        #endregion

        #region Constructor
        public ChargebleQuotationController()
        {
            _dbRepository = new GenericRepository<tblChargebleQuotation>();
            _dbRepositoryQDetail = new GenericRepository<tblChargebleQDetail>();
            _dbRepositoryQNote = new GenericRepository<tblChargebleQNote>();
            _dBChargeble = new GenericRepository<tblChargeble>();
            _dBGST = new GenericRepository<tblGSTMaster>();
            _dBNote = new GenericRepository<tblChargebleNote>();
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
                request.Sorts.Add(new SortDescriptor("ChargebleQId", ListSortDirection.Descending));
            }

            return Json(_dbRepository.GetEntities().ToDataSourceResult(request));
        }

        public ActionResult Create()
        {
            ViewBag.MachineTypeList = SelectionList.MachineTypeList().Select(m => new { m.MachineTypeId, m.MachineTypeName });
            ViewBag.MachineModelList = SelectionList.MachineModelsList().Select(m => new { m.MachineModelId, m.MachineName });
            return View(new tblChargebleQuotation { QuotationDate = DateTime.Now.Date, ChargebleBy = SessionHelper.UserId });
        }

        public ActionResult Edit(int id)
        {
            ViewBag.MachineTypeList = SelectionList.MachineTypeList().Select(m => new { m.MachineTypeId, m.MachineTypeName });
            ViewBag.MachineModelList = SelectionList.MachineModelsList().Select(m => new { m.MachineModelId, m.MachineName });
            return View("Create", _dbRepository.SelectById(id));
        }

        public ActionResult SaveModelData(tblChargebleQuotation model, string create = null)
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
                if (model.ChargebleQId > 0)
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
                        bool result = InsertAllNotes(model.ChargebleQId);
                    }
                }

                if (model.ChargebleQId > 0)
                {
                    if (create == "Save & Continue")
                    {
                        return RedirectToAction("Edit", new { id = model.ChargebleQId
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                message = CommonHelper.GetErrorMessage(ex);
            }

            return RedirectToAction("Index");
        }

        public ActionResult KendoDestroy([DataSourceRequest] DataSourceRequest request, tblChargebleQuotation model)
        {
            string deleteMessage = _dbRepository.Delete(model.ChargebleQId);

            ModelState.Clear();
            if (!string.IsNullOrEmpty(deleteMessage))
            {
                ModelState.AddModelError(deleteMessage, deleteMessage);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult GenerateChargeble(int QuotationId, string SelectedIds = null)
        {
            try
            {
                if (QuotationId > 0)
                {
                    tblChargebleQuotation quotationObj = _dbRepository.SelectById(QuotationId);

                    if (quotationObj != null)
                    {
                        List<int> selectedIDs = new List<int>();
                        List<tblChargebleQDetail> detailObj = new List<tblChargebleQDetail>();
                        if (!string.IsNullOrEmpty(SelectedIds))
                        {
                            selectedIDs = SelectedIds.Split(',').Select(int.Parse).ToList();
                        }
                        else
                        {
                            selectedIDs = _dbRepositoryQDetail.GetEntities().Where(m => m.ChargebleQId == QuotationId).Select(m => m.ChargebleQDetailId).ToList();
                        }
                        if (selectedIDs.Count > 0)
                        {
                            foreach (int id in selectedIDs)
                            {
                                tblChargebleQDetail obj = _dbRepositoryQDetail.SelectById(id);
                                detailObj.Add(obj);
                            }
                            if (detailObj.Count > 0)
                            {
                                string result = string.Empty;

                                foreach (tblChargebleQDetail singleAMC in detailObj)
                                {
                                    tblChargeble amcObj = new tblChargeble();
                                    amcObj.ChargebleQId = quotationObj.ChargebleQId;
                                    amcObj.ChargebleQDetailId = singleAMC.ChargebleQDetailId;
                                    amcObj.ChargebleDate = DateTime.Now.Date;
                                    amcObj.QuotationNo = quotationObj.QuotationNo;
                                    amcObj.CustomerId = quotationObj.CustomerId;
                                    amcObj.CustomerContactPId = quotationObj.CustomerContactPId;
                                    amcObj.ContactNo = quotationObj.ContactNo;
                                    amcObj.EmailAddress = quotationObj.Email;
                                    amcObj.Remarks = quotationObj.Remarks;
                                    amcObj.MachineTypeId = singleAMC.MachineTypeId;
                                    amcObj.MachineModelId = singleAMC.MachineModelId;
                                    amcObj.MachineSerialNo = singleAMC.MachineSerialNo;
                                    amcObj.MachineDescription = singleAMC.Description;
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
                                    amcObj.FinalAmount = finalAmount;
                                    amcObj.FinalAmountInWords = CurrencyHelper.changeCurrencyToWords(finalAmount); ;

                                    result = _dBChargeble.Insert(amcObj);
                                }

                                if (string.IsNullOrEmpty(result))
                                {
                                    quotationObj.IsConvertedIntoChargeble = true;
                                    _dbRepository.Update(quotationObj);
                                    return RedirectToAction("Index", "Chargeble");
                                }
                            }
                        }
                    }
                }
                return RedirectToAction("Edit", "ChargebleQuotation", new { id = QuotationId });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Edit", "ChargebleQuotation", new { id = QuotationId });
            }
        }

        public ActionResult ChargebleQuotationReport(int QuotationId, string Reportname)
        {
            ViewBag.QuotationId = QuotationId;
            ViewBag.Reportname = Reportname;
            return PartialView("_ChargebleQuotationReport");
        }

        #endregion

        #region Chargeble Quotation Notes 
        public ActionResult KendoReadChargebleNotes([DataSourceRequest] DataSourceRequest request, int ChargebleQId)
        {
            if (!request.Sorts.Any())
            {
                request.Sorts.Add(new SortDescriptor("ChargebleQId", ListSortDirection.Ascending));
            }

            return Json(_dbRepositoryQNote.GetEntities().Where(m => m.ChargebleQId == ChargebleQId).ToDataSourceResult(request));
        }

        public ActionResult KendoSaveNotes([DataSourceRequest] DataSourceRequest request, tblChargebleQNote model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return Json(new[] { model }.ToDataSourceResult(request, ModelState));
            }

            string message = string.Empty;

            message = model.ChargebleQNoteId > 0 ? _dbRepositoryQNote.Update(model) : _dbRepositoryQNote.Insert(model);

            ModelState.Clear();
            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError("ChargebleQNoteId", message);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult KendoDestroyNotes([DataSourceRequest] DataSourceRequest request, tblChargebleQNote model)
        {
            string deleteMessage = _dbRepositoryQNote.Delete(model.ChargebleQNoteId);

            ModelState.Clear();
            if (!string.IsNullOrEmpty(deleteMessage))
            {
                ModelState.AddModelError(deleteMessage, deleteMessage);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region Private Methods
        private bool InsertAllNotes(int ChargebleQId)
        {
            bool result = false;
            string message = string.Empty;
            List<tblChargebleNote> notelist = _dBNote.GetEntities().ToList();
            if (notelist.Count > 0)
            {
                foreach (tblChargebleNote obj in notelist)
                {
                    tblChargebleQNote amcnoteobj = new tblChargebleQNote();
                    amcnoteobj.NoteText = obj.NoteText;
                    amcnoteobj.ChargebleQId = ChargebleQId;
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