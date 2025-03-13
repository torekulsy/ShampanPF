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
    public class RecruitmentRequisitionController : Controller
    {
        
        // GET: /HRM/RecruitmentRequisition/
        ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _index(JQueryDataTableParamModel param)
        {
            RecruitmentRequisitionRepo _Repo = new RecruitmentRequisitionRepo();

            #region Column Search
            var Id = Convert.ToString(Request["sSearch_0"]);
            var Department = Convert.ToString(Request["sSearch_1"]);
            var Designation = Convert.ToString(Request["sSearch_2"]);
            var Experience = Convert.ToString(Request["sSearch_3"]);
            var Deadline = Convert.ToString(Request["sSearch_4"]);         
            var IsActive = Convert.ToString(Request["sSearch_5"]);
            var IsApproved = Convert.ToString(Request["sSearch_6"]);          
            #endregion Column Search
            

            #region Search and Filter Data

            var getAllData = _Repo.SelectAllRecruitmentRequisition();
            IEnumerable<RecruitmentRequisitionVM> filteredData;

            if (!string.IsNullOrEmpty(param.sSearch))
            {

                var isSearchable0 = Convert.ToBoolean(Request["bSearchable_0"]);
                var isSearchable1 = Convert.ToBoolean(Request["bSearchable_1"]);
                var isSearchable2 = Convert.ToBoolean(Request["bSearchable_2"]);
                var isSearchable3 = Convert.ToBoolean(Request["bSearchable_3"]);
                var isSearchable4 = Convert.ToBoolean(Request["bSearchable_4"]);
                var isSearchable5 = Convert.ToBoolean(Request["bSearchable_5"]);
                var isSearchable6 = Convert.ToBoolean(Request["bSearchable_6"]);             
          
                filteredData = getAllData.Where(c =>
                        isSearchable0 && c.Id.ToString().Contains(param.sSearch.ToLower())
                     || isSearchable1 && c.Department.ToLower().Contains(param.sSearch.ToLower())                 
                     || isSearchable2 && c.Designation.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable3 && c.Experience.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable4 && c.Deadline.ToLower().Contains(param.sSearch.ToLower())                    
                     || isSearchable5 && c.IsActive.ToString().ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable6 && c.IsApproved.ToString().ToLower().Contains(param.sSearch.ToLower())                
                    );
            }
            else
            {
                filteredData = getAllData;
            }

            #endregion Search and Filter Data

            #region Column Filtering
            if (Department != "" || Designation != "" || Experience != "" || Deadline != "" )
            {
                filteredData = filteredData
                                .Where(c => (Department == "" || c.Department.ToLower().Contains(Department.ToLower()))                                        
                                            && (Designation == "" || c.Designation.ToLower().Contains(Designation.ToLower()))
                                            && (Experience == "" || c.Experience.ToLower().Contains(Experience.ToLower()))
                                            && (Deadline == "" || c.Deadline.ToLower().Contains(Deadline.ToLower()))             
                                            && (IsActive == "" || c.IsActive.ToString().ToLower().Contains(IsActive.ToLower()))
                                            && (IsApproved == "" || c.IsApproved.ToString().ToLower().Contains(IsApproved.ToLower()))                                           
                                        );
            }
            #endregion Column Filtering

            var isSortable_0 = Convert.ToBoolean(Request["bSortable_0"]);
            var isSortable_1 = Convert.ToBoolean(Request["bSortable_1"]);
            var isSortable_2 = Convert.ToBoolean(Request["bSortable_2"]);
            var isSortable_3 = Convert.ToBoolean(Request["bSortable_3"]);
            var isSortable_4 = Convert.ToBoolean(Request["bSortable_4"]);
            var isSortable_5 = Convert.ToBoolean(Request["bSortable_5"]);
            var isSortable_6 = Convert.ToBoolean(Request["bSortable_6"]);
            var isSortable_7 = Convert.ToBoolean(Request["bSortable_7"]);          
  
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<RecruitmentRequisitionVM, string> orderingFunction = (c =>
                sortColumnIndex == 0 && isSortable_0 ? c.Id.ToString() :
                sortColumnIndex == 1 && isSortable_1 ? c.Department :
                sortColumnIndex == 2 && isSortable_2 ? c.Designation :
                sortColumnIndex == 3 && isSortable_3 ? c.Experience :
                sortColumnIndex == 4 && isSortable_4 ? c.Deadline :            
                sortColumnIndex == 5 && isSortable_5 ? c.IsActive.ToString() :
                sortColumnIndex == 6 && isSortable_6 ? c.IsApproved.ToString() :
                "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "desc")
                filteredData = filteredData.OrderBy(orderingFunction);
            else
                filteredData = filteredData.OrderByDescending(orderingFunction);

            var displayedCompanies = filteredData.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in displayedCompanies
                         select new[] { 
                Convert.ToString(c.Id)               
                ,c.DepartmentName              
                ,c.DesignationName
                ,c.Experience             
                ,c.Deadline   
                ,c.IsActive.ToString()                            
                ,c.IsApproved.ToString()                            
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

        [HttpGet]
        public ActionResult Create(RecruitmentRequisitionVM vm)
        {

            return View("Create", vm);
        }

        public ActionResult CreateEdit(RecruitmentRequisitionVM vm)
        {

            string[] result = new string[6];
            vm.CreatedAt = DateTime.Now.ToString("yyyyMMddHHmmss");
            vm.CreatedBy = identity.Name;
            vm.CreatedFrom = identity.WorkStationIP;
            vm.IsActive = true;
            try
            {

                result = new RecruitmentRequisitionRepo().InsertRecruitmentRequisition(vm);
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

        public ActionResult Edit(int id)
        {
            RecruitmentRequisitionRepo _Repo = new RecruitmentRequisitionRepo();
            RecruitmentRequisitionVM vm = new RecruitmentRequisitionVM();

            vm = _Repo.SelectById(id);
            return View(vm);
        }      
        public ActionResult Approved(int Id,bool isChecked)
        {


            return RedirectToAction("Index");
        }
        public ActionResult Approve(int id)
        {
            RecruitmentRequisitionRepo _Repo = new RecruitmentRequisitionRepo();
            string[] result = new string[6];

            result = _Repo.ApprovedRecruitmentRequisition(id);
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int Id)
        {
            string[] result = new string[6];
            RecruitmentRequisitionRepo _Repo = new RecruitmentRequisitionRepo();
            RecruitmentRequisitionVM vm = new RecruitmentRequisitionVM();

            result = _Repo.DeleteRecruitmentRequisition(Id);
            Session["result"] = result[0] + "~" + result[1];
            return View("Index");
        }
    }
}
