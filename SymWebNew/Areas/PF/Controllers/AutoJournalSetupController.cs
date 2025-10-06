﻿using JQueryDataTables.Models;
using SymOrdinary;
using SymRepository.Common;
using SymRepository.PF;
using SymViewModel.PF;
using SymWebUI.Areas.PF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SymWebUI.Areas.PF.Controllers
{
    public class AutoJournalSetupController : Controller
    {
        public AutoJournalSetupController()
        {
            ViewBag.TransType = AreaTypePFVM.TransType;
        }

        SymUserRoleRepo _repoSUR = new SymUserRoleRepo();
        ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
        AutoJournalSetupRepo _repo = new AutoJournalSetupRepo();

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

        //public ActionResult _index(JQueryDataTableParamModel param)
        //{

        //    #region Search and Filter Data
        //    var getAllData = _repo.SelectAll();
        //    IEnumerable<AutoJournalSetupVM> filteredData;
        //    if (!string.IsNullOrEmpty(param.sSearch))
        //    {
        //        //Optionally check whether the columns are searchable at all 
        //        var isSearchable1 = Convert.ToBoolean(Request["bSearchable_1"]);
        //        var isSearchable2 = Convert.ToBoolean(Request["bSearchable_2"]);
        //        var isSearchable3 = Convert.ToBoolean(Request["bSearchable_3"]);
        //        var isSearchable4 = Convert.ToBoolean(Request["bSearchable_4"]);
        //        var isSearchable5 = Convert.ToBoolean(Request["bSearchable_5"]);              
        //        filteredData = getAllData
        //            .Where(c =>
        //                  isSearchable1 && c.JournalFor.ToLower().Contains(param.sSearch.ToLower())
        //               || isSearchable2 && c.JournalName.ToLower().Contains(param.sSearch.ToLower())
        //               || isSearchable3 && c.Nature.ToLower().Contains(param.sSearch.ToLower())
        //               || isSearchable4 && c.COAName.ToString().ToLower().Contains(param.sSearch.ToLower())
        //               || isSearchable5 && c.IsActive.ToString().ToLower().Contains(param.sSearch.ToLower())
                     
        //            );
        //    }
        //    else
        //    {
        //        filteredData = getAllData;
        //    }

        //    #endregion Search and Filter Data
        //    var isSortable_1 = Convert.ToBoolean(Request["bSortable_1"]);
        //    var isSortable_2 = Convert.ToBoolean(Request["bSortable_2"]);
        //    var isSortable_3 = Convert.ToBoolean(Request["bSortable_3"]);
        //    var isSortable_4 = Convert.ToBoolean(Request["bSortable_4"]);
        //    var isSortable_5 = Convert.ToBoolean(Request["bSortable_5"]);
         
        //    var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
        //    Func<AutoJournalSetupVM, string> orderingFunction = (c =>
        //        sortColumnIndex == 1 && isSortable_1 ? c.JournalFor :
        //        sortColumnIndex == 2 && isSortable_2 ? c.JournalName :
        //        sortColumnIndex == 3 && isSortable_3 ? c.Nature :
        //        sortColumnIndex == 4 && isSortable_4 ? c.COAName.ToString() :
        //        sortColumnIndex == 5 && isSortable_5 ? c.IsActive.ToString() :
              
        //        "");
        //    var sortDirection = Request["sSortDir_0"]; // asc or desc
        //    if (sortDirection == "asc")
        //        filteredData = filteredData.OrderBy(orderingFunction);
        //    else
        //        filteredData = filteredData.OrderByDescending(orderingFunction);
        //    var displayedCompanies = filteredData.Skip(param.iDisplayStart).Take(param.iDisplayLength);
        //    var result = from c in displayedCompanies
        //                 select new[] { 
        //        Convert.ToString(c.Id)
        //        , c.JournalFor
        //        , c.JournalName
        //        , c.Nature
        //        , c.COAName.ToString()  
        //        , c.IsActive ? "Yes" : "No"
     
        //    };
        //    return Json(new
        //    {
        //        sEcho = param.sEcho,
        //        iTotalRecords = getAllData.Count(),
        //        iTotalDisplayRecords = filteredData.Count(),
        //        aaData = result
        //    },
        //                JsonRequestBehavior.AllowGet);
        //}

        public ActionResult _index(JQueryDataTableParamModel param)
        {
            var getAllData = _repo.SelectAll();
            IEnumerable<AutoJournalSetupVM> filteredData;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredData = getAllData.Where(c =>
                    c.JournalFor.ToLower().Contains(param.sSearch.ToLower()) ||
                    c.JournalName.ToLower().Contains(param.sSearch.ToLower()) ||
                    c.Nature.ToLower().Contains(param.sSearch.ToLower()) ||
                    c.COAName.ToString().ToLower().Contains(param.sSearch.ToLower()) ||
                    c.IsActive.ToString().ToLower().Contains(param.sSearch.ToLower())
                );
            }
            else
            {
                filteredData = getAllData;
            }

            var displayedCompanies = filteredData
                .Skip(param.iDisplayStart)
                .Take(param.iDisplayLength);

            var result = from c in displayedCompanies
                         select new[]
                 {
                     Convert.ToString(c.Id),
                     c.JournalFor,
                     c.JournalName,
                     c.Nature,
                     c.COAName.ToString(),
                     c.IsActive ? "Yes" : "No"
                 };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = getAllData.Count(),
                iTotalDisplayRecords = filteredData.Count(),
                aaData = result
            }, JsonRequestBehavior.AllowGet);
        }



        [Authorize(Roles = "Admin")]
        public ActionResult Create(AutoJournalSetupVM vm)
        {
            Session["permission"] = _repoSUR.SymRoleSession(identity.UserId, "10003", "add").ToString();

            vm.Operation = "add";
            return View(vm);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreateEdit(AutoJournalSetupVM vm)
        {
            string[] result = new string[6];
            ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
            try
            {
                if (vm.Operation.ToLower() == "add")
                {
                    vm.CreatedAt = DateTime.Now.ToString("yyyyMMddHHmmss");
                    vm.CreatedBy = identity.Name;
                    vm.CreatedFrom = identity.WorkStationIP;
                    vm.BranchId = "1";
                    result = _repo.Insert(vm);
                    Session["result"] = result[0] + "~" + result[1];
                    if (result[0].ToLower() == "success")
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View("Create", vm);
                    }
                }
                else if (vm.Operation.ToLower() == "update")
                {
                    vm.LastUpdateAt = DateTime.Now.ToString("yyyyMMddHHmmss");
                    vm.LastUpdateBy = identity.Name;
                    vm.LastUpdateFrom = identity.WorkStationIP;
                    result = _repo.Update(vm);
                    Session["result"] = result[0] + "~" + result[1];
                    return RedirectToAction("Index", new { id = result[2] });
                }
                else
                {
                    return View("Create", vm);
                }
            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Succeessfully!";
                FileLogger.Log(result[0].ToString() + Environment.NewLine + result[2].ToString() + Environment.NewLine + result[5].ToString(), this.GetType().Name, result[4].ToString() + Environment.NewLine + result[3].ToString());
                return View("Create", vm);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
            Session["permission"] = _repoSUR.SymRoleSession(identity.UserId, "1_5", "edit").ToString();
            AutoJournalSetupVM vm = new AutoJournalSetupVM();
            vm = _repo.SelectById(id);
            vm.Operation = "Update";
            return View("Create",vm);
        }

        [Authorize(Roles = "Master,Admin,Account")]
        public JsonResult Delete(string ids)
        {
            AutoJournalSetupVM vm = new AutoJournalSetupVM();

            ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
            Session["permission"] = _repoSUR.SymRoleSession(identity.UserId, "1_5", "delete").ToString();
            string[] a = ids.Split('~');
            string[] result = new string[6];

            vm.LastUpdateAt = DateTime.Now.ToString("yyyyMMddHHmmss");
            vm.LastUpdateBy = identity.Name;
            vm.LastUpdateFrom = identity.WorkStationIP;
            result = _repo.Delete(a);
            return Json(result[1], JsonRequestBehavior.AllowGet);
        }

    }
}
