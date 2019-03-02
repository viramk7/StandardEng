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
    public class PreCommissioningMachineController : BaseController
    {
        #region private variables

        private readonly GenericRepository<tblPreCommissioningMachine> _dbRepository;

        #endregion
        #region Constructor
        public PreCommissioningMachineController()
        {
            _dbRepository = new GenericRepository<tblPreCommissioningMachine>();
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
                request.Sorts.Add(new SortDescriptor("PCMachined", ListSortDirection.Ascending));
            }

            return Json(_dbRepository.GetEntities().Where(m => m.PreCommissioningId == PreCommissioningId).ToDataSourceResult(request));
        }

        public ActionResult KendoSave([DataSourceRequest] DataSourceRequest request, tblPreCommissioningMachine model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return Json(new[] { model }.ToDataSourceResult(request, ModelState));
            }

            string message = string.Empty;

            if (model.PCMachined > 0)
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
                ModelState.AddModelError("MachineSerialNo", message);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult KendoDestroy([DataSourceRequest] DataSourceRequest request, tblPreCommissioningMachine model)
        {
            string deleteMessage = _dbRepository.Delete(model.PCMachined);

            ModelState.Clear();
            if (!string.IsNullOrEmpty(deleteMessage))
            {
                ModelState.AddModelError(deleteMessage, deleteMessage);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult GetPCMachineList(int? PreCommissioningId = 0)
        {
            var result = _dbRepository.GetEntities().Where(m => m.PreCommissioningId == PreCommissioningId)
                                .Select(m => new { m.PCMachined, m.MachineSerialNo })
                                .OrderBy(m => new { m.MachineSerialNo }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}