using SymServices.Common;
using SymViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymRepository.Common
{
    public class AppraisalEvaluationForRepo
    {
        public List<AppraisalEvaluationFor> SelectAll()
        {
            try
            {
                return new AppraisalEvaluationDAL().SelectAll();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string[] Insert(AppraisalEvaluationFor vm)
        {
            try
            {
                return new AppraisalEvaluationDAL().Insert(vm, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AppraisalEvaluationFor SelectById(int id)
        {
            try
            {
                return new AppraisalEvaluationDAL().SelectById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[] Edit(AppraisalEvaluationFor vm)
        {
            try
            {
                return new AppraisalEvaluationDAL().Edit(vm, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[] Delete(AppraisalEvaluationFor vm, string[] Ids)
        {
            try
            {
                return new AppraisalEvaluationDAL().Delete(vm, Ids, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
