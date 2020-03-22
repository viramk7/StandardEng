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
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StandardEng.Web.Controllers
{
    public class AMCServicesController : BaseController
    {

        #region private variables
        private readonly GenericRepository<tblAMCStart> _dbRepository;

        #endregion

        #region Constructor
        public AMCServicesController()
        {
            _dbRepository = new GenericRepository<tblAMCStart>();
        }
        #endregion

        // GET: AMCServices
        public ActionResult Index()
        {
            ViewBag.AMCList = SelectionList.AMCList().Select(m => new { m.AMCId, m.QuotationNo });
            return View();
        }

        public ActionResult KendoRead([DataSourceRequest] DataSourceRequest request)
        {
            if (!request.Sorts.Any())
            {
                request.Sorts.Add(new SortDescriptor("AMCId", ListSortDirection.Descending));
            }

            return Json(_dbRepository.GetEntities().Where(m=>m.IsActive).ToDataSourceResult(request));
        }

        public ActionResult Edit(int id)
        {
            ViewBag.ServiceEngineerList = SelectionList.ServiceEngineerList().Select(m => new { m.UserId, m.Name });
            var obj = _dbRepository.GetEntities().Where(m => m.Id == id).Include(m => m.tblAMC).FirstOrDefault();
            return View("Create", obj);
        }

        public ActionResult SaveModelData(tblAMCStart model)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }

            string message = string.Empty;

            try
            {
                if (model.Id > 0)
                {
                    tblAMCStart actualObj = _dbRepository.SelectById(model.Id);
                    if(actualObj != null)
                    {
                        //It means no change
                        if(actualObj.AMCStartDate == model.AMCStartDate && actualObj.AMCEndDate == model.AMCEndDate)
                        {

                        }
                        else
                        {
                            actualObj.IsActive = false;
                            message = _dbRepository.Update(actualObj);

                            tblAMCStart obj = new tblAMCStart();
                            obj.AMCId = model.AMCId;
                            obj.AMCStartDate = model.AMCStartDate;
                            obj.AMCEndDate = model.AMCEndDate;
                            obj.IsActive = true;
                            obj.CreatedBy = SessionHelper.UserId;
                            obj.CreatedDate = DateTime.Now.Date;
                            _dbRepository.Insert(obj);
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                message = CommonHelper.GetErrorMessage(ex);
            }

            return RedirectToAction("Index");
        }
    }
}