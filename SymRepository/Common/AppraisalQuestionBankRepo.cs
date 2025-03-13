using SymServices.Common;
using SymViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymRepository.Common
{
    public class AppraisalQuestionBankRepo
    {

        //==================SelectAll=================
        public List<AppraisalQuestionBankVM> SelectAll()
        {
            try
            {
                return new AppraisalQuestionBankDAL().SelectAll();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public AppraisalQuestionBankVM SelectById(int id)
        {
            try
            {
                return new AppraisalQuestionBankDAL().SelectById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[] Insert(AppraisalQuestionBankVM vm)
        {
            try
            {
                return new AppraisalQuestionBankDAL().Insert(vm, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[] Edit(AppraisalQuestionBankVM vm)
        {
            try
            {
                return new AppraisalQuestionBankDAL().Edit(vm, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[] Delete(AppraisalQuestionBankVM vm, int Id)
        {
            try
            {
                return new AppraisalQuestionBankDAL().Delete(vm,Id, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
