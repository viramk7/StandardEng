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
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StandardEng.Web.Controllers
{
    public class CustomerController : BaseController
    {
        #region private variables

        private readonly GenericRepository<tblCustomer> _dbRepository;

        #endregion

        #region Constructor
        public CustomerController()
        {
            _dbRepository = new GenericRepository<tblCustomer>();
        }
        #endregion

        #region Public Methods
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.CountryList = SelectionList.CountryList().Select(m => new { m.CountryId, m.CountryName });
            ViewBag.StateList = SelectionList.StateList().Select(m => new { m.StateId, m.StateName });
            ViewBag.CityList = SelectionList.CityList().Select(m => new { m.CityId, m.CityName });
            return View();
        }

        public ActionResult KendoRead([DataSourceRequest] DataSourceRequest request)
        {
            if (!request.Sorts.Any())
            {
                request.Sorts.Add(new SortDescriptor("CustomerName", ListSortDirection.Ascending));
            }

            return Json(_dbRepository.GetEntities().ToDataSourceResult(request));
        }

        public ActionResult Create()
        {
            return View(new tblCustomer { IsActive = true });
        }

        public ActionResult Edit(int id)
        {
            return View("Create", _dbRepository.SelectById(id));
        }

        public ActionResult SaveModelData(tblCustomer model, string create = null)
        {
             if (!ModelState.IsValid)
            {
                return View("Create", model);
            }

            string message = string.Empty;

            try
            {
                message = model.CustomerId > 0 ? _dbRepository.Update(model) : _dbRepository.Insert(model);
            }
            catch (Exception ex)
            {
                message = CommonHelper.GetErrorMessage(ex);
            }

            if (model.CustomerId > 0)
            {
                if (create == "Save & Continue")
                {
                    return RedirectToAction("Edit", new { id = model.CustomerId });
                }
                else if (create == "Save & New")
                {
                    return RedirectToAction("Create");
                }
            }
            if (model.CustomerId > 0)
            {
                TempData[Enums.NotifyType.Success.GetDescription()] = "Customer Updated Successfully.";
            }
            else
            {
                TempData[Enums.NotifyType.Success.GetDescription()] = "Customer Created Successfully.";
            }
            return RedirectToAction("Index");
        }

        public string KendoDestroy(int id)
        {
            string deleteMessage = string.Empty;

            try
            {
                if (id != 0)
                {
                    deleteMessage = _dbRepository.Delete(id);
                }
                else
                {
                    deleteMessage = "Something went wrong.";
                }
            }
            catch (Exception ex)
            {
                deleteMessage = CommonHelper.GetDeleteException(ex);
            }

            return deleteMessage;
        }

        public string ChangeStatus(int id)
        {
            tblCustomer user = _dbRepository.SelectById(id);
            user.IsActive = !user.IsActive;
            return _dbRepository.Update(user);
        }
        #endregion
    }
}