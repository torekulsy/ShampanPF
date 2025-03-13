using SymServices.HRM;
using SymViewModel.HRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymRepository.HRM
{
    public class AppraisalEvaluationRepo
    {
        public List<AppraisalEvaluationVM> SelectQuestionSetByDepartment(string Id)
        {
            try
            {
                return new AppraisalEvaluationDAL().SelectQuestionSetByDepartment(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AppraisalEvaluationDetailVM> SelectAllQuestionByDepartmentExist(string Id)
        {
            try
            {

                return new AppraisalEvaluationDAL().SelectAllQuestionByDepartmentExist(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public UserRoleForAppraisalVM GetUesrForAppraisalExist(string Code)
        {
            try
            {
                return new AppraisalEvaluationDAL().GetUesrForAppraisalExist(Code);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[] Insert(EmployeeInfoVM vm)
        {
            try
            {
                return new AppraisalEvaluationDAL().Insert(vm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AppraisalEvaluationDetailVM> SelectMarksByEmployeeExist(string EmployeeCode, string EvaluationFor)
        {
            try
            {
                return new AppraisalEvaluationDAL().SelectMarksByEmployeeExist(EmployeeCode, EvaluationFor,null,null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
