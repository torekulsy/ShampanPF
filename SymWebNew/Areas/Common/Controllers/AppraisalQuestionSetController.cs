﻿using JQueryDataTables.Models;
using SymOrdinary;
using SymRepository.Common;
using SymViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SymWebUI.Areas.Common.Controllers
{
    public class AppraisalQuestionSetController : Controller
    {
        //
        // GET: /Common/AppraisalQuestionSet/

        SymUserRoleRepo _reposur = new SymUserRoleRepo();
        ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _index(JQueryDataTableParamModel param)
        {
            AppraisalQuestionSetRepo _Repo = new AppraisalQuestionSetRepo();

            #region Column Search
            var QuestionSetName = Convert.ToString(Request["sSearch_1"]);
            var DepartmentName = Convert.ToString(Request["sSearch_2"]);
            var CreateDate = Convert.ToString(Request["sSearch_3"]);

            #endregion Column Search

            #region Search and Filter Data

            var getAllData = _Repo.SelectAll();
            IEnumerable<AppraisalQuestionSetVM> filteredData;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                var isSearchable1 = Convert.ToBoolean(Request["bSearchable_1"]);
                var isSearchable2 = Convert.ToBoolean(Request["bSearchable_2"]);
                var isSearchable3 = Convert.ToBoolean(Request["bSearchable_3"]);

                filteredData = getAllData.Where(c =>
                     isSearchable1 && c.Id.ToString().Contains(param.sSearch.ToLower())
                     || isSearchable1 && c.QuestionSetName.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable2 && c.DepartmentName.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable3 && c.CreateDate.ToLower().Contains(param.sSearch.ToLower())
                    );
            }
            else
            {
                filteredData = getAllData;
            }

            #endregion Search and Filter Data

            #region Column Filtering
            if (QuestionSetName != "" || DepartmentName != "" || CreateDate != "")
            {
                filteredData = filteredData
                                .Where(c => (QuestionSetName == "" || c.QuestionSetName.ToLower().Contains(QuestionSetName.ToLower()))
                                      && (DepartmentName == "" || c.DepartmentName.ToLower().Contains(DepartmentName.ToLower()))
                                      && (CreateDate == "" || c.CreateDate.ToLower().Contains(CreateDate.ToLower()))
                                        );
            }
            #endregion Column Filtering

            var isSortable_1 = Convert.ToBoolean(Request["bSortable_1"]);
            var isSortable_2 = Convert.ToBoolean(Request["bSortable_2"]);
            var isSortable_3 = Convert.ToBoolean(Request["bSortable_3"]);
            var isSortable_4 = Convert.ToBoolean(Request["bSortable_4"]);

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<AppraisalQuestionSetVM, string> orderingFunction = (c =>
                sortColumnIndex == 1 && isSortable_1 ? c.Id.ToString() :
                sortColumnIndex == 2 && isSortable_2 ? c.QuestionSetName :
                sortColumnIndex == 3 && isSortable_3 ? c.DepartmentName :
                sortColumnIndex == 4 && isSortable_4 ? c.CreateDate :

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
                ,c.QuestionSetName     
                ,c.DepartmentName                  
                ,c.CreateDate                  
                
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
        public ActionResult Create()
        {

            AppraisalQuestionSetVM vm = new AppraisalQuestionSetVM();

            return View("~/Areas/Common/Views/AppraisalQuestionSet/Create.cshtml", vm);
        }

        [Authorize(Roles = "Master,Admin,Account")]
        [HttpPost]
        public ActionResult CreateEdit(AppraisalQuestionSetVM vm)
        {

            string[] result = new string[6];
            ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
            vm.CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            vm.CreateBy = identity.Name;
            vm.UpdateBy = identity.Name;
            vm.UpdateDate = DateTime.Now.ToString();
            vm.CreateFrom = identity.WorkStationIP;
            try
            {

                result = new AppraisalQuestionSetRepo().Insert(vm);
                Session["result"] = result[0] + "~" + result[1];
                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Succeessfully!";
                FileLogger.Log(result[0].ToString() + Environment.NewLine + result[2].ToString() + Environment.NewLine + result[5].ToString(), this.GetType().Name, result[4].ToString() + Environment.NewLine + result[3].ToString());
                return View(vm);
            }
        }

        [Authorize(Roles = "Master,Admin,Account")]
        [HttpGet]
        public ActionResult Edit(int id)
        {            
            AppraisalQuestionSetRepo _appraisalQuestionSetRepo = new AppraisalQuestionSetRepo();
            AppraisalQuestionSetVM vm = new AppraisalQuestionSetVM();

            vm = _appraisalQuestionSetRepo.SelectAll(Convert.ToInt32(id)).FirstOrDefault();

            SelectQuestionByDepartment(vm.DepartmentId);
        
            return View("~/Areas/Common/Views/AppraisalQuestionSet/Create.cshtml", vm);
        }
       
        [Authorize(Roles = "Master,Admin,Account")]
        [HttpGet]
        public ActionResult SelectQuestionByDepartment(string DepartmentId)
        {
            AppraisalQuestionSetVM vm = new AppraisalQuestionSetVM();

            try
            {              
                AppraisalQuestionSetRepo _appraisalQuestionSetRepo = new AppraisalQuestionSetRepo();

                //vm.appraisalCheckBoxVM = _appraisalQuestionSetRepo.SelectApprisalCheckBox();

                vm.AppraisalQuestionSetDetaiVMs = _appraisalQuestionSetRepo.SelectAllQuestionByDepartmentExist(DepartmentId);
                if (!vm.AppraisalQuestionSetDetaiVMs.Any())
                {
                    vm.AppraisalQuestionSetDetaiVMs = _appraisalQuestionSetRepo.SelectAllQuestionByDepartment(DepartmentId);
                }               

                return PartialView("~/Areas/Common/Views/AppraisalQuestionSet/_details.cshtml", vm);
            }
            catch (Exception ex)
            {
                Session["result"] = "Fail~" + ex.Message;
                return PartialView("~/Areas/Common/Views/AppraisalQuestionSet/_details.cshtml", vm);
            }
        }
               
        public ActionResult BlankItem(AppraisalQuestionSetDetailVM vm)
        {
            string[] result = new string[6];
            ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
            try
            {
                return PartialView("~/Areas/Common/Views/AppraisalQuestionSet/_details.cshtml", vm);
            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Succeessfully!";
                FileLogger.Log(result[0].ToString() + Environment.NewLine + result[2].ToString() + Environment.NewLine + result[5].ToString(), this.GetType().Name, result[4].ToString() + Environment.NewLine + result[3].ToString());
                return PartialView("~/Areas/Common/Views/AppraisalQuestionSet/_details.cshtml", vm);
            }
        }

        public ActionResult Delete(int Id)
        {
            string[] result = new string[6];
            AppraisalQuestionSetRepo _Repo = new AppraisalQuestionSetRepo();
            AppraisalQuestionSetDetailVM vm = new AppraisalQuestionSetDetailVM();

            result = _Repo.DeleteAppraisalQuestionSet(Id);
            Session["result"] = result[0] + "~" + result[1];
            return RedirectToAction("Index");
        }
        public ActionResult Index2()
        {
            return View();
        }

        public ActionResult _index2(JQueryDataTableParamModel param)
        {
            AppraisalQuestionSetRepo _Repo = new AppraisalQuestionSetRepo();
            #region Column Search
            var CategoryName = Convert.ToString(Request["sSearch_1"]);
            var QuestionName = Convert.ToString(Request["sSearch_2"]);
            #endregion Column Search
            #region Search and Filter Data

            var getAllData = _Repo.SelectAll2();
            IEnumerable<AppraisalQuestionSetDetailVM> filteredData;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                var isSearchable1 = Convert.ToBoolean(Request["bSearchable_1"]);
                var isSearchable2 = Convert.ToBoolean(Request["bSearchable_2"]);
                var isSearchable3 = Convert.ToBoolean(Request["bSearchable_3"]);

                filteredData = getAllData.Where(c =>
                     isSearchable1 && c.Id.ToString().Contains(param.sSearch.ToLower())
                     || isSearchable1 && c.CategoryName.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable2 && c.QuestionName.ToLower().Contains(param.sSearch.ToLower())
                    );
            }
            else
            {
                filteredData = getAllData;
            }

            #endregion Search and Filter Data

            #region Column Filtering
            if (CategoryName != null || QuestionName != null)
            {
                filteredData = filteredData
                                .Where(c => (CategoryName == "" || c.CategoryName.ToLower().Contains(CategoryName.ToLower()))
                                      && (QuestionName == "" || c.QuestionName.ToLower().Contains(QuestionName.ToLower()))
                                        );
            }
            #endregion Column Filtering

            var isSortable_1 = Convert.ToBoolean(Request["bSortable_1"]);
            var isSortable_2 = Convert.ToBoolean(Request["bSortable_2"]);
            var isSortable_3 = Convert.ToBoolean(Request["bSortable_3"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<AppraisalQuestionSetDetailVM, string> orderingFunction = (c =>
                sortColumnIndex == 1 && isSortable_1 ? c.Id.ToString() :
                sortColumnIndex == 2 && isSortable_2 ? c.CategoryName :
                sortColumnIndex == 3 && isSortable_3 ? c.QuestionName :
                "");

            var displayedCompanies = filteredData.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in displayedCompanies
                         select new[] { 
                Convert.ToString(c.Id)
                    ,c.CategoryName     
                    ,c.QuestionName 
                    ,c.IsOwn.ToString()
                      ,c.IsTeamLead.ToString()
                      ,c.IsHR.ToString()
                      ,c.IsCOO.ToString()
                      ,c.IsMD.ToString()
                      ,c.IsP1.ToString()
                      ,c.IsP2.ToString()
                      ,c.IsP3.ToString()
                      ,c.IsP4.ToString()
                      ,c.IsP5.ToString()         
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

        public ActionResult SelectQuestionByDepartmentForEmploye(JQueryDataTableParamModel param, string DepartmentId)
        {
            AppraisalQuestionSetRepo _Repo = new AppraisalQuestionSetRepo();
            #region Column Search
            var CategoryName = Convert.ToString(Request["sSearch_1"]);
            var QuestionName = Convert.ToString(Request["sSearch_2"]);
            #endregion Column Search
            #region Search and Filter Data

            var getAllData = _Repo.SelectAllByDepartmentId(DepartmentId);
            IEnumerable<AppraisalQuestionSetDetailVM> filteredData;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                var isSearchable1 = Convert.ToBoolean(Request["bSearchable_1"]);
                var isSearchable2 = Convert.ToBoolean(Request["bSearchable_2"]);
                var isSearchable3 = Convert.ToBoolean(Request["bSearchable_3"]);

                filteredData = getAllData.Where(c =>
                     isSearchable1 && c.Id.ToString().Contains(param.sSearch.ToLower())
                     || isSearchable1 && c.CategoryName.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable2 && c.QuestionName.ToLower().Contains(param.sSearch.ToLower())
                    );
            }
            else
            {
                filteredData = getAllData;
            }

            #endregion Search and Filter Data

            #region Column Filtering
            if (CategoryName != null || QuestionName != null)
            {
                filteredData = filteredData
                                .Where(c => (CategoryName == "" || c.CategoryName.ToLower().Contains(CategoryName.ToLower()))
                                      && (QuestionName == "" || c.QuestionName.ToLower().Contains(QuestionName.ToLower()))
                                        );
            }
            #endregion Column Filtering

            var isSortable_1 = Convert.ToBoolean(Request["bSortable_1"]);
            var isSortable_2 = Convert.ToBoolean(Request["bSortable_2"]);
            var isSortable_3 = Convert.ToBoolean(Request["bSortable_3"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<AppraisalQuestionSetDetailVM, string> orderingFunction = (c =>
                sortColumnIndex == 1 && isSortable_1 ? c.Id.ToString() :
                sortColumnIndex == 2 && isSortable_2 ? c.CategoryName :
                sortColumnIndex == 3 && isSortable_3 ? c.QuestionName :
                "");

            var displayedCompanies = filteredData.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in displayedCompanies
                         select new[] { 
                Convert.ToString(c.Id)
                    ,c.CategoryName     
                    ,c.QuestionName 
                    ,c.IsOwn.ToString()
                      ,c.IsTeamLead.ToString()
                      ,c.IsHR.ToString()
                      ,c.IsCOO.ToString()
                      ,c.IsMD.ToString()
                      ,c.IsP1.ToString()
                      ,c.IsP2.ToString()
                      ,c.IsP3.ToString()
                      ,c.IsP4.ToString()
                      ,c.IsP5.ToString()         
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
    }
}
