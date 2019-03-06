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
    public class PreCommissioningDetailController : BaseController
    {
        #region private variables

        private readonly GenericRepository<tblPreCommissioningDetail> _dbRepository;

        #endregion

        #region Constructor
        public PreCommissioningDetailController()
        {
            _dbRepository = new GenericRepository<tblPreCommissioningDetail>();
        }
        #endregion

        #region Public Methods
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult KendoRead([DataSourceRequest] DataSourceRequest request, int PreCommissionId)
        {
            if (!request.Sorts.Any())
            {
                request.Sorts.Add(new SortDescriptor("PreCommisoningDate", ListSortDirection.Descending));
            }

            DataSourceResult result = _dbRepository.GetEntities().Where(m => m.PreCommissionId == PreCommissionId).ToDataSourceResult(request);
            List<tblPreCommissioningDetail> list = result.Data as List<tblPreCommissioningDetail>;
            foreach(tblPreCommissioningDetail obj in list)
            {
                if(obj.PCAccesseriesId > 0)
                {
                    obj.PCAccessoryIdList = new List<int>();
                    obj.PCAccessoryIdList.Add(obj.PCAccesseriesId.Value);
                }
                if(obj.PCMachineId > 0)
                {
                    obj.PCMachineIdList = new List<int>();
                    obj.PCMachineIdList.Add(obj.PCMachineId.Value);
                }
            }
            return Json(result);
        }

        public ActionResult KendoSave([DataSourceRequest] DataSourceRequest request, tblPreCommissioningDetail model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return Json(new[] { model }.ToDataSourceResult(request, ModelState));
            }

            string message = string.Empty;

            if (model.PCDetailId > 0)
            {
                model.ModifiedBy = SessionHelper.UserId;
                model.ModifiedDate = DateTime.Now;
                message = _dbRepository.Update(model);
            }

            else
            {
                model.CreatedBy = SessionHelper.UserId;
                model.CreatedDate = DateTime.Now;
                if (model.PCAccessoryIdList != null && model.PCAccessoryIdList.Count > 0)
                {
                    foreach(int id in model.PCAccessoryIdList)
                    {
                        tblPreCommissioningDetail oldAccessory = _dbRepository.GetEntities().Where(m => m.PreCommissionId == model.PreCommissionId && m.PCAccesseriesId == id && m.IsLatest == true).FirstOrDefault();
                        if (oldAccessory != null)
                        {
                            oldAccessory.IsLatest = false;
                            _dbRepository.Update(oldAccessory);
                        }

                        model.PCMachineId = null;
                        model.PCAccesseriesId = id;
                        model.IsLatest = true;
                        message = _dbRepository.Insert(model);
                    }
                }

                if (model.PCMachineIdList != null && model.PCMachineIdList.Count > 0)
                {
                    foreach (int id in model.PCMachineIdList)
                    {
                        tblPreCommissioningDetail oldMachine = _dbRepository.GetEntities().Where(m => m.PreCommissionId == model.PreCommissionId && m.PCMachineId == id && m.IsLatest == true).FirstOrDefault();
                        if (oldMachine != null)
                        {
                            oldMachine.IsLatest = false;
                            _dbRepository.Update(oldMachine);
                        }

                        model.PCAccesseriesId = null;
                        model.PCMachineId = id;
                        model.IsLatest = true;
                        message = _dbRepository.Insert(model);
                    }
                }
            }

            ModelState.Clear();
            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError("PreCommisoningDate", message);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult KendoDestroy([DataSourceRequest] DataSourceRequest request, tblPreCommissioningDetail model)
        {
            string deleteMessage = _dbRepository.Delete(model.PCDetailId);

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