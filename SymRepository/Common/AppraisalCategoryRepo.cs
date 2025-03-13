using SymServices.Common;
using SymViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymRepository.Common
{
    public class AppraisalCategoryRepo
    {

        public List<AppraisalCategoryVM> SelectAll()
        {
            try
            {
                return new AppraisalCategoryDAL().SelectAll();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string[] Insert(AppraisalCategoryVM vm)
        {
            try
            {
                return new AppraisalCategoryDAL().Insert(vm, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public AppraisalCategoryVM SelectById(int id)
        {
            try
            {
                return new AppraisalCategoryDAL().SelectById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[] Edit(AppraisalCategoryVM vm)
        {
            try
            {
                return new AppraisalCategoryDAL().Edit(vm, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[] Delete(AppraisalCategoryVM vm, string[] Ids)
        {
            try
            {
                return new AppraisalCategoryDAL().Delete(vm, Ids, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
    }
}
