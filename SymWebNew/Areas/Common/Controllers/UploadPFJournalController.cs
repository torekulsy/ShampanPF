using Excel;
using SymOrdinary;
using SymRepository.Common;
using SymServices.Common;
using SymViewModel.Common;
using SymViewModel.PF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SymWebUI.Areas.Common.Controllers
{
    public class UploadPFJournalController : Controller
    {
        //
        // GET: /Common/UploadPFJournal/

        public ActionResult Index()
        {
            return View();
        }
        SymUserRoleRepo _reposur = new SymUserRoleRepo();
        ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
        public ActionResult ImportExcel(ExportImportVM VM)
        {
            string msg = string.Empty;
            string message =string.Empty;
            string msgEmptyData = string.Empty;

            string[] result = new string[6];
            try
            {
                ExportImportRepo _repo = new ExportImportRepo();
                GLJournalDetailVM vm = new GLJournalDetailVM();

                string CompanyName = new AppSettingsReader().GetValue("CompanyName", typeof(string)).ToString();
           
                string tableName = "";
                var permission = _reposur.SymRoleSession(identity.UserId, "1_38", "add").ToString();

                Session["permission"] = permission;
                if (permission == "False")
                {
                    return Redirect("/Common/Home");
                }

                #region file Copy i into Folder and read
                string fullPath = AppDomain.CurrentDomain.BaseDirectory + "Files\\Export\\" + VM.file.FileName;
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                if (VM.file != null && VM.file.ContentLength > 0)
                {
                    VM.file.SaveAs(fullPath);
                }
                DataSet ds = new DataSet();               
                DataTable dt = new System.Data.DataTable();
                DataTable dtAcc = new DataTable();
                FileStream stream = System.IO.File.Open(fullPath, FileMode.Open, FileAccess.Read);              
                IExcelDataReader reader = null;
                if (VM.file.FileName.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (VM.file.FileName.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                reader.IsFirstRowAsColumnNames = true;
                ds = reader.AsDataSet();
                dt = ds.Tables[0];
                reader.Close();
                #endregion
                tableName = dt.TableName;    
                foreach (DataRow item in dt.Rows)
                {                                       
                    dtAcc = _repo.GetAccName(item["Account Name"].ToString());

                    if (dtAcc.Rows.Count==0)
                    {
                        msg += item["Account Name"].ToString()+", ";
                        continue;                      
                    }                  
                }               
                #region Save Header and Details

                if (string.IsNullOrEmpty(msg))
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        dtAcc = _repo.GetAccName(item["Account Name"].ToString());
                        string COAId= dtAcc.Rows[0]["Id"].ToString();
                      
                        vm.GLJournalId = Convert.ToInt32(Regex.Replace(item["Voucher No"].ToString(), "[^0-9]", ""));

                        vm.GLJournalId = vm.GLJournalId;
                        vm.TransactionDate = item["Date"].ToString().Trim();
                        vm.COAName = COAId;
                        vm.TransactionType = 1;
                        vm.JournalType = 1;
                        vm.DrAmount = Convert.ToDecimal(item["Dr Amount"].ToString().Trim());
                        vm.CrAmount = Convert.ToDecimal(item["Cr Amount"].ToString().Trim());

                        if (vm.DrAmount > 0)
                        {
                            vm.IsDr = true;
                        }
                        else
                        {
                            vm.IsDr = false;
                        }

                        vm.Remarks = item["Narrration"].ToString().Trim();
                        vm.TransType = "PF";
                        vm.IsYearClosing = false;

                        result = _repo.InsertPFJournalTemp(vm);
                    }

                    DataTable dtHead = _repo.GetAccHeader();
                    if(dtHead.Rows.Count>0)
                    {
                        foreach (DataRow item in dtHead.Rows)
                        {
                            GLJournalVM jvm = new GLJournalVM();
                            jvm.Id = Convert.ToInt32(item["Id"].ToString());
                            DateTime formattedDate =Convert.ToDateTime(item["TransactionDate"].ToString());
                           jvm.Code = "JV-001/" + item["Id"].ToString();
                           jvm.TransactionDate = formattedDate.ToString("yyyyMMdd");
                           jvm.JournalType = 1;
                           jvm.TransactionType = 1;
                           jvm.TransactionValue =Convert.ToDecimal(item["TransactionValue"].ToString());
                           jvm.Remarks = item["Remarks"].ToString();
                           jvm.IsActive = true;
                           jvm.IsArchive = false;
                           jvm.CreatedBy = "admin";
                           jvm.CreatedAt = "";
                           jvm.CreatedFrom = "";
                           jvm.LastUpdateBy ="admin";                       
                           jvm.Post = false;
                           jvm.TransType = "PF";                         
                           jvm.IsYearClosing = false;

                           result = _repo.InsertHeader(jvm);
                           if (result[0] == "Success")
                           {
                               DataTable dtdetails = _repo.GetAccDetailsTemp(jvm.Id);

                               foreach (DataRow items in dtdetails.Rows)
                               {   
                                   string COAId = items["COAName"].ToString();
                                   vm.COAId = Convert.ToInt32(COAId);
                                   vm.TransactionDate = items["TransactionDate"].ToString().Trim();                           
                                   vm.TransactionType = 1;
                                   vm.JournalType = 1;
                                   vm.DrAmount = Convert.ToDecimal(items["DrAmount"].ToString().Trim());
                                   vm.CrAmount = Convert.ToDecimal(items["CrAmount"].ToString().Trim());
                                   if (vm.DrAmount > 0)
                                   {
                                       vm.IsDr = true;
                                   }
                                   else
                                   {
                                       vm.IsDr = false;
                                   }

                                   vm.Remarks = items["Remarks"].ToString().Trim();
                                   vm.TransType = "PF";
                                   vm.IsYearClosing = false;

                                   result = _repo.InsertPFJournalDetails(vm);
                               }

                           }
                        }
                    }
                }
                #endregion
                if (msgEmptyData == "" && msg == "")
                {
                    result[0] = "Success";
                    result[1] = "Import successfully";
                }
                else
                {
                    result[0] = "Fail";
                    result[1] ="Please Check Account Name "+ msg;
                }

                Session["result"] = result[0] + "~" + result[1];
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                result[0] = "Fail";
                Session["result"] = result[0] + "~" + ex.Message;
                return RedirectToAction("Index");
            }

        }
    }
}
