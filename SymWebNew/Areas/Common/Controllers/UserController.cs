﻿using SymOrdinary;
using SymRepository;
using SymRepository.Common;
using SymRepository.HRM;
using SymViewModel.Common;
using SymViewModel.HRM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
namespace SymWebUI.Areas.Common.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        //
        // GET: /Common/User/
        UserRoleRepo userRoleRepo = new UserRoleRepo();
        UserInformationRepo _infoRepo = new UserInformationRepo();
        SymUserRoleRepo _reposur = new SymUserRoleRepo();
        ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
        public ActionResult Index(string UserStatus)
        {
            var permission = _reposur.SymRoleSession(identity.UserId, "1_4", "index").ToString();
            Session["permission"] = permission;
            if (permission == "False")
            {
                return Redirect("/Common/Home");
            }
            ViewBag.UserStatus = UserStatus;
            return View();
        }
        public ActionResult _index(JQueryDataTableParamVM param, string UserStatus = null)
        {
            #region Column Search
            var idFilter = Convert.ToString(Request["sSearch_0"]);
            var codeFilter = Convert.ToString(Request["sSearch_1"]);
            var empNameFilter = Convert.ToString(Request["sSearch_2"]);
            var departmentFilter = Convert.ToString(Request["sSearch_3"]);
            var designationFilter = Convert.ToString(Request["sSearch_4"]);
            var joinDateFilter = Convert.ToString(Request["sSearch_5"]);
            //Code
            //EmpName 
            //Department 
            //Designation
            //JoinDate
            DateTime fromDate = DateTime.MinValue;
            DateTime toDate = DateTime.MaxValue;
            if (joinDateFilter.Contains('~'))
            {
                //Split date range filters with ~
                fromDate = joinDateFilter.Split('~')[0] == "" ? DateTime.MinValue : Ordinary.IsDate(joinDateFilter.Split('~')[0]) == true ? Convert.ToDateTime(joinDateFilter.Split('~')[0]) : DateTime.MinValue;
                toDate = joinDateFilter.Split('~')[1] == "" ? DateTime.MaxValue : Ordinary.IsDate(joinDateFilter.Split('~')[1]) == true ? Convert.ToDateTime(joinDateFilter.Split('~')[1]) : DateTime.MinValue;
            }
            var fromID = 0;
            var toID = 0;
            if (idFilter.Contains('~'))
            {
                //Split number range filters with ~
                fromID = idFilter.Split('~')[0] == "" ? 0 : Convert.ToInt32(idFilter.Split('~')[0]);
                toID = idFilter.Split('~')[1] == "" ? 0 : Convert.ToInt32(idFilter.Split('~')[1]);
            }
            #endregion Column Search
            UserInformationRepo _repo = new UserInformationRepo();
            List<EmployeeInfoVM> getAllData = new List<EmployeeInfoVM>();
            if (string.IsNullOrWhiteSpace(UserStatus))
            {
                getAllData = _repo.SelectAllUser();
            }
            else
            {
                getAllData = _repo.SelectAllActiveEmp();
            }
            IEnumerable<EmployeeInfoVM> filteredData;
            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Optionally check whether the columns are searchable at all 
                var isSearchable1 = Convert.ToBoolean(Request["bSearchable_1"]);
                var isSearchable2 = Convert.ToBoolean(Request["bSearchable_2"]);
                var isSearchable3 = Convert.ToBoolean(Request["bSearchable_3"]);
                var isSearchable4 = Convert.ToBoolean(Request["bSearchable_4"]);
                var isSearchable5 = Convert.ToBoolean(Request["bSearchable_5"]);
                filteredData = getAllData.Where(c =>
                    isSearchable1 && c.Code.ToLower().Contains(param.sSearch.ToLower())
                    || isSearchable2 && c.EmpName.ToLower().Contains(param.sSearch.ToLower())
                    || isSearchable3 && c.Department.ToLower().Contains(param.sSearch.ToLower())
                    || isSearchable4 && c.Designation.ToLower().Contains(param.sSearch.ToLower())
                    || isSearchable5 && c.JoinDate.ToLower().Contains(param.sSearch.ToLower())
                    );
            }
            else
            {
                filteredData = getAllData;
            }
            #region Column Filtering
            if (codeFilter != "" || empNameFilter != "" || departmentFilter != "" || designationFilter != "" || (joinDateFilter != "" && joinDateFilter != "~"))
            {
                filteredData = filteredData
                                .Where(c =>
                                    (codeFilter == "" || c.Code.ToLower().Contains(codeFilter.ToLower()))
                                    &&
                                    (empNameFilter == "" || c.EmpName.ToLower().Contains(empNameFilter.ToLower()))
                                    &&
                                    (departmentFilter == "" || c.Department.ToLower().Contains(departmentFilter.ToLower()))
                                    &&
                                    (designationFilter == "" || c.Designation.ToString().ToLower().Contains(designationFilter.ToLower()))
                                    &&
                                    (fromDate == DateTime.MinValue || fromDate <= Convert.ToDateTime(c.JoinDate))
                                    &&
                                    (toDate == DateTime.MaxValue || toDate >= Convert.ToDateTime(c.JoinDate))
                                );
            }
            #endregion Column Filtering
            var isSortable_1 = Convert.ToBoolean(Request["bSortable_1"]);
            var isSortable_2 = Convert.ToBoolean(Request["bSortable_2"]);
            var isSortable_3 = Convert.ToBoolean(Request["bSortable_3"]);
            var isSortable_4 = Convert.ToBoolean(Request["bSortable_4"]);
            var isSortable_5 = Convert.ToBoolean(Request["bSortable_5"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<EmployeeInfoVM, string> orderingFunction = (c =>
                sortColumnIndex == 1 && isSortable_1 ? c.Code :
                sortColumnIndex == 2 && isSortable_2 ? c.EmpName :
                sortColumnIndex == 3 && isSortable_3 ? c.Department :
                sortColumnIndex == 4 && isSortable_4 ? c.Designation :
                sortColumnIndex == 5 && isSortable_5 ? Ordinary.DateToString(c.JoinDate) :
                "");
            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredData = filteredData.OrderBy(orderingFunction);
            else
                filteredData = filteredData.OrderByDescending(orderingFunction);
            var displayedCompanies = filteredData.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in displayedCompanies
                         select new[] { 
                Convert.ToString(c.Id)
                , c.Code
                , c.EmpName 
                , c.Department 
                , c.Designation
                , c.JoinDate                 
            };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = getAllData.Count(),
                iTotalDisplayRecords = filteredData.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Master,Admin,Account")]
        [HttpGet]
        public ActionResult User(string EmployeeId)
        {
            var permission = _reposur.SymRoleSession(identity.UserId, "1_4", "edit").ToString();
            Session["permission"] = permission;
            if (permission == "False")
            {
                return Redirect("/Common/Home");
            }
            UserInformationRepo _infoRepo = new UserInformationRepo();
            UserLogsVM vm = _infoRepo.SelectUserByEmployee(EmployeeId);
            if (string.IsNullOrWhiteSpace(vm.Id))
            {
                vm.Id = "0";
            }
            ViewBag.RoleList = new JavaScriptSerializer().Serialize(new RoleRepo().SelectAll());
            List<UserRolesVM> useRoles = userRoleRepo.SelectAll(vm.Id, vm.BranchId);
            ViewBag.UserRoleList = new JavaScriptSerializer().Serialize(useRoles);
            return PartialView("_user", vm);
        }
        [Authorize(Roles = "Master,Admin,Account")]
        public ActionResult User(UserLogsVM vm, string UserRoles, string UserStatus = null)
        {
            string[] Roles = new JavaScriptSerializer().Deserialize<string[]>(UserRoles);
            string[] retResults = new string[6];
            UserInformationRepo _infoRepo = new UserInformationRepo();
            vm.BranchId = Ordinary.BranchId;
            vm.Password = vm.Password == null ? null : Ordinary.Encrypt(vm.Password, true);
            if (vm.Id != "0")
            {
                vm.LastUpdateAt = DateTime.Now.ToString("yyyyMMddHHmmss");
                vm.LastUpdateBy = identity.Name;
                vm.LastUpdateFrom = identity.WorkStationIP;
                retResults = _infoRepo.Update(vm, Roles);
            }
            else
            {
                vm.CreatedAt = DateTime.Now.ToString("yyyyMMddHHmmss");
                vm.CreatedBy = identity.Name;
                vm.CreatedFrom = identity.WorkStationIP;
                retResults = _infoRepo.Insert(vm);
            }
           
            Session["result"] = retResults[0] + "~" + retResults[1];
            Session["mgs"] = "mgs";
            var mgs = "";
            mgs = retResults[0] + "~" + retResults[1];
            return RedirectToAction("Index", new { mgs = mgs, UserStatus = UserStatus });
            //return RedirectToAction("Index");
        }
        #region New Methods
        [Authorize(Roles = "Master,Admin,Account")]
        [HttpGet]
        public ActionResult Create(string id)
        {
            var permission = _reposur.SymRoleSession(identity.UserId, "1_4", "add").ToString();
            Session["permission"] = permission;
            if (permission == "False")
            {
                return Redirect("/Common/Home");
            }
            UserLogsVM vm = new UserLogsVM();
            EmployeeInfoRepo _infoRepo = new EmployeeInfoRepo();
            EmployeeInfoVM infoVM = new EmployeeInfoVM();
            infoVM = _infoRepo.SelectById(id);
            vm.EmployeeId = id;
            vm.FullName = infoVM.Code+ "~"+infoVM.EmpName;
            vm.Email = infoVM.Email;
            return View(vm);
        }
        [Authorize(Roles = "Master,Admin,Account")]
        [HttpPost]
        public ActionResult Create(UserLogsVM vm)
        {
            EmployeeInfoRepo _empRepo = new EmployeeInfoRepo();
            vm.EmployeeCode = vm.FullName.Split('~')[0];
            vm.FullName = vm.FullName.Split('~')[1];
            string[] result = new string[6];
            ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
            vm.CreatedAt = DateTime.Now.ToString("yyyyMMddHHmmss");
            vm.CreatedBy = identity.Name;
            vm.CreatedFrom = identity.WorkStationIP;
            vm.BranchId = Convert.ToInt32(identity.BranchId);
            vm.Password = vm.Password == null ? null : Ordinary.Encrypt(vm.Password, true);
            try
            {
                result = _infoRepo.Insert(vm);
                Session["result"] = result[0] + "~" + result[1];
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Save Failed";
                FileLogger.Log(result[0].ToString() + Environment.NewLine + result[2].ToString() + Environment.NewLine + result[5].ToString(), this.GetType().Name, result[4].ToString() + Environment.NewLine + result[3].ToString());
                return RedirectToAction("Index");
            }
        }
        [Authorize(Roles = "Master,Admin,Account")]
        [HttpGet]
        public ActionResult Edit(string Id, string EmployeeId, string UserStatus = null)
        {
            var permission = _reposur.SymRoleSession(identity.UserId, "1_4", "edit").ToString();
            Session["permission"] = permission;
            if (permission == "False")
            {
                return Redirect("/Common/Home");
            }
            UserLogsVM vm = new UserLogsVM();
            vm = _infoRepo.SelectUserByEmployee(null, Id);
            return View(vm);
        }
        [Authorize(Roles = "Master,Admin,Account")]
        [HttpPost]
        public ActionResult Edit(UserLogsVM vm, string UserStatus = null)
        {
            string[] result = new string[6];
            ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
            vm.LastUpdateAt = DateTime.Now.ToString("yyyyMMddHHmmss");
            vm.LastUpdateBy = identity.Name;
            vm.LastUpdateFrom = identity.WorkStationIP;
            vm.BranchId = Convert.ToInt32(identity.BranchId);
            try
            {
                result = _infoRepo.Update(vm);
                Session["result"] = result[0] + "~" + result[1];
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not uncceessfully";
                FileLogger.Log(result[0].ToString() + Environment.NewLine + result[2].ToString() + Environment.NewLine + result[5].ToString(), this.GetType().Name, result[4].ToString() + Environment.NewLine + result[3].ToString());
                return RedirectToAction("Index");
            }
        }
        public ActionResult PasswordChange(string Id)
        {
            var permission = _reposur.SymRoleSession(identity.UserId, "1_4", "index").ToString();
            Session["permission"] = permission;
            if (permission == "False")
            {
                return Redirect("/Common/Home");
            }
            UserLogsVM vm = new UserLogsVM();
            vm = _infoRepo.SelectUserByEmployee(null, Id);
            vm.Password = null;
            return PartialView("PasswordChange", vm);
        }
        [HttpPost]
        public ActionResult PasswordChange(UserLogsVM vm)
        {
            string[] result = new string[6];
            try
            {
                ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
                vm.LastUpdateAt = DateTime.Now.ToString("yyyyMMddHHmmss");
                vm.LastUpdateBy = identity.Name;
                vm.LastUpdateFrom = identity.WorkStationIP;
                vm.BranchId = Convert.ToInt32(identity.BranchId);
                vm.IsAdmin = identity.IsAdmin;
                result = _infoRepo.ChangePassword(vm);
                Session["result"] = result[0] + "~" + result[1];
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Succeessfully!";
                FileLogger.Log(result[0].ToString() + Environment.NewLine + result[2].ToString() + Environment.NewLine + result[5].ToString(), this.GetType().Name, result[4].ToString() + Environment.NewLine + result[3].ToString());
                return RedirectToAction("Index");
            }
        }
        #endregion Methods
    }
}
