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
    public class ContactPersonController : BaseController   
    {
        #region private variables

        private readonly GenericRepository<tblCustomerContactPersons> _dbRepository;

        #endregion
        #region Constructor
        public ContactPersonController()
        {
            _dbRepository = new GenericRepository<tblCustomerContactPersons>();
        }
        #endregion

        #region Public Methods
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult KendoRead([DataSourceRequest] DataSourceRequest request,int CustomerId)
        {
            if (!request.Sorts.Any())
            {
                request.Sorts.Add(new SortDescriptor("ContactPersonName", ListSortDirection.Ascending));
            }

            return Json(_dbRepository.GetEntities().Where(m=>m.CustomerId == CustomerId).ToDataSourceResult(request));
        }

        public ActionResult KendoSave([DataSourceRequest] DataSourceRequest request, tblCustomerContactPersons model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return Json(new[] { model }.ToDataSourceResult(request, ModelState));
            }

            string message = string.Empty;

            message = model.ContactPersonId > 0 ? _dbRepository.Update(model) : _dbRepository.Insert(model);

            ModelState.Clear();
            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError("Name", message);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult KendoDestroy([DataSourceRequest] DataSourceRequest request, tblCustomerContactPersons model)
        {
            string deleteMessage = _dbRepository.Delete(model.ContactPersonId);

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