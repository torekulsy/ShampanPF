using JQueryDataTables.Models;
using SymOrdinary;
using SymRepository.Common;
using SymRepository.HRM;
using SymRepository.Payroll;
using SymViewModel.Common;
using SymViewModel.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
namespace SymWebUI.Areas.Common.Controllers
{
    public class AppraisalSalaryIncreamentController : Controller
    {
        //
        // GET: /Common/AppraisalSalaryIncreament/
        SymUserRoleRepo _reposur = new SymUserRoleRepo();
        ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AppraisalSalarySelectemployee(string Code)
        {
            EmployeeInfoRepo _eRepo = new EmployeeInfoRepo();
            string EmpId = _eRepo.SelectEmpByCode(Code);

            EmployeeStructureRepo _empstructRepo = new EmployeeStructureRepo();
            List<EmployeeSalaryStructureVM> employeeSalaryStructureVMs = new List<EmployeeSalaryStructureVM>();
            employeeSalaryStructureVMs = _empstructRepo.SelectEmployeeSalaryStructureDetailAll(EmpId);
            return PartialView("_EmployeeSalaryDetails", employeeSalaryStructureVMs);
        }
    }
}
