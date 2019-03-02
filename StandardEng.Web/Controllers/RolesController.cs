using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using StandardEng.Common;
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
    public class RolesController : BaseController
    {
        #region private variables

        private readonly GenericRepository<tblRole> _dbRepository;
        private readonly GenericRepository<tblRoleMenuMap> _dbRepositoryRoleMap;

        #endregion
        #region Constructor
        public RolesController()
        {
            _dbRepository = new GenericRepository<tblRole>();
            _dbRepositoryRoleMap = new GenericRepository<tblRoleMenuMap>();
        }
        #endregion
        #region Public Methods
        // GET: Roles
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult KendoRead([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                if (!request.Sorts.Any())
                {
                    request.Sorts.Add(new SortDescriptor("RoleName", ListSortDirection.Ascending));
                }

                return Json(_dbRepository.GetEntities().ToDataSourceResult(request));
            }

            catch (Exception ex)
            {
                return Json(CommonHelper.GetErrorMessage(ex));
            }
        }

        public ActionResult KendoSave([DataSourceRequest] DataSourceRequest request, tblRole model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return Json(new[] { model }.ToDataSourceResult(request, ModelState));
            }

            string message = model.RoleId > 0 ? _dbRepository.Update(model) : _dbRepository.Insert(model);

            ModelState.Clear();
            if (!string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError("RoleName", message);
            }
            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult KendoDestroy([DataSourceRequest] DataSourceRequest request, tblRole model)
        {
            string deleteMessage = _dbRepository.Delete(model.RoleId);

            ModelState.Clear();
            if (!string.IsNullOrEmpty(deleteMessage))
            {
                ModelState.AddModelError(deleteMessage, deleteMessage);
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }


        public string ChangeStatus(long id)
        {
            tblRole location = _dbRepository.SelectById(id);
            location.IsActive = !location.IsActive;
            return _dbRepository.Update(location);
        }

        public ActionResult RoleMenusView()
        {
            return PartialView("_RoleMenuAssignment");
        }

        public ActionResult GetUserRightsList(DataSourceRequest request, int RoleId)
        {
            List<AssignRoleList_Result> list = CustomRepository.GetUserRoleRights(RoleId);
            return Json(list.ToDataSourceResult(request));
        }

        [HttpPost]
        public string SaveUserRights(string model)
        {
            try
            {
                var roleActions = JsonConvert.DeserializeObject<IEnumerable<AssignRoleList_Result>>(model);

                foreach (AssignRoleList_Result roleAction in roleActions)
                {
                    var roleActionEntity = new tblRoleMenuMap
                    {
                        IsInsert = roleAction.IsInsert,
                        IsView = roleAction.IsView,
                        IsEdit = roleAction.IsEdit,
                        IsDelete = roleAction.IsDelete,
                        IsChangeStatus = roleAction.IsChangeStatus,
                        RoleMenuPK = roleAction.RoleMenuPK,
                        MenuId = roleAction.MenuId,
                        RoleId = roleAction.RoleId
                    };

                    _dbRepositoryRoleMap.Update(roleActionEntity);
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return CommonHelper.GetErrorMessage(ex, false);
            }
        }


        #endregion
    }
}