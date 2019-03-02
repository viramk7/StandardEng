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
using System.IO;
using System.Data.Entity;
using StandardEng.Data.CustomModel;

namespace StandardEng.Web.Controllers
{
    public class CommissioningController : BaseController
    {
        #region private variables

        private readonly GenericRepository<tblCommissioning> _dbRepository;
        private readonly GenericRepository<tblPreCommissioning> _dbRepositoryPreC;
        private readonly GenericRepository<tblPreCommissioningDetail> _dbRepositoryPreCD;
        private readonly GenericRepository<tblWarrantyexpires> _dbRepositoryWarranty;
        private readonly GenericRepository<tblAMCQuotation> _dbRepositoryAMCQ;
        private readonly GenericRepository<tblCustomerContactPersons> _dbRepositoryContact;
        private readonly GenericRepository<tblMachinePartsQuotation> _dbRepositoryPartsQ;
        private readonly GenericRepository<tblPreCommissioningAccessories> _dbPreAccessory;
        private readonly GenericRepository<tblPreCommissioningMachine> _dbPreMachine;

        #endregion

        #region Constructor
        public CommissioningController()
        {
            _dbRepository = new GenericRepository<tblCommissioning>();
            
            _dbRepositoryPreC = new GenericRepository<tblPreCommissioning>();
            _dbRepositoryPreCD = new GenericRepository<tblPreCommissioningDetail>();
            _dbRepositoryWarranty = new GenericRepository<tblWarrantyexpires>();
            _dbRepositoryAMCQ = new GenericRepository<tblAMCQuotation>();
            _dbRepositoryContact = new GenericRepository<tblCustomerContactPersons>();
            _dbRepositoryPartsQ = new GenericRepository<tblMachinePartsQuotation>();
            _dbPreAccessory = new GenericRepository<tblPreCommissioningAccessories>();
            _dbPreMachine = new GenericRepository<tblPreCommissioningMachine>();
        }
        #endregion

        #region Public Methods

        #region Commisiong Metods
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.CustomerList = SelectionList.CustomerList().Select(m => new { m.CustomerId, m.CustomerName });
            ViewBag.ContactPersonsList = SelectionList.ContactPersonsList().Select(m => new { m.ContactPersonId, m.ContactPersonName });
            ViewBag.MachineTypeList = SelectionList.MachineTypeList().Select(m => new { m.MachineTypeId, m.MachineTypeName });
            ViewBag.MachineModelList = SelectionList.MachineModelsList().Select(m => new { m.MachineModelId, m.MachineName });
            return View();
        }

        public ActionResult KendoRead([DataSourceRequest] DataSourceRequest request)
        {
            if (!request.Sorts.Any())
            {
                request.Sorts.Add(new SortDescriptor("CommissioningId", ListSortDirection.Ascending));
            }

            return Json(_dbRepository.GetEntities().ToDataSourceResult(request));
        }

        public ActionResult Create()
        {
            return View(new tblCommissioning());
        }

        public ActionResult Edit(int id)
        {
            tblCommissioning obj = _dbRepository.SelectById(id);
            obj.IsWarrantyPeriodChange = false;
            return View("Create", obj);
        }

        public ActionResult SaveModelData(tblCommissioning model,  HttpPostedFileBase fileUnit,string create = null)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }

            string message = string.Empty;

            try
            {
                if (fileUnit != null)
                {
                    string fileExtension = Path.GetExtension(fileUnit.FileName);
                    string FileName = Guid.NewGuid().ToString() + fileExtension;
                    string physicalPath = Path.Combine(Server.MapPath("~/Uploads"), FileName);
                    fileUnit.SaveAs(physicalPath);
                    model.CommissioningFileName = FileName;
                }

                if (model.CommissioningId > 0)
                {
                    model.ModifiedBy = SessionHelper.UserId;
                    model.ModifiedDate = DateTime.Now;
                    message = _dbRepository.Update(model);

                    if (model.IsWarrantyPeriodChange)
                    {
                        tblWarrantyexpires warrantyObj = _dbRepositoryWarranty.GetEntities().Where(m => m.CommissioningId == model.CommissioningId).FirstOrDefault();
                        if(warrantyObj != null){
                            warrantyObj.WarrantyExpireDate = model.WarrantyExpireDate;
                            _dbRepositoryWarranty.Update(warrantyObj);
                        }
                    }
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

            if (model.CommissioningId > 0)
            {
                if (create == "Save & Continue")
                {
                    return RedirectToAction("Edit", new { id = model.CommissioningId });
                }
                else if (create == "Save & New")
                {
                    return RedirectToAction("Create");
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult KendoDestroy([DataSourceRequest] DataSourceRequest request, tblCommissioning model)
        {
            string deleteMessage = _dbRepository.Delete(model.CommissioningId);

            ModelState.Clear();
            if (!string.IsNullOrEmpty(deleteMessage))
            {
                ModelState.AddModelError(deleteMessage, deleteMessage);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public FileResult DownloadFile(string file)
        {
            string fileExtension = Path.GetExtension(file);
            string filePath = Path.Combine(Server.MapPath("~/Uploads"), file);
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            if (fileExtension == ".pdf")
            {
                var response = new FileContentResult(fileBytes, "application/octet-stream");
                response.FileDownloadName = "CommissioningUploadFile.pdf";
                return response;
            }
            else
            {
                var response = new FileContentResult(fileBytes, "image/jpeg");
                response.FileDownloadName = "CommissioningUploadFile.jpeg";
                return response;
            }
           
        }
        #endregion

        #region Finalize PreCommissiong
        public ActionResult FinalizePreCommission(int id)
        {
            try
            {
                tblCommissioning commissionobj = new tblCommissioning();
                //tblPreCommissioning precommiobj = _dbRepositoryPreC.GetEntities().Where(m => m.PreCommissioningId == PreCommisionId).Include(m => m.tblMachineModels).FirstOrDefault();
                tblPreCommissioningDetail precommisdetailobj = _dbRepositoryPreCD.GetEntities().FirstOrDefault(m => m.PCDetailId == id);
                tblPreCommissioning precommiobj = _dbRepositoryPreC.GetEntities().FirstOrDefault(m => m.PreCommissioningId == precommisdetailobj.PreCommissionId);

                if(precommisdetailobj.PCMachineId > 0)
                {
                    tblPreCommissioningMachine precommimachine = _dbPreMachine.GetEntities().Include(m => m.tblMachineModels)
                                                .FirstOrDefault(m => m.PCMachined == precommisdetailobj.PCMachineId);

                    commissionobj.MachineTypeId = precommimachine.MachineTypeId;
                    commissionobj.MachineModelId = precommimachine.MachineModeld;
                    commissionobj.MachineSerialNo = precommimachine.MachineSerialNo;
                    commissionobj.WarrantyPeriod = precommimachine.tblMachineModels.WarrantyPeriod;
                    commissionobj.WarrantyExpireDate = precommisdetailobj.PreCommisoningDate.AddMonths(precommimachine.tblMachineModels.WarrantyPeriod);
                }
                if(precommisdetailobj.PCAccesseriesId > 0)
                {
                    tblPreCommissioningAccessories precommiasseccory = _dbPreAccessory.GetEntities().Include(m => m.tblMachineAccessories)
                                                .FirstOrDefault(m => m.PCAccessoriesId == precommisdetailobj.PCAccesseriesId);

                    commissionobj.AccessoriesTypeId = precommiasseccory.AccessoriesTypeId;
                    commissionobj.MachineAccessoriesId = precommiasseccory.MachineAccessoriesId;
                    commissionobj.AccessoriesSerialNo = precommiasseccory.AccessoriesSerialNo;
                    commissionobj.WarrantyPeriod = 6;
                    commissionobj.WarrantyExpireDate = precommisdetailobj.PreCommisoningDate.AddMonths(6);
                }

                tblCustomerContactPersons contactPerson = _dbRepositoryContact.GetEntities().FirstOrDefault(m => m.ContactPersonId == precommiobj.ContactPersonId);

                commissionobj.CustomerId = precommiobj.CustomerId;
                commissionobj.ContactPersonId = precommiobj.ContactPersonId;
                commissionobj.ContactPersonContactNo = contactPerson.ContactNo;
                commissionobj.PreCommissioningDetailId = precommisdetailobj.PCDetailId;
                commissionobj.ServiceEngineerId = precommisdetailobj.ServiceEngineerId;
                commissionobj.CommissioningDate = precommisdetailobj.PreCommisoningDate;
                commissionobj.IsConvertedToAMC = false;
                commissionobj.IsReadyForAMC = false;
                commissionobj.CreatedBy = SessionHelper.UserId;
                commissionobj.CreatedDate = DateTime.Now;

                string result = _dbRepository.Insert(commissionobj);

                tblWarrantyexpires warrantyObj = new tblWarrantyexpires();
                warrantyObj.CommissioningId = commissionobj.CommissioningId;
                warrantyObj.CommissioningDate = precommisdetailobj.PreCommisoningDate;
                warrantyObj.WarrantyExpireDate = commissionobj.WarrantyExpireDate;
                warrantyObj.IsConvertIntoAMC = false;
                warrantyObj.IsConvertIntoChargable = false;
                warrantyObj.CreatedBy = SessionHelper.UserId;
                warrantyObj.CreatedDate = DateTime.Now;
                string resultWarranty = _dbRepositoryWarranty.Insert(warrantyObj);

                precommisdetailobj.IsCommisioning = true;
                _dbRepositoryPreCD.Update(precommisdetailobj);

                //precommiobj.IsCommissioningDone = true;
                //_dbRepositoryPreC.Update(precommiobj);

                return RedirectToAction("Edit", "Commissioning", new { id = commissionobj.CommissioningId });
            }
            catch (Exception ex)
            {
                string Message = CommonHelper.GetDeleteException(ex);
                return RedirectToAction("Index", "Commissioning");
            }
        }
        #endregion

        #region AMC Quotation
        public ActionResult ConvertToAMCQuotationPartial(int CommissioningId)
        {
            AMCQuotationPartialModel obj = new AMCQuotationPartialModel();
            obj.CommissioningId = CommissioningId;
            return PartialView("_AMCQuotationPartial", obj);
        }

        public ActionResult GenerateAMCQuotation(AMCQuotationPartialModel model)
        {
            if(model.CommissioningId != 0)
            {
                bool isExists = _dbRepositoryAMCQ.GetEntities().Any(m => m.CommissioningId == model.CommissioningId
                                                                  && m.ActualAmount == model.QuotationAmount
                                                                  && m.GSTPercentage == model.GSTPercentage);

                if (isExists)
                {
                    return Json( new { id=String.Empty , error = "Quotation with same amount & GST percentage already exists." },JsonRequestBehavior.AllowGet);
                }
                tblCommissioning commissioningObj = _dbRepository.SelectById(model.CommissioningId);
                tblPreCommissioningDetail preCommissioningDetail = _dbRepositoryPreCD.GetEntities()
                                                .Where(m => m.PCDetailId == commissioningObj.PreCommissioningDetailId)
                                                .Include(m => m.tblPreCommissioning).FirstOrDefault();

                tblAMCQuotation quotatationObj = new tblAMCQuotation();
                quotatationObj.CommissioningId = commissioningObj.CommissioningId;
                quotatationObj.MachineModelId = commissioningObj.MachineModelId.Value;
                quotatationObj.MachineTypeId = commissioningObj.MachineTypeId.Value;
                quotatationObj.MachineSerialNo = commissioningObj.MachineSerialNo;
                quotatationObj.CustomerId = preCommissioningDetail.tblPreCommissioning.CustomerId;
                quotatationObj.QuotationDate = DateTime.Now.Date;
                quotatationObj.ActualAmount = model.QuotationAmount;
                quotatationObj.GSTPercentage = model.GSTPercentage;
                quotatationObj.GSTAmount = model.GSTAmount;
                quotatationObj.CGSTPercentage = model.GSTPercentage / 2;
                quotatationObj.CGSTAmount = model.GSTAmount / 2;
                quotatationObj.TotalAmount = model.TotalAmount;
                quotatationObj.TotalAmountInWords = CurrencyHelper.changeCurrencyToWords(model.TotalAmount);
                quotatationObj.IsConvertedIntoAMC = false;
                quotatationObj.CreatedBy = SessionHelper.UserId;
                quotatationObj.CreatedDate = DateTime.Now.Date;
                string result = _dbRepositoryAMCQ.Insert(quotatationObj);
                if (string.IsNullOrEmpty(result))
                {
                    return Json(new { id = quotatationObj.Id.ToString() , error = String.Empty }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { id = String.Empty, error = "Something Went Wrong." }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Parts Quotation
        public ActionResult GeneratePartsQuotation(int CommisionId)
        {
            tblCommissioning commissioningObj = _dbRepository.SelectById(CommisionId);
            if(commissioningObj != null)
            {
                tblCustomerContactPersons contactPerson = _dbRepositoryContact.SelectById(commissioningObj.ContactPersonId);
                tblMachinePartsQuotation quotationObj = new tblMachinePartsQuotation();
                quotationObj.CustomerId = commissioningObj.CustomerId;
                quotationObj.CustomerContactPId = commissioningObj.ContactPersonId;
                quotationObj.CustomerContactPContactNo = contactPerson.ContactNo;
                quotationObj.Email = contactPerson.ContactPersonEmail;
                quotationObj.QuotationDate = DateTime.Now.Date;
                quotationObj.ReportServiceNo = commissioningObj.ReportServiceNo;
                quotationObj.InquiryNo = String.Empty;
                quotationObj.InquiryDate = DateTime.Now.Date;
                quotationObj.PaymentDays = 0;
                quotationObj.DeliveryWeeks = 0;
                quotationObj.Freight = String.Empty;
                quotationObj.Insurance = String.Empty;
                quotationObj.ValidityDays = 0;
                quotationObj.CreatedBy = SessionHelper.UserId;
                quotationObj.CreatedDate = DateTime.Now.Date;
                string result = _dbRepositoryPartsQ.Insert(quotationObj);
                if (string.IsNullOrEmpty(result))
                {
                    return RedirectToAction("Edit", "MachinePartsQuotation", new { id = quotationObj.MachinePartsQuotationId });
                }
            }
            return RedirectToAction("Edit", "Commissioning", new { id = CommisionId });
        }
        #endregion
        #endregion
    }
}