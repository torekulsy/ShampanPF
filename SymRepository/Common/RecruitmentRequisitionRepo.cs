using SymServices.Common;
using SymViewModel.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymRepository.Common
{
    public class RecruitmentRequisitionRepo
    {

        public string[] InsertRecruitmentRequisition(RecruitmentRequisitionVM vm)
        {
            try
            {
                return new RecruitmentRequisitionDAL().InsertRecruitmentRequisition(vm, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<RecruitmentRequisitionVM> SelectAllRecruitmentRequisition()
        {
            try
            {
                return new RecruitmentRequisitionDAL().SelectAllRecruitmentRequisition();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public RecruitmentRequisitionVM SelectById(int id)
        {
            try
            {
                return new RecruitmentRequisitionDAL().SelectById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string[] ApprovedRecruitmentRequisition(int id)
        {
            try
            {
                return new RecruitmentRequisitionDAL().ApprovedRecruitmentRequisition(id, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[] DeleteRecruitmentRequisition(int Id)
        {
            try
            {
                return new RecruitmentRequisitionDAL().DeleteRecruitmentRequisition(Id, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
