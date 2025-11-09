using CrystalDecisions.CrystalReports.Engine;
using Newtonsoft.Json;
using OfficeOpenXml;
using SymOrdinary;
using SymReporting.PF;
using SymRepository.Common;
using SymRepository.PF;
using SymViewModel.Common;
using SymViewModel.HRM;
using SymViewModel.PF;
using SymWebUI.Areas.PF.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SymWebUI.Areas.PF.Controllers
{
    public class EmployeeTransferController : Controller
    {
        public EmployeeTransferController()
        {
            ViewBag.TransType = AreaTypePFVM.TransType;
        }

        EmployeeTransferRepo _transferRepo = new EmployeeTransferRepo();
        SymUserRoleRepo _reposur = new SymUserRoleRepo();
        ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;

        //
        // GET: /PF/EmployeeTransfer/
        public ActionResult Index()
        {
            var permission = _reposur.SymRoleSession(identity.UserId, "1_40", "index").ToString();
            Session["permission"] = permission;
            if (permission == "False")
            {
                return Redirect("/PF/Home");
            }
            return View("~/Areas/PF/Views/EmployeeTransfer/Index.cshtml");
        }

        public ActionResult _index(JQueryDataTableParamVM param)
        {
            var permission = _reposur.SymRoleSession(identity.UserId, "1_40", "edit").ToString();
            Session["permission"] = permission;
            if (permission == "False")
            {
                return Redirect("/PF/Home");
            }

            #region Column Search
            var idFilter = Convert.ToString(Request["sSearch_0"]);
            var codeFilter = Convert.ToString(Request["sSearch_1"]);
            var empNameFilter = Convert.ToString(Request["sSearch_2"]);
            var fromBranchFilter = Convert.ToString(Request["sSearch_3"]);
            var toBranchFilter = Convert.ToString(Request["sSearch_4"]);
            var transferDateFilter = Convert.ToString(Request["sSearch_5"]);
            var postFilter = Convert.ToString(Request["sSearch_6"]);
            #endregion

            var allData = _transferRepo.SelectAll();
            IEnumerable<EmployeeTransferVM> filteredData;

            // 🔹 Global Search
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                var isSearchable1 = Convert.ToBoolean(Request["bSearchable_1"]);
                var isSearchable2 = Convert.ToBoolean(Request["bSearchable_2"]);
                var isSearchable3 = Convert.ToBoolean(Request["bSearchable_3"]);
                var isSearchable4 = Convert.ToBoolean(Request["bSearchable_4"]);
                var isSearchable5 = Convert.ToBoolean(Request["bSearchable_5"]);
                var isSearchable6 = Convert.ToBoolean(Request["bSearchable_6"]);
                var isSearchable7 = Convert.ToBoolean(Request["bSearchable_7"]);

                filteredData = allData.Where(c =>
                       isSearchable1 && c.Code.ToLower().Contains(param.sSearch.ToLower())
                    || isSearchable2 && c.EmpName.ToLower().Contains(param.sSearch.ToLower())
                    || isSearchable3 && c.FromBranch.ToString().Contains(param.sSearch.ToLower())
                    || isSearchable4 && c.ToBranch.ToString().Contains(param.sSearch.ToLower())
                    || isSearchable5 && c.TransferDate.ToLower().Contains(param.sSearch.ToLower())
                    || isSearchable6 && c.TransferReason.ToLower().Contains(param.sSearch.ToLower())
                    || isSearchable7 && c.Post.ToString().ToLower().Contains(param.sSearch.ToLower())
                );
            }
            else
            {
                filteredData = allData;
            }

            #region Column Filtering
            if (codeFilter != "" || empNameFilter != "" || fromBranchFilter != "" || toBranchFilter != "" || transferDateFilter != "" || postFilter != "")
            {
                filteredData = filteredData.Where(c =>
                    (codeFilter == "" || c.Code.ToLower().Contains(codeFilter.ToLower())) &&
                    (empNameFilter == "" || c.EmpName.ToLower().Contains(empNameFilter.ToLower())) &&
                    (fromBranchFilter == "" || c.FromBranch.ToString().Contains(fromBranchFilter.ToLower())) &&
                    (toBranchFilter == "" || c.ToBranch.ToString().Contains(toBranchFilter.ToLower())) &&
                    (transferDateFilter == "" || c.TransferDate.ToLower().Contains(transferDateFilter.ToLower()))
                );
            }
            #endregion

            // 🔹 Sorting
            var isSortable_1 = Convert.ToBoolean(Request["bSortable_1"]);
            var isSortable_2 = Convert.ToBoolean(Request["bSortable_2"]);
            var isSortable_3 = Convert.ToBoolean(Request["bSortable_3"]);
            var isSortable_4 = Convert.ToBoolean(Request["bSortable_4"]);
            var isSortable_5 = Convert.ToBoolean(Request["bSortable_5"]);
            var isSortable_6 = Convert.ToBoolean(Request["bSortable_6"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);

            Func<EmployeeTransferVM, string> orderingFunction = (c =>
                sortColumnIndex == 1 && isSortable_1 ? c.Code :
                sortColumnIndex == 2 && isSortable_2 ? c.EmpName :
                sortColumnIndex == 3 && isSortable_3 ? c.FromBranch.ToString() :
                sortColumnIndex == 4 && isSortable_4 ? c.ToBranch.ToString() :
                sortColumnIndex == 5 && isSortable_5 ? c.TransferDate :
                sortColumnIndex == 6 && isSortable_6 ? c.TransferReason :
                "");

            var sortDirection = Request["sSortDir_0"];
            if (sortDirection == "asc")
                filteredData = filteredData.OrderBy(orderingFunction);
            else
                filteredData = filteredData.OrderByDescending(orderingFunction);

            // 🔹 Pagination
            var displayedData = filteredData.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            var result = from c in displayedData
                         select new[]
                         {
                             Convert.ToString(c.Id),
                             c.Code,
                             c.EmpName,
                             c.FromBranch.ToString(),
                             c.ToBranch.ToString(),
                             c.TransferDate,
                             c.TransferReason,
                             c.Post ? "Posted" : "Not Posted"
                         };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allData.Count(),
                iTotalDisplayRecords = filteredData.Count(),
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }

        


        public ActionResult DetailCreate(string empcode = "", string id = "0", string Operation = "")
        {
            var permission = _reposur.SymRoleSession(identity.UserId, "1_40", "add").ToString();
            Session["permission"] = permission;
            if (permission == "False")
            {
                return Redirect("/PF/Home");
            }

            EmployeeTransferVM vm = new EmployeeTransferVM();

            if (!string.IsNullOrEmpty(id) && id != "0")
            {
                vm = _transferRepo.SelectById(id);
                vm.Operation = "update";
            }
            else
            {
                vm.Operation = "add";
                vm.TransferDate = DateTime.Now.ToString("yyyy-MM-dd");
                vm.Post = false;
            }

            return PartialView("_detailCreate", vm);
        }

        [HttpPost]
        public ActionResult Create(EmployeeTransferVM vm)
        {
            string[] result = new string[6];
            try
            {
                vm.CreatedAt = DateTime.Now.ToString("yyyyMMddHHmmss");
                vm.CreatedBy = identity.Name;
                vm.CreatedFrom = identity.WorkStationIP;
                vm.TransType = AreaTypePFVM.TransType;

                result = _transferRepo.Insert(vm);
                return Json(result[0] + "~" + result[1] + "~" + result[2], JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(result[0] + "~" + result[1], JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Update(EmployeeTransferVM vm)
        {
            string[] result = new string[6];
            try
            {
                vm.LastUpdateAt = DateTime.Now.ToString("yyyyMMddHHmmss");
                vm.LastUpdateBy = identity.Name;
                vm.LastUpdateFrom = identity.WorkStationIP;
                vm.TransType = AreaTypePFVM.TransType;

                result = _transferRepo.Update(vm);
                return Json(result[0] + "~" + result[1], JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(result[0] + "~" + result[1], JsonRequestBehavior.AllowGet);
            }
        }

    }
}
