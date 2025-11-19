using CrystalDecisions.CrystalReports.Engine;
using JQueryDataTables.Models;
using SymOrdinary;
using SymRepository.Common;
using SymRepository.PF;
using SymViewModel.PF;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using SymViewModel.Common;
using SymReporting.PF;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Newtonsoft.Json;
using SymWebUI.Areas.PF.Models;
using SymRepository.Payroll;
using System.Web;

namespace SymWebUI.Areas.PF.Controllers
{
    public class WPPFController : Controller
    {
        public WPPFController()
        {
            ViewBag.TransType = AreaTypePFVM.TransType;
        }
        //
        // GET: /PF/WPPF/
        WPPFRepo _repo = new WPPFRepo();
        SymUserRoleRepo _repoSUR = new SymUserRoleRepo();
        ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;

        public ActionResult IndexFiscalPeriod(string EmployeeId = "", string fydid = "")
        {
            ViewBag.EmployeeId = EmployeeId;
            ViewBag.fydid = fydid;

            return View();
        }

        public ActionResult _indexFiscalPeriod(JQueryDataTableParamModel param, string EmployeeId = "", string fydid = "", bool isWWF = false)
        {
            WPPFRepo _repo = new WPPFRepo();
            List<PFHeaderVM> getAllData = new List<PFHeaderVM>();
            IEnumerable<PFHeaderVM> filteredData;
            ShampanIdentity Identit = (ShampanIdentity)Thread.CurrentPrincipal.Identity;

            string[] conditionFields = { "pfd.EmployeeId", "pfd.FiscalYearDetailId" };
            string[] conditionValues = { EmployeeId, fydid };

            getAllData = _repo.SelectFiscalPeriodHeader(conditionFields, conditionValues);

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                // Optionally check whether the columns are searchable at all 
                var isSearchable1 = Convert.ToBoolean(Request["bSearchable_1"]);
                var isSearchable2 = Convert.ToBoolean(Request["bSearchable_2"]);
                var isSearchable3 = Convert.ToBoolean(Request["bSearchable_3"]);
                var isSearchable4 = Convert.ToBoolean(Request["bSearchable_4"]);
                var isSearchable5 = Convert.ToBoolean(Request["bSearchable_5"]);
            

                filteredData = getAllData
                    .Where(c =>
                          isSearchable1 && c.Code.ToLower().Contains(param.sSearch.ToLower())
                       || isSearchable2 && c.ProjectName.ToLower().Contains(param.sSearch.ToLower())
                       || isSearchable3 && c.FiscalPeriod.ToLower().Contains(param.sSearch.ToLower())
                       || isSearchable4 && c.TotalPF.ToString().ToLower().Contains(param.sSearch.ToLower())
                       || isSearchable5 && c.Post.ToString().ToLower().Contains(param.sSearch.ToLower())
                       
                    );
            }
            else
            {
                filteredData = getAllData;
            }

            var isSortable_1 = Convert.ToBoolean(Request["bSortable_1"]);
            var isSortable_2 = Convert.ToBoolean(Request["bSortable_2"]);
            var isSortable_3 = Convert.ToBoolean(Request["bSortable_3"]);
            var isSortable_4 = Convert.ToBoolean(Request["bSortable_4"]);
            var isSortable_5 = Convert.ToBoolean(Request["bSortable_5"]);
          

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<PFHeaderVM, string> orderingFunction = (c =>
                sortColumnIndex == 1 && isSortable_1 ? c.Code :
                sortColumnIndex == 2 && isSortable_2 ? c.ProjectName :
                sortColumnIndex == 3 && isSortable_3 ? c.PeriodStart :
                sortColumnIndex == 4 && isSortable_4 ? c.TotalPF.ToString() :
                sortColumnIndex == 5 && isSortable_5 ? c.Post.ToString() :
               
                "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredData = filteredData.OrderBy(orderingFunction);
            else
                filteredData = filteredData.OrderByDescending(orderingFunction);

            var displayedCompanies = filteredData.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in displayedCompanies
                         select new[] { 
                     c.Id.ToString()
                    , c.Code
                    , c.ProjectName
                    , c.FiscalPeriod
                    , c.TotalPF.ToString()
                    , c.Post ? "Yes" : "No"
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

        public ActionResult PFProcess(string fydid, decimal? TotalProfit, int? FiscalYear)
        {
            if (string.IsNullOrEmpty(fydid))
                return View();

            if (TotalProfit == null || TotalProfit <= 0)
                return Json("Fail~Invalid Total Profit", JsonRequestBehavior.AllowGet);

            ShampanIdentityVM vm = new ShampanIdentityVM
            {
                CreatedAt = DateTime.Now.ToString("yyyyMMddHHmmss"),
                CreatedBy = identity.Name,
                CreatedFrom = identity.WorkStationIP
            };

            WPPFRepo repo = new WPPFRepo();
            string[] result = repo.PFProcess(TotalProfit, fydid, FiscalYear, vm);

            string mgs = result[0] + "~" + result[1];
            Session["result"] = mgs;

            return Json(mgs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProfitDistribution(string EmployeeId = "", string fydid = "")
        {
            ViewBag.EmployeeId = EmployeeId;
            ViewBag.fydid = fydid;

            return View();
        }

        public ActionResult _indexProfitDistribution(JQueryDataTableParamModel param, string EmployeeId = "", string fydid = "")
        {

            WPPFRepo _repo = new WPPFRepo();
            List<PFHeaderVM> getAllData = new List<PFHeaderVM>();
            IEnumerable<PFHeaderVM> filteredData;
            ShampanIdentity Identit = (ShampanIdentity)Thread.CurrentPrincipal.Identity;

            string[] conditionFields = { "a.EmployeeId", "a.FiscalYearDetailId" };
            string[] conditionValues = { EmployeeId, fydid };

            getAllData = _repo.SelectProfitDistribution(conditionFields, conditionValues);

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Optionally check whether the columns are searchable at all 
                var isSearchable1 = Convert.ToBoolean(Request["bSearchable_1"]);
                var isSearchable2 = Convert.ToBoolean(Request["bSearchable_2"]);
                var isSearchable3 = Convert.ToBoolean(Request["bSearchable_3"]);
                var isSearchable4 = Convert.ToBoolean(Request["bSearchable_4"]);
                var isSearchable5 = Convert.ToBoolean(Request["bSearchable_5"]);
                filteredData = getAllData
                    .Where(c =>
                          isSearchable1 && c.Code.ToLower().Contains(param.sSearch.ToLower())
                       || isSearchable2 && c.ProjectName.ToLower().Contains(param.sSearch.ToLower())
                       || isSearchable3 && c.FiscalPeriod.ToLower().Contains(param.sSearch.ToLower())
                       || isSearchable4 && c.TotalPF.ToString().ToLower().Contains(param.sSearch.ToLower())
                       || isSearchable5 && c.Post.ToString().ToLower().Contains(param.sSearch.ToLower())
                    );
            }
            else
            {
                filteredData = getAllData;
            }

            var isSortable_1 = Convert.ToBoolean(Request["bSortable_1"]);
            var isSortable_2 = Convert.ToBoolean(Request["bSortable_2"]);
            var isSortable_3 = Convert.ToBoolean(Request["bSortable_3"]);
            var isSortable_4 = Convert.ToBoolean(Request["bSortable_4"]);
            var isSortable_5 = Convert.ToBoolean(Request["bSortable_5"]);

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<PFHeaderVM, string> orderingFunction = (c =>
                sortColumnIndex == 1 && isSortable_1 ? c.Code :
                sortColumnIndex == 2 && isSortable_2 ? c.ProjectName :
                sortColumnIndex == 3 && isSortable_3 ? c.FiscalPeriod :
                sortColumnIndex == 4 && isSortable_4 ? c.TotalPF.ToString() :
                sortColumnIndex == 5 && isSortable_5 ? c.Post.ToString() :
                                                           "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredData = filteredData.OrderBy(orderingFunction);
            else
                filteredData = filteredData.OrderByDescending(orderingFunction);

            var displayedCompanies = filteredData.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in displayedCompanies
                         select new[] { 
                 //c.Id.ToString()
                 c.Code
                , c.ProjectName
                , c.FiscalPeriod
                ,c.TotalPF.ToString()
                , c.Post?"Yes":"No"
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

        public ActionResult Index_WPPF(string EmployeeId = "", string fydid = "")
        {
            ViewBag.EmployeeId = EmployeeId;
            ViewBag.fydid = fydid;

            return View();
        }

        public ActionResult _index_WPPF(JQueryDataTableParamModel param, string EmployeeId = "", string fydid = "")
        {
            WPPFRepo _repo = new WPPFRepo();
            List<PFHeaderVM> getAllData = new List<PFHeaderVM>();
            IEnumerable<PFHeaderVM> filteredData;
            ShampanIdentity Identit = (ShampanIdentity)Thread.CurrentPrincipal.Identity;

            string[] conditionFields = { "pfd.EmployeeId", "pfd.FiscalYearDetailId" };
            string[] conditionValues = { EmployeeId, fydid };

            getAllData = _repo.SelectWPPF(conditionFields, conditionValues);

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                // Optionally check whether the columns are searchable at all 
                var isSearchable1 = Convert.ToBoolean(Request["bSearchable_1"]);
                var isSearchable2 = Convert.ToBoolean(Request["bSearchable_2"]);
                var isSearchable3 = Convert.ToBoolean(Request["bSearchable_3"]);
                var isSearchable4 = Convert.ToBoolean(Request["bSearchable_4"]);
                var isSearchable5 = Convert.ToBoolean(Request["bSearchable_5"]);


                filteredData = getAllData
                    .Where(c =>
                          isSearchable1 && c.Code.ToLower().Contains(param.sSearch.ToLower())
                       || isSearchable2 && c.ProjectName.ToLower().Contains(param.sSearch.ToLower())
                       || isSearchable3 && c.FiscalPeriod.ToLower().Contains(param.sSearch.ToLower())
                       || isSearchable4 && c.TotalPF.ToString().ToLower().Contains(param.sSearch.ToLower())
                       || isSearchable5 && c.Post.ToString().ToLower().Contains(param.sSearch.ToLower())

                    );
            }
            else
            {
                filteredData = getAllData;
            }

            var isSortable_1 = Convert.ToBoolean(Request["bSortable_1"]);
            var isSortable_2 = Convert.ToBoolean(Request["bSortable_2"]);
            var isSortable_3 = Convert.ToBoolean(Request["bSortable_3"]);
            var isSortable_4 = Convert.ToBoolean(Request["bSortable_4"]);
            var isSortable_5 = Convert.ToBoolean(Request["bSortable_5"]);


            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<PFHeaderVM, string> orderingFunction = (c =>
                sortColumnIndex == 1 && isSortable_1 ? c.Code :
                sortColumnIndex == 2 && isSortable_2 ? c.ProjectName :
                sortColumnIndex == 3 && isSortable_3 ? c.PeriodStart :
                sortColumnIndex == 4 && isSortable_4 ? c.TotalPF.ToString() :
                sortColumnIndex == 5 && isSortable_5 ? c.Post.ToString() :

                "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredData = filteredData.OrderBy(orderingFunction);
            else
                filteredData = filteredData.OrderByDescending(orderingFunction);

            var displayedCompanies = filteredData.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in displayedCompanies
                         select new[] { 
                      c.Id.ToString()
                    , c.Code
                    , c.ProjectName
                    , c.FiscalPeriod
                    , c.TotalPF.ToString()
                    , c.Post ? "Yes" : "No"
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

        public ActionResult Index_WWF(string EmployeeId = "", string fydid = "")
        {
            ViewBag.EmployeeId = EmployeeId;
            ViewBag.fydid = fydid;

            return View();
        }

        public ActionResult _index_WWF(JQueryDataTableParamModel param, string EmployeeId = "", string fydid = "", bool isWWF = false)
        {
            WPPFRepo _repo = new WPPFRepo();
            List<PFHeaderVM> getAllData = new List<PFHeaderVM>();
            IEnumerable<PFHeaderVM> filteredData;
            ShampanIdentity Identit = (ShampanIdentity)Thread.CurrentPrincipal.Identity;

            string[] conditionFields = { "pfd.EmployeeId", "pfd.FiscalYearDetailId" };
            string[] conditionValues = { EmployeeId, fydid };

            getAllData = _repo.SelectWWF(conditionFields, conditionValues);

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                // Optionally check whether the columns are searchable at all 
                var isSearchable1 = Convert.ToBoolean(Request["bSearchable_1"]);
                var isSearchable2 = Convert.ToBoolean(Request["bSearchable_2"]);
                var isSearchable3 = Convert.ToBoolean(Request["bSearchable_3"]);
                var isSearchable4 = Convert.ToBoolean(Request["bSearchable_4"]);
                var isSearchable5 = Convert.ToBoolean(Request["bSearchable_5"]);


                filteredData = getAllData
                    .Where(c =>
                          isSearchable1 && c.Code.ToLower().Contains(param.sSearch.ToLower())
                       || isSearchable2 && c.ProjectName.ToLower().Contains(param.sSearch.ToLower())
                       || isSearchable3 && c.FiscalPeriod.ToLower().Contains(param.sSearch.ToLower())
                       || isSearchable4 && c.TotalPF.ToString().ToLower().Contains(param.sSearch.ToLower())
                       || isSearchable5 && c.Post.ToString().ToLower().Contains(param.sSearch.ToLower())

                    );
            }
            else
            {
                filteredData = getAllData;
            }

            var isSortable_1 = Convert.ToBoolean(Request["bSortable_1"]);
            var isSortable_2 = Convert.ToBoolean(Request["bSortable_2"]);
            var isSortable_3 = Convert.ToBoolean(Request["bSortable_3"]);
            var isSortable_4 = Convert.ToBoolean(Request["bSortable_4"]);
            var isSortable_5 = Convert.ToBoolean(Request["bSortable_5"]);


            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<PFHeaderVM, string> orderingFunction = (c =>
                sortColumnIndex == 1 && isSortable_1 ? c.Code :
                sortColumnIndex == 2 && isSortable_2 ? c.ProjectName :
                sortColumnIndex == 3 && isSortable_3 ? c.PeriodStart :
                sortColumnIndex == 4 && isSortable_4 ? c.TotalPF.ToString() :
                sortColumnIndex == 5 && isSortable_5 ? c.Post.ToString() :

                "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredData = filteredData.OrderBy(orderingFunction);
            else
                filteredData = filteredData.OrderByDescending(orderingFunction);

            var displayedCompanies = filteredData.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in displayedCompanies
                         select new[] { 
                     //c.Id.ToString()
                      c.Code
                    , c.ProjectName
                    , c.FiscalPeriod
                    , c.TotalPF.ToString()
                    , c.Post ? "Yes" : "No"
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

        public JsonResult Post(string ids)
        {
            try
            {
                // Fetch user permission for editing the PF Header based on role and permissions
                Session["permission"] = _repoSUR.SymRoleSession(identity.UserId, "10010", "edit").ToString();

                // Check if the user has permission; if not, handle appropriately
                if (Session["permission"].ToString() == "False")
                {
                    return Json("Permission Denied", JsonRequestBehavior.AllowGet);
                }

                // Initialize the PFHeaderVM and set the Id from the provided string (split by ~)
                PFHeaderVM headerVm = new PFHeaderVM
                {
                    Id = Convert.ToInt32(ids.Split('~')[0])
                };

                // Prepare the result variable to hold the outcome of the post operation
                string[] result = _repo.PostHeader(headerVm);

                // Return the second result element (status message) as the response
                return Json(result[1], JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                return Json("Error: {ex.Message}", JsonRequestBehavior.AllowGet);
            }
        }
        
    }
}
