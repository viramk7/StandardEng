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
    public class AMCQuoteDetailController : BaseController
    {
        #region private variables

        private readonly GenericRepository<tblAMCQDetail> _dbRepository;

        #endregion
        #region Constructor
        public AMCQuoteDetailController()
        {
            _dbRepository = new GenericRepository<tblAMCQDetail>();
        }
        #endregion

        #region Public Methods
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult KendoRead([DataSourceRequest] DataSourceRequest request, int AMCQId)
        {
            if (!request.Sorts.Any())
            {
                request.Sorts.Add(new SortDescriptor("AMCQId", ListSortDirection.Descending));
            }

            return Json(_dbRepository.GetEntities().Where(m => m.AMCQId == AMCQId).ToDataSourceResult(request));
        }

        public ActionResult KendoSave([DataSourceRequest] DataSourceRequest request, tblAMCQDetail model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return Json(new[] { model }.ToDataSourceResult(request, ModelState));
            }

            string message = (model.AMCQDetailId > 0) ? _dbRepository.Update(model) : _dbRepository.Insert(model);

            ModelState.Clear();
            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError("AMCQId", message);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult KendoDestroy([DataSourceRequest] DataSourceRequest request, tblAMCQDetail model)
        {
            string deleteMessage = _dbRepository.Delete(model.AMCQDetailId);

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