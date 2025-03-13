using SymServices.Common;
using SymViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymRepository.Common
{
    public class AppraisalScheduleRepo
    {
        public List<AppraisalScheduleVM> SelectAll(int Id=0)
        {
            try
            {
                return new AppraisalScheduleDAL().SelectAll(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[] Insert(AppraisalScheduleVM vm)
        {
            try
            {
                return new AppraisalScheduleDAL().Insert(vm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
