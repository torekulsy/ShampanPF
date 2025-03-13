using CrystalDecisions.CrystalReports.Engine;
using JQueryDataTables.Models;
using SymOrdinary;
using SymRepository.Common;
using SymRepository.HRM;
using SymRepository.PF;
using SymViewModel.Common;
using SymViewModel.HRM;
using SymWebUI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SymWebUI.Areas.Common.Controllers
{
    public class RequirtmentApplicantController : Controller
    {
        //
        // GET: /HRM/RequirtmentApplicant/

        ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _index(JQueryDataTableParamModel param)
        {
            ApplicantInfoRepo _Repo = new ApplicantInfoRepo();

            #region Column Search
            var ApplicantName = Convert.ToString(Request["sSearch_1"]);
            var Designation = Convert.ToString(Request["sSearch_2"]);
            var ContactNo = Convert.ToString(Request["sSearch_3"]);
            var Experience = Convert.ToString(Request["sSearch_4"]);
            var ExpectedSalary = Convert.ToString(Request["sSearch_5"]);
            var AttachmentFile = Convert.ToString(Request["sSearch_6"]);
            var IsActive = Convert.ToString(Request["sSearch_7"]);
            var IsShorlisted = Convert.ToString(Request["sSearch_8"]);


            #endregion Column Search

            #region Search and Filter Data

            var getAllData = _Repo.SelectAllApplicantInfo();
            IEnumerable<ApplicantInfoVM> filteredData;

            if (!string.IsNullOrEmpty(param.sSearch))
            {

                var isSearchable1 = Convert.ToBoolean(Request["bSearchable_1"]);
                var isSearchable2 = Convert.ToBoolean(Request["bSearchable_2"]);
                var isSearchable3 = Convert.ToBoolean(Request["bSearchable_3"]);
                var isSearchable4 = Convert.ToBoolean(Request["bSearchable_4"]);
                var isSearchable5 = Convert.ToBoolean(Request["bSearchable_5"]);
                var isSearchable6 = Convert.ToBoolean(Request["bSearchable_6"]);
                var isSearchable7 = Convert.ToBoolean(Request["bSearchable_7"]);
                var isSearchable8 = Convert.ToBoolean(Request["bSearchable_8"]);

                filteredData = getAllData.Where(c =>
                     isSearchable3 && c.Id.ToString().Contains(param.sSearch.ToLower())
                     || isSearchable1 && c.ApplicantName.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable2 && c.Designation.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable3 && c.ContactNo.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable4 && c.Experience.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable5 && c.ExpectedSalary.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable6 && c.AttachmentFile.ToString().ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable7 && c.IsActive.ToString().ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable8 && c.IsConfirmed.ToString().ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable8 && c.IsShortlisted.ToString().ToLower().Contains(param.sSearch.ToLower())

                    );
            }
            else
            {
                filteredData = getAllData;
            }

            #endregion Search and Filter Data

            #region Column Filtering
            if (ApplicantName != "" || Designation != "" || ContactNo != "" || Experience != "" || ExpectedSalary != "")
            {
                filteredData = filteredData
                                .Where(c => (ApplicantName == "" || c.ApplicantName.ToLower().Contains(ApplicantName.ToLower()))
                                            && (Designation == "" || c.Designation.ToLower().Contains(Designation.ToLower()))
                                            && (ContactNo == "" || c.ContactNo.ToLower().Contains(ContactNo.ToLower()))
                                            && (Experience == "" || c.Experience.ToLower().Contains(Experience.ToLower()))
                                            && (ExpectedSalary == "" || c.ExpectedSalary.ToString().ToLower().Contains(ExpectedSalary.ToLower()))
                                            && (AttachmentFile == "" || c.AttachmentFile.ToLower().Contains(AttachmentFile.ToLower()))
                                            && (IsActive == "" || c.IsActive.ToString().ToLower().Contains(IsActive.ToLower()))
                                            && (IsShorlisted == "" || c.IsShortlisted.ToString().ToLower().Contains(IsShorlisted.ToLower()))
                                        );
            }
            #endregion Column FilteringD:\HRM\ShampanHRM\SymWebNew\Areas\Common\Controllers\RequirtmentApplicantController.cs

            var isSortable_1 = Convert.ToBoolean(Request["bSortable_1"]);
            var isSortable_2 = Convert.ToBoolean(Request["bSortable_2"]);
            var isSortable_3 = Convert.ToBoolean(Request["bSortable_3"]);
            var isSortable_4 = Convert.ToBoolean(Request["bSortable_4"]);
            var isSortable_5 = Convert.ToBoolean(Request["bSortable_5"]);
            var isSortable_6 = Convert.ToBoolean(Request["bSortable_6"]);
            var isSortable_7 = Convert.ToBoolean(Request["bSortable_7"]);
            var isSortable_8 = Convert.ToBoolean(Request["bSortable_8"]);
            var isSortable_9 = Convert.ToBoolean(Request["bSortable_9"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<ApplicantInfoVM, string> orderingFunction = (c =>
                sortColumnIndex == 1 && isSortable_1 ? c.Id.ToString() :
                sortColumnIndex == 2 && isSortable_2 ? c.ApplicantName :
                sortColumnIndex == 3 && isSortable_3 ? c.Designation :
                sortColumnIndex == 4 && isSortable_4 ? c.ContactNo :
                sortColumnIndex == 5 && isSortable_5 ? c.Experience :
                sortColumnIndex == 6 && isSortable_6 ? c.ExpectedSalary :
                sortColumnIndex == 7 && isSortable_7 ? c.AttachmentFile.ToString() :
                sortColumnIndex == 8 && isSortable_8 ? c.IsActive.ToString() :
                sortColumnIndex == 9 && isSortable_9 ? c.IsShortlisted.ToString() :
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
                ,c.ApplicantName
                ,c.Designation              
                ,c.ContactNo 
                ,c.Experience
                ,c.ExpectedSalary               
                ,c.AttachmentFile                
                ,c.IsActive.ToString() 
                 ,c.IsShortlisted.ToString()     
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

        public ActionResult InterviewCall()
        {
            return View();
        }

        public ActionResult _InterviewCallIndex(JQueryDataTableParamModel param)
        {
            ApplicantInfoRepo _Repo = new ApplicantInfoRepo();

            #region Column Search
            var ApplicantName = Convert.ToString(Request["sSearch_1"]);
            var Designation = Convert.ToString(Request["sSearch_2"]);
            var ContactNo = Convert.ToString(Request["sSearch_3"]);
            var Experience = Convert.ToString(Request["sSearch_4"]);
            var ExpectedSalary = Convert.ToString(Request["sSearch_5"]);
            var InterviewDate = Convert.ToString(Request["sSearch_6"]);
            var InterviewWrittenMarks = Convert.ToString(Request["sSearch_7"]);
            var IsShorlisted = Convert.ToString(Request["sSearch_8"]);


            #endregion Column Search

            #region Search and Filter Data

            var getAllData = _Repo.SelectAllForInterviewCall();
            IEnumerable<ApplicantInfoVM> filteredData;

            if (!string.IsNullOrEmpty(param.sSearch))
            {

                var isSearchable1 = Convert.ToBoolean(Request["bSearchable_1"]);
                var isSearchable2 = Convert.ToBoolean(Request["bSearchable_2"]);
                var isSearchable3 = Convert.ToBoolean(Request["bSearchable_3"]);
                var isSearchable4 = Convert.ToBoolean(Request["bSearchable_4"]);
                var isSearchable5 = Convert.ToBoolean(Request["bSearchable_5"]);
                var isSearchable6 = Convert.ToBoolean(Request["bSearchable_6"]);
                var isSearchable7 = Convert.ToBoolean(Request["bSearchable_7"]);

                filteredData = getAllData.Where(c =>
                     isSearchable3 && c.Id.ToString().Contains(param.sSearch.ToLower())
                     || isSearchable1 && c.ApplicantName.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable2 && c.Designation.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable3 && c.ContactNo.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable4 && c.Experience.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable5 && c.ExpectedSalary.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable6 && c.InterviewDate.ToString().ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable7 && c.IsShortlisted.ToString().ToLower().Contains(param.sSearch.ToLower())

                    );
            }
            else
            {
                filteredData = getAllData;
            }

            #endregion Search and Filter Data

            #region Column Filtering
            if (ApplicantName != "" || Designation != "" || ContactNo != "" || Experience != "" || ExpectedSalary != "")
            {
                filteredData = filteredData
                                .Where(c => (ApplicantName == "" || c.ApplicantName.ToLower().Contains(ApplicantName.ToLower()))
                                            && (Designation == "" || c.Designation.ToLower().Contains(Designation.ToLower()))
                                            && (ContactNo == "" || c.ContactNo.ToLower().Contains(ContactNo.ToLower()))
                                            && (Experience == "" || c.Experience.ToLower().Contains(Experience.ToLower()))
                                            && (ExpectedSalary == "" || c.ExpectedSalary.ToString().ToLower().Contains(ExpectedSalary.ToLower()))
                                            && (InterviewDate == "" || c.InterviewDate.ToLower().Contains(InterviewDate.ToLower()))
                                            && (IsShorlisted == "" || c.IsShortlisted.ToString().ToLower().Contains(IsShorlisted.ToLower()))
                                        );
            }
            #endregion Column Filtering

            var isSortable_1 = Convert.ToBoolean(Request["bSortable_1"]);
            var isSortable_2 = Convert.ToBoolean(Request["bSortable_2"]);
            var isSortable_3 = Convert.ToBoolean(Request["bSortable_3"]);
            var isSortable_4 = Convert.ToBoolean(Request["bSortable_4"]);
            var isSortable_5 = Convert.ToBoolean(Request["bSortable_5"]);
            var isSortable_6 = Convert.ToBoolean(Request["bSortable_6"]);
            var isSortable_7 = Convert.ToBoolean(Request["bSortable_7"]);
            var isSortable_8 = Convert.ToBoolean(Request["bSortable_8"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<ApplicantInfoVM, string> orderingFunction = (c =>
                sortColumnIndex == 1 && isSortable_1 ? c.Id.ToString() :
                sortColumnIndex == 2 && isSortable_2 ? c.ApplicantName :
                sortColumnIndex == 3 && isSortable_3 ? c.Designation :
                sortColumnIndex == 4 && isSortable_4 ? c.ContactNo :
                sortColumnIndex == 5 && isSortable_5 ? c.Experience :
                sortColumnIndex == 6 && isSortable_6 ? c.ExpectedSalary :
                sortColumnIndex == 7 && isSortable_7 ? c.InterviewDate.ToString() :
                sortColumnIndex == 8 && isSortable_8 ? c.IsShortlisted.ToString() :
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
                ,c.ApplicantName
                ,c.Designation              
                ,c.ContactNo 
                ,c.Experience
                ,c.ExpectedSalary               
                ,c.InterviewDate 
                ,c.IsShortlisted.ToString()     
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

        public ActionResult EditInterviewCall(int id)
        {
            ApplicantInfoRepo _Repo = new ApplicantInfoRepo();
            ApplicantInfoVM vm = new ApplicantInfoVM();

            vm = _Repo.SelectById(id);
            return View(vm);
        }

        [HttpGet]
        public ActionResult Create(ApplicantInfoVM vm)
        {

            return View("Create", vm);
        }


        public ActionResult MarkSetup()
        {
            return View();
        }
        public ActionResult _MarkSetupIndex(JQueryDataTableParamModel param)
        {
            ApplicantInfoRepo _Repo = new ApplicantInfoRepo();

            #region Column Search
            var ApplicantName = Convert.ToString(Request["sSearch_1"]);
            var Designation = Convert.ToString(Request["sSearch_2"]);
            var ContactNo = Convert.ToString(Request["sSearch_3"]);
            var Experience = Convert.ToString(Request["sSearch_4"]);
            var ExpectedSalary = Convert.ToString(Request["sSearch_5"]);
            var InterviewDate = Convert.ToString(Request["sSearch_6"]);
            var InterviewWrittenMarks = Convert.ToString(Request["sSearch_7"]);
            var IsShorlisted = Convert.ToString(Request["sSearch_8"]);


            #endregion Column Search

            #region Search and Filter Data

            var getAllData = _Repo.SelectAllForMarkSetup();
            IEnumerable<ApplicantInfoVM> filteredData;

            if (!string.IsNullOrEmpty(param.sSearch))
            {

                var isSearchable1 = Convert.ToBoolean(Request["bSearchable_1"]);
                var isSearchable2 = Convert.ToBoolean(Request["bSearchable_2"]);
                var isSearchable3 = Convert.ToBoolean(Request["bSearchable_3"]);
                var isSearchable4 = Convert.ToBoolean(Request["bSearchable_4"]);
                var isSearchable5 = Convert.ToBoolean(Request["bSearchable_5"]);
                var isSearchable6 = Convert.ToBoolean(Request["bSearchable_6"]);
                var isSearchable7 = Convert.ToBoolean(Request["bSearchable_7"]);
                var isSearchable8 = Convert.ToBoolean(Request["bSearchable_8"]);

                filteredData = getAllData.Where(c =>
                     isSearchable3 && c.Id.ToString().Contains(param.sSearch.ToLower())
                     || isSearchable1 && c.ApplicantName.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable2 && c.Designation.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable3 && c.ContactNo.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable4 && c.Experience.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable5 && c.ExpectedSalary.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable6 && c.InterviewDate.ToString().ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable7 && c.InterviewWrittenMarks.ToString().ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable8 && c.IsShortlisted.ToString().ToLower().Contains(param.sSearch.ToLower())

                    );
            }
            else
            {
                filteredData = getAllData;
            }

            #endregion Search and Filter Data

            #region Column Filtering
            if (ApplicantName != "" || Designation != "" || ContactNo != "" || Experience != "" || ExpectedSalary != "")
            {
                filteredData = filteredData
                                .Where(c => (ApplicantName == "" || c.ApplicantName.ToLower().Contains(ApplicantName.ToLower()))
                                            && (Designation == "" || c.Designation.ToLower().Contains(Designation.ToLower()))
                                            && (ContactNo == "" || c.ContactNo.ToLower().Contains(ContactNo.ToLower()))
                                            && (Experience == "" || c.Experience.ToLower().Contains(Experience.ToLower()))
                                            && (ExpectedSalary == "" || c.ExpectedSalary.ToString().ToLower().Contains(ExpectedSalary.ToLower()))
                                            && (InterviewDate == "" || c.InterviewDate.ToLower().Contains(InterviewDate.ToLower()))
                                            && (InterviewWrittenMarks == "" || c.InterviewWrittenMarks.ToString().ToLower().Contains(InterviewWrittenMarks.ToLower()))
                                            && (IsShorlisted == "" || c.IsShortlisted.ToString().ToLower().Contains(IsShorlisted.ToLower()))
                                        );
            }
            #endregion Column Filtering

            var isSortable_1 = Convert.ToBoolean(Request["bSortable_1"]);
            var isSortable_2 = Convert.ToBoolean(Request["bSortable_2"]);
            var isSortable_3 = Convert.ToBoolean(Request["bSortable_3"]);
            var isSortable_4 = Convert.ToBoolean(Request["bSortable_4"]);
            var isSortable_5 = Convert.ToBoolean(Request["bSortable_5"]);
            var isSortable_6 = Convert.ToBoolean(Request["bSortable_6"]);
            var isSortable_7 = Convert.ToBoolean(Request["bSortable_7"]);
            var isSortable_8 = Convert.ToBoolean(Request["bSortable_8"]);
            var isSortable_9 = Convert.ToBoolean(Request["bSortable_9"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<ApplicantInfoVM, string> orderingFunction = (c =>
                sortColumnIndex == 1 && isSortable_1 ? c.Id.ToString() :
                sortColumnIndex == 2 && isSortable_2 ? c.ApplicantName :
                sortColumnIndex == 3 && isSortable_3 ? c.Designation :
                sortColumnIndex == 4 && isSortable_4 ? c.ContactNo :
                sortColumnIndex == 5 && isSortable_5 ? c.Experience :
                sortColumnIndex == 6 && isSortable_6 ? c.ExpectedSalary :
                sortColumnIndex == 7 && isSortable_7 ? c.InterviewDate.ToString() :
                sortColumnIndex == 8 && isSortable_8 ? c.InterviewWrittenMarks.ToString() :
                sortColumnIndex == 9 && isSortable_8 ? c.IsShortlisted.ToString() :
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
                ,c.ApplicantName
                ,c.Designation              
                ,c.ContactNo 
                ,c.Experience
                ,c.ExpectedSalary               
                ,c.InterviewDate 
                ,c.InterviewWrittenMarks 
                ,c.IsShortlisted.ToString()     
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

        public ActionResult EditMarkSetup(int id)
        {
            ApplicantInfoRepo _Repo = new ApplicantInfoRepo();
            ApplicantInfoVM vm = new ApplicantInfoVM();

            vm = _Repo.SelectById(id);
            return View(vm);
        }

        private static Thread thread;
        public ActionResult CreateEdit(ApplicantInfoVM vm, HttpPostedFileBase AttachmentFile)
        {

            string[] result = new string[6];
            vm.CreatedAt = DateTime.Now.ToString("yyyyMMddHHmmss");
            vm.CreatedBy = identity.Name;
            vm.CreatedFrom = identity.WorkStationIP;
            vm.IsActive = true;
            if (AttachmentFile != null)
            {
                vm.AttachmentFile = AttachmentFile.FileName;
            }
            try
            {

                result = new ApplicantInfoRepo().InsertApplicantInfo(vm);


                if (AttachmentFile != null && AttachmentFile.ContentLength > 0)
                {
                    var path = Path.Combine(Server.MapPath("~/Files/RecruitmentCV"), AttachmentFile.FileName);
                    AttachmentFile.SaveAs(path);
                }

                Session["result"] = result[0] + "~" + result[1];

                if (vm.Type == "Marks")
                {
                    return RedirectToAction("MarkSetup");
                }
                if (vm.Type == "Inverview")
                {
                    thread = new Thread(unused => EmpLeaveApproveEmailProcess(vm));
                    thread.Start();
                    return RedirectToAction("InterviewCall");
                }

                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Succeessfully!";
                FileLogger.Log(result[0].ToString() + Environment.NewLine + result[2].ToString() + Environment.NewLine + result[5].ToString(), this.GetType().Name, result[4].ToString() + Environment.NewLine + result[3].ToString());
                return View(vm);
            }
        }
        
        public ActionResult EditForInteviewCall(ApplicantInfoVM vm, HttpPostedFileBase AttachmentFile)
        {

            string[] result = new string[6];
            vm.CreatedAt = DateTime.Now.ToString("yyyyMMddHHmmss");
            vm.CreatedBy = identity.Name;
            vm.CreatedFrom = identity.WorkStationIP;
            vm.IsActive = true;
            if (AttachmentFile != null)
            {
                vm.AttachmentFile = AttachmentFile.FileName;
            }
            try
            {

                result = new ApplicantInfoRepo().EditApplicantInterview(vm);


                if (AttachmentFile != null && AttachmentFile.ContentLength > 0)
                {
                    var path = Path.Combine(Server.MapPath("~/Files/RecruitmentCV"), AttachmentFile.FileName);
                    AttachmentFile.SaveAs(path);
                }

                Session["result"] = result[0] + "~" + result[1];

                if (vm.Type == "Marks")
                {
                    return RedirectToAction("MarkSetup");
                }
                if (vm.Type == "Inverview")
                {
                    thread = new Thread(unused => EmpLeaveApproveEmailProcess(vm));
                    thread.Start();
                    return RedirectToAction("InterviewCall");
                }

                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Succeessfully!";
                FileLogger.Log(result[0].ToString() + Environment.NewLine + result[2].ToString() + Environment.NewLine + result[5].ToString(), this.GetType().Name, result[4].ToString() + Environment.NewLine + result[3].ToString());
                return View(vm);
            }
        }

        public ActionResult EditForMarkSetup(ApplicantInfoVM vm, HttpPostedFileBase AttachmentFile)
        {

            string[] result = new string[6];
            vm.CreatedAt = DateTime.Now.ToString("yyyyMMddHHmmss");
            vm.CreatedBy = identity.Name;
            vm.CreatedFrom = identity.WorkStationIP;
            vm.IsActive = true;
            if (AttachmentFile != null)
            {
                vm.AttachmentFile = AttachmentFile.FileName;
            }
            try
            {

                result = new ApplicantInfoRepo().EditApplicantMarkSetup(vm);


                if (AttachmentFile != null && AttachmentFile.ContentLength > 0)
                {
                    var path = Path.Combine(Server.MapPath("~/Files/RecruitmentCV"), AttachmentFile.FileName);
                    AttachmentFile.SaveAs(path);
                }

                Session["result"] = result[0] + "~" + result[1];

                if (vm.Type == "Marks")
                {
                    return RedirectToAction("MarkSetup");
                }
                if (vm.Type == "Inverview")
                {
                    thread = new Thread(unused => EmpLeaveApproveEmailProcess(vm));
                    thread.Start();
                    return RedirectToAction("InterviewCall");
                }

                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Succeessfully!";
                FileLogger.Log(result[0].ToString() + Environment.NewLine + result[2].ToString() + Environment.NewLine + result[5].ToString(), this.GetType().Name, result[4].ToString() + Environment.NewLine + result[3].ToString());
                return View(vm);
            }
        }

        public void EmpLeaveApproveEmailProcess(ApplicantInfoVM vm)
        {          

            try
            {
                EmailSettings ems = new EmailSettings();
                SettingRepo _SettingRepo = new SettingRepo();

                #region Get Data
                double totalDays = 0;

              
                #endregion

                #region Mail Data Assign

                ems.MailFromAddress = _SettingRepo.settingValue("Mail", "MailFromAddress");
                ems.Password = _SettingRepo.settingValue("Mail", "MailFromPSW");
                ems.UserName = ems.MailFromAddress;
                ems.MailHeader = _SettingRepo.settingValue("Mail", "MailSubjectInterview");             
                string mailbody = _SettingRepo.settingValue("Mail", "MailBodyInterview");               
                mailbody = mailbody.Replace("\\n", Environment.NewLine);
                mailbody = mailbody.Replace("vEmpName", vm.ApplicantName);
                mailbody = mailbody.Replace("vInterviewDate", vm.InterviewDate);
                mailbody = mailbody.Replace("vInterviewTime", vm.InterviewTime);
                mailbody = mailbody.Replace("vDesignation", vm.Designation);              

                #endregion

                ems.MailToAddress = vm.Email;
                //ems.MailToAddress = "shariful.islam@symphonysoftt.com";


                #region Email Sending

                if (!string.IsNullOrWhiteSpace(ems.MailToAddress))
                {

                    ems.MailBody = mailbody;
                    ems.FileName = vm.ApplicantName;
                    if (ems.MailFromAddress.Contains("@"))
                    {
                        using (var smpt = new SmtpClient())
                        {
                            smpt.EnableSsl = ems.USsel;
                            smpt.Host = ems.ServerName;
                            smpt.Port = ems.Port;
                            smpt.UseDefaultCredentials = false;
                            smpt.EnableSsl = true;
                            smpt.Credentials = new NetworkCredential(ems.UserName, ems.Password);

                            MailMessage mailmessage = new MailMessage(
                            ems.MailFromAddress,
                            ems.MailToAddress,
                            ems.MailHeader,
                            ems.MailBody);
                                //mailmessage.Attachments.Add(new Attachment(rptDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat), ems.FileName + ".pdf"));

                            smpt.Send(mailmessage);
                            mailmessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                            //FileLogger.Log("EmpLeaveApproveEmailProcess", this.GetType().Name, "EmpEmail Send To:" + ems.MailToAddress);

                        }


                        Thread.Sleep(500);
                    }
                }
                #endregion
            }
            catch (SmtpFailedRecipientException ex)
            {
                //FileLogger.Log("EmpLeaveApproveEmailProcess", this.GetType().Name, "EmpEmail Not Send To:" + ems.MailToAddress + " " + ex.Message + Environment.NewLine + ex.StackTrace);


                // throw ex;
            }
            thread.Abort();

        }

        public ActionResult Edit(int id)
        {
            ApplicantInfoRepo _Repo = new ApplicantInfoRepo();
            ApplicantInfoVM vm = new ApplicantInfoVM();

            vm = _Repo.SelectById(id);
            return View(vm);
        }

        public ActionResult Delete(int Id)
        {
            string[] result = new string[6];
            ApplicantInfoRepo _Repo = new ApplicantInfoRepo();
            ApplicantInfoVM vm = new ApplicantInfoVM();

            result = _Repo.DeleteApplicantInfo(Id);
            Session["result"] = result[0] + "~" + result[1];
            return View("Index");
        }

        public ActionResult Approve(int id)
        {
            ApplicantInfoRepo _Repo = new ApplicantInfoRepo();
            string[] result = new string[6];

            result = _Repo.ApproveApplicantInfo(id);
            return RedirectToAction("Index");
        }
        public ActionResult Shorlisted(int Id)
        {
            ApplicantInfoRepo _Repo = new ApplicantInfoRepo();
            string[] result = new string[6];

            result = _Repo.UpdateShortlistedApplicantInfo(Id);
            return RedirectToAction("Index");
        }
        public ActionResult ConfirmApplicant()
        {
            return View();
        }

        public ActionResult _ConfirmIndex(JQueryDataTableParamModel param)
        {
            ApplicantInfoRepo _Repo = new ApplicantInfoRepo();

            #region Column Search
            var ApplicantName = Convert.ToString(Request["sSearch_1"]);
            var Designation = Convert.ToString(Request["sSearch_2"]);
            var ContactNo = Convert.ToString(Request["sSearch_3"]);
            var Experience = Convert.ToString(Request["sSearch_4"]);
            var ExpectedSalary = Convert.ToString(Request["sSearch_5"]);
            var InterviewDate = Convert.ToString(Request["sSearch_6"]);
            var InterviewWrittenMarks = Convert.ToString(Request["sSearch_7"]);
            var IsShorlisted = Convert.ToString(Request["sSearch_8"]);


            #endregion Column Search

            #region Search and Filter Data

            var getAllData = _Repo.SelectAllForConfirm();
            IEnumerable<ApplicantInfoVM> filteredData;

            if (!string.IsNullOrEmpty(param.sSearch))
            {

                var isSearchable1 = Convert.ToBoolean(Request["bSearchable_1"]);
                var isSearchable2 = Convert.ToBoolean(Request["bSearchable_2"]);
                var isSearchable3 = Convert.ToBoolean(Request["bSearchable_3"]);
                var isSearchable4 = Convert.ToBoolean(Request["bSearchable_4"]);
                var isSearchable5 = Convert.ToBoolean(Request["bSearchable_5"]);
                var isSearchable6 = Convert.ToBoolean(Request["bSearchable_6"]);
                var isSearchable7 = Convert.ToBoolean(Request["bSearchable_7"]);
                var isSearchable8 = Convert.ToBoolean(Request["bSearchable_8"]);

                filteredData = getAllData.Where(c =>
                     isSearchable3 && c.Id.ToString().Contains(param.sSearch.ToLower())
                     || isSearchable1 && c.ApplicantName.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable2 && c.Designation.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable3 && c.ContactNo.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable4 && c.Experience.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable5 && c.ExpectedSalary.ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable6 && c.InterviewDate.ToString().ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable7 && c.InterviewWrittenMarks.ToString().ToLower().Contains(param.sSearch.ToLower())
                     || isSearchable8 && c.IsShortlisted.ToString().ToLower().Contains(param.sSearch.ToLower())

                    );
            }
            else
            {
                filteredData = getAllData;
            }

            #endregion Search and Filter Data

            #region Column Filtering
            if (ApplicantName != "" || Designation != "" || ContactNo != "" || Experience != "" || ExpectedSalary != "")
            {
                filteredData = filteredData
                                .Where(c => (ApplicantName == "" || c.ApplicantName.ToLower().Contains(ApplicantName.ToLower()))
                                            && (Designation == "" || c.Designation.ToLower().Contains(Designation.ToLower()))
                                            && (ContactNo == "" || c.ContactNo.ToLower().Contains(ContactNo.ToLower()))
                                            && (Experience == "" || c.Experience.ToLower().Contains(Experience.ToLower()))
                                            && (ExpectedSalary == "" || c.ExpectedSalary.ToString().ToLower().Contains(ExpectedSalary.ToLower()))
                                            && (InterviewDate == "" || c.InterviewDate.ToLower().Contains(InterviewDate.ToLower()))
                                            && (InterviewWrittenMarks == "" || c.InterviewWrittenMarks.ToString().ToLower().Contains(InterviewWrittenMarks.ToLower()))
                                            && (IsShorlisted == "" || c.IsShortlisted.ToString().ToLower().Contains(IsShorlisted.ToLower()))
                                        );
            }
            #endregion Column Filtering

            var isSortable_1 = Convert.ToBoolean(Request["bSortable_1"]);
            var isSortable_2 = Convert.ToBoolean(Request["bSortable_2"]);
            var isSortable_3 = Convert.ToBoolean(Request["bSortable_3"]);
            var isSortable_4 = Convert.ToBoolean(Request["bSortable_4"]);
            var isSortable_5 = Convert.ToBoolean(Request["bSortable_5"]);
            var isSortable_6 = Convert.ToBoolean(Request["bSortable_6"]);
            var isSortable_7 = Convert.ToBoolean(Request["bSortable_7"]);
            var isSortable_8 = Convert.ToBoolean(Request["bSortable_8"]);
            var isSortable_9 = Convert.ToBoolean(Request["bSortable_9"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<ApplicantInfoVM, string> orderingFunction = (c =>
                sortColumnIndex == 1 && isSortable_1 ? c.Id.ToString() :
                sortColumnIndex == 2 && isSortable_2 ? c.ApplicantName :
                sortColumnIndex == 3 && isSortable_3 ? c.Designation :
                sortColumnIndex == 4 && isSortable_4 ? c.ContactNo :
                sortColumnIndex == 5 && isSortable_5 ? c.Experience :
                sortColumnIndex == 6 && isSortable_6 ? c.ExpectedSalary :
                sortColumnIndex == 7 && isSortable_7 ? c.InterviewDate.ToString() :
                sortColumnIndex == 8 && isSortable_8 ? c.InterviewWrittenMarks.ToString() :
                sortColumnIndex == 9 && isSortable_8 ? c.IsShortlisted.ToString() :
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
                ,c.ApplicantName
                ,c.Designation              
                ,c.ContactNo 
                ,c.Experience
                ,c.ExpectedSalary  
                ,c.InterviewWrittenMarks 
                ,c.InterviewDate                
                ,c.IsShortlisted.ToString()     
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

        public ActionResult Confirmed(int id)
        {
            ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;

            ApplicantInfoRepo _Repo = new ApplicantInfoRepo();

            string EmpName = "";
            DataTable dt = _Repo.GetApplicantInfo(id);
            if (dt.Rows.Count > 0)
            {
                EmpName = dt.Rows[0]["ApplicantName"].ToString();
            }

            string[] result = new string[6];
            string[] data = new string[5];
            data[0] = id.ToString();
            data[1] = identity.Name;
            data[2] = DateTime.Now.ToString("yyyyMMddHHmmss");
            data[3] = identity.WorkStationIP;
            data[4] = EmpName;

            EmployeeInfoRepo rmpRepo = new EmployeeInfoRepo();
            result = rmpRepo.InsertApplicant(data);
            if (result[0] == "Success")
            {
                result = rmpRepo.UpdateIsConfirmed(id);
                Session["result"] = result[0] + "~" + result[1];
            }

            return RedirectToAction("ConfirmApplicant");

        }

        [HttpGet]
        public ActionResult ReportView(string id)
        {
            try
            {
                string ReportHead = "";
                string rptLocation = "";
                string ImageFileName = "";
                ReportDocument doc = new ReportDocument();
                DataSet ds = new DataSet();              
                ApplicantInfoRepo _Repo = new ApplicantInfoRepo();

                ds = _Repo.ApplicantReport(id);

                ReportHead = "There are no data to Preview for GL Transaction for Bank Deposit";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ReportHead = "Applicant Info";
                    ImageFileName = ds.Tables[0].Rows[0]["ImageFileName"].ToString();
                }
                ds.Tables[0].TableName = "dtApplicantInfo";

                rptLocation = AppDomain.CurrentDomain.BaseDirectory + @"Files\ReportFiles\Common\\ApplicantInformation.rpt";

                doc.Load(rptLocation);
                doc.SetDataSource(ds);
                string ApplicantImage = AppDomain.CurrentDomain.BaseDirectory + "Files\\EmployeeInfo\\" + ImageFileName;
                string companyLogo = AppDomain.CurrentDomain.BaseDirectory + "Images\\COMPANYLOGO.png";
                FormulaFieldDefinitions ffds = doc.DataDefinition.FormulaFields;
                CompanyRepo _CompanyRepo = new CompanyRepo();
                CompanyVM cvm = _CompanyRepo.SelectAll().FirstOrDefault();

                doc.DataDefinition.FormulaFields["ReportHeaderA4"].Text = "'" + companyLogo + "'";
                doc.DataDefinition.FormulaFields["ApplicantImage"].Text = "'" + ApplicantImage + "'";
                doc.DataDefinition.FormulaFields["ReportHead"].Text = "'" + ReportHead + "'";              
                doc.DataDefinition.FormulaFields["Address"].Text = "'" + cvm.Address + "'";
                doc.DataDefinition.FormulaFields["CompanyName"].Text = "'" + cvm.Name + "'";
                
                if (ds.Tables[1].Rows.Count > 0)
                {
                    doc.Subreports["SubReportEdu"].SetDataSource(ds.Tables[1]);
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    doc.Subreports["SubReportPQ"].SetDataSource(ds.Tables[2]);
                }
                if (ds.Tables[3].Rows.Count > 0)
                {
                    doc.Subreports["SubReportTS"].SetDataSource(ds.Tables[3]);
                }
                if (ds.Tables[4].Rows.Count > 0)
                {
                    doc.Subreports["SubReportLS"].SetDataSource(ds.Tables[4]);
                }
                if (ds.Tables[5].Rows.Count > 0)
                {
                    doc.Subreports["SubReportEH"].SetDataSource(ds.Tables[5]);
                }
                if (ds.Tables[6].Rows.Count > 0)
                {
                    doc.Subreports["SubReportSkl"].SetDataSource(ds.Tables[6]);
                }
                var rpt = RenderReportAsPDF(doc);
                doc.Close();
                return rpt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private FileStreamResult RenderReportAsPDF(ReportDocument rptDoc)
        {
            Stream stream = rptDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/PDF");
        }

    }
}
