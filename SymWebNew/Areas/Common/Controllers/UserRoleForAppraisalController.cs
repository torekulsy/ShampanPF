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
    public class UserRoleForAppraisalController : Controller
    {
        //
        // GET: /Common/UserRoleForAppraisal/

        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Master,Admin,Account")]
        [HttpGet]
        public ActionResult Create()
        {
            AppraisalQuestionSetVM vm = new AppraisalQuestionSetVM();
            AppraisalMarksRepo _appraisalMarksRepo = new AppraisalMarksRepo();

            vm.AppraisalQuestionSetDetaiVMs = _appraisalMarksRepo.GetUesrForAppraisalExist();

            if (!vm.AppraisalQuestionSetDetaiVMs.Any())
            {
                vm.AppraisalQuestionSetDetaiVMs = _appraisalMarksRepo.GetUesrForAppraisal();
            }           
       
            return View("~/Areas/Common/Views/UserRoleForAppraisal/Create.cshtml", vm);
        }

        [Authorize(Roles = "Master,Admin,Account")]
        [HttpPost]
        public ActionResult CreateEdit(AppraisalQuestionSetVM vm)
        {
            //AppraisalQuestionSetVM vm = new AppraisalQuestionSetVM();
            string[] result = new string[6];
            ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
            vm.CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            vm.CreateBy = identity.Name;
            vm.UpdateBy = identity.Name;
            vm.UpdateDate = DateTime.Now.ToString();
            vm.CreateFrom = identity.WorkStationIP;
            try
            {

                result = new AppraisalMarksRepo().Insert(vm);
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
    }
}
