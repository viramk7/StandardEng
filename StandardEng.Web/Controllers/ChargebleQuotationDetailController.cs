using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using StandardEng.Data.DB;
using StandardEng.Data.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StandardEng.Web.Controllers
{
    public class ChargebleQuotationDetailController : BaseController
    {
        #region private variables

        private readonly GenericRepository<tblChargebleQDetail> _dbRepository;

        #endregion
        #region Constructor
        public ChargebleQuotationDetailController()
        {
            _dbRepository = new GenericRepository<tblChargebleQDetail>();
        }
        #endregion

        #region Public Methods
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult KendoRead([DataSourceRequest] DataSourceRequest request, int ChargebleQId)
        {
            if (!request.Sorts.Any())
            {
                request.Sorts.Add(new SortDescriptor("ChargebleQId", ListSortDirection.Descending));
            }

            return Json(_dbRepository.GetEntities().Where(m => m.ChargebleQId == ChargebleQId).ToDataSourceResult(request));
        }

        public ActionResult KendoSave([DataSourceRequest] DataSourceRequest request, tblChargebleQDetail model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return Json(new[] { model }.ToDataSourceResult(request, ModelState));
            }

            string message = (model.ChargebleQDetailId > 0) ? _dbRepository.Update(model) : _dbRepository.Insert(model);

            ModelState.Clear();
            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError("ChargebleQId", message);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult KendoDestroy([DataSourceRequest] DataSourceRequest request, tblChargebleQDetail model)
        {
            string deleteMessage = _dbRepository.Delete(model.ChargebleQDetailId);

            ModelState.Clear();
            if (!string.IsNullOrEmpty(deleteMessage))
            {
                ModelState.AddModelError(deleteMessage, deleteMessage);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        #endregion
    }
}