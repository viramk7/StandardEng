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
    public class ChargebleMasterNotesController : BaseController
    {
        #region private variables

        private readonly GenericRepository<tblChargebleNote> _dbRepository;

        #endregion
        #region Constructor
        public ChargebleMasterNotesController()
        {
            _dbRepository = new GenericRepository<tblChargebleNote>();
        }
        #endregion

        #region Public Methods
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult KendoRead([DataSourceRequest] DataSourceRequest request)
        {
            if (!request.Sorts.Any())
            {
                request.Sorts.Add(new SortDescriptor("NoteId", ListSortDirection.Ascending));
            }

            return Json(_dbRepository.GetEntities().ToDataSourceResult(request));
        }

        public ActionResult KendoSave([DataSourceRequest] DataSourceRequest request, tblChargebleNote model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return Json(new[] { model }.ToDataSourceResult(request, ModelState));
            }

            string message = string.Empty;

            message = model.NoteId > 0 ? _dbRepository.Update(model) : _dbRepository.Insert(model);

            ModelState.Clear();
            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError("NoteId", message);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult KendoDestroy([DataSourceRequest] DataSourceRequest request, tblChargebleNote model)
        {
            string deleteMessage = _dbRepository.Delete(model.NoteId);

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