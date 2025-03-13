using CrystalDecisions.CrystalReports.Engine;
using JQueryDataTables.Models;
using Newtonsoft.Json;
using OfficeOpenXml;
using SymOrdinary;
using SymReporting.PF;
using SymRepository.Common;
using SymRepository.HRM;
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
    public class EmployeeInfoPFController : Controller
    {
        //
        // GET: /PF/EmployeeInfoPF/
        ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _index(JQueryDataTableParamModel param)
        {
            EmployeeInfoForPFRepo _Repo = new EmployeeInfoForPFRepo();

            #region Column Search
            var Id = Convert.ToString(Request["sSearch_0"]);
            var Code = Convert.ToString(Request["sSearch_1"]);
            var Name = Convert.ToString(Request["sSearch_2"]);
            var Department = Convert.ToString(Request["sSearch_3"]);
            var Designation = Convert.ToString(Request["sSearch_4"]);
            var Project = Convert.ToString(Request["sSearch_5"]);
            var Section = Convert.ToString(Request["sSearch_6"]);
            var DateOfBirth = Convert.ToString(Request["sSearch_7"]);
            var JoinDate = Convert.ToString(Request["sSearch_8"]);
            #endregion Column Search
            #region Search and Filter Data

            var getAllData = _Repo.SelectAllEmployeeInfoForPF();
            IEnumerable<EmployeeInfoForPFVM> filteredData;

            if (!string.IsNullOrEmpty(param.sSearch))
            {

                var isSearchable0 = Convert.ToBoolean(Request["bSearchable_0"]);
                var isSearchable1 = Convert.ToBoolean(Request["bSearchable_1"]);
                var isSearchable2 = Convert.ToBoolean(Request["bSearchable_2"]);
                var isSearchable3 = Convert.ToBoolean(Request["bSearchable_3"]);
                var isSearchable4 = Convert.ToBoolean(Request["bSearchable_4"]);
                var isSearchable5 = Convert.ToBoolean(Request["bSearchable_5"]);
                var isSearchable6 = Convert.ToBoolean(Request["bSearchable_6"]);
                var isSearchable7 = Convert.ToBoolean(Request["bSearchable_7"]);
                var isSearchable8 = Convert.ToBoolean(Request["bSearchable_8"]);
                filteredData = getAllData.Where(c =>
                        isSearchable0 && c.Id.ToString().Contains(param.sSearch.ToLower())
                     || isSearchable1 && c.Code.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable2 && c.Name.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable1 && c.Department.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable2 && c.Designation.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable3 && c.Project.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable4 && c.Section.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable5 && c.DateOfBirth.ToString().ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable6 && c.JoinDate.ToString().ToLower().Contains(param.sSearch.ToLower())
                    );
            }
            else
            {
                filteredData = getAllData;
            }

            #endregion Search and Filter Data

            #region Column Filtering
            if (Code != "" || Name != "" || Department != "" || Designation != "" || Project != "" || Section != "" || DateOfBirth != "" || JoinDate != "")
            {
                filteredData = filteredData
                                .Where(c => (Code == "" || c.Code.ToLower().Contains(Code.ToLower()))
                                            && (Name == "" || c.Name.ToLower().Contains(Name.ToLower()))
                                            && (Department == "" || c.Department.ToLower().Contains(Department.ToLower()))
                                            && (Designation == "" || c.Designation.ToLower().Contains(Designation.ToLower()))
                                            && (Project == "" || c.Project.ToLower().Contains(Project.ToLower()))
                                            && (Section == "" || c.Section.ToLower().Contains(Section.ToLower()))
                                            && (DateOfBirth == "" || c.DateOfBirth.ToString().ToLower().Contains(DateOfBirth.ToLower()))
                                            && (JoinDate == "" || c.JoinDate.ToString().ToLower().Contains(JoinDate.ToLower()))
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
            var isSortable_8 = Convert.ToBoolean(Request["bSortable_8"]);

            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<EmployeeInfoForPFVM, string> orderingFunction = (c =>
                sortColumnIndex == 0 && isSortable_0 ? c.Id.ToString() :
                sortColumnIndex == 1 && isSortable_1 ? c.Code :
                sortColumnIndex == 2 && isSortable_2 ? c.Name :
                   sortColumnIndex == 3 && isSortable_3 ? c.Department :
                sortColumnIndex == 4 && isSortable_4 ? c.Designation :
                sortColumnIndex == 5 && isSortable_5 ? c.Project :
                sortColumnIndex == 6 && isSortable_6 ? c.Section :
                sortColumnIndex == 7 && isSortable_7 ? c.DateOfBirth.ToString() :
                sortColumnIndex == 8 && isSortable_8 ? c.JoinDate.ToString() :
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
                ,c.Code              
                ,c.Name
                ,c.Department             
                ,c.Designation   
                ,c.Project  
                ,c.Section  
                ,c.DateOfBirth  
                ,c.JoinDate                                                                 
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
        public ActionResult Create(EmployeeInfoForPFVM vm)
        {
            return View("Create", vm);
        }
        public ActionResult CreateEdit(EmployeeInfoForPFVM vm)
        {

            string[] result = new string[6];
            vm.CreatedAt = DateTime.Now.ToString("yyyyMMddHHmmss");
            vm.CreatedBy = identity.Name;
            vm.CreatedFrom = identity.WorkStationIP;
            vm.IsActive = true;
            try
            {

                result = new EmployeeInfoForPFRepo().InsertEmployeeInfoForPF(vm);
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
        
        public ActionResult Import()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            EmployeeInfoForPFRepo _Repo = new EmployeeInfoForPFRepo();
            EmployeeInfoForPFVM vm = new EmployeeInfoForPFVM();

            vm = _Repo.SelectById(id);
            return View(vm);
        }
        public ActionResult Delete(int Id)
        {
            string[] result = new string[6];
            EmployeeInfoForPFRepo _Repo = new EmployeeInfoForPFRepo();
            EmployeeInfoForPFVM vm = new EmployeeInfoForPFVM();

            result = _Repo.DeleteEmployeeInfoForPF(Id);
            Session["result"] = result[0] + "~" + result[1];
            return View("Index");
        }

   
    }
}
