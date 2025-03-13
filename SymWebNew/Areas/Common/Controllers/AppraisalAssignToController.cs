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
    public class AppraisalAssignToController : Controller
    {
        //
        // GET: /Common/AppraisalAssignTo/
        SymUserRoleRepo _reposur = new SymUserRoleRepo();
        ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _index(JQueryDataTableParamModel param)
        {
            AppraisalAssignToRepo _Repo = new AppraisalAssignToRepo();

            #region Column Search
            var AssignToName = Convert.ToString(Request["sSearch_1"]);
            var Weightage = Convert.ToString(Request["sSearch_2"]);
            var IsActive = Convert.ToString(Request["sSearch_3"]);       
            #endregion Column Search

            

            #region Search and Filter Data

            var getAllData = _Repo.SelectAll();
            IEnumerable<AppraisalAssignToVM> filteredData;

            if (!string.IsNullOrEmpty(param.sSearch))
            {

                var isSearchable1 = Convert.ToBoolean(Request["bSearchable_1"]);
                var isSearchable2 = Convert.ToBoolean(Request["bSearchable_2"]);
                var isSearchable3 = Convert.ToBoolean(Request["bSearchable_3"]);
                var isSearchable4 = Convert.ToBoolean(Request["bSearchable_4"]);

                filteredData = getAllData.Where(c =>
                     isSearchable3 && c.Id.ToString().Contains(param.sSearch.ToLower())
                     || isSearchable1 && c.AssignToName.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable1 && c.Weightage.ToString().ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable2 && c.IsActive.ToString().ToLower().Contains(param.sSearch.ToLower())                       

                    );
            }
            else
            {
                filteredData = getAllData;
            }

            #endregion Search and Filter Data

            #region Column Filtering
            if (AssignToName != "" || Weightage != "")
            {
                filteredData = filteredData
                                .Where(c => (AssignToName == "" || c.AssignToName.ToLower().Contains(AssignToName.ToLower()))
                                    && (IsActive == "" || c.Weightage.ToString().ToLower().Contains(IsActive.ToLower()))
                                    && (IsActive == "" || c.IsActive.ToString().ToLower().Contains(IsActive.ToLower()))  
                                        );
            }
            #endregion Column Filtering

            var isSortable_1 = Convert.ToBoolean(Request["bSortable_1"]);
            var isSortable_2 = Convert.ToBoolean(Request["bSortable_2"]);
            var isSortable_3 = Convert.ToBoolean(Request["bSortable_3"]);
            var isSortable_4 = Convert.ToBoolean(Request["bSortable_4"]);

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<AppraisalAssignToVM, string> orderingFunction = (c =>
                sortColumnIndex == 1 && isSortable_1 ? c.Id.ToString() :
                sortColumnIndex == 2 && isSortable_2 ? c.AssignToName :
                sortColumnIndex == 3 && isSortable_3 ? c.Weightage.ToString() :
                sortColumnIndex == 4 && isSortable_4 ? c.IsActive.ToString() :             

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
                ,c.AssignToName              
                ,c.Weightage.ToString()              
                ,Convert.ToString(c.IsActive)              
                
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
            return View();
        }

        [Authorize(Roles = "Master,Admin,Account")]
        [HttpPost]
        public ActionResult Create(AppraisalAssignToVM vm)
        {

            string[] result = new string[6];
            ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
            vm.CreatedDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            vm.CreatedBy = identity.Name;
            vm.CreatedFrom = identity.WorkStationIP;
            vm.IsActive = true;
            try
            {

                result = new AppraisalAssignToRepo().Insert(vm);
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
            AppraisalAssignToRepo _appraisalAssignToRepo = new AppraisalAssignToRepo();
            AppraisalAssignToVM vm = new AppraisalAssignToVM();

            vm = _appraisalAssignToRepo.SelectById(id);
            return View(vm);
        }

        [Authorize(Roles = "Master,Admin,Account")]
        [HttpPost]
        public ActionResult Edit(AppraisalAssignToVM vm)
        {

            string[] result = new string[6];
            ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
            vm.CreatedDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            vm.CreatedBy = identity.Name;
            vm.CreatedFrom = identity.WorkStationIP;           
            try
            {

                result = new AppraisalAssignToRepo().Edit(vm);
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
        public JsonResult Delete(string ids)
        {

            Session["permission"] = _reposur.SymRoleSession(identity.UserId, "1_9", "delete").ToString();
            AppraisalAssignToVM vm = new AppraisalAssignToVM();

            string[] result = new string[6];

            vm.UpdatedBy = identity.Name;
            vm.UpdatedDate = DateTime.Now.ToString();
            vm.CreatedFrom = identity.WorkStationIP;
            vm.IsActive = false;
            string[] a = ids.Split('~');

            AppraisalAssignToRepo proRepo = new AppraisalAssignToRepo();
            result = proRepo.Delete(vm, a);
            return Json(result[1], JsonRequestBehavior.AllowGet);
        }

    }
}
