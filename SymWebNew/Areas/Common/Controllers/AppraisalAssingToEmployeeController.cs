using JQueryDataTables.Models;
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
    public class AppraisalAssingToEmployeeController : Controller
    {
        //
        // GET: /Common/AppraisalAssingToEmployee/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _index(JQueryDataTableParamModel param)
        {
            AppraisalAssingToEmployeeRepo _Repo = new AppraisalAssingToEmployeeRepo();

            #region Column Search
            var QuestionSetName = Convert.ToString(Request["sSearch_1"]);
            var EmployeeCode = Convert.ToString(Request["sSearch_2"]);
            var DepartmentName = Convert.ToString(Request["sSearch_3"]);
            var CreateDate = Convert.ToString(Request["sSearch_4"]);

            #endregion Column Search

            #region Search and Filter Data

            var getAllData = _Repo.SelectAll();
            IEnumerable<AppraisalQuestionSetVM> filteredData;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                var isSearchable1 = Convert.ToBoolean(Request["bSearchable_1"]);
                var isSearchable2 = Convert.ToBoolean(Request["bSearchable_2"]);
                var isSearchable3 = Convert.ToBoolean(Request["bSearchable_3"]);
                var isSearchable4 = Convert.ToBoolean(Request["bSearchable_4"]);

                filteredData = getAllData.Where(c =>
                     isSearchable1 && c.Id.ToString().Contains(param.sSearch.ToLower())
                     || isSearchable1 && c.QuestionSetName.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable2 && c.EmployeeCode.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable3 && c.DepartmentName.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable4 && c.CreateDate.ToLower().Contains(param.sSearch.ToLower())
                    );
            }
            else
            {
                filteredData = getAllData;
            }

            #endregion Search and Filter Data

            #region Column Filtering
            if (QuestionSetName != "" || DepartmentName != "" || CreateDate != "" || EmployeeCode !="")
            {
                filteredData = filteredData
                                .Where(c => (QuestionSetName == "" || c.QuestionSetName.ToLower().Contains(QuestionSetName.ToLower()))
                                      && (EmployeeCode == "" || c.EmployeeCode.ToLower().Contains(EmployeeCode.ToLower()))
                                      && (DepartmentName == "" || c.DepartmentName.ToLower().Contains(DepartmentName.ToLower()))
                                      && (CreateDate == "" || c.CreateDate.ToLower().Contains(CreateDate.ToLower()))
                                        );
            }
            #endregion Column Filtering

            var isSortable_1 = Convert.ToBoolean(Request["bSortable_1"]);
            var isSortable_2 = Convert.ToBoolean(Request["bSortable_2"]);
            var isSortable_3 = Convert.ToBoolean(Request["bSortable_3"]);
            var isSortable_4 = Convert.ToBoolean(Request["bSortable_4"]);
            var isSortable_5 = Convert.ToBoolean(Request["bSortable_5"]);

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<AppraisalQuestionSetVM, string> orderingFunction = (c =>
                sortColumnIndex == 1 && isSortable_1 ? c.Id.ToString() :
                sortColumnIndex == 2 && isSortable_2 ? c.QuestionSetName :
                sortColumnIndex == 3 && isSortable_3 ? c.EmployeeCode :
                sortColumnIndex == 4 && isSortable_4 ? c.DepartmentName :
                sortColumnIndex == 5 && isSortable_5 ? c.CreateDate :

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
                ,c.EmployeeCode                  
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

            return View("~/Areas/Common/Views/AppraisalAssingToEmployee/Create.cshtml", vm);
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

                result = new AppraisalAssingToEmployeeRepo().Insert(vm);
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
            AppraisalAssingToEmployeeRepo _appraisalAssingToEmployeeRepo = new AppraisalAssingToEmployeeRepo();
            AppraisalQuestionSetVM vm = new AppraisalQuestionSetVM();

            vm = _appraisalAssingToEmployeeRepo.SelectAll(Convert.ToInt32(id)).FirstOrDefault();
         
            return View("~/Areas/Common/Views/AppraisalAssingToEmployee/Create.cshtml", vm);
        }


        [Authorize(Roles = "Master,Admin,Account")]
        [HttpGet]
        public ActionResult SelectQuestionByEmployee(string DepartmentId, string EmployeeCode, string EvaluationFor)
        {
            AppraisalQuestionSetVM vm = new AppraisalQuestionSetVM();
            try
            {
                AppraisalAssingToEmployeeRepo _appraisalAssingToEmployeeRepo = new AppraisalAssingToEmployeeRepo();

                vm.AppraisalQuestionSetDetaiVMs = _appraisalAssingToEmployeeRepo.SelectAllQuestionByEmployeeExist(DepartmentId, EmployeeCode, EvaluationFor);
                if (!vm.AppraisalQuestionSetDetaiVMs.Any())
                {
                    vm.AppraisalQuestionSetDetaiVMs = _appraisalAssingToEmployeeRepo.SelectAllQuestionByDepartment(DepartmentId);
                }

                return PartialView("~/Areas/Common/Views/AppraisalAssingToEmployee/_details.cshtml", vm);
            }
            catch (Exception ex)
            {
                Session["result"] = "Fail~" + ex.Message;
                return PartialView("~/Areas/Common/Views/AppraisalAssingToEmployee/_details.cshtml", vm);
            }
        }

        public ActionResult BlankItem(AppraisalQuestionSetDetailVM vm)
        {
            string[] result = new string[6];
            ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
            try
            {
                return PartialView("~/Areas/Common/Views/AppraisalAssingToEmployee/_details.cshtml", vm);
            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Succeessfully!";
                FileLogger.Log(result[0].ToString() + Environment.NewLine + result[2].ToString() + Environment.NewLine + result[5].ToString(), this.GetType().Name, result[4].ToString() + Environment.NewLine + result[3].ToString());
                return PartialView("~/Areas/Common/Views/AppraisalAssingToEmployee/_details.cshtml", vm);
            }
        }

        [Authorize(Roles = "Master,Admin,Account")]
        public JsonResult Delete(string[] ids)
        {
            AppraisalCategoryVM vm = new AppraisalCategoryVM();
            string[] result = new string[6];

            
            for (int i = 0; i < ids.Length; i++)
            {
                if (ids[i].EndsWith("~"))
                {
                    ids[i] = ids[i].Substring(0, ids[i].Length - 1);
                }
            }
            vm.Id = Convert.ToInt32(ids[0]);

            AppraisalAssingToEmployeeRepo proRepo = new AppraisalAssingToEmployeeRepo();
              result = proRepo.Delete(vm);
            return Json(result[1], JsonRequestBehavior.AllowGet);

        }
    }

}
