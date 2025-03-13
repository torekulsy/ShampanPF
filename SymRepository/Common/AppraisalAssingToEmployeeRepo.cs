using SymServices.Common;
using SymViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymRepository.Common
{
    public class AppraisalAssingToEmployeeRepo
    {
        public List<AppraisalQuestionSetVM> SelectAll(int Id = 0)
        {
            try
            {
                return new AppraisalAssingToEmployeeDAL().SelectAll(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[] Insert(AppraisalQuestionSetVM vm)
        {
            try
            {
                return new AppraisalAssingToEmployeeDAL().Insert(vm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<AppraisalQuestionSetDetailVM> SelectAllQuestionByEmployeeExist(string did, string EmpCode, string EvaluationFor)
        {
            try
            {
                return new AppraisalAssingToEmployeeDAL().SelectAllQuestionByEmployeeExist(did, EmpCode, EvaluationFor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AppraisalQuestionSetDetailVM> SelectAllQuestionByDepartmentExist(string EmpCode)
        {
            try
            {
                return new AppraisalAssingToEmployeeDAL().SelectAllQuestionByDepartmentExist(EmpCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<AppraisalQuestionSetDetailVM> SelectAllQuestionByDepartment(string DepartmentId)
        {
            try
            {

                return new AppraisalAssingToEmployeeDAL().SelectAllQuestionByDepartment(DepartmentId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[] Delete(AppraisalCategoryVM vm)
        {
            try
            {
                return new AppraisalAssingToEmployeeDAL().Delete(vm, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
