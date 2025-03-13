using SymRepository.Common;
using SymViewModel.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SymWebUI.Controllers
{
    public class JobsController : Controller
    {
        //
        // GET: /Jobs/
        JobCircularRepo _repo = new JobCircularRepo();
        public ActionResult Index()
        {
            List<JobCircularVM> vm = new List<JobCircularVM>();

            vm = _repo.SelectAll();
                 
            return View(vm);
        }
        public ActionResult JobDetails(string Id)
        {
           JobCircularVM vm = new JobCircularVM();

           vm = _repo.SelectById(Id);

            return View(vm);
        }
        public ActionResult ApplicantApply(string Id,string JobTitle)       
        {
            ApplicantInfoVM vm = new ApplicantInfoVM();
            ApplicantInfoRepo _repo = new ApplicantInfoRepo();
            vm = _repo.GetCompanyInfo();
            vm.Jobid = Id;
            vm.JobTitle = JobTitle;
            return View(vm);
        }

        public ActionResult CreateEdit(ApplicantInfoVM vm, HttpPostedFileBase AttachmentFile, HttpPostedFileBase AttachmentImaage)
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
            if(AttachmentImaage !=null)
            {
                vm.ImageFileName = AttachmentImaage.FileName;
            }
            try
            {
                result = _repo.InsertApplicantInfo(vm);

                if (result[0] == "Success")
                {
                    if (AttachmentImaage != null && AttachmentImaage.ContentLength > 0)
                    {
                        var path = Path.Combine(Server.MapPath("~/Files/EmployeeInfo"), vm.ImageFileName);                       
                        AttachmentImaage.SaveAs(path);
                    }
                    if (AttachmentFile != null && AttachmentFile.ContentLength > 0)
                    {
                        var pathfile = Path.Combine(Server.MapPath("~/Files/RecruitmentCV"), vm.AttachmentFile);
                        AttachmentFile.SaveAs(pathfile);
                    }                  
                }
                       
                Session["result"] = result[0] + "~" + result[1];
                return RedirectToAction("ApplicantApply");

            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Succeessfully!";              
                return View(vm);
            }
        }

        public ActionResult BlankItem(EducationVM vm)
        {
            string[] result = new string[6];           
            try
            {
                return PartialView("~/Views/Jobs/_education.cshtml", vm);
            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Succeessfully!";
                return PartialView("~/Views/Jobs/ApplicantApply.cshtml", vm);
            }
        }


        public ActionResult BlankItemPQ(EducationVM vm)
        {
            string[] result = new string[6];
            try
            {
                return PartialView("~/Views/Jobs/_ProfessionalQualification.cshtml", vm);
            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Succeessfully!";
                return PartialView("~/Views/Jobs/ApplicantApply.cshtml", vm);
            }
        }
    }
}
