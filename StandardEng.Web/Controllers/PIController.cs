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
    public class PIController : Controller
    {
        #region private variables

        private readonly GenericRepository<tblPerformaInvoice> _dbRepository;
        #endregion

        #region Constructor
        public PIController()
        {
            _dbRepository = new GenericRepository<tblPerformaInvoice>();
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
            //    request.Sorts.Add(new SortDescriptor("PerformaInvoiceId", ListSortDirection.Ascending));
            //}

            //return Json(_dbRepository.GetEntities().ToDataSourceResult(request));
            return Json(CustomRepository.GetPerformaInvoieList().ToDataSourceResult(request));
        }

        //public ActionResult Create()
        //{
        //    ViewBag.MachineTypeList = SelectionList.MachineTypeList().Select(m => new { m.MachineTypeId, m.MachineTypeName });
        //    ViewBag.MachineModelList = SelectionList.MachineModelsList().Select(m => new { m.MachineModelId, m.MachineName });
        //    ViewBag.MachinePartList = SelectionList.MachinePartsList().Select(m => new { m.MachinePartId, m.ProductValue });
        //    ViewBag.GSTPercentageList = SelectionList.GSTPercentageList().Select(m => new { m.Id, Percentage = m.Percentage + " %" });
        //    return View(new tblPerformaInvoice { QuotationDate = DateTime.Now.Date, InquiryDate = DateTime.Now.Date });
        //}

        public ActionResult Edit(int id)
        {
            ViewBag.MachineTypeList = SelectionList.MachineTypeList().Select(m => new { m.MachineTypeId, m.MachineTypeName });
            ViewBag.MachineModelList = SelectionList.MachineModelsList().Select(m => new { m.MachineModelId, m.MachineName });
            ViewBag.MachinePartList = SelectionList.MachinePartsList().Select(m => new { m.MachinePartId, m.ProductValue });
            ViewBag.GSTPercentageList = SelectionList.GSTPercentageList().Select(m => new { m.Id, Percentage = m.Percentage + " %" });
            return View("Create", _dbRepository.SelectById(id));
        }

        public ActionResult SaveModelData(tblPerformaInvoice model, string create = null)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }

            string message = string.Empty;

            try
            {
                if (model.PerformaInvoiceId > 0)
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

            if (model.PerformaInvoiceId > 0)
            {
                if (create == "Save & Continue")
                {
                    return RedirectToAction("Edit", new { id = model.PerformaInvoiceId });
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