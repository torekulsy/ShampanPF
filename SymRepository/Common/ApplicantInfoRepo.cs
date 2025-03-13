using SymServices.Common;
using SymViewModel.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SymRepository.Common
{
    public class ApplicantInfoRepo
    {
        public string[] InsertApplicantInfo(ApplicantInfoVM vm)
        {
            try
            {
                return new ApplicantInfoDAL().InsertApplicantInfo(vm, null, null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public string[] EditApplicantInterview(ApplicantInfoVM vm)
        {
            try
            {
                return new ApplicantInfoDAL().EditApplicantInterview(vm, null, null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public string[] EditApplicantMarkSetup(ApplicantInfoVM vm)
        {
            try
            {
                return new ApplicantInfoDAL().EditApplicantMarkSetup(vm, null, null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        public List<ApplicantInfoVM> SelectAllApplicantInfo()
        {
            try
            {
                return new ApplicantInfoDAL().SelectAllApplicantInfo();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ApplicantInfoVM> SelectAllForInterviewCall()
        {
            try
            {
                return new ApplicantInfoDAL().SelectAllForInterviewCall();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ApplicantInfoVM> SelectAllForMarkSetup()
        {
            try
            {
                return new ApplicantInfoDAL().SelectAllForMarkSetup();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ApplicantInfoVM> SelectAllForConfirm()
        {
            try
            {
                return new ApplicantInfoDAL().SelectAllForConfirm();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }     
        
        public ApplicantInfoVM SelectById(int id)
        {
            try
            {
                return new ApplicantInfoDAL().SelectById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[] DeleteApplicantInfo(int id)
        {
            try
            {
                return new ApplicantInfoDAL().DeleteApplicantInfo(id,null,null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public string[] ApproveApplicantInfo(int id)
        {
            try
            {
                return new ApplicantInfoDAL().ApproveApplicantInfo(id, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[] UpdateShortlistedApplicantInfo(int id)
        {
            try
            {
                return new ApplicantInfoDAL().UpdateShortlistedApplicantInfo(id, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public System.Data.DataTable GetApplicantInfo(int id)
        {
            try
            {
                return new ApplicantInfoDAL().GetApplicantInfo(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ApplicantInfoVM GetCompanyInfo()
        {
            try
            {
                return new ApplicantInfoDAL().GetCompanyInfo();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataSet ApplicantReport(string id)
        {
            try
            {
                return new ApplicantInfoDAL().ApplicantReport(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string[] InsertQualificationDetails(ApplicantInfoVM vm)
        {
            try
            {
                return new ApplicantInfoDAL().InsertQualificationDetails(vm,null,null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string[] InsertApplicantTrainingDetails(ApplicantInfoVM vm)
        {
            try
            {
                return new ApplicantInfoDAL().InsertApplicantTrainingDetails(vm, null, null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public string[] InsertApplicantLanguageDetails(ApplicantInfoVM vm)
        {
            try
            {
                return new ApplicantInfoDAL().InsertApplicantLanguageDetails(vm, null, null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string[] InsertApplicantEmployeementHistoryDetails(ApplicantInfoVM vm)
        {
            try
            {
                return new ApplicantInfoDAL().InsertApplicantEmployeementHistoryDetails(vm, null, null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string[] InsertApplicantSkillDetails(ApplicantInfoVM vm)
        {
            try
            {
                return new ApplicantInfoDAL().InsertApplicantSkillDetails(vm, null, null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string[] InsertApplicantEducationlDetails(ApplicantInfoVM vm)
        {
            try
            {
                return new ApplicantInfoDAL().InsertApplicantEducationlDetails(vm, null, null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string[] DeleteEducationlDetails(string Id)
        {
            try
            {
                return new ApplicantInfoDAL().DeleteEducationlDetails(Id, null, null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string[] DeleteProfessionalDetails(string Id)
        {
            try
            {
                return new ApplicantInfoDAL().DeleteProfessionalDetails(Id, null, null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string[] DeleteTrainingDetails(string Id)
        {
            try
            {
                return new ApplicantInfoDAL().DeleteTrainingDetails(Id, null, null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string[] DeleteLanguageDetails(string Id)
        {
            try
            {
                return new ApplicantInfoDAL().DeleteLanguageDetails(Id, null, null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string[] DeleteEmployeementHistoryDetails(string Id)
        {
            try
            {
                return new ApplicantInfoDAL().DeleteEmployeementHistoryDetails(Id, null, null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string[] DeleteSkillDetails(string Id)
        {
            try
            {
                return new ApplicantInfoDAL().DeleteSkillDetails(Id, null, null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string[] DeleteMarks(string Id)
        {
            try
            {
                return new ApplicantInfoDAL().DeleteMarks(Id, null, null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public string[] DeleteSalary(string Id)
        {
            try
            {
                return new ApplicantInfoDAL().DeleteSalary(Id, null, null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
