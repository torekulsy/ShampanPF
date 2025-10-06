﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using JQueryDataTables.Models;
using OfficeOpenXml;
using SymOrdinary;
using SymRepository.Common;
using SymRepository.PF;
using SymViewModel.Common;
using SymViewModel.PF;
using SymWebUI.Areas.PF.Models;

namespace SymWebUI.Areas.PF.Controllers
{
    public class ProfitDistributionNewController : Controller
    {
        public ProfitDistributionNewController()
        {
            ViewBag.TransType = AreaTypePFVM.TransType;
        }
        //
        // GET: /PF/ProfitDistribution/

        SymUserRoleRepo _repoSUR = new SymUserRoleRepo();
        ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
        ProfitDistributionNewRepo _repo = new ProfitDistributionNewRepo();

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            string permission = _repoSUR.SymRoleSession(identity.UserId, "10003", "index").ToString();
            Session["permission"] = permission;
            if (permission == "False")
            {
                return Redirect("/PF/Home");
            }
            return View();
        }

        public ActionResult _index(JQueryDataTableParamModel param)
        {
            //00     //Id
            //01     //DistributionDate
            //04     //TotalProfit
            //05     //Post
            //06     //IsPaid

            #region Search and Filter Data
            var getAllData = _repo.SelectAll();
            IEnumerable<ProfitDistributionNewVM> filteredData;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                var isSearchable1 = Convert.ToBoolean(Request["bSearchable_1"]);
                var isSearchable2 = Convert.ToBoolean(Request["bSearchable_2"]);
                var isSearchable3 = Convert.ToBoolean(Request["bSearchable_3"]);
                var isSearchable4 = Convert.ToBoolean(Request["bSearchable_4"]);
                var isSearchable5 = Convert.ToBoolean(Request["bSearchable_5"]);
                var isSearchable6 = Convert.ToBoolean(Request["bSearchable_6"]);

                filteredData = getAllData.Where(c =>
                    
                       isSearchable1 && c.EmployeeCode.ToLower().Contains(param.sSearch.ToLower())
                    || isSearchable2 && c.EmployeeName.ToString().ToLower().Contains(param.sSearch.ToLower())

                    || isSearchable3 && c.DistributionDate.ToString().ToLower().Contains(param.sSearch.ToLower())
                    || isSearchable4 && c.TotalProfit.ToString().ToLower().Contains(param.sSearch.ToLower())

                    || isSearchable5 && c.Post.ToString().ToLower().Contains(param.sSearch.ToLower())
                    || isSearchable6 && c.IsPaid.ToString().ToLower().Contains(param.sSearch.ToLower())
                );
            }
            else
            {
                filteredData = getAllData;
            }
            #endregion Search and Filter Data
            var isSortable_1 = Convert.ToBoolean(Request["bSortable_1"]);
            var isSortable_2 = Convert.ToBoolean(Request["bSortable_2"]);
            var isSortable_3 = Convert.ToBoolean(Request["bSortable_3"]);
            var isSortable_4 = Convert.ToBoolean(Request["bSortable_4"]);
            var isSortable_5 = Convert.ToBoolean(Request["bSortable_5"]);
            var isSortable_6 = Convert.ToBoolean(Request["bSortable_6"]);

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<ProfitDistributionNewVM, string> orderingFunction = (c =>

                sortColumnIndex == 1 && isSortable_1 ? c.EmployeeCode :
                sortColumnIndex == 2 && isSortable_2 ? c.EmployeeName.ToString() :

                sortColumnIndex == 3 && isSortable_3 ? Ordinary.DateToString(c.DistributionDate) :
                sortColumnIndex == 4 && isSortable_4 ? c.TotalProfit.ToString() :

                sortColumnIndex == 4 && isSortable_4 ? c.Post.ToString() :
                sortColumnIndex == 4 && isSortable_4 ? c.IsPaid.ToString() :
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
                    ,c.EmployeeCode
                    ,c.EmployeeName
                    , c.DistributionDate
                    , c.TotalProfit.ToString()
                    , c.Post ? "Posted" : "Not Posted"
                    , c.IsPaid ? "Paid" : "Not Paid"
     
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




        public ActionResult Process(int PreDistributionFundId = 0)
        {
            Session["permission"] = _repoSUR.SymRoleSession(identity.UserId, "10003", "add").ToString();
            PreDistributionFundRepo preDistributionFundRepo = new PreDistributionFundRepo();

            ProfitDistributionNewVM vm = new ProfitDistributionNewVM();
            vm.Operation = "add";
            vm.PreDistributionFundId = PreDistributionFundId.ToString();

            vm.PreDistributionFund = preDistributionFundRepo.SelectAll(PreDistributionFundId).FirstOrDefault();

            return View(vm);
        }

        
        public ActionResult ProcessDistribution(ProfitDistributionNewVM vm)
        {
            try
            {
                Session["permission"] = _repoSUR.SymRoleSession(identity.UserId, "10003", "add").ToString();
                vm.BranchId = Session["BranchId"].ToString();

                ResultVM result = _repo.Process(vm);

                Session["result"] = result.Status + "~" + result.Message;
            }
            catch (Exception e)
            {
                Session["result"] = "Fail" + "~" + e.Message;

            }

            return RedirectToAction("Process", new { vm.PreDistributionFundId });

        }
    }
}