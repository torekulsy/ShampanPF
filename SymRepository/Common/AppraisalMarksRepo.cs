using SymServices.Common;
using SymViewModel.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymRepository.Common
{
    public class AppraisalMarksRepo
    {
        public List<AppraisalMarksDetailVM> SelectAllQuestionByEmployeeExist(string AssignFrom, string did, string EmpCode)
        {
            try
            {
                return new AppraisalMarksDAL().SelectAllQuestionByEmployeeExist(AssignFrom, did, EmpCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AppraisalMarksDetailVM> SelectMarksByEmployeeExist(string did, string EmpCode, string EvaluationForId, string AssignFrom)
        {
            try
            {
                return new AppraisalMarksDAL().SelectMarksByEmployeeExist(did, EmpCode, EvaluationForId, AssignFrom);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AppraisalMarksDetailVM> SelectAllQuestionByDepartment(string DepartmentId)
        {
            try
            {

                return new AppraisalMarksDAL().SelectAllQuestionByDepartment(DepartmentId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AppraisalQuestionSetDetailVM> GetUesrForAppraisal()
        {
            try
            {

                return new AppraisalMarksDAL().GetUesrForAppraisal();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AppraisalQuestionSetDetailVM> GetUesrForAppraisalExist()
        {
            try
            {

                return new AppraisalMarksDAL().GetUesrForAppraisalExist();
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
                return new AppraisalMarksDAL().Insert(vm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[] InsertAdminMarks(AppraisalMarksVM vm)
        {
            try
            {
                return new AppraisalMarksDAL().InsertAdminMarks(vm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable AppraisalEvaluationReport(string codeFrom)
        {
            try
            {
                return new AppraisalMarksDAL().AppraisalEvaluationReport(codeFrom);
            }
            catch (Exception ex)
            {
                throw ex;
            }          
        }

        public DataTable GetAppraisalWeightage()
        {
            try
            {
                return new AppraisalMarksDAL().GetAppraisalWeightage();
            }
            catch (Exception ex)
            {
                throw ex;
            }       
        }

        public DataTable PrintSheet(AppraisalMarksVM vm)
        {
            try
            {
                return new AppraisalMarksDAL().PrintSheet(vm);
            }
            catch (Exception ex)
            {
                throw ex;
            }      
        }

        public List<AppraisalHeightMarksVM> GetAppraisalHeightMarks()
        {
            try
            {
                return new AppraisalMarksDAL().GetAppraisalHeightMarks();
            }
            catch (Exception ex)
            {
                throw ex;
            }      
        }
    }
}
