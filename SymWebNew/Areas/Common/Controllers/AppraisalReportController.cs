using CrystalDecisions.CrystalReports.Engine;
using SymRepository.Common;
using SymViewModel.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SymWebUI.Areas.Common.Controllers
{
    public class AppraisalReportController : Controller
    {
        //
        // GET: /Common/AppraisalReport/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Report(string[] codeFrom)
        {
          
            try
            {

                string result = "('" + string.Join("','",codeFrom)+ "')";
                string EmployeeId = result.Replace(",", "','");

             
                #region Data CallD:\HRM\ShampanHRM\SymWebNew\Areas\Common\Controllers\AppraisalReportController.cs

                ReportDocument doc = new ReportDocument();
                AppraisalMarksRepo _empRepo = new AppraisalMarksRepo();
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                dt = _empRepo.AppraisalEvaluationReport(EmployeeId);
                dt1 = _empRepo.GetAppraisalWeightage();
                string[] assign = new string[10];
                foreach (var item in dt1.Rows)
                {
                    assign[0] = dt1.Rows[0]["Weightage"].ToString();
                    assign[1] = dt1.Rows[1]["Weightage"].ToString();
                    assign[2] = dt1.Rows[2]["Weightage"].ToString();
                    assign[3] = dt1.Rows[3]["Weightage"].ToString();
                    assign[4] = dt1.Rows[4]["Weightage"].ToString();
                    assign[5] = dt1.Rows[5]["Weightage"].ToString();
                    assign[6] = dt1.Rows[6]["Weightage"].ToString();
                    assign[7] = dt1.Rows[7]["Weightage"].ToString();
                    assign[8] = dt1.Rows[8]["Weightage"].ToString();
                    assign[9] = dt1.Rows[9]["Weightage"].ToString();                   
                }             


                  dt.TableName = "dtAppraisalReport";

                #endregion

                #region Report Call

                string ReportHead = "";
                if (dt == null)
                {
                    ReportHead = "There are no data to Preview for Leave Application From";
                }
                string rptLocation = "";
                rptLocation = AppDomain.CurrentDomain.BaseDirectory + @"Files\ReportFiles\Common\rptEmployeeAppraisalEvaluation.rpt";

                CompanyRepo _CompanyRepo = new CompanyRepo();
                CompanyVM cvm = _CompanyRepo.SelectAll().FirstOrDefault();

                string companyLogo = AppDomain.CurrentDomain.BaseDirectory + "Images\\COMPANYLOGO.png";

                doc.Load(rptLocation);
                companyLogo = AppDomain.CurrentDomain.BaseDirectory + "Images\\COMPANYLOGO.png";
                doc.DataDefinition.FormulaFields["Address"].Text = "'" + cvm.Address + "'";
                doc.DataDefinition.FormulaFields["CompanyName"].Text = "'" + cvm.Name + "'";
                doc.DataDefinition.FormulaFields["CompanyLogo"].Text = "'" + companyLogo + "'";
                doc.DataDefinition.FormulaFields["ReportHead"].Text = "'" + ReportHead + "'";

                doc.DataDefinition.FormulaFields["wOwn"].Text = "'" + assign[0] + "'";
                doc.DataDefinition.FormulaFields["wTeamLead"].Text = "'" + assign[1] + "'";
                doc.DataDefinition.FormulaFields["wHR"].Text = "'" + assign[2] + "'";
                doc.DataDefinition.FormulaFields["wCOO"].Text = "'" + assign[3] + "'";
                doc.DataDefinition.FormulaFields["wMD"].Text = "'" + assign[4] + "'";
                doc.DataDefinition.FormulaFields["wP1"].Text = "'" + assign[5] + "'";
                doc.DataDefinition.FormulaFields["wP2"].Text = "'" + assign[6] + "'";
                doc.DataDefinition.FormulaFields["wP3"].Text = "'" + assign[7] + "'";
                doc.DataDefinition.FormulaFields["wP4"].Text = "'" + assign[8] + "'";
                doc.DataDefinition.FormulaFields["wP5"].Text = "'" + assign[9] + "'";       

                #endregion


                doc.SetDataSource(dt);
              
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
