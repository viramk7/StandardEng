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
    public class ChargebleController : BaseController
    {
        #region private variables
        private readonly GenericRepository<tblChargeble> _dbRepository;
        private readonly GenericRepository<tblChargebleQuotation> _dbRepositoryQuotation;
        #endregion

        #region Constructor
        public ChargebleController()
        {
            _dbRepository = new GenericRepository<tblChargeble>();
            _dbRepositoryQuotation = new GenericRepository<tblChargebleQuotation>();
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
                request.Sorts.Add(new SortDescriptor("ChargebleId", ListSortDirection.Descending));
            }

            return Json(CustomRepository.GetChargebleList().ToDataSourceResult(request));
        }


        //public ActionResult Create()
        //{
        //    return View(new tblAMC());
        //}

        public ActionResult Edit(int id)
        {
            return View("Create", _dbRepository.SelectById(id));
        }

        public ActionResult SaveModelData(tblChargeble model)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }

            string message = string.Empty;

            try
            {
                if (model.ChargebleId > 0)
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

        public ActionResult KendoDestroy([DataSourceRequest] DataSourceRequest request, tblChargeble model)
        {

            //tblChargebleQuotation quotationObj = _dbRepositoryQuotation.SelectById(model.ChargebleQId);
            //quotationObj.IsConvertedIntoChargeble = false;
            //_dbRepositoryQuotation.Update(quotationObj);

            string deleteMessage = _dbRepository.Delete(model.ChargebleId);

            ModelState.Clear();
            if (!string.IsNullOrEmpty(deleteMessage))
            {
                ModelState.AddModelError(deleteMessage, deleteMessage);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult ChargebleReport(int ChargebleId, string Reportname)
        {
            ViewBag.ChargebleId = ChargebleId;
            ViewBag.Reportname = Reportname;
            return PartialView("_ChargebleReport");
        }
        #endregion
    }
}