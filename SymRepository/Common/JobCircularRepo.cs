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
   public class JobCircularRepo
    {
        #region Methods
        //==================SelectAll=================
       public List<JobCircularVM> DropDown()
        {
            try
            {
                return new JobCircularDAL().SelectAll();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //==================SelectAll=================
        public List<JobCircularVM> SelectAll()
        {
            try
            {
                return new JobCircularDAL().SelectAll();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //==================SelectByID=================
        public JobCircularVM SelectById(string Id)
        {
            try
            {
                return new JobCircularDAL().SelectById(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet SelectByIdForReport(string Id)
        {
            try
            {
                return new JobCircularDAL().SelectByIdForReport(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //==================Insert =================
        public string[] Insert(JobCircularVM JobCircularVM)
        {
            try
            {
                return new JobCircularDAL().Insert(JobCircularVM, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //==================Update =================
        public string[] Update(JobCircularVM JobCircularVM)
        {
            try
            {
                return new JobCircularDAL().Update(JobCircularVM, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        //==================Delete =================
        public string[] Delete(JobCircularVM JobCircularVM)
        {
            try
            {
                return new JobCircularDAL().Delete(JobCircularVM, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        public List<JobCircularVM> JobDashboard()
        {
            try
            {
                return new JobCircularDAL().JobDashboard();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ApplicantInfoVM> ApplicantProfileList(string JobId)
        {
            try
            {
                return new JobCircularDAL().ApplicantProfileList(JobId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ApplicantInfoVM ApplicantProfile(string Id)
        {
            try
            {
                return new JobCircularDAL().ApplicantProfile(Id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string[] ApplicantApplyEdit(ApplicantInfoVM vm)
        {

            try
            {
                return new JobCircularDAL().ApplicantApplyEdit(vm, null, null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ProfessionalQualificationVM> ApplicantPQ(string Id)
        {
            try
            {
                return new JobCircularDAL().ApplicantPQ(Id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ApplicantTrainingVM> ApplicantTS(string Id)
        {
            try
            {
                return new JobCircularDAL().ApplicantTS(Id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<ApplicantLanguageVM> ApplicantLS(string Id)
        {
            try
            {
                return new JobCircularDAL().ApplicantLS(Id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ApplicantEmployeementHistoryVM> ApplicantTEH(string Id)
        {
            try
            {
                return new JobCircularDAL().ApplicantTEH(Id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ApplicantSkillVM> ApplicantSK(string Id)
        {
            try
            {
                return new JobCircularDAL().ApplicantSK(Id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<EducationVM> ApplicantED(string Id)
        {
            try
            {
                return new JobCircularDAL().ApplicantED(Id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
       
        public string[] EditApplicantStatus(string Id)
        {
            try
            {
                return new JobCircularDAL().EditApplicantStatus(Id,null,null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public string[] EditApplicantStatusReject(string Id)
        {
            try
            {
                return new JobCircularDAL().EditApplicantStatusReject(Id, null, null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string[] InsertApplicantMarks(ApplicantInfoVM vm)
        {
            try
            {
                return new JobCircularDAL().InsertApplicantMarks(vm, null, null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string[] InsertApplicantSalary(ApplicantInfoVM vm)
        {
            try
            {
                return new JobCircularDAL().InsertApplicantSalary(vm, null, null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
