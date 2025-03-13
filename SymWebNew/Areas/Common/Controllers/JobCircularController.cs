using SymOrdinary;
using SymRepository.Enum;
using SymViewModel.Common;
using SymViewModel.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using SymRepository.Common;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using SymWebUI.Models;
using System.Net;
using System.Net.Mail;
using System.IO;

namespace SymWebUI.Areas.Common.Controllers
{
    public class JobCircularController : Controller
    {
        //
        // GET: /Enum/JobCircular/
        SymUserRoleRepo _reposur = new SymUserRoleRepo();
        ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
        JobCircularRepo _repo = new JobCircularRepo();
        #region Actions
        public ActionResult Index()
        {
            Session["permission"] = _reposur.SymRoleSession(identity.UserId, "1_17", "index").ToString();
            var getAllData = _repo.SelectAll();

            return View(getAllData);
        }



        public ActionResult _index(JQueryDataTableParamVM param)
        {
            #region Column Search
            var idFilter = Convert.ToString(Request["sSearch_0"]);
            var NameFilter = Convert.ToString(Request["sSearch_1"]);
            var IsActiveFilter = Convert.ToString(Request["sSearch_2"]);
            var remarksFilter = Convert.ToString(Request["sSearch_3"]);
            var IsActiveFilter1 = IsActiveFilter.ToLower() == "active" ? true.ToString() : false.ToString();
            #endregion Column Search

            var getAllData = _repo.SelectAll();
            IEnumerable<JobCircularVM> filteredData;

            if (!string.IsNullOrEmpty(param.sSearch))
            {

                var isSearchable1 = Convert.ToBoolean(Request["bSearchable_1"]);
                var isSearchable2 = Convert.ToBoolean(Request["bSearchable_2"]);
                var isSearchable3 = Convert.ToBoolean(Request["bSearchable_3"]);
                var isSearchable4 = Convert.ToBoolean(Request["bSearchable_4"]);
                var isSearchable5 = Convert.ToBoolean(Request["bSearchable_5"]);
                var isSearchable6 = Convert.ToBoolean(Request["bSearchable_6"]);

                filteredData = getAllData
                   .Where(c => isSearchable1 && c.Id.ToLower().Contains(param.sSearch.ToLower())
                               || isSearchable2 && c.JobTitle.ToLower().Contains(param.sSearch.ToLower())
                               || isSearchable3 && c.DesignationName.ToLower().Contains(param.sSearch.ToLower())
                               || isSearchable4 && c.Expriance.ToLower().Contains(param.sSearch.ToLower())
                               || isSearchable5 && c.Deadline.ToLower().Contains(param.sSearch.ToLower())
                               || isSearchable6 && c.IsActive.ToString().ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredData = getAllData;
            }
            //#endregion Column Filtering
            var isSortable_1 = Convert.ToBoolean(Request["bSortable_1"]);
            var isSortable_2 = Convert.ToBoolean(Request["bSortable_2"]);
            var isSortable_3 = Convert.ToBoolean(Request["bSortable_3"]);
            var isSortable_4 = Convert.ToBoolean(Request["bSortable_4"]);
            var isSortable_5 = Convert.ToBoolean(Request["bSortable_5"]);
            var isSortable_6 = Convert.ToBoolean(Request["bSortable_6"]);
            var isSortable_7 = Convert.ToBoolean(Request["bSortable_7"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<JobCircularVM, string> orderingFunction = (c => sortColumnIndex == 1 && isSortable_1 ? c.Id :
                                                           sortColumnIndex == 2 && isSortable_2 ? c.JobTitle :
                                                           sortColumnIndex == 3 && isSortable_3 ? c.DesignationName :
                                                           sortColumnIndex == 4 && isSortable_4 ? c.Expriance :
                                                           sortColumnIndex == 5 && isSortable_5 ? c.Deadline :
                                                           sortColumnIndex == 6 && isSortable_6 ? c.IsActive.ToString() :
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
                             , c.JobTitle
                             , c.DesignationName
                             , c.Expriance
                             , c.Deadline
                          
                             , Convert.ToString(c.IsActive == true ? "Active" : "Inactive") 
                       
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
        public ActionResult Create()
        {
            Session["permission"] = _reposur.SymRoleSession(identity.UserId, "1_17", "add").ToString();
            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(JobCircularVM vm)
        {
            string[] result = new string[6];
            ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
            vm.CreatedAt = DateTime.Now.ToString("yyyyMMddHHmmss");
            vm.CreatedBy = identity.Name;
            vm.CreatedFrom = identity.WorkStationIP;
            try
            {
                result = _repo.Insert(vm);
                Session["result"] = result[0] + "~" + result[1];
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Save Succeessfully";
                FileLogger.Log(result[0].ToString() + Environment.NewLine + result[2].ToString() + Environment.NewLine + result[5].ToString(), this.GetType().Name, result[4].ToString() + Environment.NewLine + result[3].ToString());
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            Session["permission"] = _reposur.SymRoleSession(identity.UserId, "1_17", "edit").ToString();
            JobCircularVM vm = new JobCircularVM();
            vm = _repo.SelectById(id);
            return View(vm);
            //return PartialView(_repo.SelectById(id));
        }

        [HttpPost]
        public ActionResult Edit(JobCircularVM vm, string btn)
        {
            string[] result = new string[6];
            ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
            vm.LastUpdateAt = DateTime.Now.ToString("yyyyMMddHHmmss");
            vm.LastUpdateBy = identity.Name;
            vm.LastUpdateFrom = identity.WorkStationIP;
            try
            {

                result = _repo.Update(vm);
                Session["result"] = result[0] + "~" + result[1];
                return RedirectToAction("Index");

            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Updated";
                FileLogger.Log(result[0].ToString() + Environment.NewLine + result[2].ToString() + Environment.NewLine + result[5].ToString(), this.GetType().Name, result[4].ToString() + Environment.NewLine + result[3].ToString());
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public ActionResult EmailSend(string JobCercularId)
        {
            JobCircularVM vm = new JobCircularVM();
            vm.Id = JobCercularId;

            return PartialView("~/Areas/Common/Views/JobCircular/EmailSend.cshtml", vm);
        }
      

        public ActionResult EmailSendApplicant(string ApplicantId, string Email)
        {
            ApplicantInfoVM vm = new ApplicantInfoVM();
            vm.Email = Email;
            vm.Id = ApplicantId;

            return PartialView("~/Areas/Common/Views/JobCircular/EmailSendApplicant.cshtml", vm);
        }
        public ActionResult EmailSendToApplicant(ApplicantInfoVM vm)
        {
            ApplicantInfoVM vms = new ApplicantInfoVM();
            vms = _repo.ApplicantProfile(vm.Id);
            vms.EmailText = vm.EmailText;

            thread = new Thread(unused => EmpEmailProcessToApplicant(vms));
            thread.Start();

            return RedirectToAction("ApplicantProfileList", new { JobId = vms.Jobid });
        }
        public void EmpEmailProcessToApplicant(ApplicantInfoVM vm)
        {
            try
            {
                EmailSettings ems = new EmailSettings();
                SettingRepo _SettingRepo = new SettingRepo();

                #region Mail Data Assign
                ems.ServerName = _SettingRepo.settingValue("Mail", "ServerName");
                ems.MailFromAddress = _SettingRepo.settingValue("Mail", "MailFromAddress");
                ems.Password = _SettingRepo.settingValue("Mail", "MailFromPSW");
                ems.UserName = ems.MailFromAddress;
                ems.MailHeader = "Job cercular general message";
                string mailbody = vm.EmailText;
            
                #endregion

                ems.MailToAddress = vm.Email;

                #region Email Sending

                if (!string.IsNullOrWhiteSpace(ems.MailToAddress))
                {
                    ems.MailBody = mailbody;
                    ems.FileName = "Job Cercular";
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
                         
                            smpt.Send(mailmessage);
                            mailmessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                        }
                        Thread.Sleep(500);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                FileLogger.Log("EmpEmailProcess", this.GetType().Name, "Email sending failed: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }
        public ActionResult Delete(string JobCercularId)
        {
            Session["permission"] = _reposur.SymRoleSession(identity.UserId, "1_17", "delete").ToString();
            JobCircularVM vm = new JobCircularVM();

            string[] result = new string[6];

            vm.LastUpdateAt = DateTime.Now.ToString("yyyyMMddHHmmss");
            vm.LastUpdateBy = identity.Name;
            vm.LastUpdateFrom = identity.WorkStationIP;
            vm.Id = JobCercularId;
            try
            {
                result = _repo.Delete(vm);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Deleted";
                return RedirectToAction("Index");
            }
        }
        #endregion Actions

        private static Thread thread;
        [HttpPost]
        public ActionResult EmailSendTo(JobCircularVM vm)
        {
            CompanyRepo _CompanyRepo = new CompanyRepo();
            CompanyVM cvm = _CompanyRepo.SelectAll().FirstOrDefault();

            ReportDocument rptDoc = new ReportDocument();
            // Generate data for the report
            DataSet ds = new DataSet();
            DataTable dt1 = new DataTable("dtJobCercular");
            ds.Tables.Add(dt1);
            ds = _repo.SelectByIdForReport(vm.Id);

            //string rptLocation = AppDomain.CurrentDomain.BaseDirectory + @"Files\ReportFiles\HRM\JobCercular.rpt";
            //rptDoc.Load(rptLocation);
            //string companyLogo = AppDomain.CurrentDomain.BaseDirectory + "Images\\COMPANYLOGO.png";
            //rptDoc.DataDefinition.FormulaFields["CompanyLogo"].Text = "'" + companyLogo + "'";
            //rptDoc.DataDefinition.FormulaFields["Address"].Text = "'" + cvm.Address + "'";
            //rptDoc.DataDefinition.FormulaFields["CompanyName"].Text = "'" + cvm.Name + "'";

            //rptDoc.SetDataSource(ds);

            //var rpt = RenderReportAsPDF(rptDoc);

            //rptDoc.Close();

            // return rpt;

            thread = new Thread(unused => EmpEmailProcess(vm, rptDoc));
            thread.Start();

            return RedirectToAction("Index");
        }
        public void EmpEmailProcess(JobCircularVM vm, ReportDocument rptDoc)
        {
            try
            {
                //DataSet DsTemp = new DataSet();
                //DataTable dt1 = new DataTable();

                string email = vm.EmailSendTo;
                vm = _repo.SelectById(vm.Id);
                vm.EmailSendTo = email;
                EmailSettings ems = new EmailSettings();
                SettingRepo _SettingRepo = new SettingRepo();

                #region Mail Data Assign

                ems.MailFromAddress = _SettingRepo.settingValue("Mail", "MailFromAddress");
                ems.Password = _SettingRepo.settingValue("Mail", "MailFromPSW");
                ems.UserName = ems.MailFromAddress;
                ems.MailHeader = _SettingRepo.settingValue("Mail", "MailSubjectJobCercular");
                string mailbody = _SettingRepo.settingValue("Mail", "MailBodyJobCercular");
                mailbody = mailbody.Replace("\\n", Environment.NewLine);
                mailbody = mailbody.Replace("vPostname", vm.JobTitle);

                #endregion

                ems.MailToAddress = vm.EmailSendTo;

                #region Email Sending

                if (!string.IsNullOrWhiteSpace(ems.MailToAddress))
                {
                    //rptDoc.SetDataSource(DsTemp);
                    //dt1.TableName = "dtJobCercular";

                    ems.MailBody = mailbody;
                    ems.FileName = "Job Cercular";
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
                            //  mailmessage.Attachments.Add(new Attachment(rptDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat), ems.FileName + ".pdf"));

                            smpt.Send(mailmessage);
                            mailmessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                        }


                        Thread.Sleep(500);
                    }
                }
                #endregion

                //rptDoc.Close();
                //rptDoc.Dispose();
            }
            catch (Exception ex)
            {
                FileLogger.Log("EmpEmailProcess", this.GetType().Name, "Email sending failed: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }

        private FileStreamResult RenderReportAsPDF(ReportDocument rptDoc)
        {
            Stream stream = rptDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/PDF");
        }


        #region Education
        public ActionResult _indexEducation(JQueryDataTableParamVM param, string Id)//EmployeeId
        {
            ReqEmployeeEducationRepo _empEdRepo = new ReqEmployeeEducationRepo();
            var getAllData = _empEdRepo.SelectAllByEmployeeId(Id);
            IEnumerable<ReqEmployeeEducationVM> filteredData;
            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {

                //Optionally check whether the columns are searchable at all 
                var isSearchable1 = Convert.ToBoolean(Request["bSearchable_1"]);
                var isSearchable2 = Convert.ToBoolean(Request["bSearchable_2"]);
                var isSearchable3 = Convert.ToBoolean(Request["bSearchable_3"]);
                var isSearchable4 = Convert.ToBoolean(Request["bSearchable_4"]);

                filteredData = getAllData
                   .Where(c => isSearchable1 && c.Degree_E.ToLower().Contains(param.sSearch.ToLower())
                               ||
                               isSearchable2 && c.Major.ToLower().Contains(param.sSearch.ToLower())
                               ||
                                isSearchable3 && c.Institute.ToLower().Contains(param.sSearch.ToLower())
                               ||
                               isSearchable4 && c.YearOfPassing.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredData = getAllData;
            }

            var isSortable_1 = Convert.ToBoolean(Request["bSortable_1"]);
            var isSortable_2 = Convert.ToBoolean(Request["bSortable_2"]);
            var isSortable_3 = Convert.ToBoolean(Request["bSortable_3"]);
            var isSortable_4 = Convert.ToBoolean(Request["bSortable_4"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<ReqEmployeeEducationVM, string> orderingFunction = (c => sortColumnIndex == 1 && isSortable_1 ? c.Degree_E :
                                                           sortColumnIndex == 2 && isSortable_2 ? c.Major :
                                                           sortColumnIndex == 3 && isSortable_3 ? c.Institute :
                                                           sortColumnIndex == 3 && isSortable_4 ? c.YearOfPassing :
                                                           "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredData = filteredData.OrderBy(orderingFunction);
            else
                filteredData = filteredData.OrderByDescending(orderingFunction);

            var displayedCompanies = filteredData.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in displayedCompanies
                         select new[] 
                         { 
                             Convert.ToString(c.Id)
                             , c.Degree_E //+ "~" + Convert.ToString(c.Id)
                             , c.Major
                             , c.Institute
                             , c.YearOfPassing 
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
        public ActionResult Education(int Id)
        {
            ReqEmployeeEducationRepo _infoRepo = new ReqEmployeeEducationRepo();
            JobCircularVM vm = new JobCircularVM();//_infoRepo.SelectById(EmployeeId);

            if (Id != 0)
            {
                vm.educationVM = new ReqEmployeeEducationRepo().SelectById(Id);
            }
            else
            {
                ReqEmployeeEducationVM edu = new ReqEmployeeEducationVM();
                vm.educationVM = edu;
            }
            return PartialView("_editEducation", vm);
        }

        [Authorize(Roles = "Master,Admin,Account")]
        [HttpPost]
        public ActionResult Education(JobCircularVM vm, HttpPostedFileBase EducationF)
        {
            ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
            Session["permission"] = _reposur.SymRoleSession(identity.UserId, "1_18", "edit").ToString();
            string[] retResults = new string[6];
            ReqEmployeeEducationRepo empEduApp = new ReqEmployeeEducationRepo();
            ReqEmployeeEducationVM edu = new ReqEmployeeEducationVM();
            edu = vm.educationVM;
            if (EducationF != null && EducationF.ContentLength > 0)
            {
                edu.FileName = EducationF.FileName;
            }
            if (edu.Id <= 0)
            {
                edu.CreatedAt = DateTime.Now.ToString("yyyyMMddHHmmss");
                edu.CreatedBy = identity.Name;
                edu.CreatedFrom = identity.WorkStationIP;
                retResults = empEduApp.Insert(edu);
            }
            else
            {
                edu.LastUpdateAt = DateTime.Now.ToString("yyyyMMddHHmmss");
                edu.LastUpdateBy = identity.Name;
                edu.LastUpdateFrom = identity.WorkStationIP;
                retResults = empEduApp.Update(edu);
            }
            if (EducationF != null && EducationF.ContentLength > 0)
            {
                var path = Path.Combine(Server.MapPath("~/Files/Education"), retResults[2] + EducationF.FileName);
                EducationF.SaveAs(path);
            }
            var mgs = retResults[0] + "~" + retResults[1];
            //Session["mgs"] = "mgs";
            Session["result"] = mgs;

            return RedirectToAction("Index", new { Id = edu.EmployeeId, mgs = mgs });
            //return Json(mgs, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Master,Admin,Account")]
        public JsonResult EducationDelete(string ids)
        {
            ReqEmployeeEducationRepo empEduApp = new ReqEmployeeEducationRepo();
            ReqEmployeeEducationVM vm = new ReqEmployeeEducationVM();

            ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
            string[] a = ids.Split('~');
            string[] result = new string[6];

            vm.LastUpdateAt = DateTime.Now.ToString("yyyyMMddHHmmss");
            vm.LastUpdateBy = identity.Name;
            vm.LastUpdateFrom = identity.WorkStationIP;
            result = empEduApp.Delete(vm, a);
            return Json(result[1], JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult JobDashboard()
        {
              List<JobCircularVM> vm = new List<JobCircularVM>();
              vm = _repo.JobDashboard();
          return View(vm);
        }
        public ActionResult ApplicantProfileList(string JobId)
        {           
            List<ApplicantInfoVM> vms = new List<ApplicantInfoVM>();
            vms = _repo.ApplicantProfileList(JobId);
         


            return View(vms);
        }
        public ActionResult EditShotListed(string Id, string Jobid)
        {
            string[] result = new string[6];
            ApplicantInfoVM vm = new ApplicantInfoVM();
            vm.Id = Id;
            vm.Jobid = Jobid;

            result = _repo.EditApplicantStatus(Id);

            Session["result"] = result[0] + "~" + result[1];
            return RedirectToAction("ApplicantProfileList", new { JobId = vm.Jobid });
        }
        public ActionResult EditRejected(string Id, string Jobid)
        {
            string[] result = new string[6];
            ApplicantInfoVM vm = new ApplicantInfoVM();
            vm.Id = Id;
            vm.Jobid = Jobid;

            result = _repo.EditApplicantStatusReject(Id);

            Session["result"] = result[0] + "~" + result[1];
            return RedirectToAction("ApplicantProfileList", new { JobId = vm.Jobid });
        }
        public ActionResult ApplicantProfile(string Id)
        {
            ApplicantInfoVM vm = new ApplicantInfoVM();
            vm = _repo.ApplicantProfile(Id);
            vm.educationVMS = _repo.ApplicantED(Id);
            vm.professionalQualificationVMS = _repo.ApplicantPQ(Id);
            vm.applicantTrainingVMS = _repo.ApplicantTS(Id);
            vm.applicantLanguageVMS = _repo.ApplicantLS(Id);
            vm.applicantEmployeementHistoryVMS = _repo.ApplicantTEH(Id);
            vm.applicantSkillVMS = _repo.ApplicantSK(Id);
            return View(vm);
        }
        public ActionResult ApplicantApplyEdit(string Id)
        {
            ApplicantInfoVM vm = new ApplicantInfoVM();     
           
              vm = _repo.ApplicantProfile(Id);
              vm.educationVMS = _repo.ApplicantED(Id);
              vm.professionalQualificationVMS = _repo.ApplicantPQ(Id);
              vm.applicantTrainingVMS = _repo.ApplicantTS(Id);
              vm.applicantLanguageVMS = _repo.ApplicantLS(Id);
              vm.applicantEmployeementHistoryVMS = _repo.ApplicantTEH(Id);
              vm.applicantSkillVMS = _repo.ApplicantSK(Id);
            return View(vm);
        }


        public ActionResult InsertQualificationDetails(ApplicantInfoVM vm)
        {
            ApplicantInfoRepo _repo = new ApplicantInfoRepo();

            string[] result = new string[6];
            vm.CreatedAt = DateTime.Now.ToString("yyyyMMddHHmmss");
            vm.IsActive = true;
            vm.IsApproved = false;
           
            try
            {
                result = _repo.InsertQualificationDetails(vm);

                if (result[0] == "Success")
                {
                    Session["result"] = result[0] + "~" + result[1];
                }

               // return RedirectToAction("ApplicantApplyEdit",vm);
                return RedirectToAction("ApplicantApplyEdit", new { Id = vm.Id }); 

            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Succeessfully!";
                return View(vm);
            }
        }

        public ActionResult ApplicantApplyInsertEdit(ApplicantInfoVM vm, HttpPostedFileBase AttachmentFile, HttpPostedFileBase AttachmentImaage)
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
            if (AttachmentImaage != null)
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
                return RedirectToAction("ApplicantApplyEdit", new { Id = vm.Id });

            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Succeessfully!";
                return View(vm);
            }
        }
        public ActionResult InsertApplicantTrainingDetails(ApplicantInfoVM vm)
        {
            ApplicantInfoRepo _repo = new ApplicantInfoRepo();

            string[] result = new string[6];
            vm.CreatedAt = DateTime.Now.ToString("yyyyMMddHHmmss");
            vm.IsActive = true;
            vm.IsApproved = false;

            try
            {
                result = _repo.InsertApplicantTrainingDetails(vm);

                if (result[0] == "Success")
                {
                    Session["result"] = result[0] + "~" + result[1];
                }
                return RedirectToAction("ApplicantApplyEdit", new { Id = vm.Id });                
            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Succeessfully!";
                return View(vm);
            }
        }
        public ActionResult InsertApplicantLanguageDetails(ApplicantInfoVM vm)
        {
            ApplicantInfoRepo _repo = new ApplicantInfoRepo();

            string[] result = new string[6];
            vm.CreatedAt = DateTime.Now.ToString("yyyyMMddHHmmss");
            vm.IsActive = true;
            vm.IsApproved = false;

            try
            {
                result = _repo.InsertApplicantLanguageDetails(vm);

                if (result[0] == "Success")
                {
                    Session["result"] = result[0] + "~" + result[1];
                }
                return RedirectToAction("ApplicantApplyEdit", new { Id = vm.Id });                
            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Succeessfully!";
                return View(vm);
            }
        }
        public ActionResult InsertApplicantEmployeementHistoryDetails(ApplicantInfoVM vm)
        {
            ApplicantInfoRepo _repo = new ApplicantInfoRepo();

            string[] result = new string[6];
            vm.CreatedAt = DateTime.Now.ToString("yyyyMMddHHmmss");
            vm.IsActive = true;
            vm.IsApproved = false;

            try
            {
                result = _repo.InsertApplicantEmployeementHistoryDetails(vm);

                if (result[0] == "Success")
                {
                    Session["result"] = result[0] + "~" + result[1];
                }
                return RedirectToAction("ApplicantApplyEdit", new { Id = vm.Id });
            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Succeessfully!";
                return View(vm);
            }
        }
        public ActionResult InsertApplicantSkillDetails(ApplicantInfoVM vm)
        {
            ApplicantInfoRepo _repo = new ApplicantInfoRepo();

            string[] result = new string[6];
            vm.CreatedAt = DateTime.Now.ToString("yyyyMMddHHmmss");
            vm.IsActive = true;
            vm.IsApproved = false;

            try
            {
                result = _repo.InsertApplicantSkillDetails(vm);

                if (result[0] == "Success")
                {
                    Session["result"] = result[0] + "~" + result[1];
                }
                return RedirectToAction("ApplicantApplyEdit", new { Id = vm.Id }); 
            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Succeessfully!";
                return View(vm);
            }
        }
        public ActionResult InsertApplicantEducationlDetails(ApplicantInfoVM vm)
        {
            ApplicantInfoRepo _repo = new ApplicantInfoRepo();

            string[] result = new string[6];
            vm.CreatedAt = DateTime.Now.ToString("yyyyMMddHHmmss");
            vm.IsActive = true;
            vm.IsApproved = false;

            try
            {
                result = _repo.InsertApplicantEducationlDetails(vm);

                if (result[0] == "Success")
                {
                    Session["result"] = result[0] + "~" + result[1];
                }

                return RedirectToAction("ApplicantApplyEdit", new { Id = vm.Id });            

            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Succeessfully!";
                return View(vm);
            }
        }

        public ActionResult MarkForApplicant(string Id, string Jobid)
        {
            ApplicantInfoVM vm = new ApplicantInfoVM();

            return PartialView("~/Areas/Common/Views/JobCircular/MarkForApplicant.cshtml", vm);
        }

        public ActionResult InsertApplicantMarks(ApplicantInfoVM vm)
        {
            string[] result = new string[6];
            ApplicantInfoVM vms = new ApplicantInfoVM();
            vms = _repo.ApplicantProfile(vm.Id);

            result = _repo.InsertApplicantMarks(vm);

            return RedirectToAction("ApplicantProfileList", new { JobId = vms.Jobid });
        }



        public ActionResult SalaryForApplicant(string Id, string Jobid)
        {
            ApplicantInfoVM vm = new ApplicantInfoVM();

            return PartialView("~/Areas/Common/Views/JobCircular/SalaryForApplicant.cshtml", vm);
        }

        public ActionResult InsertApplicantSalary(ApplicantInfoVM vm)
        {
            string[] result = new string[6];
            ApplicantInfoVM vms = new ApplicantInfoVM();
            vms = _repo.ApplicantProfile(vm.Id);

            result = _repo.InsertApplicantSalary(vm);

            return RedirectToAction("ApplicantProfileList", new { JobId = vms.Jobid });
        }

        public ActionResult DeleteEducation(string Id,string ApplicantId)
        {

            ApplicantInfoRepo _repo = new ApplicantInfoRepo();

            string[] result = new string[6];
         
            try
            {
                result = _repo.DeleteEducationlDetails(Id);

                if (result[0] == "Success")
                {
                    Session["result"] = result[0] + "~" + result[1];
                }

                return RedirectToAction("ApplicantApplyEdit", new { Id = ApplicantId }); 

            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Succeessfully!";
                return View();
            }
           
        }
        public ActionResult DeleteProfessional(string Id, string ApplicantId)
        {

            ApplicantInfoRepo _repo = new ApplicantInfoRepo();

            string[] result = new string[6];

            try
            {
                result = _repo.DeleteProfessionalDetails(Id);

                if (result[0] == "Success")
                {
                    Session["result"] = result[0] + "~" + result[1];
                }

                return RedirectToAction("ApplicantApplyEdit", new { Id = ApplicantId });

            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Succeessfully!";
                return View();
            }

        }

        public ActionResult DeleteTraining(string Id, string ApplicantId)
        {

            ApplicantInfoRepo _repo = new ApplicantInfoRepo();

            string[] result = new string[6];

            try
            {
                result = _repo.DeleteTrainingDetails(Id);

                if (result[0] == "Success")
                {
                    Session["result"] = result[0] + "~" + result[1];
                }

                return RedirectToAction("ApplicantApplyEdit", new { Id = ApplicantId });

            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Succeessfully!";
                return View();
            }

        }

        public ActionResult DeleteLanguage(string Id, string ApplicantId)
        {

            ApplicantInfoRepo _repo = new ApplicantInfoRepo();

            string[] result = new string[6];

            try
            {
                result = _repo.DeleteLanguageDetails(Id);

                if (result[0] == "Success")
                {
                    Session["result"] = result[0] + "~" + result[1];
                }

                return RedirectToAction("ApplicantApplyEdit", new { Id = ApplicantId });

            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Succeessfully!";
                return View();
            }

        }
        public ActionResult DeleteEmployeementHistory(string Id, string ApplicantId)
        {

            ApplicantInfoRepo _repo = new ApplicantInfoRepo();

            string[] result = new string[6];

            try
            {
                result = _repo.DeleteEmployeementHistoryDetails(Id);

                if (result[0] == "Success")
                {
                    Session["result"] = result[0] + "~" + result[1];
                }

                return RedirectToAction("ApplicantApplyEdit", new { Id = ApplicantId });

            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Succeessfully!";
                return View();
            }

        }
        public ActionResult DeleteSkill(string Id, string ApplicantId)
        {

            ApplicantInfoRepo _repo = new ApplicantInfoRepo();

            string[] result = new string[6];

            try
            {
                result = _repo.DeleteSkillDetails(Id);

                if (result[0] == "Success")
                {
                    Session["result"] = result[0] + "~" + result[1];
                }

                return RedirectToAction("ApplicantApplyEdit", new { Id = ApplicantId });

            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Succeessfully!";
                return View();
            }

        }

        public ActionResult DeleteMarks(string Id, string Jobid)
        {

            ApplicantInfoRepo _repo = new ApplicantInfoRepo();

            string[] result = new string[6];

            try
            {
                result = _repo.DeleteMarks(Id);

                if (result[0] == "Success")
                {
                    Session["result"] = result[0] + "~" + result[1];
                }

                return RedirectToAction("ApplicantProfileList", new { JobId = Jobid });

            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Succeessfully!";
                return View();
            }

        }
        public ActionResult DeleteSalary(string Id, string Jobid)
        {

            ApplicantInfoRepo _repo = new ApplicantInfoRepo();

            string[] result = new string[6];

            try
            {
                result = _repo.DeleteSalary(Id);

                if (result[0] == "Success")
                {
                    Session["result"] = result[0] + "~" + result[1];
                }

                return RedirectToAction("ApplicantProfileList", new { JobId = Jobid });

            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Succeessfully!";
                return View();
            }

        }
    }
}
