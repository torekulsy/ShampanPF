using SymServices.Common;
using SymViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymRepository.Common
{
    public class AppraisalQuestionSetRepo
    {

        public List<AppraisalQuestionSetVM> SelectAll(int Id = 0)
        {
            try
            {
                return new AppraisalQuestionSetDAL().SelectAll(Id);
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
                return new AppraisalQuestionSetDAL().Insert(vm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AppraisalQuestionSetDetailVM> SelectAllDetails(int Id)
        {
            try
            {

                return new AppraisalQuestionSetDAL().SelectAllDetails(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<AppraisalCheckBoxVM> SelectApprisalCheckBox()
        {
            try
            {

                return new AppraisalQuestionSetDAL().SelectApprisalCheckBox();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<AppraisalQuestionSetDetailVM> SelectAllQuestionByDepartment(string Id)
        {
            try
            {

                return new AppraisalQuestionSetDAL().SelectAllQuestionByDepartment(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<AppraisalQuestionSetDetailVM> SelectAllQuestionByDepartmentExist(string Id)
        {
            try
            {

                return new AppraisalQuestionSetDAL().SelectAllQuestionByDepartmentExist(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[] DeleteAppraisalQuestionSet(int Id)
        {
            try
            {
                return new AppraisalQuestionSetDAL().DeleteAppraisalQuestionSet(Id, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<AppraisalQuestionSetDetailVM> SelectAll2()
        {
            try
            {
                return new AppraisalQuestionSetDAL().SelectAll2();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<AppraisalQuestionSetDetailVM> SelectAllByDepartmentId(string DepartmentId)
        {
            try
            {
                return new AppraisalQuestionSetDAL().SelectAllByDepartmentId(DepartmentId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
