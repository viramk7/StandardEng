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
    public class MachinePartsQuotationDetailController : BaseController
    {
        #region private variables

        private readonly GenericRepository<tblMachinePartsQuotationDetail> _dbRepository;

        #endregion
        #region Constructor
        public MachinePartsQuotationDetailController()
        {
            _dbRepository = new GenericRepository<tblMachinePartsQuotationDetail>();
        }
        #endregion

        #region Public Methods
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult KendoRead([DataSourceRequest] DataSourceRequest request, int MachinePartsQuotationId)
        {
            if (!request.Sorts.Any())
            {
                request.Sorts.Add(new SortDescriptor("MPQDetailId", ListSortDirection.Ascending));
            }

            return Json(_dbRepository.GetEntities().Where(m => m.MachinePartsQuotationId == MachinePartsQuotationId).ToDataSourceResult(request));
        }

        public ActionResult KendoSave([DataSourceRequest] DataSourceRequest request, tblMachinePartsQuotationDetail model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return Json(new[] { model }.ToDataSourceResult(request, ModelState));
            }

            string message = string.Empty;

            if (model.MPQDetailId > 0)
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
                ModelState.AddModelError("MPQDetailId", message);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult KendoDestroy([DataSourceRequest] DataSourceRequest request, tblMachinePartsQuotationDetail model)
        {
            string deleteMessage = _dbRepository.Delete(model.MPQDetailId);

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