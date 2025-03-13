using SymOrdinary;
using SymServices.Common;
using SymViewModel.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using JQueryDataTables.Models;
using System.Threading;
using System.Web.Mvc;



namespace SymServices.Common
{
    public class ApplicantInfoDAL
    {
        #region Global Variables
        private const string FieldDelimeter = DBConstant.FieldDelimeter;
        private DBSQLConnection _dbsqlConnection = new DBSQLConnection();
        CommonDAL _cDal = new CommonDAL();
        #endregion    
        public string[] InsertApplicantInfo(ApplicantInfoVM vm, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {

            #region Initializ
            string sqlText = "";
            int Id = 0;
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = Id.ToString();// Return Id
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "InsertApplicantInfo"; //Method Name
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            #endregion
            #region Try
            try
            {
                #region Validation
                //if (string.IsNullOrEmpty(ApplicantInfoVM.Id))
                //{
                //    retResults[1] = "Please Input ApplicantInfo";
                //    return retResults;
                //}
                #endregion Validation
                #region open connection and transaction
                #region New open connection and transaction
                if (VcurrConn != null)
                {
                    currConn = VcurrConn;
                }
                if (Vtransaction != null)
                {
                    transaction = Vtransaction;
                }
                #endregion New open connection and transaction
                if (currConn == null)
                {
                    currConn = _dbsqlConnection.GetConnection();
                    if (currConn.State != ConnectionState.Open)
                    {
                        currConn.Open();
                    }
                }
                if (transaction == null)
                {
                    transaction = currConn.BeginTransaction("");
                }
                #endregion open connection and transaction
                #region Save

                if (vm != null)
                {
                    if (vm.Id != null)
                    {
                        sqlText = "  ";
                        sqlText += @"Update ApplicantInfo  set                     
                           ApplicantName=@ApplicantName
                          ,Designation=@Designation
                          ,PresentAddress =@PresentAddress
                          ,PermanentAddress=@PermanentAddress
                          ,ContactNo=@ContactNo
                          ,Email=@Email  
                          ,LastEducation=@LastEducation
                          ,Gender=@Gender
                          ,Experience=@Experience
                          ,NoticePeriod=@NoticePeriod
                          ,PresentSalary=@PresentSalary
                          ,ExpectedSalary=@ExpectedSalary
                          ,CoverLetter=@CoverLetter                         
                          ,Studying=@Studying                       
                          ,LookingFor=@LookingFor
                          ,AvailableFor=@AvailableFor
                          ,FatherName=@FatherName
                          ,MotherName=@MotherName
                          ,MaritalStatus=@MaritalStatus
                          ,Nationality=@Nationality
                          ,Religion=@Religion
                          ,BloodGroup=@BloodGroup                       
                          ,Height=@Height
                          ,Weight=@Weight
                          ,FaceBook=@FaceBook
                          ,linkedIn=@linkedIn
                          ,VideoCv=@VideoCv   
                          ";
                        if (!string.IsNullOrEmpty(vm.AttachmentFile))
                        {
                            sqlText += ",AttachmentFile =@AttachmentFile ";
                        }
                        if (!string.IsNullOrEmpty(vm.ImageFileName))
                        {
                            sqlText += ",ImageFileName =@ImageFileName ";
                        }      

                        sqlText += " where Id=@Id ";
                      
                        SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                        cmdInsert.Parameters.AddWithValue("@Id", vm.Id);
                        cmdInsert.Parameters.AddWithValue("@ApplicantName", vm.ApplicantName);
                        cmdInsert.Parameters.AddWithValue("@Designation", vm.Designation);
                        cmdInsert.Parameters.AddWithValue("@PresentAddress", vm.PresentAddress);
                        cmdInsert.Parameters.AddWithValue("@PermanentAddress", vm.PermanentAddress);
                        cmdInsert.Parameters.AddWithValue("@ContactNo", vm.ContactNo);
                        cmdInsert.Parameters.AddWithValue("@Email", vm.Email);
                        cmdInsert.Parameters.AddWithValue("@LastEducation", vm.LastEducation);
                        cmdInsert.Parameters.AddWithValue("@Gender", vm.Gender);
                        cmdInsert.Parameters.AddWithValue("@Experience", vm.Experience);
                        cmdInsert.Parameters.AddWithValue("@NoticePeriod", vm.NoticePeriod ?? "");
                        cmdInsert.Parameters.AddWithValue("@PresentSalary", vm.PresentSalary);
                        cmdInsert.Parameters.AddWithValue("@ExpectedSalary", vm.ExpectedSalary);
                        cmdInsert.Parameters.AddWithValue("@Studying", vm.Studying );                      
                        cmdInsert.Parameters.AddWithValue("@LookingFor", vm.LookingFor);
                        cmdInsert.Parameters.AddWithValue("@AvailableFor", vm.AvailableFor);
                        cmdInsert.Parameters.AddWithValue("@FatherName", vm.FatherName);
                        cmdInsert.Parameters.AddWithValue("@MotherName", vm.MotherName);
                        cmdInsert.Parameters.AddWithValue("@DateOfBirth", vm.DateOfBirth);
                        cmdInsert.Parameters.AddWithValue("@MaritalStatus", vm.MaritalStatus);
                        cmdInsert.Parameters.AddWithValue("@CoverLetter", vm.CoverLetter);
                        cmdInsert.Parameters.AddWithValue("@Nationality", vm.Nationality);
                        cmdInsert.Parameters.AddWithValue("@Religion", vm.Religion);
                        cmdInsert.Parameters.AddWithValue("@BloodGroup", vm.BloodGroup);
                        cmdInsert.Parameters.AddWithValue("@Height", vm.Height);
                        cmdInsert.Parameters.AddWithValue("@Weight", vm.Weight);
                        cmdInsert.Parameters.AddWithValue("@FaceBook", vm.FaceBook);
                        cmdInsert.Parameters.AddWithValue("@linkedIn", vm.linkedIn);
                        cmdInsert.Parameters.AddWithValue("@VideoCv", vm.VideoCv);
                        if (!string.IsNullOrEmpty(vm.AttachmentFile))
                        {
                            cmdInsert.Parameters.AddWithValue("@AttachmentFile", vm.AttachmentFile);
                        }
                        if (!string.IsNullOrEmpty(vm.ImageFileName))
                        {
                            cmdInsert.Parameters.AddWithValue("@ImageFileName", vm.ImageFileName);
                        }
                                           
                        cmdInsert.ExecuteNonQuery();

                        //if (vm.educationVMS != null )
                        //{
                        //    InsertDetails(vm, currConn, transaction);
                        //}                        
                    }
                    else
                    {

                        sqlText = "  ";
                        sqlText += @" INSERT INTO ApplicantInfo(
                           [ApplicantName]
                          ,[Designation]
                          ,[PresentAddress]
                          ,[PermanentAddress]
                          ,[ContactNo]
                          ,[Email]
                          ,[LastEducation]
                          ,[Gender]
                          ,[Experience]
                          ,[NoticePeriod]
                          ,[PresentSalary]
                          ,[ExpectedSalary]
                          ,[AttachmentFile]
                          ,[CoverLetter]
                          ,[IsActive]
                          ,[IsArchive]  
                          ,IsApproved   
                          ,RecentCompany       
                          ,IsConfirmed 
                          ,Studying
                            ,EmploymentHistory
                            ,AcademicQualification
                            ,ProfessionalQualification
                            ,LookingFor
                            ,AvailableFor
                            ,FatherName
                            ,MotherName
                            ,DateOfBirth
                            ,MaritalStatus
                            ,Nationality
                            ,Religion
                            ,BloodGroup
                            ,JobId
                            ,ImageFileName

                        ) 
                           VALUES (
                           @ApplicantName
                          ,@Designation
                          ,@PresentAddress
                          ,@PermanentAddress
                          ,@ContactNo
                          ,@Email
                          ,@LastEducation
                          ,@Gender
                          ,@Experience
                          ,@NoticePeriod
                          ,@PresentSalary
                          ,@ExpectedSalary
                          ,@AttachmentFile
                          ,@CoverLetter
                          ,@IsActive
                          ,@IsArchive 
                          ,@IsApproved      
                          ,@RecentCompany     
                          ,@IsConfirmed        
                          ,@Studying     
                            ,@EmploymentHistory
                            ,@AcademicQualification
                            ,@ProfessionalQualification
                            ,@LookingFor
                            ,@AvailableFor
                            ,@FatherName
                            ,@MotherName
                            ,@DateOfBirth
                            ,@MaritalStatus
                            ,@Nationality
                            ,@Religion
                            ,@BloodGroup  
                            ,@JobId   
                            ,@ImageFileName                 
                        ) 
                         ";
                        SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                        cmdInsert.Parameters.AddWithValue("@ApplicantName", vm.ApplicantName);
                        cmdInsert.Parameters.AddWithValue("@Designation", vm.Designation);
                        cmdInsert.Parameters.AddWithValue("@PresentAddress", vm.PresentAddress);
                        cmdInsert.Parameters.AddWithValue("@PermanentAddress", vm.PermanentAddress);
                        cmdInsert.Parameters.AddWithValue("@ContactNo", vm.ContactNo);
                        cmdInsert.Parameters.AddWithValue("@Email", vm.Email);
                        cmdInsert.Parameters.AddWithValue("@LastEducation", vm.LastEducation);
                        cmdInsert.Parameters.AddWithValue("@Gender", vm.Gender);
                        cmdInsert.Parameters.AddWithValue("@Experience", vm.Experience);
                        cmdInsert.Parameters.AddWithValue("@NoticePeriod", vm.NoticePeriod ?? "");
                        cmdInsert.Parameters.AddWithValue("@PresentSalary", vm.PresentSalary);
                        cmdInsert.Parameters.AddWithValue("@ExpectedSalary", vm.ExpectedSalary);
                        cmdInsert.Parameters.AddWithValue("@AttachmentFile", vm.AttachmentFile);
                        cmdInsert.Parameters.AddWithValue("@CoverLetter", vm.CoverLetter);
                        cmdInsert.Parameters.AddWithValue("@IsActive", vm.IsActive);
                        cmdInsert.Parameters.AddWithValue("@IsArchive", vm.IsArchive);
                        cmdInsert.Parameters.AddWithValue("@IsApproved", vm.IsApproved);
                        cmdInsert.Parameters.AddWithValue("@RecentCompany", vm.RecentCompany);
                        cmdInsert.Parameters.AddWithValue("@IsConfirmed", false);
                        cmdInsert.Parameters.AddWithValue("@Studying", vm.Studying);
                        cmdInsert.Parameters.AddWithValue("@EmploymentHistory", vm.EmploymentHistory ?? "");
                        cmdInsert.Parameters.AddWithValue("@AcademicQualification", vm.AcademicQualification ?? "");
                        cmdInsert.Parameters.AddWithValue("@ProfessionalQualification", vm.ProfessionalQualification ?? "");
                        cmdInsert.Parameters.AddWithValue("@LookingFor", vm.LookingFor);
                        cmdInsert.Parameters.AddWithValue("@AvailableFor", vm.AvailableFor);
                        cmdInsert.Parameters.AddWithValue("@FatherName", vm.FatherName);
                        cmdInsert.Parameters.AddWithValue("@MotherName", vm.MotherName);
                        cmdInsert.Parameters.AddWithValue("@DateOfBirth", vm.DateOfBirth);
                        cmdInsert.Parameters.AddWithValue("@MaritalStatus", vm.MaritalStatus);
                        cmdInsert.Parameters.AddWithValue("@Nationality", vm.Nationality);
                        cmdInsert.Parameters.AddWithValue("@Religion", vm.Religion);
                        cmdInsert.Parameters.AddWithValue("@BloodGroup", vm.BloodGroup);
                        cmdInsert.Parameters.AddWithValue("@JobId", vm.Jobid ?? "");
                        cmdInsert.Parameters.AddWithValue("@ImageFileName", vm.ImageFileName);
                
                        cmdInsert.Transaction = transaction;
                        var exeRes = cmdInsert.ExecuteScalar();
                         Id = Convert.ToInt32(exeRes);

                        //if (vm.educationVM.Count > 0)
                        //{
                        //    InsertDetails(vm, currConn, transaction);
                        //}
                    }
                }
                else
                {
                    retResults[1] = "This ApplicantInfo already used";
                    throw new ArgumentNullException("Please Input ApplicantInfo Value", "");
                }


                #endregion User Create

                #region Commit
                if (Vtransaction == null)
                {
                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
                #endregion Commit
                #region SuccessResult
                retResults[0] = "Success";
                retResults[1] = "Your profile has been updated successfully";
                #endregion SuccessResult
            }
            #endregion try
            #region Catch and Finall
            catch (Exception ex)
            {
                retResults[0] = "Fail";//Success or Fail
                retResults[4] = ex.Message.ToString(); //catch ex
                if (Vtransaction != null)
                {
                    try
                    {
                        if (Vtransaction == null) { transaction.Rollback(); }
                    }
                    catch (Exception)
                    {
                        retResults[1] = "Unexpected error to update InsertApplicantInfo.";
                        return retResults;
                        // throw new ArgumentNullException("Unexpected error to update ApplicantInfo.", "ApplicantInfo");
                    }
                }
                return retResults;
            }
            finally
            {
                if (VcurrConn == null)
                {
                    if (currConn != null)
                    {
                        if (currConn.State == ConnectionState.Open)
                        {
                            currConn.Close();
                        }
                    }
                }
            }
            #endregion
            #region Results
            return retResults;
            #endregion
        }

        private string[] InsertDetails(ApplicantInfoVM vm, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {
            #region Initializ
            string sqlText = "";
            int NextId = 0;
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = NextId.ToString();// Return Id
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "InvestmentNameDetailsVM"; //Method Name
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            int transResult = 0;
            #endregion
            #region Try
            try
            {
                #region Validation
                #endregion Validation
                #region open connection and transaction
                #region New open connection and transaction
                if (VcurrConn != null)
                {
                    currConn = VcurrConn;
                }
                if (Vtransaction != null)
                {
                    transaction = Vtransaction;
                }
                #endregion New open connection and transaction
                if (currConn == null)
                {
                    currConn = _dbsqlConnection.GetConnection();
                    if (currConn.State != ConnectionState.Open)
                    {
                        currConn.Open();
                    }
                }
                if (transaction == null)
                {
                    transaction = currConn.BeginTransaction("");
                }
                #endregion open connection and transaction
                #region Save

                

                List<EducationVM> VMs =new List<EducationVM>();
                if (vm != null && vm.educationVMS.Count > 0)
                {
                    #region SqlText
                    sqlText = "  ";
                    sqlText += @" 
                                    INSERT INTO ApplicantEducation (                                
                                    ExamTitle
                                    ,Major
                                    ,Institute
                                    ,PassYear
                                    ,Duration
                                   ,Achievment
                                    ) VALUES (                                  
                                    @ExamTitle
                                    ,@Major
                                    ,@Institute
                                    ,@PassYear
                                    ,@Duration
                                    ,@Achievment
                                    ) 
                                    ";
                    #endregion SqlText

                    #region SqlExecution
                    foreach (EducationVM item in vm.educationVMS)
                    {
                        SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);                        
                        cmdInsert.Parameters.AddWithValue("@ExamTitle",item.ExamTitle);
                        cmdInsert.Parameters.AddWithValue("@Major",item.Major);
                        cmdInsert.Parameters.AddWithValue("@Institute",item.Institute);
                        cmdInsert.Parameters.AddWithValue("@PassYear",item.PassYear);
                        cmdInsert.Parameters.AddWithValue("@Duration",item.Duration);
                        cmdInsert.Parameters.AddWithValue("@Achievment",item.Achievment);
                        var exeRes = cmdInsert.ExecuteNonQuery();
                        transResult = Convert.ToInt32(exeRes);
                    }
                    #endregion SqlExecution
                }
                else
                {
                    retResults[1] = "No Detail Found!";
                    throw new ArgumentNullException(retResults[1], "");
                }
                #endregion Save
                #region Commit
                if (Vtransaction == null)
                {
                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
                #endregion Commit
                #region SuccessResult
                retResults[0] = "Success";
                retResults[1] = "Data Save Successfully.";
                retResults[2] = NextId.ToString();
                #endregion SuccessResult
            }
            #endregion try
            #region Catch and Finall
            catch (Exception ex)
            {
                retResults[0] = "Fail";//Success or Fail
                retResults[4] = ex.Message.ToString(); //catch ex
                if (Vtransaction == null) { transaction.Rollback(); }
                return retResults;
            }
            finally
            {
                if (VcurrConn == null)
                {
                    if (currConn != null)
                    {
                        if (currConn.State == ConnectionState.Open)
                        {
                            currConn.Close();
                        }
                    }
                }
            }
            #endregion
            #region Results
            return retResults;
            #endregion
        }

        public string[] InsertQualificationDetails(ApplicantInfoVM vm, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {         
            #region Initializ
            string sqlText = "";
            int NextId = 0;
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = NextId.ToString();// Return Id
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "InsertQualificationDetailsVM"; //Method Name
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            int transResult = 0;
            #endregion
            #region Try
            try
            {
                #region Validation
                #endregion Validation
                #region open connection and transaction
                #region New open connection and transaction
                if (VcurrConn != null)
                {
                    currConn = VcurrConn;
                }
                if (Vtransaction != null)
                {
                    transaction = Vtransaction;
                }
                #endregion New open connection and transaction
                if (currConn == null)
                {
                    currConn = _dbsqlConnection.GetConnection();
                    if (currConn.State != ConnectionState.Open)
                    {
                        currConn.Open();
                    }
                }
                if (transaction == null)
                {
                    transaction = currConn.BeginTransaction("");
                }
                #endregion open connection and transaction
                #region Save

                if (vm != null )
                {
                    #region SqlText
                    sqlText = "  ";
                    sqlText += @" 
                                   INSERT INTO ApplicantProfessionalQualification (                                
                                   Certification
                                  ,PQInstitute
                                  ,Location
                                  ,FromDate
                                  ,ToDate
                                  ,ApplicantId
                                    ) VALUES (                                  
                                    @Certification
                                    ,@PQInstitute
                                    ,@Location
                                    ,@FromDate
                                    ,@ToDate   
                                    ,@ApplicantId                              
                                    ) 
                                    ";
                    #endregion SqlText

                    #region SqlExecution
                 
                        SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                        cmdInsert.Parameters.AddWithValue("@Certification", vm.professionalQualificationVM.Certification);
                        cmdInsert.Parameters.AddWithValue("@PQInstitute", vm.professionalQualificationVM.PQInstitute);
                        cmdInsert.Parameters.AddWithValue("@Location", vm.professionalQualificationVM.Location);
                        cmdInsert.Parameters.AddWithValue("@FromDate", vm.professionalQualificationVM.FromDate);
                        cmdInsert.Parameters.AddWithValue("@ToDate", vm.professionalQualificationVM.ToDate);
                        cmdInsert.Parameters.AddWithValue("@ApplicantId", vm.Id);
                        
                        var exeRes = cmdInsert.ExecuteNonQuery();
                        transResult = Convert.ToInt32(exeRes);
                   
                    #endregion SqlExecution
                }
                else
                {
                    retResults[1] = "No Detail Found!";
                    throw new ArgumentNullException(retResults[1], "");
                }
                #endregion Save
                #region Commit
                if (Vtransaction == null)
                {
                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
                #endregion Commit
                #region SuccessResult
                retResults[0] = "Success";
                retResults[1] = "Data Save Successfully.";
                retResults[2] = NextId.ToString();
                #endregion SuccessResult
            }
            #endregion try
            #region Catch and Finall
            catch (Exception ex)
            {
                retResults[0] = "Fail";//Success or Fail
                retResults[4] = ex.Message.ToString(); //catch ex
                if (Vtransaction == null) { transaction.Rollback(); }
                return retResults;
            }
            finally
            {
                if (VcurrConn == null)
                {
                    if (currConn != null)
                    {
                        if (currConn.State == ConnectionState.Open)
                        {
                            currConn.Close();
                        }
                    }
                }
            }
            #endregion
            #region Results
            return retResults;
            #endregion
        }

        public string[] EditApplicantInterview(ApplicantInfoVM vm, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {

            #region Initializ
            string sqlText = "";
            int Id = 0;
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = Id.ToString();// Return Id
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "InsertApplicantInfo"; //Method Name
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            #endregion
            #region Try
            try
            {
                #region Validation
                //if (string.IsNullOrEmpty(ApplicantInfoVM.Id))
                //{
                //    retResults[1] = "Please Input ApplicantInfo";
                //    return retResults;
                //}
                #endregion Validation
                #region open connection and transaction
                #region New open connection and transaction
                if (VcurrConn != null)
                {
                    currConn = VcurrConn;
                }
                if (Vtransaction != null)
                {
                    transaction = Vtransaction;
                }
                #endregion New open connection and transaction
                if (currConn == null)
                {
                    currConn = _dbsqlConnection.GetConnection();
                    if (currConn.State != ConnectionState.Open)
                    {
                        currConn.Open();
                    }
                }
                if (transaction == null)
                {
                    transaction = currConn.BeginTransaction("");
                }
                #endregion open connection and transaction
                #region Save

                if (vm != null)
                {
                    if (vm.Id != null)
                    {
                        sqlText = "  ";
                        sqlText += @"Update ApplicantInfo  set                     
                           InterviewDate=@InterviewDate
                          ,InterviewTime=@InterviewTime                        
                          ";                      
                        sqlText += " where Id=@Id ";

                        SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                        cmdInsert.Parameters.AddWithValue("@Id", vm.Id);
                        cmdInsert.Parameters.AddWithValue("@InterviewDate", vm.InterviewDate);
                        cmdInsert.Parameters.AddWithValue("@InterviewTime", vm.InterviewTime);                      

                        cmdInsert.ExecuteNonQuery();
                    }                   
                }
                else
                {
                    retResults[1] = "This ApplicantInfo already used";
                    throw new ArgumentNullException("Please Input ApplicantInfo Value", "");
                }


                #endregion User Create

                #region Commit
                if (Vtransaction == null)
                {
                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
                #endregion Commit
                #region SuccessResult
                retResults[0] = "Success";
                retResults[1] = "Your application has been submited Successfully";
                #endregion SuccessResult
            }
            #endregion try
            #region Catch and Finall
            catch (Exception ex)
            {
                retResults[0] = "Fail";//Success or Fail
                retResults[4] = ex.Message.ToString(); //catch ex
                if (Vtransaction != null)
                {
                    try
                    {
                        if (Vtransaction == null) { transaction.Rollback(); }
                    }
                    catch (Exception)
                    {
                        retResults[1] = "Unexpected error to update InsertApplicantInfo.";
                        return retResults;
                        // throw new ArgumentNullException("Unexpected error to update ApplicantInfo.", "ApplicantInfo");
                    }
                }
                return retResults;
            }
            finally
            {
                if (VcurrConn == null)
                {
                    if (currConn != null)
                    {
                        if (currConn.State == ConnectionState.Open)
                        {
                            currConn.Close();
                        }
                    }
                }
            }
            #endregion
            #region Results
            return retResults;
            #endregion
        }

        public string[] EditApplicantMarkSetup(ApplicantInfoVM vm, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {

            #region Initializ
            string sqlText = "";
            int Id = 0;
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = Id.ToString();// Return Id
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "InsertApplicantInfo"; //Method Name
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            #endregion
            #region Try
            try
            {
                #region Validation
                //if (string.IsNullOrEmpty(ApplicantInfoVM.Id))
                //{
                //    retResults[1] = "Please Input ApplicantInfo";
                //    return retResults;
                //}
                #endregion Validation
                #region open connection and transaction
                #region New open connection and transaction
                if (VcurrConn != null)
                {
                    currConn = VcurrConn;
                }
                if (Vtransaction != null)
                {
                    transaction = Vtransaction;
                }
                #endregion New open connection and transaction
                if (currConn == null)
                {
                    currConn = _dbsqlConnection.GetConnection();
                    if (currConn.State != ConnectionState.Open)
                    {
                        currConn.Open();
                    }
                }
                if (transaction == null)
                {
                    transaction = currConn.BeginTransaction("");
                }
                #endregion open connection and transaction
                #region Save

                if (vm != null)
                {
                    if (vm.Id != null)
                    {
                        sqlText = "  ";
                        sqlText += @"Update ApplicantInfo  set                     
                           InterviewWrittenMarks=@InterviewWrittenMarks
                          ,InterviewVivaMarks=@InterviewVivaMarks                        
                          ";
                        sqlText += " where Id=@Id ";

                        SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                        cmdInsert.Parameters.AddWithValue("@Id", vm.Id);
                        cmdInsert.Parameters.AddWithValue("@InterviewWrittenMarks", vm.InterviewWrittenMarks);
                        cmdInsert.Parameters.AddWithValue("@InterviewVivaMarks", vm.InterviewVivaMarks);

                        cmdInsert.ExecuteNonQuery();
                    }
                }
                else
                {
                    retResults[1] = "This ApplicantInfo already used";
                    throw new ArgumentNullException("Please Input ApplicantInfo Value", "");
                }


                #endregion User Create

                #region Commit
                if (Vtransaction == null)
                {
                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
                #endregion Commit
                #region SuccessResult
                retResults[0] = "Success";
                retResults[1] = "Your application has been submited Successfully";
                #endregion SuccessResult
            }
            #endregion try
            #region Catch and Finall
            catch (Exception ex)
            {
                retResults[0] = "Fail";//Success or Fail
                retResults[4] = ex.Message.ToString(); //catch ex
                if (Vtransaction != null)
                {
                    try
                    {
                        if (Vtransaction == null) { transaction.Rollback(); }
                    }
                    catch (Exception)
                    {
                        retResults[1] = "Unexpected error to update InsertApplicantInfo.";
                        return retResults;
                        // throw new ArgumentNullException("Unexpected error to update ApplicantInfo.", "ApplicantInfo");
                    }
                }
                return retResults;
            }
            finally
            {
                if (VcurrConn == null)
                {
                    if (currConn != null)
                    {
                        if (currConn.State == ConnectionState.Open)
                        {
                            currConn.Close();
                        }
                    }
                }
            }
            #endregion
            #region Results
            return retResults;
            #endregion
        }
        
        public string[] DeleteApplicantInfo(int Id, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {

            #region Initializ
            string sqlText = "";          
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = Id.ToString();// Return Id
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "InsertApplicantInfo"; //Method Name
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            #endregion
            #region Try
            try
            {
                #region Validation
             
                #endregion Validation
                #region open connection and transaction
                #region New open connection and transaction
                if (VcurrConn != null)
                {
                    currConn = VcurrConn;
                }
                if (Vtransaction != null)
                {
                    transaction = Vtransaction;
                }
                #endregion New open connection and transaction
                if (currConn == null)
                {
                    currConn = _dbsqlConnection.GetConnection();
                    if (currConn.State != ConnectionState.Open)
                    {
                        currConn.Open();
                    }
                }
                if (transaction == null)
                {
                    transaction = currConn.BeginTransaction("");
                }
                #endregion open connection and transaction
                #region Save

                if (Id != null)
                {
                    sqlText = "  ";
                    sqlText += @"Update ApplicantInfo  set                        
                          IsActive =@IsActive
                          ";
                    sqlText += " where Id=@Id ";

                    SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                    cmdInsert.Parameters.AddWithValue("@Id", Id);
                    cmdInsert.Parameters.AddWithValue("@IsActive", false);
                    cmdInsert.ExecuteNonQuery();
                }
                #endregion User Create

                #region Commit
                if (Vtransaction == null)
                {
                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
                #endregion Commit
                #region SuccessResult
                retResults[0] = "Success";
                retResults[1] = "Applicant has been Deleted Successfully";
                #endregion SuccessResult
            }
            #endregion try
            #region Catch and Finall
            catch (Exception ex)
            {
                retResults[0] = "Fail";//Success or Fail
                retResults[4] = ex.Message.ToString(); //catch ex
                if (Vtransaction != null)
                {
                    try
                    {
                        if (Vtransaction == null) { transaction.Rollback(); }
                    }
                    catch (Exception)
                    {
                        retResults[1] = "Unexpected error to update InsertApplicantInfo.";
                        return retResults;                       
                    }
                }
                return retResults;
            }
            finally
            {
                if (VcurrConn == null)
                {
                    if (currConn != null)
                    {
                        if (currConn.State == ConnectionState.Open)
                        {
                            currConn.Close();
                        }
                    }
                }
            }
            #endregion
            #region Results
            return retResults;
            #endregion
        }

        public List<ApplicantInfoVM> SelectAllApplicantInfo()
        {
            #region Variables

            SqlConnection currConn = null;
            string sqlText = "";
            List<ApplicantInfoVM> applicantInfoVMs = new List<ApplicantInfoVM>();
            ApplicantInfoVM ApplicantInfoVM;
            #endregion
            try
            {
                #region open connection and transaction

                currConn = _dbsqlConnection.GetConnection();
                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }

                #endregion open connection and transaction

                #region sql statement
                sqlText = @"SELECT * FROM ApplicantInfo WHERE IsActive=1 order by InterviewWrittenMarks+InterviewVivaMarks desc";
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    ApplicantInfoVM = new ApplicantInfoVM();
                    ApplicantInfoVM.Id = dr["Id"].ToString();
                    ApplicantInfoVM.ApplicantName = dr["ApplicantName"].ToString();
                    ApplicantInfoVM.Designation = dr["Designation"].ToString();
                    ApplicantInfoVM.PresentAddress = dr["PresentAddress"].ToString();
                    ApplicantInfoVM.PermanentAddress = dr["PermanentAddress"].ToString();
                    ApplicantInfoVM.ContactNo = dr["ContactNo"].ToString();
                    ApplicantInfoVM.Email = dr["Email"].ToString();
                    ApplicantInfoVM.LastEducation = dr["LastEducation"].ToString();
                    ApplicantInfoVM.Gender = dr["Gender"].ToString();
                    ApplicantInfoVM.Experience = dr["Experience"].ToString();
                    ApplicantInfoVM.NoticePeriod = dr["Noticeperiod"].ToString();
                    ApplicantInfoVM.ExpectedSalary = dr["ExpectedSalary"].ToString();
                    ApplicantInfoVM.AttachmentFile = dr["AttachmentFile"].ToString();
                    ApplicantInfoVM.CoverLetter = dr["CoverLetter"].ToString();
                    ApplicantInfoVM.IsActive = Convert.ToBoolean(dr["IsActive"].ToString());
                    ApplicantInfoVM.IsArchive = Convert.ToBoolean(dr["IsArchive"].ToString());
                    ApplicantInfoVM.IsApproved = Convert.ToBoolean(dr["IsApproved"].ToString());
                    ApplicantInfoVM.IsConfirmed = Convert.ToBoolean(dr["IsConfirmed"].ToString());
                    ApplicantInfoVM.IsShortlisted = Convert.ToBoolean(dr["IsShortlisted"].ToString());
                    ApplicantInfoVM.InterviewDate = dr["InterviewDate"].ToString() + " : " + dr["InterviewTime"].ToString();                 
                    ApplicantInfoVM.InterviewWrittenMarks = (Convert.ToInt32(dr["InterviewWrittenMarks"].ToString()) + Convert.ToInt32(dr["InterviewVivaMarks"].ToString())).ToString();
                    ApplicantInfoVM.InterviewVivaMarks = dr["InterviewVivaMarks"].ToString();
                    ApplicantInfoVM.RecentCompany = dr["RecentCompany"].ToString();
                    applicantInfoVMs.Add(ApplicantInfoVM);

                }
                dr.Close();

                #endregion
            }
            #region catch


            catch (SqlException sqlex)
            {
                throw new ArgumentNullException("", "SQL:" + sqlText + FieldDelimeter + sqlex.Message.ToString());
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("", "SQL:" + sqlText + FieldDelimeter + ex.Message.ToString());
            }

            #endregion
            #region finally

            finally
            {
                if (currConn != null)
                {
                    if (currConn.State == ConnectionState.Open)
                    {
                        currConn.Close();
                    }
                }
            }

            #endregion

            return applicantInfoVMs;
        }

        public List<ApplicantInfoVM> SelectAllForInterviewCall()
        {
            #region Variables

            SqlConnection currConn = null;
            string sqlText = "";
            List<ApplicantInfoVM> applicantInfoVMs = new List<ApplicantInfoVM>();
            ApplicantInfoVM ApplicantInfoVM;
            #endregion
            try
            {
                #region open connection and transaction

                currConn = _dbsqlConnection.GetConnection();
                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }

                #endregion open connection and transaction

                #region sql statement
                sqlText = @"SELECT * FROM ApplicantInfo WHERE IsShortlisted=1 and IsActive=1 order by ApplicantName";
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    ApplicantInfoVM = new ApplicantInfoVM();
                    ApplicantInfoVM.Id = dr["Id"].ToString();
                    ApplicantInfoVM.ApplicantName = dr["ApplicantName"].ToString();
                    ApplicantInfoVM.Designation = dr["Designation"].ToString();
                    ApplicantInfoVM.PresentAddress = dr["PresentAddress"].ToString();
                    ApplicantInfoVM.PermanentAddress = dr["PermanentAddress"].ToString();
                    ApplicantInfoVM.ContactNo = dr["ContactNo"].ToString();
                    ApplicantInfoVM.Email = dr["Email"].ToString();
                    ApplicantInfoVM.LastEducation = dr["LastEducation"].ToString();
                    ApplicantInfoVM.Gender = dr["Gender"].ToString();
                    ApplicantInfoVM.Experience = dr["Experience"].ToString();
                    ApplicantInfoVM.NoticePeriod = dr["Noticeperiod"].ToString();
                    ApplicantInfoVM.ExpectedSalary = dr["ExpectedSalary"].ToString();
                    ApplicantInfoVM.AttachmentFile = dr["AttachmentFile"].ToString();
                    ApplicantInfoVM.CoverLetter = dr["CoverLetter"].ToString();
                    ApplicantInfoVM.IsActive = Convert.ToBoolean(dr["IsActive"].ToString());
                    ApplicantInfoVM.IsArchive = Convert.ToBoolean(dr["IsArchive"].ToString());
                    ApplicantInfoVM.IsApproved = Convert.ToBoolean(dr["IsApproved"].ToString());
                    ApplicantInfoVM.IsConfirmed = Convert.ToBoolean(dr["IsConfirmed"].ToString());
                    ApplicantInfoVM.IsShortlisted = Convert.ToBoolean(dr["IsShortlisted"].ToString());
                    ApplicantInfoVM.InterviewDate = dr["InterviewDate"].ToString() + " : " + dr["InterviewTime"].ToString();                 
                    ApplicantInfoVM.InterviewWrittenMarks = (Convert.ToInt32(dr["InterviewWrittenMarks"].ToString()) + Convert.ToInt32(dr["InterviewVivaMarks"].ToString())).ToString();
                    ApplicantInfoVM.InterviewVivaMarks = dr["InterviewVivaMarks"].ToString();
                    ApplicantInfoVM.RecentCompany = dr["RecentCompany"].ToString();
                    applicantInfoVMs.Add(ApplicantInfoVM);

                }
                dr.Close();

                #endregion
            }
            #region catch


            catch (SqlException sqlex)
            {
                throw new ArgumentNullException("", "SQL:" + sqlText + FieldDelimeter + sqlex.Message.ToString());
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("", "SQL:" + sqlText + FieldDelimeter + ex.Message.ToString());
            }

            #endregion
            #region finally

            finally
            {
                if (currConn != null)
                {
                    if (currConn.State == ConnectionState.Open)
                    {
                        currConn.Close();
                    }
                }
            }

            #endregion

            return applicantInfoVMs;
        }

        public List<ApplicantInfoVM> SelectAllForMarkSetup()
        {
            #region Variables

            SqlConnection currConn = null;
            string sqlText = "";
            List<ApplicantInfoVM> applicantInfoVMs = new List<ApplicantInfoVM>();
            ApplicantInfoVM ApplicantInfoVM;
            #endregion
            try
            {
                #region open connection and transaction

                currConn = _dbsqlConnection.GetConnection();
                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }

                #endregion open connection and transaction

                #region sql statement
                sqlText = @"SELECT * FROM ApplicantInfo WHERE IsShortlisted=1 and IsActive=1 and InterviewDate is not null order by ApplicantName";
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    ApplicantInfoVM = new ApplicantInfoVM();
                    ApplicantInfoVM.Id = dr["Id"].ToString();
                    ApplicantInfoVM.ApplicantName = dr["ApplicantName"].ToString();
                    ApplicantInfoVM.Designation = dr["Designation"].ToString();
                    ApplicantInfoVM.PresentAddress = dr["PresentAddress"].ToString();
                    ApplicantInfoVM.PermanentAddress = dr["PermanentAddress"].ToString();
                    ApplicantInfoVM.ContactNo = dr["ContactNo"].ToString();
                    ApplicantInfoVM.Email = dr["Email"].ToString();
                    ApplicantInfoVM.LastEducation = dr["LastEducation"].ToString();
                    ApplicantInfoVM.Gender = dr["Gender"].ToString();
                    ApplicantInfoVM.Experience = dr["Experience"].ToString();
                    ApplicantInfoVM.NoticePeriod = dr["Noticeperiod"].ToString();
                    ApplicantInfoVM.ExpectedSalary = dr["ExpectedSalary"].ToString();
                    ApplicantInfoVM.AttachmentFile = dr["AttachmentFile"].ToString();
                    ApplicantInfoVM.CoverLetter = dr["CoverLetter"].ToString();
                    ApplicantInfoVM.IsActive = Convert.ToBoolean(dr["IsActive"].ToString());
                    ApplicantInfoVM.IsArchive = Convert.ToBoolean(dr["IsArchive"].ToString());
                    ApplicantInfoVM.IsApproved = Convert.ToBoolean(dr["IsApproved"].ToString());
                    ApplicantInfoVM.IsConfirmed = Convert.ToBoolean(dr["IsConfirmed"].ToString());
                    ApplicantInfoVM.IsShortlisted = Convert.ToBoolean(dr["IsShortlisted"].ToString());
                    ApplicantInfoVM.InterviewDate = dr["InterviewDate"].ToString() + " : " + dr["InterviewTime"].ToString();
                    ApplicantInfoVM.InterviewWrittenMarks = (Convert.ToInt32(dr["InterviewWrittenMarks"].ToString()) + Convert.ToInt32(dr["InterviewVivaMarks"].ToString())).ToString();
                    ApplicantInfoVM.InterviewVivaMarks = dr["InterviewVivaMarks"].ToString();
                    ApplicantInfoVM.RecentCompany = dr["RecentCompany"].ToString();
                    applicantInfoVMs.Add(ApplicantInfoVM);

                }
                dr.Close();

                #endregion
            }
            #region catch


            catch (SqlException sqlex)
            {
                throw new ArgumentNullException("", "SQL:" + sqlText + FieldDelimeter + sqlex.Message.ToString());
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("", "SQL:" + sqlText + FieldDelimeter + ex.Message.ToString());
            }

            #endregion
            #region finally

            finally
            {
                if (currConn != null)
                {
                    if (currConn.State == ConnectionState.Open)
                    {
                        currConn.Close();
                    }
                }
            }

            #endregion

            return applicantInfoVMs;
        }

        public List<ApplicantInfoVM> SelectAllForConfirm()
        {
            #region Variables

            SqlConnection currConn = null;
            string sqlText = "";
            List<ApplicantInfoVM> applicantInfoVMs = new List<ApplicantInfoVM>();
            ApplicantInfoVM ApplicantInfoVM;
            #endregion
            try
            {
                #region open connection and transaction

                currConn = _dbsqlConnection.GetConnection();
                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }

                #endregion open connection and transaction

                #region sql statement
                sqlText = @"WITH SumMarks AS (
                        SELECT 
                            ApplicantId, 
                            SUM(Marks) AS TotalMarks
                        FROM ApplicantMarks
                        GROUP BY ApplicantId
                    ),
                    RankedMarks AS (
                        SELECT 
                            am.*, 
                            ROW_NUMBER() OVER (PARTITION BY am.ApplicantId ORDER BY am.Id DESC) AS RowNum
                        FROM ApplicantMarks am
                    )
                    SELECT 
                        ai.*, 
                        sm.TotalMarks, 
                        rm.*
                    FROM ApplicantInfo ai
                    LEFT JOIN SumMarks sm ON ai.Id = sm.ApplicantId
                    LEFT JOIN RankedMarks rm ON ai.Id = rm.ApplicantId AND rm.RowNum = 1
                    WHERE ai.IsActive = 1 
                      AND ai.IsShortlisted = 1 
                      AND ai.IsConfirmed = 0
                    ";
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    ApplicantInfoVM = new ApplicantInfoVM();
                    ApplicantInfoVM.Id = dr["Id"].ToString();
                    ApplicantInfoVM.ApplicantName = dr["ApplicantName"].ToString();
                    ApplicantInfoVM.Designation = dr["Designation"].ToString();
                    ApplicantInfoVM.PresentAddress = dr["PresentAddress"].ToString();
                    ApplicantInfoVM.PermanentAddress = dr["PermanentAddress"].ToString();
                    ApplicantInfoVM.ContactNo = dr["ContactNo"].ToString();
                    ApplicantInfoVM.Email = dr["Email"].ToString();
                    ApplicantInfoVM.LastEducation = dr["LastEducation"].ToString();
                    ApplicantInfoVM.Gender = dr["Gender"].ToString();
                    ApplicantInfoVM.Experience = dr["Experience"].ToString();
                    ApplicantInfoVM.NoticePeriod = dr["Noticeperiod"].ToString();
                    ApplicantInfoVM.ExpectedSalary = dr["ExpectedSalary"].ToString();
                    ApplicantInfoVM.AttachmentFile = dr["AttachmentFile"].ToString();
                    ApplicantInfoVM.CoverLetter = dr["CoverLetter"].ToString();
                    ApplicantInfoVM.IsActive = Convert.ToBoolean(dr["IsActive"].ToString());
                    ApplicantInfoVM.IsArchive = Convert.ToBoolean(dr["IsArchive"].ToString());
                    ApplicantInfoVM.IsApproved = Convert.ToBoolean(dr["IsApproved"].ToString());
                    ApplicantInfoVM.IsConfirmed = Convert.ToBoolean(dr["IsConfirmed"].ToString());
                    ApplicantInfoVM.IsShortlisted = Convert.ToBoolean(dr["IsShortlisted"].ToString());
                    ApplicantInfoVM.InterviewDate = dr["InterviewDate"].ToString() + " : " + dr["InterviewTime"].ToString();
                    ApplicantInfoVM.InterviewWrittenMarks = dr["TotalMarks"].ToString();
                    ApplicantInfoVM.InterviewVivaMarks = dr["InterviewVivaMarks"].ToString();
                    ApplicantInfoVM.RecentCompany = dr["RecentCompany"].ToString();
                    applicantInfoVMs.Add(ApplicantInfoVM);

                }
                dr.Close();

                #endregion
            }
            #region catch


            catch (SqlException sqlex)
            {
                throw new ArgumentNullException("", "SQL:" + sqlText + FieldDelimeter + sqlex.Message.ToString());
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("", "SQL:" + sqlText + FieldDelimeter + ex.Message.ToString());
            }

            #endregion
            #region finally

            finally
            {
                if (currConn != null)
                {
                    if (currConn.State == ConnectionState.Open)
                    {
                        currConn.Close();
                    }
                }
            }

            #endregion

            return applicantInfoVMs;
        }


        public ApplicantInfoVM SelectById(int Id)
        {
            #region Variables

            SqlConnection currConn = null;
            string sqlText = "";
            ApplicantInfoVM ApplicantInfoVM = new ApplicantInfoVM();
            #endregion
            try
            {
                #region open connection and transaction

                currConn = _dbsqlConnection.GetConnection();
                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }

                #endregion open connection and transaction

                #region sql statement

                sqlText = @"SELECT * FROM  ApplicantInfo WHERE IsActive=1 ";
                sqlText += @" and Id=@Id ";
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.Parameters.AddWithValue("@Id", Id);
                objComm.CommandType = CommandType.Text;
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    ApplicantInfoVM = new ApplicantInfoVM();
                    ApplicantInfoVM.Id = dr["Id"].ToString();
                    ApplicantInfoVM.ApplicantName = dr["ApplicantName"].ToString();
                    ApplicantInfoVM.Designation = dr["Designation"].ToString();
                    ApplicantInfoVM.PresentAddress = dr["PresentAddress"].ToString();
                    ApplicantInfoVM.PermanentAddress = dr["PermanentAddress"].ToString();
                    ApplicantInfoVM.ContactNo = dr["ContactNo"].ToString();
                    ApplicantInfoVM.Email = dr["Email"].ToString();
                    ApplicantInfoVM.LastEducation = dr["LastEducation"].ToString();
                    ApplicantInfoVM.Gender = dr["Gender"].ToString();
                    ApplicantInfoVM.Experience = dr["Experience"].ToString();
                    ApplicantInfoVM.NoticePeriod = dr["Noticeperiod"].ToString();
                    ApplicantInfoVM.ExpectedSalary = dr["ExpectedSalary"].ToString();
                    ApplicantInfoVM.AttachmentFile = dr["AttachmentFile"].ToString();
                    ApplicantInfoVM.CoverLetter = dr["CoverLetter"].ToString();
                    ApplicantInfoVM.IsActive = Convert.ToBoolean(dr["IsActive"].ToString());
                    ApplicantInfoVM.IsArchive = Convert.ToBoolean(dr["IsArchive"].ToString());
                    ApplicantInfoVM.InterviewDate = dr["InterviewDate"].ToString();
                    ApplicantInfoVM.InterviewTime = dr["InterviewTime"].ToString();
                    ApplicantInfoVM.InterviewWrittenMarks = dr["InterviewWrittenMarks"].ToString();
                    ApplicantInfoVM.InterviewVivaMarks = dr["InterviewVivaMarks"].ToString();
                    ApplicantInfoVM.RecentCompany = dr["RecentCompany"].ToString();


                    ApplicantInfoVM.Studying = dr["Studying"].ToString();
                    ApplicantInfoVM.IsShortlisted = Convert.ToBoolean(dr["IsShortlisted"].ToString());
                    ApplicantInfoVM.EmploymentHistory = dr["EmploymentHistory"].ToString();
                    ApplicantInfoVM.AcademicQualification = dr["AcademicQualification"].ToString();
                    ApplicantInfoVM.ProfessionalQualification = dr["ProfessionalQualification"].ToString();
                    ApplicantInfoVM.LookingFor = dr["LookingFor"].ToString();
                    ApplicantInfoVM.AvailableFor = dr["AvailableFor"].ToString();
                    ApplicantInfoVM.FatherName = dr["FatherName"].ToString();
                    ApplicantInfoVM.MotherName = dr["MotherName"].ToString();
                    ApplicantInfoVM.DateOfBirth = dr["DateOfBirth"].ToString();
                    ApplicantInfoVM.MaritalStatus = dr["MaritalStatus"].ToString();
                    ApplicantInfoVM.Nationality = dr["Nationality"].ToString();
                    ApplicantInfoVM.Religion = dr["Religion"].ToString();
                    ApplicantInfoVM.BloodGroup = dr["BloodGroup"].ToString();
                    ApplicantInfoVM.Jobid = dr["JobId"].ToString();
                
                }
                dr.Close();

                #endregion
            }
            #region catch


            catch (SqlException sqlex)
            {
                throw new ArgumentNullException("", "SQL:" + sqlText + FieldDelimeter + sqlex.Message.ToString());
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("", "SQL:" + sqlText + FieldDelimeter + ex.Message.ToString());
            }

            #endregion
            #region finally

            finally
            {
                if (currConn != null)
                {
                    if (currConn.State == ConnectionState.Open)
                    {
                        currConn.Close();
                    }
                }
            }

            #endregion

            return ApplicantInfoVM;
        }

        public string[] UpdateShortlistedApplicantInfo(int id, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {
            #region Initializ
            string sqlText = "";
            int Id = 0;
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = Id.ToString();// Return Id
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "DeleteApplicantInfo"; //Method Name
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            #endregion
            #region Try
            try
            {
                #region Validation
                //if (string.IsNullOrEmpty(ApplicantInfoVM.Id))
                //{
                //    retResults[1] = "Please Input ApplicantInfo ";
                //    return retResults;
                //}
                #endregion Validation
                #region open connection and transaction
                #region New open connection and transaction
                if (VcurrConn != null)
                {
                    currConn = VcurrConn;
                }
                if (Vtransaction != null)
                {
                    transaction = Vtransaction;
                }
                #endregion New open connection and transaction
                if (currConn == null)
                {
                    currConn = _dbsqlConnection.GetConnection();
                    if (currConn.State != ConnectionState.Open)
                    {
                        currConn.Open();
                    }
                }
                if (transaction == null)
                {
                    transaction = currConn.BeginTransaction("");
                }
                #endregion open connection and transaction
                #region Save

                if (id > 0)
                {
                    bool IsShortlisted = false;

                    sqlText = @"SELECT IsShortlisted FROM ApplicantInfo where Id=@Id";
                    SqlCommand objComm = new SqlCommand(sqlText, currConn);
                    objComm.Parameters.AddWithValue("@Id", id);
                    objComm.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(objComm);
                    transaction.Commit();
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() == "True")
                        {
                            IsShortlisted = false;
                        }
                        else
                        {
                            IsShortlisted = true;
                        }
                    }

                    sqlText = "  ";
                    sqlText += @"Update ApplicantInfo set                         
                          IsShortlisted =@IsShortlisted                        
                           where Id=@Id   
                         ";
                    SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                    cmdInsert.Parameters.AddWithValue("@Id", id);
                    cmdInsert.Parameters.AddWithValue("@IsShortlisted", IsShortlisted);

                    cmdInsert.ExecuteNonQuery();

                }
                else
                {
                    retResults[1] = "This ApplicantInfo already used";
                    throw new ArgumentNullException("Please Input ApplicantInfo Value", "");
                }


                #endregion User Create

                #region Commit
                if (Vtransaction == null)
                {
                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
                #endregion Commit
                #region SuccessResult
                retResults[0] = "Success";
                retResults[1] = "Applicant Shorlisted Successfully";
                #endregion SuccessResult
            }
            #endregion try
            #region Catch and Finall
            catch (Exception ex)
            {
                retResults[0] = "Fail";//Success or Fail
                retResults[4] = ex.Message.ToString(); //catch ex
                if (Vtransaction != null)
                {
                    try
                    {
                        if (Vtransaction == null) { transaction.Rollback(); }
                    }
                    catch (Exception)
                    {
                        retResults[1] = "Unexpected error to update InsertApplicantInfo.";
                        return retResults;
                        // throw new ArgumentNullException("Unexpected error to update InsertApplicantInfo.", "InsertApplicantInfo");
                    }
                }
                return retResults;
            }
            finally
            {
                if (VcurrConn == null)
                {
                    if (currConn != null)
                    {
                        if (currConn.State == ConnectionState.Open)
                        {
                            currConn.Close();
                        }
                    }
                }
            }
            #endregion
            #region Results
            return retResults;
            #endregion
        }

        public string[] ApproveApplicantInfo(int id, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {
            #region Initializ
            string sqlText = "";
            int Id = 0;
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = Id.ToString();// Return Id
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "DeleteApplicantInfo"; //Method Name
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            #endregion
            #region Try
            try
            {
                #region Validation
                //if (string.IsNullOrEmpty(ApplicantInfoVM.Id))
                //{
                //    retResults[1] = "Please Input ApplicantInfo ";
                //    return retResults;
                //}
                #endregion Validation
                #region open connection and transaction
                #region New open connection and transaction
                if (VcurrConn != null)
                {
                    currConn = VcurrConn;
                }
                if (Vtransaction != null)
                {
                    transaction = Vtransaction;
                }
                #endregion New open connection and transaction
                if (currConn == null)
                {
                    currConn = _dbsqlConnection.GetConnection();
                    if (currConn.State != ConnectionState.Open)
                    {
                        currConn.Open();
                    }
                }
                if (transaction == null)
                {
                    transaction = currConn.BeginTransaction("");
                }
                #endregion open connection and transaction
                #region Save

                if (id > 0)
                {
                    bool IsApproved = false;

                    sqlText = @"SELECT IsApproved FROM ApplicantInfo where Id=@Id";
                    SqlCommand objComm = new SqlCommand(sqlText, currConn);
                    objComm.Parameters.AddWithValue("@Id", id);
                    objComm.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(objComm);
                    transaction.Commit();
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString() == "True")
                        {
                            IsApproved = false;
                        }
                        else
                        {
                            IsApproved = true;
                        }
                    }

                    sqlText = "  ";
                    sqlText += @"Update ApplicantInfo set                         
                          IsApproved =@IsApproved                        
                           where Id=@Id   
                         ";
                    SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                    cmdInsert.Parameters.AddWithValue("@Id", id);
                    cmdInsert.Parameters.AddWithValue("@IsApproved", IsApproved);                

                    cmdInsert.ExecuteNonQuery();

                }
                else
                {
                    retResults[1] = "This ApplicantInfo already used";
                    throw new ArgumentNullException("Please Input ApplicantInfo Value", "");
                }


                #endregion User Create

                #region Commit
                if (Vtransaction == null)
                {
                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
                #endregion Commit
                #region SuccessResult
                retResults[0] = "Success";
                retResults[1] = "Data Save Successfully";
                #endregion SuccessResult
            }
            #endregion try
            #region Catch and Finall
            catch (Exception ex)
            {
                retResults[0] = "Fail";//Success or Fail
                retResults[4] = ex.Message.ToString(); //catch ex
                if (Vtransaction != null)
                {
                    try
                    {
                        if (Vtransaction == null) { transaction.Rollback(); }
                    }
                    catch (Exception)
                    {
                        retResults[1] = "Unexpected error to update InsertApplicantInfo.";
                        return retResults;
                        // throw new ArgumentNullException("Unexpected error to update InsertApplicantInfo.", "InsertApplicantInfo");
                    }
                }
                return retResults;
            }
            finally
            {
                if (VcurrConn == null)
                {
                    if (currConn != null)
                    {
                        if (currConn.State == ConnectionState.Open)
                        {
                            currConn.Close();
                        }
                    }
                }
            }
            #endregion
            #region Results
            return retResults;
            #endregion
        }

        public DataTable GetApplicantInfo(int id)
        {
            #region Variables

            SqlConnection currConn = null;
            string sqlText = "";
            DataTable dt = new DataTable();
            #endregion
            try
            {
                #region open connection and transaction

                currConn = _dbsqlConnection.GetConnection();
                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }

                #endregion open connection and transaction

                #region sql statement
                sqlText = @"Select * from ApplicantInfo where Id=@Id";
                SqlCommand objComm = new SqlCommand(sqlText, currConn);
                objComm.Parameters.AddWithValue("@Id", id);
                objComm.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(objComm);
                da.Fill(dt);

                #endregion
            }
            #region catch


            catch (SqlException sqlex)
            {
                throw new ArgumentNullException("", "SQL:" + sqlText + FieldDelimeter + sqlex.Message.ToString());
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("", "SQL:" + sqlText + FieldDelimeter + ex.Message.ToString());
            }

            #endregion
            #region finally

            finally
            {
                if (currConn != null)
                {
                    if (currConn.State == ConnectionState.Open)
                    {
                        currConn.Close();
                    }
                }
            }

            #endregion

            return dt;
        }

        public string[] ConfirmedApplicant(int id, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {
            #region Initialization
            string sqlText = "";
            string[] retResults = new string[6];
            retResults[0] = "Fail";
            retResults[1] = "Fail";
            retResults[2] = "0";
            retResults[3] = "";
            retResults[4] = "ex";
            retResults[5] = "ConfirmedApplicant";

            SqlConnection currConn = VcurrConn ?? _dbsqlConnection.GetConnection();
            SqlTransaction transaction = Vtransaction;
            #endregion

            try
            {
                #region Open Connection
                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }

                if (transaction == null)
                {
                    transaction = currConn.BeginTransaction();
                }
                #endregion

                #region Validate Input
                if (id <= 0)
                {
                    retResults[1] = "Invalid ApplicantInfo ID.";
                    throw new ArgumentNullException("ID must be greater than 0.");
                }
                #endregion

                sqlText = @"UPDATE ApplicantInfo SET IsConfirmed = @IsConfirmed WHERE Id = @Id";
                using (SqlCommand updateCmd = new SqlCommand(sqlText, currConn, transaction))
                {
                    updateCmd.Parameters.AddWithValue("@Id", id);
                    updateCmd.Parameters.AddWithValue("@IsConfirmed", true);
                    updateCmd.ExecuteNonQuery();
                }

                #region Commit Transaction
                if (Vtransaction == null)
                {
                    transaction.Commit();
                }
                #endregion

                #region Success
                retResults[0] = "Success";
                retResults[1] = "Data saved successfully.";
                #endregion
            }
            catch (Exception ex)
            {
                #region Rollback
                if (transaction != null && Vtransaction == null)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch
                    {
                        retResults[1] = "Rollback failed.";
                    }
                }
                #endregion

                retResults[0] = "Fail";
                retResults[4] = ex.Message;
            }
            finally
            {
                #region Close Connection
                if (VcurrConn == null && currConn != null && currConn.State == ConnectionState.Open)
                {
                    currConn.Close();
                }
                #endregion
            }

            return retResults;
        }

        public ApplicantInfoVM GetCompanyInfo()
        {
            #region Variables

            SqlConnection currConn = null;
            string sqlText = "";
            ApplicantInfoVM ApplicantInfoVM = new ApplicantInfoVM();
            #endregion
            try
            {
                #region open connection and transaction

                currConn = _dbsqlConnection.GetConnection();
                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }

                #endregion open connection and transaction

                #region sql statement

                sqlText = @"SELECT * FROM  Company";               
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;               
                objComm.CommandType = CommandType.Text;
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    ApplicantInfoVM = new ApplicantInfoVM();
                    ApplicantInfoVM.CompanyName = dr["Name"].ToString();
                    ApplicantInfoVM.Phone = dr["Phone"].ToString();
                    ApplicantInfoVM.Mail = dr["Mail"].ToString();                 
                }
                dr.Close();

                #endregion
            }
            #region catch


            catch (SqlException sqlex)
            {
                throw new ArgumentNullException("", "SQL:" + sqlText + FieldDelimeter + sqlex.Message.ToString());
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("", "SQL:" + sqlText + FieldDelimeter + ex.Message.ToString());
            }

            #endregion
            #region finally

            finally
            {
                if (currConn != null)
                {
                    if (currConn.State == ConnectionState.Open)
                    {
                        currConn.Close();
                    }
                }
            }

            #endregion

            return ApplicantInfoVM;
        }
     
        public DataSet ApplicantReport(string Id, SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
        {

            #region Variables
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            string sqlText = "";
            DataSet ds = new DataSet();
            #endregion
            try
            {
                #region open connection and transaction
                #region New open connection and transaction
                if (VcurrConn != null)
                {
                    currConn = VcurrConn;
                }
                if (Vtransaction != null)
                {
                    transaction = Vtransaction;
                }
                #endregion New open connection and transaction
                if (currConn == null)
                {
                    currConn = _dbsqlConnection.GetConnection();
                    if (currConn.State != ConnectionState.Open)
                    {
                        currConn.Open();
                    }
                }
                if (transaction == null)
                {
                    transaction = currConn.BeginTransaction("");
                }
                #endregion open connection and transaction
            
                #region sql statement

                #region SqlText

                sqlText = @"
                 Select * from ApplicantInfo where Id=@id
                 Select * from ApplicantEducation where ApplicantId=@id
                 Select * from ApplicantProfessionalQualification where ApplicantId=@id
                 Select * from ApplicantTraining where ApplicantId=@id
                 Select * from ApplicantLanguage where ApplicantId=@id
                 Select * from ApplicantEmployeementHistory where ApplicantId=@id
                 Select * from ApplicantSkill where ApplicantId=@id
                ";

                #endregion SqlText

                #region SqlExecution

                SqlDataAdapter da = new SqlDataAdapter(sqlText, currConn);
                da.SelectCommand.Transaction = transaction;
                da.SelectCommand.Parameters.AddWithValue("@id", Id);
              
                da.Fill(ds);
                DataTable dt = new DataTable(); 
                dt = ds.Tables[0];
               
                #endregion SqlExecution

                if (Vtransaction == null && transaction != null)
                {
                    transaction.Commit();
                }
                #endregion
            }

            #region catch

            catch (Exception ex)
            {
                throw new ArgumentNullException("", "SQL:" + sqlText + FieldDelimeter + ex.Message.ToString());
            }

            #endregion

            #region finally

            finally
            {
                if (VcurrConn == null && currConn != null && currConn.State == ConnectionState.Open)
                {
                    currConn.Close();
                }
            }
            #endregion

            return ds;
        }
        public string[] InsertApplicantTrainingDetails(ApplicantInfoVM vm, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {
            #region Initializ
            string sqlText = "";
            int NextId = 0;
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = NextId.ToString();// Return Id
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "InsertApplicantTrainingDetails"; //Method Name
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            int transResult = 0;
            #endregion
            #region Try
            try
            {
                #region Validation
                #endregion Validation
                #region open connection and transaction
                #region New open connection and transaction
                if (VcurrConn != null)
                {
                    currConn = VcurrConn;
                }
                if (Vtransaction != null)
                {
                    transaction = Vtransaction;
                }
                #endregion New open connection and transaction
                if (currConn == null)
                {
                    currConn = _dbsqlConnection.GetConnection();
                    if (currConn.State != ConnectionState.Open)
                    {
                        currConn.Open();
                    }
                }
                if (transaction == null)
                {
                    transaction = currConn.BeginTransaction("");
                }
                #endregion open connection and transaction
                #region Save

                if (vm != null)
                {
                    #region SqlText
                    sqlText = "  ";
                    sqlText += @" 
                                   INSERT INTO ApplicantTraining (                                
                                  
                                   TrainingTitle
                                  ,Topic
                                  ,Institute
                                  ,Country
                                  ,Location
                                  ,Year
                                  ,Duration
                                  ,ApplicantId
                                    ) VALUES (                                  
                                     @TrainingTitle
                                    ,@Topic
                                    ,@Institute
                                    ,@Country
                                    ,@Location   
                                    ,@Year   
                                    ,@Duration 
                                    ,@ApplicantId                         
                                    ) 
                                    ";
                    #endregion SqlText
                    #region SqlExecution
                    SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                    cmdInsert.Parameters.AddWithValue("@TrainingTitle", vm.applicantTrainingVM.TrainingTitle);
                    cmdInsert.Parameters.AddWithValue("@Topic", vm.applicantTrainingVM.Topic);
                    cmdInsert.Parameters.AddWithValue("@Institute", vm.applicantTrainingVM.Institute);
                    cmdInsert.Parameters.AddWithValue("@Country", vm.applicantTrainingVM.Country);
                    cmdInsert.Parameters.AddWithValue("@Location", vm.applicantTrainingVM.Location);
                    cmdInsert.Parameters.AddWithValue("@Year", vm.applicantTrainingVM.Year);
                    cmdInsert.Parameters.AddWithValue("@Duration", vm.applicantTrainingVM.Duration);
                    cmdInsert.Parameters.AddWithValue("@ApplicantId", vm.Id);
                    var exeRes = cmdInsert.ExecuteNonQuery();
                    transResult = Convert.ToInt32(exeRes);

                    #endregion SqlExecution
                }
                else
                {
                    retResults[1] = "No Detail Found!";
                    throw new ArgumentNullException(retResults[1], "");
                }
                #endregion Save
                #region Commit
                if (Vtransaction == null)
                {
                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
                #endregion Commit
                #region SuccessResult
                retResults[0] = "Success";
                retResults[1] = "Data Save Successfully.";
                retResults[2] = NextId.ToString();
                #endregion SuccessResult
            }
            #endregion try
            #region Catch and Finall
            catch (Exception ex)
            {
                retResults[0] = "Fail";//Success or Fail
                retResults[4] = ex.Message.ToString(); //catch ex
                if (Vtransaction == null) { transaction.Rollback(); }
                return retResults;
            }
            finally
            {
                if (VcurrConn == null)
                {
                    if (currConn != null)
                    {
                        if (currConn.State == ConnectionState.Open)
                        {
                            currConn.Close();
                        }
                    }
                }
            }
            #endregion
            #region Results
            return retResults;
            #endregion
        }

        public string[] InsertApplicantLanguageDetails(ApplicantInfoVM vm, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {
            #region Initializ
            string sqlText = "";
            int NextId = 0;
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = NextId.ToString();// Return Id
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "InsertApplicantLanguageDetails"; //Method Name
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            int transResult = 0;
            #endregion
            #region Try
            try
            {
                #region Validation
                #endregion Validation
                #region open connection and transaction
                #region New open connection and transaction
                if (VcurrConn != null)
                {
                    currConn = VcurrConn;
                }
                if (Vtransaction != null)
                {
                    transaction = Vtransaction;
                }
                #endregion New open connection and transaction
                if (currConn == null)
                {
                    currConn = _dbsqlConnection.GetConnection();
                    if (currConn.State != ConnectionState.Open)
                    {
                        currConn.Open();
                    }
                }
                if (transaction == null)
                {
                    transaction = currConn.BeginTransaction("");
                }
                #endregion open connection and transaction
                #region Save

                if (vm != null)
                {
                    #region SqlText
                    sqlText = "  ";
                    sqlText += @" 
                                   INSERT INTO ApplicantLanguage (                                                                 
                                   Language
                                  ,Reading
                                  ,Writing
                                  ,Speaking
                                  ,ApplicantId
                                    ) VALUES (                                  
                                     @Language
                                    ,@Reading
                                    ,@Writing
                                    ,@Speaking 
                                    ,@ApplicantId                         
                                    ) 
                                    ";
                    #endregion SqlText
                    #region SqlExecution
                    SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                    cmdInsert.Parameters.AddWithValue("@Language", vm.applicantLanguageVM.Language);
                    cmdInsert.Parameters.AddWithValue("@Reading", vm.applicantLanguageVM.Reading);
                    cmdInsert.Parameters.AddWithValue("@Writing", vm.applicantLanguageVM.Writing);
                    cmdInsert.Parameters.AddWithValue("@Speaking", vm.applicantLanguageVM.Speaking);
                    cmdInsert.Parameters.AddWithValue("@ApplicantId", vm.Id);
                    var exeRes = cmdInsert.ExecuteNonQuery();
                    transResult = Convert.ToInt32(exeRes);

                    #endregion SqlExecution
                }
                else
                {
                    retResults[1] = "No Detail Found!";
                    throw new ArgumentNullException(retResults[1], "");
                }
                #endregion Save
                #region Commit
                if (Vtransaction == null)
                {
                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
                #endregion Commit
                #region SuccessResult
                retResults[0] = "Success";
                retResults[1] = "Data Save Successfully.";
                retResults[2] = NextId.ToString();
                #endregion SuccessResult
            }
            #endregion try
            #region Catch and Finall
            catch (Exception ex)
            {
                retResults[0] = "Fail";//Success or Fail
                retResults[4] = ex.Message.ToString(); //catch ex
                if (Vtransaction == null) { transaction.Rollback(); }
                return retResults;
            }
            finally
            {
                if (VcurrConn == null)
                {
                    if (currConn != null)
                    {
                        if (currConn.State == ConnectionState.Open)
                        {
                            currConn.Close();
                        }
                    }
                }
            }
            #endregion
            #region Results
            return retResults;
            #endregion
        }
        public string[] InsertApplicantEmployeementHistoryDetails(ApplicantInfoVM vm, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {
            #region Initializ
            string sqlText = "";
            int NextId = 0;
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = NextId.ToString();// Return Id
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "InsertApplicantLanguageDetails"; //Method Name
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            int transResult = 0;
            #endregion
            #region Try
            try
            {
                #region Validation
                #endregion Validation
                #region open connection and transaction
                #region New open connection and transaction
                if (VcurrConn != null)
                {
                    currConn = VcurrConn;
                }
                if (Vtransaction != null)
                {
                    transaction = Vtransaction;
                }
                #endregion New open connection and transaction
                if (currConn == null)
                {
                    currConn = _dbsqlConnection.GetConnection();
                    if (currConn.State != ConnectionState.Open)
                    {
                        currConn.Open();
                    }
                }
                if (transaction == null)
                {
                    transaction = currConn.BeginTransaction("");
                }
                #endregion open connection and transaction
                #region Save

                if (vm != null)
                {
                    #region SqlText
                    sqlText = "  ";
                    sqlText += @" 
                                   INSERT INTO ApplicantEmployeementHistory (                                                                 
                                     CompanyName
                                    ,CompanyBusiness
                                    ,ApplicantDesignation
                                    ,ApplicantDepartment
                                    ,EmploymentPeriod
                                    ,ToDate
                                    ,CurrentlyWorking
                                    ,Responsibilities
                                    ,AreaOfExperience
                                    ,CompanyLocation
                                    ,ApplicantId
                                    ) VALUES (                                  
                                     @CompanyName
                                    ,@CompanyBusiness
                                    ,@ApplicantDesignation
                                    ,@ApplicantDepartment
                                    ,@EmploymentPeriod
                                    ,@ToDate
                                    ,@CurrentlyWorking
                                    ,@Responsibilities
                                    ,@AreaOfExperience
                                    ,@CompanyLocation
                                    ,@ApplicantId                         
                                    ) 
                                    ";
                    #endregion SqlText
                    #region SqlExecution
                    SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                    cmdInsert.Parameters.AddWithValue("@CompanyName", vm.applicantEmployeementHistoryVM.CompanyName);
                    cmdInsert.Parameters.AddWithValue("@CompanyBusiness", vm.applicantEmployeementHistoryVM.CompanyBusiness);
                    cmdInsert.Parameters.AddWithValue("@ApplicantDesignation", vm.applicantEmployeementHistoryVM.ApplicantDesignation);
                    cmdInsert.Parameters.AddWithValue("@ApplicantDepartment", vm.applicantEmployeementHistoryVM.ApplicantDepartment);
                    cmdInsert.Parameters.AddWithValue("@EmploymentPeriod", vm.applicantEmployeementHistoryVM.EmploymentPeriod);
                    cmdInsert.Parameters.AddWithValue("@ToDate", vm.applicantEmployeementHistoryVM.ToDate);
                    cmdInsert.Parameters.AddWithValue("@CurrentlyWorking", vm.applicantEmployeementHistoryVM.CurrentlyWorking);
                    cmdInsert.Parameters.AddWithValue("@Responsibilities", vm.applicantEmployeementHistoryVM.Responsibilities);
                    cmdInsert.Parameters.AddWithValue("@AreaOfExperience", vm.applicantEmployeementHistoryVM.AreaOfExperience);
                    cmdInsert.Parameters.AddWithValue("@CompanyLocation", vm.applicantEmployeementHistoryVM.CompanyLocation);
                    cmdInsert.Parameters.AddWithValue("@ApplicantId", vm.Id);
                    var exeRes = cmdInsert.ExecuteNonQuery();
                    transResult = Convert.ToInt32(exeRes);

                    #endregion SqlExecution
                }
                else
                {
                    retResults[1] = "No Detail Found!";
                    throw new ArgumentNullException(retResults[1], "");
                }
                #endregion Save
                #region Commit
                if (Vtransaction == null)
                {
                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
                #endregion Commit
                #region SuccessResult
                retResults[0] = "Success";
                retResults[1] = "Data Save Successfully.";
                retResults[2] = NextId.ToString();
                #endregion SuccessResult
            }
            #endregion try
            #region Catch and Finall
            catch (Exception ex)
            {
                retResults[0] = "Fail";//Success or Fail
                retResults[4] = ex.Message.ToString(); //catch ex
                if (Vtransaction == null) { transaction.Rollback(); }
                return retResults;
            }
            finally
            {
                if (VcurrConn == null)
                {
                    if (currConn != null)
                    {
                        if (currConn.State == ConnectionState.Open)
                        {
                            currConn.Close();
                        }
                    }
                }
            }
            #endregion
            #region Results
            return retResults;
            #endregion
        }
        public string[] InsertApplicantSkillDetails(ApplicantInfoVM vm, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {
            #region Initializ
            string sqlText = "";
            int NextId = 0;
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = NextId.ToString();// Return Id
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "InsertApplicantSkillDetails"; //Method Name
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            int transResult = 0;
            #endregion
            #region Try
            try
            {
                #region Validation
                #endregion Validation
                #region open connection and transaction
                #region New open connection and transaction
                if (VcurrConn != null)
                {
                    currConn = VcurrConn;
                }
                if (Vtransaction != null)
                {
                    transaction = Vtransaction;
                }
                #endregion New open connection and transaction
                if (currConn == null)
                {
                    currConn = _dbsqlConnection.GetConnection();
                    if (currConn.State != ConnectionState.Open)
                    {
                        currConn.Open();
                    }
                }
                if (transaction == null)
                {
                    transaction = currConn.BeginTransaction("");
                }
                #endregion open connection and transaction
                #region Save

                if (vm != null)
                {
                    #region SqlText
                    sqlText = "  ";
                    sqlText += @" 
                                   INSERT INTO ApplicantSkill (                                                                 
                                   Skill
                                  ,SkillDescription
                                  ,ExtraCurricular
                                  ,ApplicantId
                                    ) VALUES (                                  
                                     @Skill
                                    ,@SkillDescription
                                    ,@ExtraCurricular
                                    ,@ApplicantId                         
                                    ) 
                                    ";
                    #endregion SqlText
                    #region SqlExecution
                    SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                    cmdInsert.Parameters.AddWithValue("@Skill", vm.applicantSkillVM.Skill);
                    cmdInsert.Parameters.AddWithValue("@SkillDescription", vm.applicantSkillVM.SkillDescription);
                    cmdInsert.Parameters.AddWithValue("@ExtraCurricular", vm.applicantSkillVM.ExtraCurricular);
                    cmdInsert.Parameters.AddWithValue("@ApplicantId", vm.Id);
                    var exeRes = cmdInsert.ExecuteNonQuery();
                    transResult = Convert.ToInt32(exeRes);

                    #endregion SqlExecution
                }
                else
                {
                    retResults[1] = "No Detail Found!";
                    throw new ArgumentNullException(retResults[1], "");
                }
                #endregion Save
                #region Commit
                if (Vtransaction == null)
                {
                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
                #endregion Commit
                #region SuccessResult
                retResults[0] = "Success";
                retResults[1] = "Data Save Successfully.";
                retResults[2] = NextId.ToString();
                #endregion SuccessResult
            }
            #endregion try
            #region Catch and Finall
            catch (Exception ex)
            {
                retResults[0] = "Fail";//Success or Fail
                retResults[4] = ex.Message.ToString(); //catch ex
                if (Vtransaction == null) { transaction.Rollback(); }
                return retResults;
            }
            finally
            {
                if (VcurrConn == null)
                {
                    if (currConn != null)
                    {
                        if (currConn.State == ConnectionState.Open)
                        {
                            currConn.Close();
                        }
                    }
                }
            }
            #endregion
            #region Results
            return retResults;
            #endregion
        }
        public string[] InsertApplicantEducationlDetails(ApplicantInfoVM vm, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {
            #region Initializ
            string sqlText = "";
            int NextId = 0;
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = NextId.ToString();// Return Id
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "InsertApplicantEducationlDetails"; //Method Name
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            int transResult = 0;
            #endregion
            #region Try
            try
            {
                #region Validation
                #endregion Validation
                #region open connection and transaction
                #region New open connection and transaction
                if (VcurrConn != null)
                {
                    currConn = VcurrConn;
                }
                if (Vtransaction != null)
                {
                    transaction = Vtransaction;
                }
                #endregion New open connection and transaction
                if (currConn == null)
                {
                    currConn = _dbsqlConnection.GetConnection();
                    if (currConn.State != ConnectionState.Open)
                    {
                        currConn.Open();
                    }
                }
                if (transaction == null)
                {
                    transaction = currConn.BeginTransaction("");
                }
                #endregion open connection and transaction
                #region Save

                if (vm != null)
                {
                    #region SqlText
                    sqlText = "  ";
                    sqlText += @" 
                                   INSERT INTO ApplicantEducation (                                                                 
                                   ExamTitle
                                  ,Major
                                  ,Institute
                                  ,PassYear
                                  ,Duration
                                  ,Achievment
                                  ,ApplicantId
                                    ) VALUES (                                  
                                     @ExamTitle
                                    ,@Major
                                    ,@Institute
                                    ,@PassYear
                                    ,@Duration
                                    ,@Achievment
                                    ,@ApplicantId                         
                                    ) 
                                    ";
                    #endregion SqlText
                    #region SqlExecution
                    SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                    cmdInsert.Parameters.AddWithValue("@ExamTitle", vm.educationVM.ExamTitle);
                    cmdInsert.Parameters.AddWithValue("@Major", vm.educationVM.Major);
                    cmdInsert.Parameters.AddWithValue("@Institute", vm.educationVM.Institute);
                    cmdInsert.Parameters.AddWithValue("@PassYear", vm.educationVM.PassYear);
                    cmdInsert.Parameters.AddWithValue("@Duration", vm.educationVM.Duration);
                    cmdInsert.Parameters.AddWithValue("@Achievment", vm.educationVM.Achievment);
                    cmdInsert.Parameters.AddWithValue("@ApplicantId", vm.Id);
                    var exeRes = cmdInsert.ExecuteNonQuery();
                    transResult = Convert.ToInt32(exeRes);

                    #endregion SqlExecution
                }
                else
                {
                    retResults[1] = "No Detail Found!";
                    throw new ArgumentNullException(retResults[1], "");
                }
                #endregion Save
                #region Commit
                if (Vtransaction == null)
                {
                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
                #endregion Commit
                #region SuccessResult
                retResults[0] = "Success";
                retResults[1] = "Data Save Successfully.";
                retResults[2] = NextId.ToString();
                #endregion SuccessResult
            }
            #endregion try
            #region Catch and Finall
            catch (Exception ex)
            {
                retResults[0] = "Fail";//Success or Fail
                retResults[4] = ex.Message.ToString(); //catch ex
                if (Vtransaction == null) { transaction.Rollback(); }
                return retResults;
            }
            finally
            {
                if (VcurrConn == null)
                {
                    if (currConn != null)
                    {
                        if (currConn.State == ConnectionState.Open)
                        {
                            currConn.Close();
                        }
                    }
                }
            }
            #endregion
            #region Results
            return retResults;
            #endregion
        }
        public string[] DeleteEducationlDetails(string Id, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {
            #region Initializ
            string sqlText = "";
            int NextId = 0;
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = NextId.ToString();// Return Id
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "DeleteEducationlDetails"; //Method Name
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            int transResult = 0;
            #endregion
            #region Try
            try
            {
                #region Validation
                #endregion Validation
                #region open connection and transaction
                #region New open connection and transaction
                if (VcurrConn != null)
                {
                    currConn = VcurrConn;
                }
                if (Vtransaction != null)
                {
                    transaction = Vtransaction;
                }
                #endregion New open connection and transaction
                if (currConn == null)
                {
                    currConn = _dbsqlConnection.GetConnection();
                    if (currConn.State != ConnectionState.Open)
                    {
                        currConn.Open();
                    }
                }
                if (transaction == null)
                {
                    transaction = currConn.BeginTransaction("");
                }
                #endregion open connection and transaction
                #region Save

                if (Id != null)
                {
                    #region SqlText
                    sqlText = "  ";
                    sqlText += @" 
                                  Delete from ApplicantEducation where Id=@Id
                                    ";
                    #endregion SqlText
                    #region SqlExecution
                    SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);                
                    cmdInsert.Parameters.AddWithValue("@Id", Id);
                    var exeRes = cmdInsert.ExecuteNonQuery();
                    transResult = Convert.ToInt32(exeRes);

                    #endregion SqlExecution
                }
                else
                {
                    retResults[1] = "No Detail Found!";
                    throw new ArgumentNullException(retResults[1], "");
                }
                #endregion Save
                #region Commit
                if (Vtransaction == null)
                {
                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
                #endregion Commit
                #region SuccessResult
                retResults[0] = "Success";
                retResults[1] = "Data Deleted Successfully.";
                retResults[2] = NextId.ToString();
                #endregion SuccessResult
            }
            #endregion try
            #region Catch and Finall
            catch (Exception ex)
            {
                retResults[0] = "Fail";//Success or Fail
                retResults[4] = ex.Message.ToString(); //catch ex
                if (Vtransaction == null) { transaction.Rollback(); }
                return retResults;
            }
            finally
            {
                if (VcurrConn == null)
                {
                    if (currConn != null)
                    {
                        if (currConn.State == ConnectionState.Open)
                        {
                            currConn.Close();
                        }
                    }
                }
            }
            #endregion
            #region Results
            return retResults;
            #endregion
        }
        public string[] DeleteProfessionalDetails(string Id, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {
            #region Initializ
            string sqlText = "";
            int NextId = 0;
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = NextId.ToString();// Return Id
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "DeleteProfessionalDetails"; //Method Name
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            int transResult = 0;
            #endregion
            #region Try
            try
            {
                #region Validation
                #endregion Validation
                #region open connection and transaction
                #region New open connection and transaction
                if (VcurrConn != null)
                {
                    currConn = VcurrConn;
                }
                if (Vtransaction != null)
                {
                    transaction = Vtransaction;
                }
                #endregion New open connection and transaction
                if (currConn == null)
                {
                    currConn = _dbsqlConnection.GetConnection();
                    if (currConn.State != ConnectionState.Open)
                    {
                        currConn.Open();
                    }
                }
                if (transaction == null)
                {
                    transaction = currConn.BeginTransaction("");
                }
                #endregion open connection and transaction
                #region Save

                if (Id != null)
                {
                    #region SqlText
                    sqlText = "  ";
                    sqlText += @" 
                                  Delete from ApplicantProfessionalQualification where Id=@Id
                                    ";
                    #endregion SqlText
                    #region SqlExecution
                    SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                    cmdInsert.Parameters.AddWithValue("@Id", Id);
                    var exeRes = cmdInsert.ExecuteNonQuery();
                    transResult = Convert.ToInt32(exeRes);

                    #endregion SqlExecution
                }
                else
                {
                    retResults[1] = "No Detail Found!";
                    throw new ArgumentNullException(retResults[1], "");
                }
                #endregion Save
                #region Commit
                if (Vtransaction == null)
                {
                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
                #endregion Commit
                #region SuccessResult
                retResults[0] = "Success";
                retResults[1] = "Data Deleted Successfully.";
                retResults[2] = NextId.ToString();
                #endregion SuccessResult
            }
            #endregion try
            #region Catch and Finall
            catch (Exception ex)
            {
                retResults[0] = "Fail";//Success or Fail
                retResults[4] = ex.Message.ToString(); //catch ex
                if (Vtransaction == null) { transaction.Rollback(); }
                return retResults;
            }
            finally
            {
                if (VcurrConn == null)
                {
                    if (currConn != null)
                    {
                        if (currConn.State == ConnectionState.Open)
                        {
                            currConn.Close();
                        }
                    }
                }
            }
            #endregion
            #region Results
            return retResults;
            #endregion
        }

        public string[] DeleteTrainingDetails(string Id, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {
            #region Initializ
            string sqlText = "";
            int NextId = 0;
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = NextId.ToString();// Return Id
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "DeleteTrainingDetails"; //Method Name
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            int transResult = 0;
            #endregion
            #region Try
            try
            {
                #region Validation
                #endregion Validation
                #region open connection and transaction
                #region New open connection and transaction
                if (VcurrConn != null)
                {
                    currConn = VcurrConn;
                }
                if (Vtransaction != null)
                {
                    transaction = Vtransaction;
                }
                #endregion New open connection and transaction
                if (currConn == null)
                {
                    currConn = _dbsqlConnection.GetConnection();
                    if (currConn.State != ConnectionState.Open)
                    {
                        currConn.Open();
                    }
                }
                if (transaction == null)
                {
                    transaction = currConn.BeginTransaction("");
                }
                #endregion open connection and transaction
                #region Save

                if (Id != null)
                {
                    #region SqlText
                    sqlText = "  ";
                    sqlText += @" 
                                  Delete from ApplicantTraining where Id=@Id
                                    ";
                    #endregion SqlText
                    #region SqlExecution
                    SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                    cmdInsert.Parameters.AddWithValue("@Id", Id);
                    var exeRes = cmdInsert.ExecuteNonQuery();
                    transResult = Convert.ToInt32(exeRes);

                    #endregion SqlExecution
                }
                else
                {
                    retResults[1] = "No Detail Found!";
                    throw new ArgumentNullException(retResults[1], "");
                }
                #endregion Save
                #region Commit
                if (Vtransaction == null)
                {
                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
                #endregion Commit
                #region SuccessResult
                retResults[0] = "Success";
                retResults[1] = "Data Deleted Successfully.";
                retResults[2] = NextId.ToString();
                #endregion SuccessResult
            }
            #endregion try
            #region Catch and Finall
            catch (Exception ex)
            {
                retResults[0] = "Fail";//Success or Fail
                retResults[4] = ex.Message.ToString(); //catch ex
                if (Vtransaction == null) { transaction.Rollback(); }
                return retResults;
            }
            finally
            {
                if (VcurrConn == null)
                {
                    if (currConn != null)
                    {
                        if (currConn.State == ConnectionState.Open)
                        {
                            currConn.Close();
                        }
                    }
                }
            }
            #endregion
            #region Results
            return retResults;
            #endregion
        }
        public string[] DeleteLanguageDetails(string Id, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {
            #region Initializ
            string sqlText = "";
            int NextId = 0;
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = NextId.ToString();// Return Id
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "DeleteLanguageDetails"; //Method Name
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            int transResult = 0;
            #endregion
            #region Try
            try
            {
                #region Validation
                #endregion Validation
                #region open connection and transaction
                #region New open connection and transaction
                if (VcurrConn != null)
                {
                    currConn = VcurrConn;
                }
                if (Vtransaction != null)
                {
                    transaction = Vtransaction;
                }
                #endregion New open connection and transaction
                if (currConn == null)
                {
                    currConn = _dbsqlConnection.GetConnection();
                    if (currConn.State != ConnectionState.Open)
                    {
                        currConn.Open();
                    }
                }
                if (transaction == null)
                {
                    transaction = currConn.BeginTransaction("");
                }
                #endregion open connection and transaction
                #region Save

                if (Id != null)
                {
                    #region SqlText
                    sqlText = "  ";
                    sqlText += @" 
                                  Delete from ApplicantLanguage where Id=@Id
                                    ";
                    #endregion SqlText
                    #region SqlExecution
                    SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                    cmdInsert.Parameters.AddWithValue("@Id", Id);
                    var exeRes = cmdInsert.ExecuteNonQuery();
                    transResult = Convert.ToInt32(exeRes);

                    #endregion SqlExecution
                }
                else
                {
                    retResults[1] = "No Detail Found!";
                    throw new ArgumentNullException(retResults[1], "");
                }
                #endregion Save
                #region Commit
                if (Vtransaction == null)
                {
                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
                #endregion Commit
                #region SuccessResult
                retResults[0] = "Success";
                retResults[1] = "Data Deleted Successfully.";
                retResults[2] = NextId.ToString();
                #endregion SuccessResult
            }
            #endregion try
            #region Catch and Finall
            catch (Exception ex)
            {
                retResults[0] = "Fail";//Success or Fail
                retResults[4] = ex.Message.ToString(); //catch ex
                if (Vtransaction == null) { transaction.Rollback(); }
                return retResults;
            }
            finally
            {
                if (VcurrConn == null)
                {
                    if (currConn != null)
                    {
                        if (currConn.State == ConnectionState.Open)
                        {
                            currConn.Close();
                        }
                    }
                }
            }
            #endregion
            #region Results
            return retResults;
            #endregion
        }
        public string[] DeleteEmployeementHistoryDetails(string Id, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {
            #region Initializ
            string sqlText = "";
            int NextId = 0;
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = NextId.ToString();// Return Id
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "DeleteEmployeementHistoryDetails"; //Method Name
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            int transResult = 0;
            #endregion
            #region Try
            try
            {
                #region Validation
                #endregion Validation
                #region open connection and transaction
                #region New open connection and transaction
                if (VcurrConn != null)
                {
                    currConn = VcurrConn;
                }
                if (Vtransaction != null)
                {
                    transaction = Vtransaction;
                }
                #endregion New open connection and transaction
                if (currConn == null)
                {
                    currConn = _dbsqlConnection.GetConnection();
                    if (currConn.State != ConnectionState.Open)
                    {
                        currConn.Open();
                    }
                }
                if (transaction == null)
                {
                    transaction = currConn.BeginTransaction("");
                }
                #endregion open connection and transaction
                #region Save

                if (Id != null)
                {
                    #region SqlText
                    sqlText = "  ";
                    sqlText += @" 
                                  Delete from ApplicantEmployeementHistory where Id=@Id
                                    ";
                    #endregion SqlText
                    #region SqlExecution
                    SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                    cmdInsert.Parameters.AddWithValue("@Id", Id);
                    var exeRes = cmdInsert.ExecuteNonQuery();
                    transResult = Convert.ToInt32(exeRes);

                    #endregion SqlExecution
                }
                else
                {
                    retResults[1] = "No Detail Found!";
                    throw new ArgumentNullException(retResults[1], "");
                }
                #endregion Save
                #region Commit
                if (Vtransaction == null)
                {
                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
                #endregion Commit
                #region SuccessResult
                retResults[0] = "Success";
                retResults[1] = "Data Deleted Successfully.";
                retResults[2] = NextId.ToString();
                #endregion SuccessResult
            }
            #endregion try
            #region Catch and Finall
            catch (Exception ex)
            {
                retResults[0] = "Fail";//Success or Fail
                retResults[4] = ex.Message.ToString(); //catch ex
                if (Vtransaction == null) { transaction.Rollback(); }
                return retResults;
            }
            finally
            {
                if (VcurrConn == null)
                {
                    if (currConn != null)
                    {
                        if (currConn.State == ConnectionState.Open)
                        {
                            currConn.Close();
                        }
                    }
                }
            }
            #endregion
            #region Results
            return retResults;
            #endregion
        }
        public string[] DeleteSkillDetails(string Id, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {
            #region Initializ
            string sqlText = "";
            int NextId = 0;
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = NextId.ToString();// Return Id
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "DeleteSkillDetails"; //Method Name
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            int transResult = 0;
            #endregion
            #region Try
            try
            {
                #region Validation
                #endregion Validation
                #region open connection and transaction
                #region New open connection and transaction
                if (VcurrConn != null)
                {
                    currConn = VcurrConn;
                }
                if (Vtransaction != null)
                {
                    transaction = Vtransaction;
                }
                #endregion New open connection and transaction
                if (currConn == null)
                {
                    currConn = _dbsqlConnection.GetConnection();
                    if (currConn.State != ConnectionState.Open)
                    {
                        currConn.Open();
                    }
                }
                if (transaction == null)
                {
                    transaction = currConn.BeginTransaction("");
                }
                #endregion open connection and transaction
                #region Save

                if (Id != null)
                {
                    #region SqlText
                    sqlText = "  ";
                    sqlText += @" 
                                  Delete from ApplicantSkill where Id=@Id
                                    ";
                    #endregion SqlText
                    #region SqlExecution
                    SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                    cmdInsert.Parameters.AddWithValue("@Id", Id);
                    var exeRes = cmdInsert.ExecuteNonQuery();
                    transResult = Convert.ToInt32(exeRes);

                    #endregion SqlExecution
                }
                else
                {
                    retResults[1] = "No Detail Found!";
                    throw new ArgumentNullException(retResults[1], "");
                }
                #endregion Save
                #region Commit
                if (Vtransaction == null)
                {
                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
                #endregion Commit
                #region SuccessResult
                retResults[0] = "Success";
                retResults[1] = "Data Deleted Successfully.";
                retResults[2] = NextId.ToString();
                #endregion SuccessResult
            }
            #endregion try
            #region Catch and Finall
            catch (Exception ex)
            {
                retResults[0] = "Fail";//Success or Fail
                retResults[4] = ex.Message.ToString(); //catch ex
                if (Vtransaction == null) { transaction.Rollback(); }
                return retResults;
            }
            finally
            {
                if (VcurrConn == null)
                {
                    if (currConn != null)
                    {
                        if (currConn.State == ConnectionState.Open)
                        {
                            currConn.Close();
                        }
                    }
                }
            }
            #endregion
            #region Results
            return retResults;
            #endregion
        }
        public string[] DeleteMarks(string Id, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {
            #region Initializ
            string sqlText = "";
            int NextId = 0;
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = NextId.ToString();// Return Id
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "DeleteMarks"; //Method Name
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            int transResult = 0;
            #endregion
            #region Try
            try
            {
                #region Validation
                #endregion Validation
                #region open connection and transaction
                #region New open connection and transaction
                if (VcurrConn != null)
                {
                    currConn = VcurrConn;
                }
                if (Vtransaction != null)
                {
                    transaction = Vtransaction;
                }
                #endregion New open connection and transaction
                if (currConn == null)
                {
                    currConn = _dbsqlConnection.GetConnection();
                    if (currConn.State != ConnectionState.Open)
                    {
                        currConn.Open();
                    }
                }
                if (transaction == null)
                {
                    transaction = currConn.BeginTransaction("");
                }
                #endregion open connection and transaction
                #region Save

                if (Id != null)
                {
                    #region SqlText
                    sqlText = "  ";
                    sqlText += @" 
                                  Delete from ApplicantMarks where Id=@Id
                                    ";
                    #endregion SqlText
                    #region SqlExecution
                    SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                    cmdInsert.Parameters.AddWithValue("@Id", Id);
                    var exeRes = cmdInsert.ExecuteNonQuery();
                    transResult = Convert.ToInt32(exeRes);

                    #endregion SqlExecution
                }
                else
                {
                    retResults[1] = "No Detail Found!";
                    throw new ArgumentNullException(retResults[1], "");
                }
                #endregion Save
                #region Commit
                if (Vtransaction == null)
                {
                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
                #endregion Commit
                #region SuccessResult
                retResults[0] = "Success";
                retResults[1] = "Data Deleted Successfully.";
                retResults[2] = NextId.ToString();
                #endregion SuccessResult
            }
            #endregion try
            #region Catch and Finall
            catch (Exception ex)
            {
                retResults[0] = "Fail";//Success or Fail
                retResults[4] = ex.Message.ToString(); //catch ex
                if (Vtransaction == null) { transaction.Rollback(); }
                return retResults;
            }
            finally
            {
                if (VcurrConn == null)
                {
                    if (currConn != null)
                    {
                        if (currConn.State == ConnectionState.Open)
                        {
                            currConn.Close();
                        }
                    }
                }
            }
            #endregion
            #region Results
            return retResults;
            #endregion
        }
        public string[] DeleteSalary(string Id, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {
            #region Initializ
            string sqlText = "";
            int NextId = 0;
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = NextId.ToString();// Return Id
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "DeleteSalary"; //Method Name
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            int transResult = 0;
            #endregion
            #region Try
            try
            {
                #region Validation
                #endregion Validation
                #region open connection and transaction
                #region New open connection and transaction
                if (VcurrConn != null)
                {
                    currConn = VcurrConn;
                }
                if (Vtransaction != null)
                {
                    transaction = Vtransaction;
                }
                #endregion New open connection and transaction
                if (currConn == null)
                {
                    currConn = _dbsqlConnection.GetConnection();
                    if (currConn.State != ConnectionState.Open)
                    {
                        currConn.Open();
                    }
                }
                if (transaction == null)
                {
                    transaction = currConn.BeginTransaction("");
                }
                #endregion open connection and transaction
                #region Save

                if (Id != null)
                {
                    #region SqlText
                    sqlText = "  ";
                    sqlText += @" 
                                  Delete from ApplicantSalary where Id=@Id
                                    ";
                    #endregion SqlText
                    #region SqlExecution
                    SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                    cmdInsert.Parameters.AddWithValue("@Id", Id);
                    var exeRes = cmdInsert.ExecuteNonQuery();
                    transResult = Convert.ToInt32(exeRes);

                    #endregion SqlExecution
                }
                else
                {
                    retResults[1] = "No Detail Found!";
                    throw new ArgumentNullException(retResults[1], "");
                }
                #endregion Save
                #region Commit
                if (Vtransaction == null)
                {
                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
                #endregion Commit
                #region SuccessResult
                retResults[0] = "Success";
                retResults[1] = "Data Deleted Successfully.";
                retResults[2] = NextId.ToString();
                #endregion SuccessResult
            }
            #endregion try
            #region Catch and Finall
            catch (Exception ex)
            {
                retResults[0] = "Fail";//Success or Fail
                retResults[4] = ex.Message.ToString(); //catch ex
                if (Vtransaction == null) { transaction.Rollback(); }
                return retResults;
            }
            finally
            {
                if (VcurrConn == null)
                {
                    if (currConn != null)
                    {
                        if (currConn.State == ConnectionState.Open)
                        {
                            currConn.Close();
                        }
                    }
                }
            }
            #endregion
            #region Results
            return retResults;
            #endregion
        }
    }
}
