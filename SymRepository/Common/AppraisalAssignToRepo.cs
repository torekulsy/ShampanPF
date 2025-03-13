using SymServices.Common;
using SymViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymRepository.Common
{
    public class AppraisalAssignToRepo
    {
        public List<AppraisalAssignToVM> SelectAll()
        {
            try
            {
                return new AppraisalAssignToDAL().SelectAll();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string[] Insert(AppraisalAssignToVM vm)
        {
            try
            {
                return new AppraisalAssignToDAL().Insert(vm, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AppraisalAssignToVM SelectById(int id)
        {
            try
            {
                return new AppraisalAssignToDAL().SelectById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[] Edit(AppraisalAssignToVM vm)
        {
            try
            {
                return new AppraisalAssignToDAL().Edit(vm, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[] Delete(AppraisalAssignToVM vm, string[] Ids)
        {
            try
            {
                return new AppraisalAssignToDAL().Delete(vm, Ids, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
