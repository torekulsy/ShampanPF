using System;
using System.Transactions;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using SymWebUI.Filters;
using SymWebUI.Models;
using SymOrdinary;
using JQueryDataTables.Models;
using SymRepository.Common;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using SymViewModel.Common;

namespace SymWebUI.Controllers
{
    public class ApplicantController : Controller
    {
        //
        // GET: /Applicant/

        public ActionResult Index()
        {
            ApplicantInfoVM vm = new ApplicantInfoVM();
            ApplicantInfoRepo _repo = new ApplicantInfoRepo();
            vm = _repo.GetCompanyInfo();

            return View(vm);
        }     
        public ActionResult CreateEdit(ApplicantInfoVM vm, HttpPostedFileBase AttachmentFile)
        {
            ApplicantInfoRepo _repo = new ApplicantInfoRepo();

            string[] result = new string[6];
            vm.CreatedAt = DateTime.Now.ToString("yyyyMMddHHmmss");        
            vm.IsActive = true;
            vm.IsApproved = false;
            if (AttachmentFile != null)
            {
                vm.AttachmentFile = AttachmentFile.FileName;
            }
            try
            {

                result = _repo.InsertApplicantInfo(vm);


                if (AttachmentFile != null && AttachmentFile.ContentLength > 0)
                {
                    var path = Path.Combine(Server.MapPath("~/Files/RecruitmentCV"), AttachmentFile.FileName);
                    AttachmentFile.SaveAs(path);
                }

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

    }
}
