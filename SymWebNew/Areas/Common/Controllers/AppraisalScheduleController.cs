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
    public class AppraisalScheduleController : Controller
    {
        //
        // GET: /Common/AppraisalSchedule/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _index(JQueryDataTableParamModel param)
        {
            AppraisalScheduleRepo _Repo = new AppraisalScheduleRepo();

            #region Column Search
            var QuestionSetName = Convert.ToString(Request["sSearch_1"]);
            var ScheduleName = Convert.ToString(Request["sSearch_2"]);
            var StartDate = Convert.ToString(Request["sSearch_3"]);
            var EndDate = Convert.ToString(Request["sSearch_4"]);

            #endregion Column Search

            #region Search and Filter Data

            var getAllData = _Repo.SelectAll();
            IEnumerable<AppraisalScheduleVM> filteredData;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                var isSearchable1 = Convert.ToBoolean(Request["bSearchable_1"]);
                var isSearchable2 = Convert.ToBoolean(Request["bSearchable_2"]);
                var isSearchable3 = Convert.ToBoolean(Request["bSearchable_3"]);
                var isSearchable4 = Convert.ToBoolean(Request["bSearchable_4"]);

                filteredData = getAllData.Where(c =>
                     isSearchable1 && c.Id.ToString().Contains(param.sSearch.ToLower())
                     || isSearchable1 && c.QuestionSetName.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable2 && c.ScheduleName.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable3 && c.StartDate.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable4 && c.EndDate.ToLower().Contains(param.sSearch.ToLower())
                    );
            }
            else
            {
                filteredData = getAllData;
            }

            #endregion Search and Filter Data

            #region Column Filtering
            if (ScheduleName != "" || QuestionSetName != "" || StartDate != "" || EndDate != "")
            {
                filteredData = filteredData
                                .Where(c => (ScheduleName == "" || c.ScheduleName.ToLower().Contains(ScheduleName.ToLower()))
                                      && (QuestionSetName == "" || c.QuestionSetName.ToLower().Contains(QuestionSetName.ToLower()))
                                      && (StartDate == "" || c.StartDate.ToLower().Contains(StartDate.ToLower()))
                                      && (EndDate == "" || c.EndDate.ToLower().Contains(EndDate.ToLower()))
                                        );
            }
            #endregion Column Filtering

            var isSortable_1 = Convert.ToBoolean(Request["bSortable_1"]);
            var isSortable_2 = Convert.ToBoolean(Request["bSortable_2"]);
            var isSortable_3 = Convert.ToBoolean(Request["bSortable_3"]);
            var isSortable_4 = Convert.ToBoolean(Request["bSortable_4"]);
            var isSortable_5 = Convert.ToBoolean(Request["bSortable_5"]);

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<AppraisalScheduleVM, string> orderingFunction = (c =>
                sortColumnIndex == 1 && isSortable_1 ? c.Id.ToString() :
                sortColumnIndex == 2 && isSortable_2 ? c.ScheduleName :
                sortColumnIndex == 3 && isSortable_3 ? c.QuestionSetName :
                sortColumnIndex == 4 && isSortable_4 ? c.StartDate :
                sortColumnIndex == 5 && isSortable_5 ? c.EndDate :

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
                ,c.ScheduleName                  
                ,c.QuestionSetName                  
                ,c.StartDate                  
                ,c.EndDate                  
                
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

            AppraisalScheduleVM vm = new AppraisalScheduleVM();

            return View("~/Areas/Common/Views/AppraisalSchedule/Create.cshtml", vm);
        }

        [Authorize(Roles = "Master,Admin,Account")]
        [HttpPost]
        public ActionResult CreateEdit(AppraisalScheduleVM vm)
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

                result = new AppraisalScheduleRepo().Insert(vm);
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
            vm.AppraisalQuestionSetDetaiVMs = _appraisalQuestionSetRepo.SelectAllDetails(id);

            vm.Operation = "update";
            return View("~/Areas/Common/Views/AppraisalQuestionSet/Create.cshtml", vm);
        }
    }
}
