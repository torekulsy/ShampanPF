using SymServices.Common;
using SymViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymRepository.Common
{
   public class ReqEmployeeEducationRepo
    {
        #region Methods
        //==================SelectAll=================
        public List<ReqEmployeeEducationVM> SelectAll()
        {
            try
            {
                return new ReqEmployeeEducationDAL().SelectAll();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //==================SelectAll=================
        public List<ReqEmployeeEducationVM> SelectAllByEmployeeId(string EmployeeId)
        {
            try
            {
                return new ReqEmployeeEducationDAL().SelectAllByEmployeeId(EmployeeId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //==================SelectByID=================
        public ReqEmployeeEducationVM SelectById(int Id)
        {
            try
            {
                return new ReqEmployeeEducationDAL().SelectById(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //==================Insert =================
        public string[] Insert(ReqEmployeeEducationVM empEducationVM)
        {
            try
            {
                return new ReqEmployeeEducationDAL().Insert(empEducationVM, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //==================Update =================
        public string[] Update(ReqEmployeeEducationVM educationVM)
        {
            try
            {
                return new ReqEmployeeEducationDAL().Update(educationVM, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //==================Select =================
        //public EmployeeEducationVM Select(string query, int Id)//FITST, LAST, NEXT, PREVIOUS
        //{

        //    try
        //    {
        //        return new EmployeeEducationDAL().Select(query,Id);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //==================Delete =================
        public string[] Delete(ReqEmployeeEducationVM employeeEducationVM, string[] ids)
        {
            try
            {
                return new ReqEmployeeEducationDAL().Delete(employeeEducationVM, ids, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //==================SelectAllForReport=================
        public List<ReqEmployeeEducationVM> SelectAllForReport(string CodeF, string CodeT, string DepartmentId, string SectionId, string ProjectId
          , string DesignationId, string dtpFrom, string dtpTo, string EmployeeId)
        {
            try
            {
                return new ReqEmployeeEducationDAL().SelectAllForReport(CodeF, CodeT, DepartmentId, SectionId, ProjectId, DesignationId, dtpFrom, dtpTo, EmployeeId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion
    }
}
