using JQueryDataTables.Models;
using SymOrdinary;
using SymRepository.Common;
using SymViewModel.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;


namespace SymWebUI.Areas.Common.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //GET: /Common/Home/    


        public ActionResult Index()
        {
            AdminInfoDashboardVM vm = new AdminInfoDashboardVM();
            HrmInfoDashboardVM vmHRM = new HrmInfoDashboardVM();
            PayrollInfoDashboardVM vmPayroll = new PayrollInfoDashboardVM();
            TaxInfoDashboardVM vmTax = new TaxInfoDashboardVM();
            PfInfoDashboardVM vmPf = new PfInfoDashboardVM();  
            GfInfoDashboardVM vmGF = new GfInfoDashboardVM();
            HomePageInfoDashboardRepo _Repo = new HomePageInfoDashboardRepo();

            //vm = _Repo.GetAdminInfoDashboard();

            //vmHRM = _Repo.GetHrmInfoDashboard();
            //vm.HrmInfoDashboardVMS = vmHRM;

            //vmPayroll = _Repo.GetPayrollInfoDashboard();
            //vm.PayrollInfoDashboardVMS = vmPayroll;

            //vmTax = _Repo.GetTaxInfoDashboard();
            //vm.TaxInfoDashboardVMS = vmTax;

            //vmPf = _Repo.GetPfInfoDashboard();
            //vm.PfInfoDashboardVMS = vmPf;

            //vmGF = _Repo.GetGfInfoDashboard();
            //vm.GfInfoDashboardVMS = vmGF;
      
            return View("Index", vm);
        }

        //public ActionResult AdminInfo()
        //{
        //    AdminInfoDashboardVM vm = new AdminInfoDashboardVM();
        //    HomePageInfoDashboardRepo __Repo = new HomePageInfoDashboardRepo();

        //    vm = __Repo.GetAdminInfoDashboard();

        //    var ch = new chart
        //    {
        //        Present = vm.Present,
        //        Absent = vm.Absent,
        //        Late = vm.Late,
        //        InMiss = vm.InMiss
        //    };

        //    return Json(ch, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult HRMInfo()
        //{
        //    HrmInfoDashboardVM vmHRM = new HrmInfoDashboardVM();
        //    HomePageInfoDashboardRepo _Repo = new HomePageInfoDashboardRepo();

        //    vmHRM = _Repo.GetHrmInfoDashboard();

        //    var ch = new charthrm
        //    {
        //        TotalEmployee = vmHRM.TotalEmployee,
        //        InactiveEmployee = vmHRM.InactiveEmployee,
        //        NotApplicable = vmHRM.NotApplicable,
        //        Male = vmHRM.Male,
        //        Female = vmHRM.Female
        //    };

        //    return Json(ch, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult PayrollInfo()
        //{
        //    PayrollInfoDashboardVM vmPayroll = new PayrollInfoDashboardVM();
        //    HomePageInfoDashboardRepo _Repo = new HomePageInfoDashboardRepo();

        //    vmPayroll = _Repo.GetPayrollInfoDashboard();

        //    var ch = new chartpayroll
        //    {
        //        TotalPerson = vmPayroll.TotalPerson,
        //        GrossSalary = vmPayroll.GrossSalary,              
        //    };

        //    return Json(ch, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult TaxInfo()
        //{
        //    TaxInfoDashboardVM vmTax = new TaxInfoDashboardVM();
        //    HomePageInfoDashboardRepo _Repo = new HomePageInfoDashboardRepo();

        //    vmTax = _Repo.GetTaxInfoDashboard();

        //    var ch = new chartTax
        //    {
        //        TotalPerson = vmTax.TotalPerson,
        //        TaxValue = vmTax.TaxValue,
        //    };

        //    return Json(ch, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult PFInfo()
        //{
        //    PfInfoDashboardVM vmPf = new PfInfoDashboardVM();         
        //    HomePageInfoDashboardRepo _Repo = new HomePageInfoDashboardRepo();

        //    vmPf = _Repo.GetPfInfoDashboard();

        //    var ch = new chartPF
        //    {
        //        TotalPerson = vmPf.TotalPerson,
        //        PFValue = vmPf.PFValue,
        //    };

        //    return Json(ch, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult GFInfo()
        //{
        //    GfInfoDashboardVM vmGF = new GfInfoDashboardVM();
        //    HomePageInfoDashboardRepo _Repo = new HomePageInfoDashboardRepo();

        //    vmGF = _Repo.GetGfInfoDashboard();

        //    var ch = new chartGF
        //    {
        //        TotalPerson = vmGF.TotalPerson,
        //        GFValue = vmGF.GFValue,
        //    };

        //    return Json(ch, JsonRequestBehavior.AllowGet);
        //}

        public class chart
        {
            public string Present { get; set; }
            public string Absent { get; set; }
            public string Late { get; set; }
            public string InMiss { get; set; }
        }

        public class charthrm
        {
            public decimal TotalEmployee { get; set; }
            public decimal InactiveEmployee { get; set; }
            public decimal NotApplicable { get; set; }
            public decimal Male { get; set; }
            public decimal Female { get; set; }
        }

        public class chartpayroll
        {
            public decimal TotalPerson { get; set; }
            public decimal GrossSalary { get; set; }       
        }
        public class chartTax
        {
            public decimal TotalPerson { get; set; }
            public decimal TaxValue { get; set; }
        }
        public class chartPF
        {
            public decimal TotalPerson { get; set; }
            public decimal PFValue { get; set; }
        }
        public class chartGF
        {
            public decimal TotalPerson { get; set; }
            public decimal GFValue { get; set; }
        }
    }
}
