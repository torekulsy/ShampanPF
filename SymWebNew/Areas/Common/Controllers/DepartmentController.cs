﻿using JQueryDataTables.Models;
using SymOrdinary;
using SymRepository.Common;
using SymViewModel.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Web;
using SymRepository.HRM;
using SymViewModel.HRM;
using System.Data;

namespace SymWebUI.Areas.Common.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        //
        // GET: /Common/Bank/
        ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
        SymUserRoleRepo _reposur = new SymUserRoleRepo();
        DepartmentRepo _repo = new DepartmentRepo();
        //#region Actions
        public ActionResult Index()
        {
            ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
            Session["permission"] = _reposur.SymRoleSession(identity.UserId, "1_5", "index").ToString();
            return View();
        }
        public ActionResult _index(JQueryDataTableParamModel param)
        {
            #region Column Search
            var idFilter = Convert.ToString(Request["sSearch_0"]);
            var codeFilter = Convert.ToString(Request["sSearch_1"]);
            var nameFilter = Convert.ToString(Request["sSearch_2"]);
            var isActiveFilter = Convert.ToString(Request["sSearch_3"]);
            var remarksFilter = Convert.ToString(Request["sSearch_4"]);

            var isActiveFilter1 = isActiveFilter.ToLower() == "active" ? true.ToString() : false.ToString();
            #endregion Column Search

            #region Search and Filter Data

            var getAllData = _repo.SelectAll();
            IEnumerable<DepartmentVM> filteredData;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                var isSearchable1 = Convert.ToBoolean(Request["bSearchable_1"]);
                var isSearchable2 = Convert.ToBoolean(Request["bSearchable_2"]);
                var isSearchable3 = Convert.ToBoolean(Request["bSearchable_3"]);
                var isSearchable4 = Convert.ToBoolean(Request["bSearchable_4"]);

                filteredData = getAllData.Where(c => 
                    isSearchable1 && c.Code.ToLower().Contains(param.sSearch.ToLower())
                    || isSearchable2 && c.Name.ToLower().Contains(param.sSearch.ToLower())
                    || isSearchable3 && c.IsActive.ToString().ToLower().Contains(param.sSearch.ToLower())
                    || isSearchable4 && c.Remarks.ToLower().Contains(param.sSearch.ToLower())
                    );
            }
            else
            {
                filteredData = getAllData;
            }
            #endregion Search and Filter Data

            #region Column Filtering
            if (codeFilter != "" || nameFilter != "" || isActiveFilter != "" || remarksFilter != "")
            {
                filteredData = filteredData.Where(c => 
                    (codeFilter == "" || c.Code.ToLower().Contains(codeFilter.ToLower()))
                    && (nameFilter == "" || c.Name.ToLower().Contains(nameFilter.ToLower()))
                    && (isActiveFilter == "" || c.IsActive.ToString().ToLower().Contains(isActiveFilter1.ToLower()))
                    && (remarksFilter == "" || c.Remarks.ToLower().Contains(remarksFilter.ToLower()))
                    );
            }

            #endregion Column Filtering


            var isSortable_1 = Convert.ToBoolean(Request["bSortable_1"]);
            var isSortable_2 = Convert.ToBoolean(Request["bSortable_2"]);
            var isSortable_3 = Convert.ToBoolean(Request["bSortable_3"]);
            var isSortable_4 = Convert.ToBoolean(Request["bSortable_4"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<DepartmentVM, string> orderingFunction = (c =>
                sortColumnIndex == 1 && isSortable_1 ? c.Code :
                sortColumnIndex == 2 && isSortable_2 ? c.Name :
                sortColumnIndex == 3 && isSortable_3 ? c.IsActive.ToString() :
                sortColumnIndex == 4 && isSortable_4 ? c.Remarks :
                "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredData = filteredData.OrderBy(orderingFunction);
            else
                filteredData = filteredData.OrderByDescending(orderingFunction);

            var displayedCompanies = filteredData.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in displayedCompanies select new[] { 
                Convert.ToString(c.Id)
                , c.Code//+"~"+c.Name+"~"+(c.IsActive  ? "Active" : "Inactive")+"~"+c.Remarks
                , c.Name
                , c.IsActive ? "Active" : "Inactive" 
                , c.Remarks
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

        [Authorize(Roles = "Master,Admin,Account")]
        [HttpGet]
        public ActionResult Create()
        {
            ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
            Session["permission"] = _reposur.SymRoleSession(identity.UserId, "1_5", "add").ToString();
            return PartialView();
        }

        [Authorize(Roles = "Master,Admin,Account")]
        [HttpPost]
        public ActionResult Create(DepartmentVM vm)
        {
            string[] result = new string[6];
            ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
            vm.CreatedAt = DateTime.Now.ToString("yyyyMMddHHmmss");
            vm.CreatedBy = identity.Name;
            vm.CreatedFrom = identity.WorkStationIP;
            vm.BranchId = Convert.ToInt32(identity.BranchId);
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
        [Authorize(Roles = "Master,Admin,Account")]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
            Session["permission"] = _reposur.SymRoleSession(identity.UserId, "1_5", "edit").ToString();
            DepartmentVM vm = new DepartmentVM();
            vm = _repo.SelectById(id);
            return PartialView(vm);
        }

        [Authorize(Roles = "Master,Admin,Account")]
        [HttpPost]
        public ActionResult Edit(DepartmentVM vm, string btn)
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

        [Authorize(Roles = "Master,Admin,Account")]
        public ActionResult Delete(string id)
        {
            
            ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
            Session["permission"] = _reposur.SymRoleSession(identity.UserId, "1_5", "delete").ToString();
            DepartmentVM vm = new DepartmentVM();
            vm.LastUpdateAt = DateTime.Now.ToString("yyyyMMddHHmmss");
            vm.LastUpdateBy = identity.Name;
            vm.LastUpdateFrom = identity.WorkStationIP;
            vm.Id = id;
            try
            {
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                Session["result"] = "Fail~Data Not Deleted";
                return RedirectToAction("Index");
            }

        }
        [Authorize(Roles = "Master,Admin,Account")]
        public JsonResult DepartmentDelete(string ids)
        {
            DepartmentVM vm = new DepartmentVM();

            ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
            Session["permission"] = _reposur.SymRoleSession(identity.UserId, "1_5", "delete").ToString();
            string[] a = ids.Split('~');
            string[] result = new string[6];

            vm.LastUpdateAt = DateTime.Now.ToString("yyyyMMddHHmmss");
            vm.LastUpdateBy = identity.Name;
            vm.LastUpdateFrom = identity.WorkStationIP;
            result = _repo.Delete(vm, a);
            return Json(result[1], JsonRequestBehavior.AllowGet);
        }
        public JsonResult Areer(string value,string date,string employeeId)
        {
            
            return Json("successed", JsonRequestBehavior.AllowGet);
        }
       
        //#endregion Actions


        public ActionResult Import()
        {
            return View();
        }
        public ActionResult ExportExcell(ExportImportVM VM)
        {
            identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
            SymUserRoleRepo _reposur = new SymUserRoleRepo();
            DataTable dt = new DataTable();
            ExcelPackage excel = new ExcelPackage();

            try
            {

                ExportImportRepo _repo = new ExportImportRepo();

                dt = _repo.SelectDepartmentInfo(VM);

                #region Excel

                string filename = "DepartmentInfo Data";
                var workSheet = excel.Workbook.Worksheets.Add("Departmentinfo Data");
                workSheet.Cells[1, 1].LoadFromDataTable(dt, true);
                using (var memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //Response.AddHeader("content-disposition", "attachment;  filename=" + FileName);
                    Response.AddHeader("content-disposition", "attachment;  filename=" + filename + ".xlsx");
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }

                #endregion

            }
            catch (Exception ex)
            {
                Session["result"] = "Fail" + "~" + ex.Message.Replace("\r", "").Replace("\n", "");
                return RedirectToAction("Index");
            }

            finally { }
            return RedirectToAction("Index");

            // return Json(rVM, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadExcel(DepartmentVM vm)
        {
            string[] result = new string[6];
            try
            {
                vm.CreatedAt = DateTime.Now.ToString("yyyyMMddHHmmss");
                vm.CreatedBy = identity.Name;
                vm.CreatedFrom = identity.WorkStationIP;

                result = new DepartmentRepo().ImportExcelFile(vm);
                Session["result"] = result[0] + "~" + result[1];
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                Session["result"] = result[0] + "~" + result[1];
                FileLogger.Log(result[0].ToString() + Environment.NewLine + result[2].ToString() + Environment.NewLine + result[5].ToString(), this.GetType().Name, result[4].ToString() + Environment.NewLine + result[3].ToString());
                return RedirectToAction("Index");
            }
        }
    }
}
