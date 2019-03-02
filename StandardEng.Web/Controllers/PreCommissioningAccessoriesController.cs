using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
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
    public class PreCommissioningAccessoriesController : BaseController
    {
        #region private variables

        private readonly GenericRepository<tblPreCommissioningAccessories> _dbRepository;

        #endregion
        #region Constructor
        public PreCommissioningAccessoriesController()
        {
            _dbRepository = new GenericRepository<tblPreCommissioningAccessories>();
        }
        #endregion

        #region Public Methods
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult KendoRead([DataSourceRequest] DataSourceRequest request, int PreCommissioningId)
        {
            if (!request.Sorts.Any())
            {
                request.Sorts.Add(new SortDescriptor("PCAccessoriesId", ListSortDirection.Ascending));
            }

            return Json(_dbRepository.GetEntities().Where(m => m.PreCommissioningId == PreCommissioningId).ToDataSourceResult(request));
        }

        public ActionResult KendoSave([DataSourceRequest] DataSourceRequest request, tblPreCommissioningAccessories model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return Json(new[] { model }.ToDataSourceResult(request, ModelState));
            }

            string message = string.Empty;

            if (model.PCAccessoriesId > 0)
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

            ModelState.Clear();
            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError("AccessoriesSerialNo", message);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult KendoDestroy([DataSourceRequest] DataSourceRequest request, tblPreCommissioningAccessories model)
        {
            string deleteMessage = _dbRepository.Delete(model.PCAccessoriesId);

            ModelState.Clear();
            if (!string.IsNullOrEmpty(deleteMessage))
            {
                ModelState.AddModelError(deleteMessage, deleteMessage);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult GetPCAccessoryList(int? PreCommissioningId = 0)
        {
            var result = _dbRepository.GetEntities().Where(m => m.PreCommissioningId == PreCommissioningId)
                                .Select(m => new { m.PCAccessoriesId, m.AccessoriesSerialNo })
                                .OrderBy(m => new { m.AccessoriesSerialNo }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}