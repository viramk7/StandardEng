using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using StandardEng.Data.DB;
using StandardEng.Data.Repository;
using StandardEng.Web.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

            DataSourceResult result = CustomRepository.GetPreCommisioningDetailData(PreCommissionId).ToDataSourceResult(request);
            //DataSourceResult result = _dbRepository.GetEntities().Where(m => m.PreCommissionId == PreCommissionId).ToDataSourceResult(request);
            List<GetPreCommisioningDetailData_Result> list = result.Data as List<GetPreCommisioningDetailData_Result>;
            foreach (GetPreCommisioningDetailData_Result obj in list)
            {
                if (obj.PCAccesseriesId > 0)
                {
                    obj.PCAccessoryIdList = new List<int>();
                    obj.PCAccessoryIdList.Add(obj.PCAccesseriesId.Value);
                }
                if (obj.PCMachineId > 0)
                {
                    obj.PCMachineIdList = new List<int>();
                    obj.PCMachineIdList.Add(obj.PCMachineId.Value);
                }
            }
            return Json(list.ToDataSourceResult(request));
        }

        public ActionResult KendoSave([DataSourceRequest] DataSourceRequest request, GetPreCommisioningDetailData_Result obj)
        {
            if (obj == null || !ModelState.IsValid)
            {
                return Json(new[] { obj }.ToDataSourceResult(request, ModelState));
            }

            string message = string.Empty;

            tblPreCommissioningDetail model = new tblPreCommissioningDetail();
            model.PCDetailId = obj.PCDetailId;
            model.PCMachineId = obj.PCMachineId;
            model.PCAccesseriesId = obj.PCAccesseriesId;
            model.PCMachineIdList = obj.PCMachineIdList;
            model.PCAccessoryIdList = obj.PCAccessoryIdList;
            model.PreCommisoningDate = obj.PreCommisoningDate;
            model.ServiceEngineerId = obj.ServiceEngineerId;
            model.PrecommisioningRemark = obj.PrecommisioningRemark;
            model.PreCommissionId = obj.PreCommissionId;
            model.PreCommisioningFileName = obj.PreCommisioningFileName;
            model.CreatedBy = obj.CreatedBy;
            model.CreatedDate = obj.CreatedDate;
            model.IsLatest = obj.IsLatest;

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

        public ActionResult KendoDestroy([DataSourceRequest] DataSourceRequest request, GetPreCommisioningDetailData_Result model)
        {
            string deleteMessage = _dbRepository.Delete(model.PCDetailId);

            ModelState.Clear();
            if (!string.IsNullOrEmpty(deleteMessage))
            {
                ModelState.AddModelError(deleteMessage, deleteMessage);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult SaveUploadFile(HttpPostedFileBase fileImport)
        {
            string extension = Path.GetExtension(fileImport.FileName);
            string fileName = Guid.NewGuid() + Path.GetFileName(fileImport.FileName);
            string physicalPath = Path.Combine(Server.MapPath("~/Uploads"), fileName);
            fileImport.SaveAs(physicalPath);
            return Json(new { FileName = fileName }, "text/plain");
        }

        public FileResult DownloadFile(string file)
        {
            if(!string.IsNullOrEmpty(file) && file != "null")
            {
                string fileExtension = Path.GetExtension(file);
                string filePath = Path.Combine(Server.MapPath("~/Uploads"), file);
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                if (fileExtension == ".pdf")
                {
                    var response = new FileContentResult(fileBytes, "application/octet-stream");
                    response.FileDownloadName = "PreCommissioningUploadFile.pdf";
                    return response;
                }
                else
                {
                    var response = new FileContentResult(fileBytes, "image/jpeg");
                    response.FileDownloadName = "PreCommissioningUploadFile.jpeg";
                    return response;
                }
            }
            return null;

        }
        #endregion
    }
}