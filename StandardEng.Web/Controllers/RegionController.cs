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
    public class RegionController : BaseController
    {
        #region private variables

        private readonly GenericRepository<tblRegion> _dbRepository;

        #endregion

        #region Constructor
        public RegionController()
        {
            _dbRepository = new GenericRepository<tblRegion>();
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
                request.Sorts.Add(new SortDescriptor("Name", ListSortDirection.Ascending));
            }

            return Json(_dbRepository.GetEntities().ToDataSourceResult(request));
        }

        public ActionResult KendoSave([DataSourceRequest] DataSourceRequest request, tblRegion model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return Json(new[] { model }.ToDataSourceResult(request, ModelState));
            }

            string message = string.Empty;

            message = model.Id > 0 ? _dbRepository.Update(model) : _dbRepository.Insert(model);

            ModelState.Clear();
            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError("CountryName", message);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult KendoDestroy([DataSourceRequest] DataSourceRequest request, tblRegion model)
        {
            string deleteMessage = _dbRepository.Delete(model.Id);

            ModelState.Clear();
            if (!string.IsNullOrEmpty(deleteMessage))
            {
                ModelState.AddModelError(deleteMessage, deleteMessage);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public string ChangeStatus(long id)
        {
            tblRegion user = _dbRepository.SelectById(id);
            user.IsActive = !user.IsActive;
            return _dbRepository.Update(user);
        }

        #endregion
    }
}