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
    public class MachinePartsQuotationController : BaseController
    {
        #region private variables

        private readonly GenericRepository<tblMachinePartsQuotation> _dbRepository;
        private readonly GenericRepository<tblMachinePartsQuotationDetail> _dbRepositoryDetail;
        private readonly GenericRepository<tblPerformaInvoice> _dbRepositoryPI;
        private readonly GenericRepository<tblPerformaInvoiceDetail> _dbRepositoryPIDetail;
        #endregion

        #region Constructor
        public MachinePartsQuotationController()
        {
            _dbRepository = new GenericRepository<tblMachinePartsQuotation>();
            _dbRepositoryDetail = new GenericRepository<tblMachinePartsQuotationDetail>();
            _dbRepositoryPI = new GenericRepository<tblPerformaInvoice>();
            _dbRepositoryPIDetail = new GenericRepository<tblPerformaInvoiceDetail>();
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
            //    request.Sorts.Add(new SortDescriptor("MachinePartsQuotationId", ListSortDirection.Ascending));
            //}

            return Json(CustomRepository.GetPartsQuoatationList().ToDataSourceResult(request));
        }

        public ActionResult Create()
        {
            ViewBag.MachineTypeList = SelectionList.MachineTypeList().Select(m => new { m.MachineTypeId, m.MachineTypeName });
            ViewBag.MachineModelList = SelectionList.MachineModelsList().Select(m => new { m.MachineModelId, m.MachineName });
            ViewBag.MachinePartList = SelectionList.MachinePartsList().Select(m => new { m.MachinePartId, m.ProductValue });
            ViewBag.GSTPercentageList = SelectionList.GSTPercentageList().Select(m => new { m.Id, Percentage = m.Percentage + " %" });
            return View(new tblMachinePartsQuotation { QuotationDate = DateTime.Now.Date , InquiryDate = DateTime.Now.Date , IsPIGenerated = false });
        }

        public ActionResult Edit(int id)
        {
            ViewBag.MachineTypeList = SelectionList.MachineTypeList().Select(m => new { m.MachineTypeId, m.MachineTypeName });
            ViewBag.MachineModelList = SelectionList.MachineModelsList().Select(m => new { m.MachineModelId, m.MachineName });
            ViewBag.MachinePartList = SelectionList.MachinePartsList().Select(m => new { m.MachinePartId, m.ProductValue });
            ViewBag.GSTPercentageList = SelectionList.GSTPercentageList().Select(m => new { m.Id, Percentage = m.Percentage + " %" });
            return View("Create", _dbRepository.SelectById(id));
        }

        public ActionResult SaveModelData(tblMachinePartsQuotation model, string create = null)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }

            string message = string.Empty;

            try
            {
                if (model.QuotationAmount.HasValue)
                {
                    model.QuotationAmount = Math.Round(model.QuotationAmount.Value);
                    model.QuotationAmountInWords = CurrencyHelper.changeCurrencyToWords(model.QuotationAmount.Value);
                }
                
                if (model.MachinePartsQuotationId > 0)
                {
                    model.TotalFreightAmount = model.TotalFreightAmount.HasValue ? Math.Round(model.TotalFreightAmount.Value) : model.TotalFreightAmount;
                    model.TotalFinalAmount = model.TotalFinalAmount.HasValue ? Math.Round(model.TotalFinalAmount.Value) : model.TotalFinalAmount;
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

            if (model.MachinePartsQuotationId > 0)
            {
                if (create == "Save & Continue")
                {
                    return RedirectToAction("Edit", new { id = model.MachinePartsQuotationId });
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

        public ActionResult PartsQuotationReport(int PartsQuotationId, string Reportname)
        {
            ViewBag.PartsQuotationId = PartsQuotationId;
            ViewBag.Reportname = Reportname;
            return PartialView("_PartsQuotationReport");
        }

        public ActionResult GeneratePIFromQuotation(int QuotationId , string SelectedIds = null)
        {
            if (QuotationId > 0)
            {
                tblMachinePartsQuotation quotationObj = _dbRepository.SelectById(QuotationId);
                if(quotationObj != null)
                {
                    List<int> selectedIDs = new List<int>();
                    List<tblMachinePartsQuotationDetail> detailObj = new List<tblMachinePartsQuotationDetail>();
                    if (!string.IsNullOrEmpty(SelectedIds))
                    {
                        selectedIDs = SelectedIds.Split(',').Select(int.Parse).ToList();
                    }
                    else
                    {
                        selectedIDs = _dbRepositoryDetail.GetEntities().Where(m => m.MachinePartsQuotationId == QuotationId).Select(m => m.MPQDetailId).ToList();
                    }
                    
                    if(selectedIDs.Count > 0)
                    {

                    
                    foreach(int id in selectedIDs)
                    {
                        tblMachinePartsQuotationDetail obj = _dbRepositoryDetail.SelectById(id);
                        detailObj.Add(obj);
                    }
                    if(detailObj.Count > 0)
                    {
                        tblPerformaInvoice invoiceObj = new tblPerformaInvoice();
                        invoiceObj.MPQuotationId = quotationObj.MachinePartsQuotationId;
                        invoiceObj.QuotationNo = quotationObj.QuotationNo;
                        invoiceObj.QuotationDate = quotationObj.QuotationDate;
                        invoiceObj.CustomerId = quotationObj.CustomerId;
                        invoiceObj.CustomerContactPId = quotationObj.CustomerContactPId;
                        invoiceObj.CustomerContactPContactNo = quotationObj.CustomerContactPContactNo;
                        invoiceObj.ReportServiceNo = quotationObj.ReportServiceNo;
                        invoiceObj.InquiryNo = quotationObj.InquiryNo;
                        invoiceObj.InquiryDate = quotationObj.InquiryDate;
                        invoiceObj.PaymentTerms = quotationObj.PaymentTerms;
                        invoiceObj.DeliveryWeeks = quotationObj.DeliveryWeeks;
                        invoiceObj.Insurance = quotationObj.Insurance;
                        invoiceObj.ValidityDays = quotationObj.ValidityDays;
                        invoiceObj.Email = quotationObj.Email;
                        invoiceObj.TotalFinalAmount = quotationObj.TotalFinalAmount;
                        invoiceObj.FreightAmount = quotationObj.FreightAmount;
                        invoiceObj.QuotationAmount = quotationObj.QuotationAmount;
                        invoiceObj.TotalFreightAmount = quotationObj.TotalFreightAmount;
                        invoiceObj.ServiceEngineerId = quotationObj.ServiceEngineerId;
                        invoiceObj.FreightPercentage = quotationObj.FreightPercentage;
                        invoiceObj.Remarks = quotationObj.Remarks;
                        invoiceObj.SequenceNo = quotationObj.SequenceNo;
                        invoiceObj.CreatedBy = SessionHelper.UserId;
                        invoiceObj.CreatedDate = DateTime.Now;
                        string result = _dbRepositoryPI.Insert(invoiceObj);
                        if (string.IsNullOrEmpty(result))
                        {
                            foreach(tblMachinePartsQuotationDetail obj in detailObj)
                            {
                                tblPerformaInvoiceDetail piDetailObj = new tblPerformaInvoiceDetail();
                                piDetailObj.PerformaInvoiceId = invoiceObj.PerformaInvoiceId;
                                piDetailObj.MPQDetailId = obj.MPQDetailId;
                                piDetailObj.MachineTypeId = obj.MachineTypeId;
                                piDetailObj.MachineModelId = obj.MachineModelId;
                                piDetailObj.MachineModelSerialNo = obj.MachineModelSerialNo;
                                piDetailObj.MachinePartsId = obj.MachinePartsId;
                                piDetailObj.MachinePartsNo = obj.MachinePartsNo;
                                piDetailObj.MachinePartDescription = obj.MachinePartDescription;
                                piDetailObj.PartsHSNCode = obj.PartsHSNCode;
                                piDetailObj.PartsQuantity = obj.PartsQuantity;
                                piDetailObj.UnitPrice = obj.UnitPrice;
                                piDetailObj.TotalPrice = obj.TotalPrice;
                                piDetailObj.PAndFPercentage = obj.PAndFPercentage;
                                piDetailObj.ProfitMarginPercentage = obj.ProfitMarginPercentage;
                                piDetailObj.DiscountPercentage = obj.DiscountPercentage;
                                piDetailObj.TaxablePrice = obj.TaxablePrice;
                                piDetailObj.GSTPercentage = obj.GSTPercentage;
                                piDetailObj.GSTAmount = obj.GSTAmount;
                                piDetailObj.FinalAmount = obj.FinalAmount;
                                piDetailObj.CreatedBy = SessionHelper.UserId;
                                piDetailObj.CreatedDate = DateTime.Now;

                                _dbRepositoryPIDetail.Insert(piDetailObj);

                            }

                            quotationObj.IsPIGenerated = true;
                            _dbRepository.Update(quotationObj);

                            return RedirectToAction("Edit", "PI", new { id = invoiceObj.PerformaInvoiceId });
                        }
                    }
                    }
                    else
                    {
                        TempData["Error"] = "There are no products to quoatation details. So please add products first.";
                        return View("Index");
                    }
                }
            }
            TempData["Error"] = "Something Went Wrong. Please try again later!"; 
            return View("Index");
        }
        #endregion
    }
}