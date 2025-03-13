using CrystalDecisions.CrystalReports.Engine;
using JQueryDataTables.Models;
using SymOrdinary;
using SymRepository.Common;
using SymRepository.HRM;
using SymRepository.Payroll;
using SymServices.Common;
using SymViewModel.Common;
using SymViewModel.Payroll;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SymWebUI.Areas.Common.Controllers
{
    public class AppraisalMarksController : Controller
    {
        //
        // GET: /Common/AppraisalMarks/

        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Master,Admin,Account")]
        [HttpGet]
        public ActionResult Create()
        {
            ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;

            AppraisalMarksVM vm = new AppraisalMarksVM();

            return View("~/Areas/Common/Views/AppraisalMarks/Create.cshtml", vm);
        }

        [Authorize(Roles = "Master,Admin,Account")]
        [HttpPost]
        public ActionResult CreateEdit(AppraisalMarksVM vm)
        {
            string[] result = new string[6];
            ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
            vm.CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            vm.CreateBy = identity.Name;
            vm.UpdateBy = identity.Name;
            vm.UpdateDate = DateTime.Now.ToString();
            vm.CreateFrom = identity.WorkStationIP;
            vm.AssignToId = identity.EmployeeCode;
            try
            {
                result = new AppraisalMarksRepo().InsertAdminMarks(vm);
                Session["result"] = result[0] + "~" + result[1];
                return RedirectToAction("Create");
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
        public ActionResult SelectQuestionByEmployee(string DepartmentId, string EmployeeCode, string EvaluationFor, string AssignFrom)
        {
            AppraisalMarksVM vm = new AppraisalMarksVM();
            try
            {
                AppraisalMarksRepo _appraisalMarksRepo = new AppraisalMarksRepo();

                vm.AppraisalMarksDetailVMs = _appraisalMarksRepo.SelectMarksByEmployeeExist(DepartmentId, EmployeeCode, EvaluationFor, AssignFrom);
                if (!vm.AppraisalMarksDetailVMs.Any())
                {
                    vm.AppraisalMarksDetailVMs = _appraisalMarksRepo.SelectAllQuestionByEmployeeExist(AssignFrom, DepartmentId, EmployeeCode);
                }

                return PartialView("~/Areas/Common/Views/AppraisalMarks/_details.cshtml", vm);
            }
            catch (Exception ex)
            {
                Session["result"] = "Fail~" + ex.Message;
                return PartialView("~/Areas/Common/Views/AppraisalMarks/_details.cshtml", vm);
            }
        }

        public ActionResult BlankItem(AppraisalMarksVM vm)
        {
            string[] result = new string[6];           
            try
            {
                return PartialView("~/Areas/Common/Views/AppraisalMarks/_details.cshtml", vm);
            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Succeessfully!";
                FileLogger.Log(result[0].ToString() + Environment.NewLine + result[2].ToString() + Environment.NewLine + result[5].ToString(), this.GetType().Name, result[4].ToString() + Environment.NewLine + result[3].ToString());
                return PartialView("~/Areas/Common/Views/AppraisalMarks/_details.cshtml", vm);
            }
        }
        public ActionResult PrintSheet(AppraisalMarksVM vm)
        {
            try
            {
                string CompanyName = new AppSettingsReader().GetValue("CompanyName", typeof(string)).ToString();
           
                ReportDocument doc = new ReportDocument();
                DataSet ds = new DataSet();

                AppraisalMarksRepo _appraisalMarksRepo = new AppraisalMarksRepo();

                vm.AppraisalMarksDetailVMs = _appraisalMarksRepo.SelectAllQuestionByEmployeeExist(vm.AssignToName, vm.DepartmentId, vm.EmployeeCode);
                DataTable getAllData = Ordinary.ToDataTable(vm.AppraisalMarksDetailVMs);

                ds.Tables.Add(getAllData);
                ds.Tables[0].TableName = "dtApprasilMarks";

                string rptLocation = AppDomain.CurrentDomain.BaseDirectory + @"Files\ReportFiles\Common\AppraisalEmployeeMarks.rpt";
                
                doc.Load(rptLocation);
                doc.SetDataSource(ds);

                string LogoName = "COMPANYLOGO";
                string companyLogo = AppDomain.CurrentDomain.BaseDirectory + "Images\\" + LogoName + ".png";
                doc.DataDefinition.FormulaFields["CompanyLogo"].Text = "'" + companyLogo + "'";
                doc.DataDefinition.FormulaFields["AssignToName"].Text = "'" + vm.AssignToName + "'";
                doc.DataDefinition.FormulaFields["EmployeeName"].Text = "'" + vm.EmployeeName + "'";
                doc.DataDefinition.FormulaFields["DepartmentName"].Text = "'" + vm.DepartmentName + "'";
                           
                var rpt = RenderReportAsPDF(doc);
                doc.Close();
                return rpt;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private FileStreamResult RenderReportAsPDF(ReportDocument rptDoc)
        {
            Stream stream = rptDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/PDF");
        }

        public ActionResult heightmarks()
        {
           
            return View();
        }

        public ActionResult GetAppraisalHeightMarks(JQueryDataTableParamModel param)
        {
            #region Column Search
            var IdFilter = Convert.ToString(Request["sSearch_0"]);
            var DepartmentFilter = Convert.ToString(Request["sSearch_1"]);
            var EmployeeCodeFilter = Convert.ToString(Request["sSearch_2"]);
            var EmpNameFilter = Convert.ToString(Request["sSearch_3"]);
            var TotalMarksFilter = Convert.ToString(Request["sSearch_4"]);
            var NOQFilter = Convert.ToString(Request["sSearch_4"]);         

            #endregion Column Search

            #region Search and Filter Data
            AppraisalMarksRepo _appraisalMarksRepo = new AppraisalMarksRepo();
            var getAllData = _appraisalMarksRepo.GetAppraisalHeightMarks();
            IEnumerable<AppraisalHeightMarksVM> filteredData;

            if (!string.IsNullOrEmpty(param.sSearch))
            {                
                var isSearchable1 = Convert.ToBoolean(Request["bSearchable_1"]);
                var isSearchable2 = Convert.ToBoolean(Request["bSearchable_2"]);
                var isSearchable3 = Convert.ToBoolean(Request["bSearchable_3"]);
                var isSearchable4 = Convert.ToBoolean(Request["bSearchable_4"]);
                var isSearchable5 = Convert.ToBoolean(Request["bSearchable_5"]);

                filteredData = getAllData.Where(c =>
                    isSearchable1 && c.Department.ToLower().Contains(param.sSearch.ToLower())
                    || isSearchable2 && c.EmployeeCode.ToLower().Contains(param.sSearch.ToLower())
                    || isSearchable3 && c.EmpName.ToString().ToLower().Contains(param.sSearch.ToLower())
                    || isSearchable4 && c.TotalMarks.ToLower().Contains(param.sSearch.ToLower())
                    || isSearchable5 && c.NOQ.ToLower().Contains(param.sSearch.ToLower())
                    );
            }
            else
            {
                filteredData = getAllData;
            }
            #endregion Search and Filter Data

            #region Column Filtering
            if (DepartmentFilter != "" || EmployeeCodeFilter != "" || EmpNameFilter != "" || TotalMarksFilter != "")
            {
                filteredData = filteredData.Where(c =>
                    (DepartmentFilter == "" || c.Department.ToLower().Contains(DepartmentFilter.ToLower()))
                    && (EmployeeCodeFilter == "" || c.EmployeeCode.ToLower().Contains(EmployeeCodeFilter.ToLower()))
                    && (EmpNameFilter == "" || c.EmpName.ToString().ToLower().Contains(EmpNameFilter.ToLower()))
                    && (TotalMarksFilter == "" || c.TotalMarks.ToLower().Contains(TotalMarksFilter.ToLower()))
                    && (NOQFilter == "" || c.NOQ.ToLower().Contains(NOQFilter.ToLower()))
                    );
            }

            #endregion Column Filtering


            var isSortable_1 = Convert.ToBoolean(Request["bSortable_1"]);
            var isSortable_2 = Convert.ToBoolean(Request["bSortable_2"]);
            var isSortable_3 = Convert.ToBoolean(Request["bSortable_3"]);
            var isSortable_4 = Convert.ToBoolean(Request["bSortable_4"]);
            var isSortable_5 = Convert.ToBoolean(Request["bSortable_5"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<AppraisalHeightMarksVM, string> orderingFunction = (c =>
                sortColumnIndex == 1 && isSortable_1 ? c.Department :
                sortColumnIndex == 2 && isSortable_2 ? c.EmployeeCode :
                sortColumnIndex == 3 && isSortable_3 ? c.EmpName:
                sortColumnIndex == 4 && isSortable_4 ? c.TotalMarks :
                sortColumnIndex == 4 && isSortable_5 ? c.NOQ :
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
                , c.Department
                , c.EmployeeCode
                , c.EmpName             
                , c.TotalMarks
                ,c.NOQ
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
        
        [HttpPost]
        public ActionResult Process([System.Web.Http.FromBody] ProcessRequestModel request)
        {
            string[] result = new string[6];  
            EmployeeSalaryIncrementRepo _empincreament = new EmployeeSalaryIncrementRepo();
            ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;

            foreach (var emp in request.Employees)
            {
                EmployeeInfoRepo _eRepo = new EmployeeInfoRepo();
                string EmpId = _eRepo.SelectEmpByCode(emp.EmployeeCode);

                EmployeeStructureRepo _empstructRepo = new EmployeeStructureRepo();
                List<EmployeeSalaryStructureVM> VMs = new List<EmployeeSalaryStructureVM>();
                VMs = _empstructRepo.SelectEmployeeSalaryStructureDetailAll(EmpId);

                SettingDAL _settingDal = new SettingDAL();
                string Incremeteffectof = _settingDal.settingValue("Appraisal", "IncrementEffectOn").Trim();

                decimal EmpPercent = 100 / (Convert.ToDecimal(emp.NOQ) * 10) * Convert.ToDecimal(emp.TotalMarks);
                decimal IncrementPercent = 0;

                DataTable dtMarksPercent = _empstructRepo.GetMarksPercent();

                foreach (DataRow row in dtMarksPercent.Rows)
                {
                    int marksFrom = Convert.ToInt32(row["MarksFrom"]);
                    int marksTo = Convert.ToInt32(row["MarksTo"]);
                    int increment = Convert.ToInt32(row["IncrementPercent"]);               
                    if (EmpPercent >= marksFrom && EmpPercent <= marksTo)
                    {
                        IncrementPercent = increment;
                        break; 
                    }
                }

                decimal OldBasicIncr = 0;
                decimal NewGrossIncr = 0;

                if (Incremeteffectof == "Basic")
                {
                    DataTable dtSalaryPercent = _empstructRepo.dtSalaryPercent(Incremeteffectof);
                    var basicItem = VMs.FirstOrDefault(e => e.SalaryType == "Basic");
                    if (basicItem != null)
                    {
                        OldBasicIncr = (basicItem.TotalValue * IncrementPercent) / 100;
                        basicItem.IncrementValue = OldBasicIncr;
                    }
                    foreach (var item in VMs)
                    {
                        if (item.SalaryType == "Basic")
                            continue;

                        decimal percentage = 0;
                        foreach (DataRow row in dtSalaryPercent.Rows)
                        {
                            if (row["SalaryType"].ToString() == item.SalaryType)
                            {
                                decimal.TryParse(row["IncrementPercent"].ToString(), out percentage);
                                break;
                            }
                        }

                        if (item.SalaryType == "Gross")
                        {                         
                            item.IncrementValue = (item.TotalValue * percentage) / 100;
                        }
                        else
                        {                        
                            item.IncrementValue = (OldBasicIncr * percentage) / 100;
                        }
                    }
                }
                else if (Incremeteffectof == "Gross")
                {
                    DataTable dtSalaryPercent = _empstructRepo.dtSalaryPercent(Incremeteffectof);
                    var grossItem = VMs.FirstOrDefault(e => e.SalaryType == "Gross");
                    if (grossItem != null)
                    {
                        NewGrossIncr = (grossItem.TotalValue * IncrementPercent) / 100;
                        grossItem.IncrementValue = NewGrossIncr;
                    }
                    foreach (var item in VMs.Where(x => x.SalaryType != "Gross"))
                    {                     
                        decimal percentage = 0;
                        foreach (DataRow row in dtSalaryPercent.Rows)
                        {
                            if (row["SalaryType"].ToString() == item.SalaryType)
                            {
                                decimal.TryParse(row["IncrementPercent"].ToString(), out percentage);
                                break;
                            }
                        }
                        item.IncrementValue = (NewGrossIncr * percentage) / 100;
                    }
                }  

                EmployeeSalaryOtherIncreamentRepo _esoIncrementRepo = new EmployeeSalaryOtherIncreamentRepo();
                EmployeeSalaryStructureVM vm = new EmployeeSalaryStructureVM();
                vm.EmployeeId = EmpId;
                vm.IncrementDate = emp.IncrementDate;
                              
                try
                {
                    vm.CreatedAt = DateTime.Now.ToString("yyyyMMddHHmmss");
                    vm.CreatedBy = identity.Name;
                    vm.CreatedFrom = identity.WorkStationIP;
                    vm.BranchId = identity.BranchId;

                    result = _esoIncrementRepo.InsertEmployeeSalaryStructure(VMs, vm);                    
                    
                }
                catch (Exception)
                {
                    Session["result"] = "Fail~Data Not Succeessfully!";
                    FileLogger.Log(result[0].ToString() + Environment.NewLine + result[2].ToString() + Environment.NewLine + result[5].ToString(), this.GetType().Name, result[4].ToString() + Environment.NewLine + result[3].ToString());
                    return RedirectToAction("Index");
                }

            }
            var msg = result[0] + "~" + result[1];
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
            
        public class ProcessRequestModel
        {
            public List<EmployeeData> Employees { get; set; }
        }

        public class EmployeeData
        {
            public string EmployeeCode { get; set; }
            public string TotalMarks { get; set; }
            public string NOQ { get; set; }
            public string IncrementDate { get; set; }
        }
    }
}
