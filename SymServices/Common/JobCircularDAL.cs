using SymOrdinary;
using SymViewModel;
using SymViewModel.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SymServices.Common
{
    public class JobCircularDAL
    {
        #region Global Variables
        private const string FieldDelimeter = DBConstant.FieldDelimeter;
        private DBSQLConnection _dbsqlConnection = new DBSQLConnection();

        #endregion

        #region Methods
        //==================SelectAll=================
        public List<JobCircularVM> SelectAll()
        {

            #region Variables

            SqlConnection currConn = null;
            string sqlText = "";
            List<JobCircularVM> JobCircularVMs = new List<JobCircularVM>();
            JobCircularVM JobCircularVM;
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

                sqlText = @"SELECT
jc.Id
,jc.BranchId
,jc.JobTitle
,jc.DesignationId
,desi.Name DesignationName
,jc.Deadline
,jc.Expriance
,jc.Description
,jc.Remarks
,jc.IsActive
,jc.IsArchive
,jc.CreatedBy
,jc.CreatedAt
,jc.CreatedFrom
,jc.LastUpdateBy
,jc.LastUpdateAt
,jc.LastUpdateFrom
,jc.DegreeName
,jc.JobLocation
From JobCircular jc 
left outer join Designation desi on jc.DesignationId=desi.Id
Where  jc.IsArchive=0
";
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    JobCircularVM = new JobCircularVM();
                    JobCircularVM.Id = dr["Id"].ToString();
                    JobCircularVM.BranchId = Convert.ToInt32(dr["BranchId"]);
                    JobCircularVM.JobTitle = dr["JobTitle"].ToString();
                    JobCircularVM.DesignationId = dr["DesignationId"].ToString();
                    JobCircularVM.DesignationName = dr["DesignationName"].ToString();
                    JobCircularVM.Deadline = dr["Deadline"].ToString();
                    JobCircularVM.Expriance = dr["Expriance"].ToString();
                    JobCircularVM.Description = dr["Description"].ToString();
                    JobCircularVM.Remarks = dr["Remarks"].ToString();
                    JobCircularVM.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    JobCircularVM.CreatedAt = Ordinary.StringToDate(dr["CreatedAt"].ToString());
                    JobCircularVM.CreatedBy = dr["CreatedBy"].ToString();
                    JobCircularVM.CreatedFrom = dr["CreatedFrom"].ToString();
                    JobCircularVM.LastUpdateAt = Ordinary.StringToDate(dr["LastUpdateAt"].ToString());
                    JobCircularVM.LastUpdateBy = dr["LastUpdateBy"].ToString();
                    JobCircularVM.LastUpdateFrom = dr["LastUpdateFrom"].ToString();
                    JobCircularVM.DegreeName = dr["DegreeName"].ToString();
                    JobCircularVM.JobLocation = dr["JobLocation"].ToString();
                    JobCircularVMs.Add(JobCircularVM);
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

            return JobCircularVMs;
        }
        //==================SelectByID=================
        public JobCircularVM SelectById(string Id)
        {

            #region Variables

            SqlConnection currConn = null;
            string sqlText = "";
            JobCircularVM JobCircularVM = new JobCircularVM();

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

                sqlText = @"SELECT
                            jc.DesignationId DesignationId,jc.*,dg.Name as DesignationName
                            From JobCircular jc 							
                            Left Outer Join Designation dg on jc.DesignationId=dg.Id
                            where jc.Id=@Id
                            ";

                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
                objComm.Parameters.AddWithValue("@Id", Id);

                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    JobCircularVM.Id = dr["Id"].ToString();
                    JobCircularVM.BranchId = Convert.ToInt32(dr["BranchId"]);
                    JobCircularVM.JobTitle = dr["JobTitle"].ToString();
                    JobCircularVM.DesignationId = dr["DesignationId"].ToString();
                    JobCircularVM.DesignationName = dr["DesignationName"].ToString();
                    JobCircularVM.Deadline = dr["Deadline"].ToString();
                    JobCircularVM.Expriance = dr["Expriance"].ToString();
                    JobCircularVM.Description = dr["Description"].ToString();
                    JobCircularVM.Remarks = dr["Remarks"].ToString();
                    JobCircularVM.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    JobCircularVM.CreatedAt = Ordinary.StringToDate(dr["CreatedAt"].ToString());
                    JobCircularVM.CreatedBy = dr["CreatedBy"].ToString();
                    JobCircularVM.CreatedFrom = dr["CreatedFrom"].ToString();
                    JobCircularVM.LastUpdateAt = Ordinary.StringToDate(dr["LastUpdateAt"].ToString());
                    JobCircularVM.LastUpdateBy = dr["LastUpdateBy"].ToString();
                    JobCircularVM.LastUpdateFrom = dr["LastUpdateFrom"].ToString();
                    JobCircularVM.JobLocation = dr["JobLocation"].ToString();
                    JobCircularVM.Vacancy = dr["Vacancy"].ToString();
                    JobCircularVM.EmploymentStatus = dr["EmploymentStatus"].ToString();
                    JobCircularVM.Workplace = dr["Workplace"].ToString();
                    JobCircularVM.SalaryMax = dr["SalaryMax"].ToString();
                    JobCircularVM.SalaryMin = dr["SalaryMin"].ToString();
                    JobCircularVM.AgeMax = dr["AgeMax"].ToString();
                    JobCircularVM.AgeMin = dr["AgeMin"].ToString();
                    JobCircularVM.DegreeName = dr["DegreeName"].ToString();
                    JobCircularVM.ContactPerson = dr["ContactPerson"].ToString();
                    JobCircularVM.HRDesignation = dr["ContactDesignation"].ToString();
                    JobCircularVM.ContactNo = dr["ContactNo"].ToString();
                    JobCircularVM.AdditionalRequirement = dr["AdditionalRequirement"].ToString();
                    JobCircularVM.ShouldHaveArea = dr["ShouldHaveArea"].ToString();
                    JobCircularVM.ShouldHaveBusiness = dr["ShouldHaveBusiness"].ToString();
                    JobCircularVM.SkillExperties = dr["SkillExperties"].ToString();
                    JobCircularVM.Compensation = dr["Compensation"].ToString();
                    JobCircularVM.ReadBeforeApply = dr["ReadBeforeApply"].ToString();
                    JobCircularVM.Benifit = dr["Benifit"].ToString();

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

            return JobCircularVM;
        }

        public DataSet SelectByIdForReport(string Id)
        {

            #region Variables
            DataSet ds = new DataSet();
            SqlConnection currConn = null;
            string sqlText = "";
            JobCircularVM JobCircularVM = new JobCircularVM();

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

                sqlText = @"SELECT
                            jc.DesignationId DesignationId,jc.*,dg.Name as DesignationName
                            From JobCircular jc 							
                            Left Outer Join Designation dg on jc.DesignationId=dg.Id
                            where jc.Id=@Id
                            ";

                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
                objComm.Parameters.AddWithValue("@Id", Id);

                SqlDataAdapter da = new SqlDataAdapter(objComm);
                da.Fill(ds);

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

            return ds;
        }
        //==================Insert =================
        public string[] Insert(JobCircularVM vm, SqlConnection VcurrConn, SqlTransaction Vtransaction)
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
            retResults[5] = "InsertJobCircular"; //Method Name

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
                #region Exist
                CommonDAL cdal = new CommonDAL();
                bool check = false;
                string tableName = "JobCircular";
                string[] fieldName = { "JobTitle" };
                string[] fieldValue = { vm.JobTitle.Trim() };

                for (int i = 0; i < fieldName.Length; i++)
                {
                    check = cdal.CheckDuplicateInInsertWithBranch(tableName, fieldName[i], fieldValue[i], vm.BranchId, currConn, transaction);
                    if (check == true)
                    {
                        retResults[1] = "This " + fieldName[i] + ": \"" + fieldValue[i] + "\" already used!";
                        throw new ArgumentNullException("This " + fieldName[i] + ": \"" + fieldValue[i] + "\" already used!", "");
                    }
                }
                #endregion Exist


                #endregion open connection and transaction

                #region Save
                sqlText = "Select IsNull(isnull(max(convert(int,  SUBSTRING(CONVERT(varchar(10), id),CHARINDEX('_', CONVERT(varchar(10), id))+1,10))),0),0) from JobCircular where BranchId=@BranchId";
                SqlCommand cmd2 = new SqlCommand(sqlText, currConn);
                cmd2.Parameters.AddWithValue("@BranchId", vm.BranchId);
                cmd2.Transaction = transaction;
                var exeRes = cmd2.ExecuteScalar();
                int count = Convert.ToInt32(exeRes);
                vm.Id = vm.BranchId.ToString() + "_" + (count + 1);
                //int foundId = (int)objfoundId;
                if (1 == 1)
                {

                    sqlText = "  ";
                    sqlText += @" INSERT INTO JobCircular(Id,BranchId,JobTitle,DesignationId,Deadline,Expriance,Description,Remarks,IsActive,IsArchive,CreatedBy,CreatedAt,CreatedFrom,
                                    JobLocation,Vacancy,EmploymentStatus,Workplace,SalaryMax,SalaryMin,AgeMax,AgeMin,DegreeName,ContactPerson,ContactDesignation,ContactNo,
                                    AdditionalRequirement,ShouldHaveArea,ShouldHaveBusiness,SkillExperties,Compensation,ReadBeforeApply,Benifit
                                    ) 
                                VALUES (@Id,@BranchId,@JobTitle,@DesignationId,@Deadline,@Expriance,@Description,@Remarks,@IsActive,@IsArchive,@CreatedBy,@CreatedAt,@CreatedFrom,
                                    @JobLocation,@Vacancy,@EmploymentStatus,@Workplace,@SalaryMax,@SalaryMin,@AgeMax,@AgeMin,@DegreeName,@ContactPerson,@ContactDesignation,@ContactNo,
                                    @AdditionalRequirement,@ShouldHaveArea,@ShouldHaveBusiness,@SkillExperties,@Compensation,@ReadBeforeApply,@Benifit
                                    )";

                    SqlCommand cmdInsert = new SqlCommand(sqlText, currConn);

                    cmdInsert.Parameters.AddWithValue("@Id", vm.Id);
                    cmdInsert.Parameters.AddWithValue("@BranchId", vm.BranchId);
                    cmdInsert.Parameters.AddWithValue("@JobTitle", vm.JobTitle);
                    cmdInsert.Parameters.AddWithValue("@DesignationId", vm.DesignationId);
                    cmdInsert.Parameters.AddWithValue("@Deadline", vm.Deadline);
                    cmdInsert.Parameters.AddWithValue("@Expriance", vm.Expriance);
                    cmdInsert.Parameters.AddWithValue("@Description", vm.Description);
                    cmdInsert.Parameters.AddWithValue("@Remarks", vm.Remarks ?? Convert.DBNull);
                    cmdInsert.Parameters.AddWithValue("@IsActive", true);
                    cmdInsert.Parameters.AddWithValue("@IsArchive", false);
                    cmdInsert.Parameters.AddWithValue("@CreatedBy", vm.CreatedBy);
                    cmdInsert.Parameters.AddWithValue("@CreatedAt", vm.CreatedAt);
                    cmdInsert.Parameters.AddWithValue("@CreatedFrom", vm.CreatedFrom);
                    cmdInsert.Parameters.AddWithValue("@JobLocation", vm.JobLocation);
                    cmdInsert.Parameters.AddWithValue("@Vacancy", vm.Vacancy);
                    cmdInsert.Parameters.AddWithValue("@EmploymentStatus", vm.EmploymentStatus);
                    cmdInsert.Parameters.AddWithValue("@Workplace", vm.Workplace);
                    cmdInsert.Parameters.AddWithValue("@SalaryMax", vm.SalaryMax);
                    cmdInsert.Parameters.AddWithValue("@SalaryMin", vm.SalaryMin);
                    cmdInsert.Parameters.AddWithValue("@AgeMax", vm.AgeMax);
                    cmdInsert.Parameters.AddWithValue("@AgeMin", vm.AgeMin);
                    cmdInsert.Parameters.AddWithValue("@DegreeName", vm.DegreeName);
                    cmdInsert.Parameters.AddWithValue("@ContactPerson", vm.ContactPerson);
                    cmdInsert.Parameters.AddWithValue("@ContactDesignation", vm.HRDesignation);
                    cmdInsert.Parameters.AddWithValue("@ContactNo", vm.ContactNo);
                    cmdInsert.Parameters.AddWithValue("@AdditionalRequirement", vm.AdditionalRequirement);
                    cmdInsert.Parameters.AddWithValue("@ShouldHaveArea", vm.ShouldHaveArea);
                    cmdInsert.Parameters.AddWithValue("@ShouldHaveBusiness", vm.ShouldHaveBusiness);
                    cmdInsert.Parameters.AddWithValue("@SkillExperties", vm.SkillExperties);
                    cmdInsert.Parameters.AddWithValue("@Compensation", vm.Compensation);
                    cmdInsert.Parameters.AddWithValue("@ReadBeforeApply", vm.ReadBeforeApply);
                    cmdInsert.Parameters.AddWithValue("@Benifit", vm.Benifit);


                    cmdInsert.Transaction = transaction;
                    cmdInsert.ExecuteNonQuery();

                }
                else
                {
                    retResults[1] = "This JobCircular already used!";
                    throw new ArgumentNullException("Please Input JobCircular Value", "");
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
                retResults[2] = vm.Id;

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
        //==================Update =================
        public string[] Update(JobCircularVM vm, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {
            #region Variables

            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = "0";
            retResults[3] = "sqlText"; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "JobCircular Update"; //Method Name

            int transResult = 0;

            string sqlText = "";
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            bool iSTransSuccess = false;

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

                if (transaction == null) { transaction = currConn.BeginTransaction("UpdateToJobCircular"); }

                #endregion open connection and transaction
                #region Exist

                CommonDAL cdal = new CommonDAL();
                bool check = false;
                string tableName = "JobCircular";
                string[] fieldName = { "JobTitle" };
                string[] fieldValue = { vm.JobTitle.Trim() };

                for (int i = 0; i < fieldName.Length; i++)
                {
                    check = cdal.CheckDuplicateInUpdateWithBranch(vm.Id, tableName, fieldName[i], fieldValue[i], vm.BranchId, currConn, transaction);
                    if (check == true)
                    {
                        retResults[1] = "This " + fieldName[i] + ": \"" + fieldValue[i] + "\" already used!";
                        throw new ArgumentNullException("This " + fieldName[i] + ": \"" + fieldValue[i] + "\" already used!", "");
                    }
                }
                #endregion Exist
                if (vm != null)
                {
                    #region Update Settings

                    sqlText = "";
                    sqlText = "update JobCircular set";
                    sqlText += " BranchId=@BranchId";
                    sqlText += " , JobTitle=@JobTitle";
                    sqlText += " , Deadline=@Deadline";
                    sqlText += " , Expriance=@Expriance";
                    sqlText += " , Description=@Description";
                    sqlText += " , Remarks=@Remarks";
                    sqlText += " , IsActive=@IsActive";
                    sqlText += " , LastUpdateBy=@LastUpdateBy";
                    sqlText += " , LastUpdateAt=@LastUpdateAt";
                    sqlText += " , LastUpdateFrom=@LastUpdateFrom";
                    sqlText += " , JobLocation=@JobLocation";
                    sqlText += " , Vacancy=@Vacancy";
                    sqlText += " , EmploymentStatus=@EmploymentStatus";
                    sqlText += " , Workplace=@Workplace";
                    sqlText += " , SalaryMax=@SalaryMax";
                    sqlText += " , SalaryMin=@SalaryMin";
                    sqlText += " , AgeMax=@AgeMax";
                    sqlText += " , AgeMin=@AgeMin";
                    sqlText += " , DegreeName=@DegreeName";
                    sqlText += " , ContactPerson=@ContactPerson";
                    sqlText += " , ContactDesignation=@ContactDesignation";
                    sqlText += " , ContactNo=@ContactNo";
                    sqlText += " , AdditionalRequirement=@AdditionalRequirement";
                    sqlText += " , ShouldHaveArea=@ShouldHaveArea";
                    sqlText += " , ShouldHaveBusiness=@ShouldHaveBusiness";
                    sqlText += " , SkillExperties=@SkillExperties";
                    sqlText += " , Compensation=@Compensation";
                    sqlText += " , ReadBeforeApply=@ReadBeforeApply";
                    sqlText += " , Benifit=@Benifit";
                    sqlText += " where Id=@Id";

                    SqlCommand cmdUpdate = new SqlCommand(sqlText, currConn);
                    cmdUpdate.Parameters.AddWithValue("@Id", vm.Id);
                    cmdUpdate.Parameters.AddWithValue("@BranchId", vm.BranchId);
                    cmdUpdate.Parameters.AddWithValue("@JobTitle", vm.JobTitle);
                    cmdUpdate.Parameters.AddWithValue("@Deadline", vm.Deadline);
                    cmdUpdate.Parameters.AddWithValue("@Expriance", vm.Expriance);
                    cmdUpdate.Parameters.AddWithValue("@Description", vm.Description);

                    cmdUpdate.Parameters.AddWithValue("@Remarks", vm.Remarks ?? Convert.DBNull);//, JobCircularVM.Remarks);
                    cmdUpdate.Parameters.AddWithValue("@IsActive", vm.IsActive == null ? true : false);
                    cmdUpdate.Parameters.AddWithValue("@LastUpdateBy", vm.LastUpdateBy);
                    cmdUpdate.Parameters.AddWithValue("@LastUpdateAt", vm.LastUpdateAt);
                    cmdUpdate.Parameters.AddWithValue("@LastUpdateFrom", vm.LastUpdateFrom);

                    cmdUpdate.Parameters.AddWithValue("@JobLocation", vm.JobLocation);
                    cmdUpdate.Parameters.AddWithValue("@Vacancy", vm.Vacancy);
                    cmdUpdate.Parameters.AddWithValue("@EmploymentStatus", vm.EmploymentStatus);
                    cmdUpdate.Parameters.AddWithValue("@Workplace", vm.Workplace);
                    cmdUpdate.Parameters.AddWithValue("@SalaryMax", vm.SalaryMax);
                    cmdUpdate.Parameters.AddWithValue("@SalaryMin", vm.SalaryMin);
                    cmdUpdate.Parameters.AddWithValue("@AgeMax", vm.AgeMax);
                    cmdUpdate.Parameters.AddWithValue("@AgeMin", vm.AgeMin);
                    cmdUpdate.Parameters.AddWithValue("@DegreeName", vm.DegreeName);
                    cmdUpdate.Parameters.AddWithValue("@ContactPerson", vm.ContactPerson);
                    cmdUpdate.Parameters.AddWithValue("@ContactDesignation", vm.HRDesignation);
                    cmdUpdate.Parameters.AddWithValue("@ContactNo", vm.ContactNo);

                    cmdUpdate.Parameters.AddWithValue("@AdditionalRequirement", vm.AdditionalRequirement);
                    cmdUpdate.Parameters.AddWithValue("@ShouldHaveArea", vm.ShouldHaveArea);
                    cmdUpdate.Parameters.AddWithValue("@ShouldHaveBusiness", vm.ShouldHaveBusiness);
                    cmdUpdate.Parameters.AddWithValue("@SkillExperties", vm.SkillExperties);
                    cmdUpdate.Parameters.AddWithValue("@Compensation", vm.Compensation);
                    cmdUpdate.Parameters.AddWithValue("@ReadBeforeApply", vm.ReadBeforeApply);
                    cmdUpdate.Parameters.AddWithValue("@Benifit", vm.Benifit);

                    cmdUpdate.Transaction = transaction;
                    var exeRes = cmdUpdate.ExecuteNonQuery();
                    transResult = Convert.ToInt32(exeRes);

                    retResults[2] = vm.Id.ToString();// Return Id
                    retResults[3] = sqlText; //  SQL Query

                    #region Commit

                    if (transResult <= 0)
                    {
                        // throw new ArgumentNullException("Education Update", JobCircularVM.BranchId + " could not updated.");
                    }

                    #endregion Commit

                    #endregion Update Settings
                    iSTransSuccess = true;
                }
                else
                {
                    throw new ArgumentNullException("JobCircular Update", "Could not found any item.");
                }
                if (iSTransSuccess == true)
                {
                    if (Vtransaction == null)
                    {
                        if (transaction != null)
                        {
                            transaction.Commit();
                        }
                    }
                    retResults[0] = "Success";
                    retResults[1] = "Data Update Successfully.";
                }
                else
                {
                    retResults[1] = "Unexpected error to update JobCircular.";
                    throw new ArgumentNullException("", "");
                }
            }
            #region catch
            catch (Exception ex)
            {
                retResults[0] = "Fail";//Success or Fail
                retResults[4] = ex.Message; //catch ex
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
            return retResults;
        }
        //==================Delete =================
        public string[] Delete(JobCircularVM JobCircularVM, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {
            #region Variables

            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = "0";// Return Id
            retResults[3] = "sqlText"; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "DeleteJobCircular"; //Method Name

            int transResult = 0;
            int countId = 0;
            string sqlText = "";
            SqlConnection currConn = null;
            SqlTransaction transaction = null;

            bool iSTransSuccess = false;

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

                if (transaction == null) { transaction = currConn.BeginTransaction("DeleteToJobCircular"); }

                #endregion open connection and transaction
                #region Check is  it used

                #endregion Check is  it used

                if (JobCircularVM.Id != "")
                {
                    #region Update Settings
                    sqlText = "";
                    sqlText = "update JobCircular set";
                    sqlText += " IsActive=@IsActive,";
                    sqlText += " IsArchive=@IsArchive,";
                    sqlText += " LastUpdateBy=@LastUpdateBy,";
                    sqlText += " LastUpdateAt=@LastUpdateAt,";
                    sqlText += " LastUpdateFrom=@LastUpdateFrom";
                    sqlText += " where Id=@Id";

                    SqlCommand cmdUpdate = new SqlCommand(sqlText, currConn);
                    cmdUpdate.Parameters.AddWithValue("@Id", JobCircularVM.Id);
                    cmdUpdate.Parameters.AddWithValue("@IsActive", false);
                    cmdUpdate.Parameters.AddWithValue("@IsArchive", true);
                    cmdUpdate.Parameters.AddWithValue("@LastUpdateBy", JobCircularVM.LastUpdateBy);
                    cmdUpdate.Parameters.AddWithValue("@LastUpdateAt", JobCircularVM.LastUpdateAt);
                    cmdUpdate.Parameters.AddWithValue("@LastUpdateFrom", JobCircularVM.LastUpdateFrom);

                    cmdUpdate.Transaction = transaction;
                    var exeRes = cmdUpdate.ExecuteNonQuery();
                    transResult = Convert.ToInt32(exeRes);

                    retResults[2] = "";// Return Id
                    retResults[3] = sqlText; //  SQL Query

                    #region Commit

                    if (transResult <= 0)
                    {
                        throw new ArgumentNullException("JobCircular Delete", JobCircularVM.Id + " could not Delete.");
                    }

                    #endregion Commit

                    #endregion Update Settings
                    iSTransSuccess = true;
                }
                else
                {
                    throw new ArgumentNullException("JobCircular Information Delete", "Could not found any item.");
                }


                if (iSTransSuccess == true)
                {
                    if (Vtransaction == null)
                    {
                        if (transaction != null)
                        {
                            transaction.Commit();
                        }
                    }
                    retResults[0] = "Success";
                    retResults[1] = "Data Delete Successfully.";

                }
                else
                {
                    retResults[1] = "Unexpected error to delete JobCircular Information.";
                    throw new ArgumentNullException("", "");
                }

            }
            #region catch
            catch (Exception ex)
            {
                retResults[0] = "Fail";//Success or Fail
                retResults[4] = ex.Message; //catch ex
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

            return retResults;
        }
        #endregion

        public List<JobCircularVM> JobDashboard()
        {
            #region Variables
            SqlConnection currConn = null;
            string sqlText = "";
            List<JobCircularVM> vms = new List<JobCircularVM>();
            JobCircularVM vm = new JobCircularVM();
            #endregion

            try
            {
                #region Open connection
                currConn = _dbsqlConnection.GetConnection();
                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }
                #endregion

                #region SQL statement
                sqlText = @"SELECT 
                            COUNT(ai.JobId) AS TotalApplicant,
                            jc.Id AS JobId,  -- Use jc.Id if JobId is stored as Id in JobCircular
                            jc.Deadline,
                            jc.CreatedAt,
                            jc.JobTitle,
                            COUNT(CASE WHEN ISNULL(ai.IsConfirmed, 0) = 1 THEN 1 END) AS Confirmed,
                            COUNT(CASE WHEN ISNULL(ai.IsShortlisted, 0) = 1 THEN 1 END) AS Shortlisted,
                            COUNT(CASE WHEN ISNULL(ai.IsViewed, 0) = 1 THEN 1 END) AS Viewed,
                            COUNT(CASE WHEN ISNULL(ai.IsViewed, 0) = 0 THEN 1 END) AS NotViewed
                        FROM 
                            ApplicantInfo ai
                        LEFT OUTER JOIN 
                            JobCircular jc ON jc.Id = ai.JobId 
                        WHERE 
                            ai.JobId IS NOT NULL
                        GROUP BY 
                            jc.Id, jc.Deadline, jc.CreatedAt, jc.JobTitle";
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    vm = new JobCircularVM();
                    vm.TotalApplicant = Convert.ToInt32(dr["TotalApplicant"].ToString());
                    vm.Deadline = dr["Deadline"].ToString();
                    vm.JobTitle = dr["JobTitle"].ToString();
                    vm.CreatedAt = dr["CreatedAt"].ToString();
                    vm.JobId = dr["JobId"].ToString();
                    vm.Confirmed = dr["Confirmed"].ToString();
                    vm.Shortlisted = dr["Shortlisted"].ToString();
                    vm.Viewed = dr["Viewed"].ToString();
                    vm.NotViewed = dr["NotViewed"].ToString();             
                    vms.Add(vm);


                    vm.ApplicatTotalMarksVMS = GetApplicatTotalMarksDasboard(vm.JobId);
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
                if (currConn != null && currConn.State == ConnectionState.Open)
                {
                    currConn.Close();
                }
            }
            #endregion

            return vms;
        }
        private List<ApplicatTotalMarksVM> GetApplicatTotalMarksDasboard( string JobId)
        {
            #region Variables
            SqlConnection currConn = null;
            string sqlText = "";

            List<ApplicatTotalMarksVM> avms = new List<ApplicatTotalMarksVM>();
            ApplicatTotalMarksVM vm = new ApplicatTotalMarksVM();

            #endregion

            try
            {
                #region Open connection
                currConn = _dbsqlConnection.GetConnection();
                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }
                #endregion

                #region SQL statement
                sqlText = @"SELECT ai.ApplicantName
                         ,  AVG(Marks) AS AverageMarks    
                      FROM ApplicantMarks am
                      Left Outer Join ApplicantInfo ai on ai.Id=am.ApplicantId 
                     where JobId=@JobId
                      Group by JobId,ApplicantName";
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
                objComm.Parameters.AddWithValue("@JobId", JobId);
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    vm = new ApplicatTotalMarksVM();
                    vm.AvaMarks = dr["AverageMarks"].ToString();
                    vm.ApplicantName = dr["ApplicantName"].ToString();
                    avms.Add(vm);
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
                if (currConn != null && currConn.State == ConnectionState.Open)
                {
                    currConn.Close();
                }
            }
            #endregion
            return avms;
        }
        public List<ApplicantInfoVM> ApplicantProfileList(string JobId)
        {
            #region Variables
            SqlConnection currConn = null;
            string sqlText = "";
            List<ApplicantInfoVM> vms = new List<ApplicantInfoVM>();
            ApplicantInfoVM vm = new ApplicantInfoVM();
                                
            #endregion

            try
            {
                #region Open connection
                currConn = _dbsqlConnection.GetConnection();
                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }
                #endregion

                #region SQL statement
                sqlText = @"WITH LastEducation AS (
                    SELECT
                        ae.*,
                        ROW_NUMBER() OVER (PARTITION BY ae.ApplicantId ORDER BY ae.Id DESC) AS rn
                    FROM ApplicantEducation ae
                ),
                LastSkill AS (
                    SELECT
                        askill.*,
                        ROW_NUMBER() OVER (PARTITION BY askill.ApplicantId ORDER BY askill.Id DESC) AS rn
                    FROM ApplicantSkill askill
                )
                SELECT ai.*, le.*, ls.*, case when ai.IsShortlisted='1' then 'Shortlisted'  when ai.IsArchive='1' then 'Rejected'  end as Status,
				 DATEDIFF(YEAR, DateOfBirth, GETDATE()) 
			     - CASE 
				  WHEN MONTH(DateOfBirth) > MONTH(GETDATE()) 
					   OR (MONTH(DateOfBirth) = MONTH(GETDATE()) AND DAY(DateOfBirth) > DAY(GETDATE())) 
				  THEN 1 
				  ELSE 0 
				END AS Age
                FROM ApplicantInfo ai
                LEFT OUTER JOIN LastEducation le ON le.ApplicantId = ai.Id AND le.rn = 1
                LEFT OUTER JOIN LastSkill ls ON ls.ApplicantId = ai.Id AND ls.rn = 1
                WHERE ai.JobId = @JobId";
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
                objComm.Parameters.AddWithValue("@JobId", JobId);
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    vm = new ApplicantInfoVM();
                    vm.ApplicantName = dr["ApplicantName"].ToString();
                    vm.PresentAddress = dr["PresentAddress"].ToString();
                    vm.LastEducation = dr["LastEducation"].ToString();
                    vm.ExpectedSalary = dr["ExpectedSalary"].ToString();
                    vm.Email = dr["Email"].ToString();
                    vm.ContactNo = dr["ContactNo"].ToString();
                    vm.Experience = dr["Experience"].ToString();
                    vm.CreatedAt = dr["CreatedAt"].ToString();
                    vm.ImageFileName = dr["ImageFileName"].ToString();
                    vm.Id = dr["Id"].ToString();
                    vm.ExamTitle = dr["ExamTitle"].ToString();
                    vm.Institute = dr["Institute"].ToString();
                    vm.Skill = dr["Skill"].ToString();
                    vm.SkillDescription = dr["SkillDescription"].ToString();
                    vm.Status = dr["Status"].ToString();
                    vm.FaceBook = dr["FaceBook"].ToString();
                    vm.linkedIn = dr["linkedIn"].ToString();
                    vm.VideoCv = dr["VideoCv"].ToString();
                    vm.Age = dr["Age"].ToString();
                    vm.Jobid = JobId;

                    vm.ApplicantSalaryVMS = GetApplicantSalary(vm.Id, JobId);
                    vm.ApplicantMarksVMS = GetApplicantMarks(vm.Id, JobId);
                    vm.ApplicatTotalMarksVMS = GetApplicatTotalMarksVM(vm.Id, JobId);
                    vm.MatchPerventsVMs = GetMatchPervents(vm.Id, JobId);
                    vms.Add(vm);
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
                if (currConn != null && currConn.State == ConnectionState.Open)
                {
                    currConn.Close();
                }
            }
            #endregion

            return vms;
        }

        private List<MatchPerventsVM> GetMatchPervents(string Id, string JobId)
        {
            #region Variables
            SqlConnection currConn = null;
            string sqlText = "";

            List<MatchPerventsVM> avms = new List<MatchPerventsVM>();
            MatchPerventsVM vm = new MatchPerventsVM();

            #endregion

            try
            {
                #region Open connection
                currConn = _dbsqlConnection.GetConnection();
                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }
                #endregion

                #region SQL statement
                sqlText = @"
                        WITH ApplicantAge AS (   
                            SELECT 
                                Id,
                                DATEDIFF(YEAR, DateOfBirth, GETDATE()) AS Age,
                                TRY_CAST(Experience AS INT) AS Experience,
                                ExpectedSalary,
                                LastEducation
                            FROM 
                                [ShampanHRM_DB].[dbo].[ApplicantInfo]
                        ),
                        JobMatching AS (
                            SELECT 
                                a.Id,
                                j.Expriance AS JobExperience,
                                j.SalaryMax,
                                j.AgeMax,
                                j.DegreeName,
                                a.Experience AS ApplicantExperience,
                                a.ExpectedSalary,
                                a.Age,
                                a.LastEducation,

                                CASE 
                                    WHEN a.Experience >= j.Expriance THEN 100
                                    ELSE CAST((a.Experience * 100 / j.Expriance) AS INT)
                                END AS ExperienceMatch,

                                CASE 
                                    WHEN a.ExpectedSalary <= j.SalaryMax THEN 100
                                    ELSE 0
                                END AS SalaryMatch,

                                CASE 
                                    WHEN a.Age <= j.AgeMax THEN 100
                                    ELSE 0
                                END AS AgeMatch,

                                CASE 
                                    WHEN a.LastEducation = j.DegreeName THEN 100
                                    ELSE 0
                                END AS EducationMatch
                            FROM 
                                ApplicantAge a
                            LEFT JOIN 
                                [ShampanHRM_DB].[dbo].[JobCircular] j
                            ON 
                                a.LastEducation = j.DegreeName 
                        )
                        -- Calculate individual and average match percentage
                        SELECT 
                            Id,
                            ISNULL(AVG(CAST((ExperienceMatch * 0.4 + SalaryMatch * 0.2 + AgeMatch * 0.2 + EducationMatch * 0.2) AS INT)),0) AS 
	                        MatchPercentage
                        FROM 
                            JobMatching  
                        WHERE Id=@Id
                        GROUP BY 
                            Id";
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
                objComm.Parameters.AddWithValue("@Id", Id);
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    vm = new MatchPerventsVM();
                    vm.Id =Convert.ToInt32(dr["Id"].ToString());
                    vm.MatchPercentage =Convert.ToInt32( dr["MatchPercentage"].ToString());
                    avms.Add(vm);
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
                if (currConn != null && currConn.State == ConnectionState.Open)
                {
                    currConn.Close();
                }
            }
            #endregion
            return avms;
        }

        private List<ApplicatTotalMarksVM> GetApplicatTotalMarksVM(string Id, string JobId)
        {
            #region Variables
            SqlConnection currConn = null;
            string sqlText = "";

            List<ApplicatTotalMarksVM> avms = new List<ApplicatTotalMarksVM>();
            ApplicatTotalMarksVM vm = new ApplicatTotalMarksVM();

            #endregion

            try
            {
                #region Open connection
                currConn = _dbsqlConnection.GetConnection();
                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }
                #endregion

                #region SQL statement
                        sqlText = @"SELECT ApplicantId
                             ,AVG(Marks) AS AverageMarks
                             ,SUM(Marks) TotalMarks
                          FROM ApplicantMarks 
                          where ApplicantId=@ApplicantId
                          Group by ApplicantId";
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
                objComm.Parameters.AddWithValue("@ApplicantId",Id);
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    vm = new ApplicatTotalMarksVM();
                    vm.AvaMarks = dr["AverageMarks"].ToString();
                    vm.Total = dr["TotalMarks"].ToString();                  
                    avms.Add(vm);
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
                if (currConn != null && currConn.State == ConnectionState.Open)
                {
                    currConn.Close();
                }
            }
            #endregion
            return avms;
        }

        public List<ApplicantMarksVM> GetApplicantMarks(string Id,string JobId)
        {
            #region Variables
            SqlConnection currConn = null;
            string sqlText = "";

            List<ApplicantMarksVM> svms = new List<ApplicantMarksVM>();
            ApplicantMarksVM vm = new ApplicantMarksVM();

            #endregion

            try
            {
                #region Open connection
                currConn = _dbsqlConnection.GetConnection();
                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }
                #endregion

                #region SQL statement
                sqlText = @"Select * from ApplicantMarks where ApplicantId=@ApplicantId";
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
                objComm.Parameters.AddWithValue("@ApplicantId", Id);
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    vm = new ApplicantMarksVM();
                    vm.ApplicantId = dr["ApplicantId"].ToString();
                    vm.UserName = dr["UserName"].ToString();
                    vm.Marks = Convert.ToInt32(dr["Marks"].ToString());
                    vm.Id = Convert.ToInt32(dr["Id"].ToString());
                    vm.JobId = JobId;
                    svms.Add(vm);
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
                if (currConn != null && currConn.State == ConnectionState.Open)
                {
                    currConn.Close();
                }
            }
            #endregion

            return svms;
        }

        public List<ApplicantSalaryVM> GetApplicantSalary(string Id, string JobId)
        {
            #region Variables
            SqlConnection currConn = null;
            string sqlText = "";

            List<ApplicantSalaryVM> svms = new List<ApplicantSalaryVM>();
            ApplicantSalaryVM vm = new ApplicantSalaryVM();

            #endregion

            try
            {
                #region Open connection
                currConn = _dbsqlConnection.GetConnection();
                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }
                #endregion

                #region SQL statement
                sqlText = @"Select * from ApplicantSalary where ApplicantId=@ApplicantId";
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
                objComm.Parameters.AddWithValue("@ApplicantId", Id);
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    vm = new ApplicantSalaryVM();
                    vm.ApplicantId = dr["ApplicantId"].ToString();
                    vm.Date = dr["Date"].ToString();
                    vm.Salary = Convert.ToInt32(dr["Salary"].ToString());
                    vm.Id = Convert.ToInt32(dr["Id"].ToString());
                    vm.JobId = JobId;
                    svms.Add(vm);
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
                if (currConn != null && currConn.State == ConnectionState.Open)
                {
                    currConn.Close();
                }
            }
            #endregion

            return svms;
        }

        public ApplicantInfoVM ApplicantProfile(string Id)
        {
            #region Variables
            SqlConnection currConn = null;
            string sqlText = "";
            ApplicantInfoVM vm = new ApplicantInfoVM();

            #endregion

            try
            {
                #region Open connection
                currConn = _dbsqlConnection.GetConnection();
                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }
                #endregion

                #region SQL statement
                sqlText = @"Select * from ApplicantInfo where Id=@Id";
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
                objComm.Parameters.AddWithValue("@Id", Id);
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    vm = new ApplicantInfoVM();
                    vm.ApplicantName = dr["ApplicantName"].ToString();
                    vm.PresentAddress = dr["PresentAddress"].ToString();
                    vm.PermanentAddress = dr["PermanentAddress"].ToString();
                    vm.ContactNo = dr["ContactNo"].ToString();
                    vm.Designation = dr["Designation"].ToString();
                    vm.Email = dr["Email"].ToString();
                    vm.LastEducation = dr["LastEducation"].ToString();
                    vm.Gender = dr["Gender"].ToString();
                    vm.Experience = dr["Experience"].ToString();
                    vm.PresentSalary = dr["PresentSalary"].ToString();
                    vm.ExpectedSalary = dr["ExpectedSalary"].ToString();
                    vm.AttachmentFile = dr["AttachmentFile"].ToString();
                    vm.CreatedAt = dr["CreatedAt"].ToString();
                    vm.RecentCompany = dr["RecentCompany"].ToString();
                    vm.EmploymentHistory = dr["EmploymentHistory"].ToString();
                    vm.FatherName = dr["FatherName"].ToString();
                    vm.MotherName = dr["MotherName"].ToString();
                    vm.DateOfBirth = dr["DateOfBirth"].ToString();
                    vm.MaritalStatus = dr["MaritalStatus"].ToString();
                    vm.Nationality = dr["Nationality"].ToString();
                    vm.Religion = dr["Religion"].ToString();
                    vm.BloodGroup = dr["BloodGroup"].ToString();
                    vm.ImageFileName = dr["ImageFileName"].ToString();
                    vm.CoverLetter = dr["CoverLetter"].ToString();
                    vm.LookingFor = dr["LookingFor"].ToString();
                    vm.AvailableFor = dr["AvailableFor"].ToString();
                    vm.NoticePeriod = dr["NoticePeriod"].ToString();
                    vm.Studying = dr["Studying"].ToString();
                    vm.Height = dr["Height"].ToString();
                    vm.Weight = dr["Weight"].ToString();
                    vm.FaceBook = dr["FaceBook"].ToString();
                    vm.linkedIn = dr["linkedIn"].ToString();
                    vm.VideoCv = dr["VideoCv"].ToString();
                    vm.Jobid = dr["JobId"].ToString();
                    vm.Id = dr["Id"].ToString();
                }
                dr.Close();

                sqlText = "";
                sqlText = @"Update ApplicantInfo set IsViewed =@IsViewed  where Id=@Id";
                SqlCommand cmdInsert = new SqlCommand(sqlText, currConn);
                cmdInsert.Parameters.AddWithValue("@Id", vm.Id);
                cmdInsert.Parameters.AddWithValue("@IsViewed", true);              
                cmdInsert.ExecuteNonQuery();

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
                if (currConn != null && currConn.State == ConnectionState.Open)
                {
                    currConn.Close();
                }
            }
            #endregion

            return vm;
        }

        public string[] ApplicantApplyEdit(ApplicantInfoVM vm, SqlConnection VcurrConn, SqlTransaction Vtransaction)
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
            retResults[5] = "ApplicantApplyEdit"; //Method Name
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
                          ,IsActive =@IsActive
                          ,IsArchive=@IsArchive
                          ,IsApproved=@IsApproved                        
                          ,Studying=@Studying
                          ,IsShortlisted=@IsShortlisted
                          ,EmploymentHistory=@EmploymentHistory
                          ,AcademicQualification=@AcademicQualification
                          ,ProfessionalQualification=@ProfessionalQualification
                          ,LookingFor=@LookingFor
                          ,AvailableFor=@AvailableFor
                          ,FatherName=@FatherName
                          ,MotherName=@MotherName
                          ,MaritalStatus=@MaritalStatus
                          ,Nationality=@Nationality
                          ,Religion=@Religion
                          ,BloodGroup=@BloodGroup
                          ,JobId=@JobId                      
                          ";
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
                        cmdInsert.Parameters.AddWithValue("@NoticePeriod", vm.NoticePeriod);
                        cmdInsert.Parameters.AddWithValue("@PresentSalary", vm.PresentSalary);
                        cmdInsert.Parameters.AddWithValue("@ExpectedSalary", vm.ExpectedSalary);
                        cmdInsert.Parameters.AddWithValue("@Studying", vm.Studying);
                        cmdInsert.Parameters.AddWithValue("@IsShortlisted", vm.IsShortlisted);
                        cmdInsert.Parameters.AddWithValue("@EmploymentHistory", vm.EmploymentHistory);
                        cmdInsert.Parameters.AddWithValue("@AcademicQualification", vm.AcademicQualification);
                        cmdInsert.Parameters.AddWithValue("@ProfessionalQualification", vm.ProfessionalQualification);
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
                        cmdInsert.Parameters.AddWithValue("@JobId", vm.Jobid);
                        if (!string.IsNullOrEmpty(vm.AttachmentFile))
                        {
                            cmdInsert.Parameters.AddWithValue("@AttachmentFile", vm.AttachmentFile);
                        }
                        cmdInsert.Parameters.AddWithValue("@IsActive", vm.IsActive);
                        cmdInsert.Parameters.AddWithValue("@IsArchive", vm.IsArchive);
                        cmdInsert.Parameters.AddWithValue("@IsApproved", vm.IsApproved);

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

        public List<ProfessionalQualificationVM> ApplicantPQ(string Id)
        {

            SqlConnection currConn = null;
            string sqlText = "";
            List<ProfessionalQualificationVM> vms = new List<ProfessionalQualificationVM>();
            ProfessionalQualificationVM vm = new ProfessionalQualificationVM();

            try
            {
                currConn = _dbsqlConnection.GetConnection();
                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }


                sqlText = @"Select * from ApplicantProfessionalQualification where ApplicantId=@Id";
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
                objComm.Parameters.AddWithValue("@Id", Id);
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    vm = new ProfessionalQualificationVM();
                    vm.Certification = dr["Certification"].ToString();
                    vm.PQInstitute = dr["PQInstitute"].ToString();
                    vm.Location = dr["Location"].ToString();
                    vm.FromDate = dr["FromDate"].ToString();
                    vm.ToDate = dr["ToDate"].ToString();
                    vm.Id =Convert.ToInt32(dr["Id"].ToString());
                    vm.ApplicantId = dr["ApplicantId"].ToString();
                    vms.Add(vm);

                }
                dr.Close();
            }
            catch (Exception ex)
            {

            }
            {
                if (currConn != null && currConn.State == ConnectionState.Open)
                {
                    currConn.Close();
                }
            }

            return vms;
        }
        public List<ApplicantTrainingVM> ApplicantTS(string Id)
        {

            SqlConnection currConn = null;
            string sqlText = "";
            List<ApplicantTrainingVM> vms = new List<ApplicantTrainingVM>();
            ApplicantTrainingVM vm = new ApplicantTrainingVM();

            try
            {
                currConn = _dbsqlConnection.GetConnection();
                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }


                sqlText = @"Select * from ApplicantTraining where ApplicantId=@Id";
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
                objComm.Parameters.AddWithValue("@Id", Id);
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    vm = new ApplicantTrainingVM();
                    vm.TrainingTitle = dr["TrainingTitle"].ToString();
                    vm.Topic = dr["Topic"].ToString();
                    vm.Institute = dr["Institute"].ToString();
                    vm.Country = dr["Country"].ToString();
                    vm.Location = dr["Location"].ToString();
                    vm.Year = dr["Year"].ToString();
                    vm.Duration = dr["Duration"].ToString();
                    vm.Id = Convert.ToInt32(dr["Id"].ToString());
                    vm.ApplicantId = dr["ApplicantId"].ToString();
                    vms.Add(vm);
                }
                dr.Close();
            }
            catch (Exception ex)
            {

            }
            {
                if (currConn != null && currConn.State == ConnectionState.Open)
                {
                    currConn.Close();
                }
            }

            return vms;
        }
        public List<ApplicantLanguageVM> ApplicantLS(string Id)
        {

            SqlConnection currConn = null;
            string sqlText = "";
            List<ApplicantLanguageVM> vms = new List<ApplicantLanguageVM>();
            ApplicantLanguageVM vm = new ApplicantLanguageVM();

            try
            {
                currConn = _dbsqlConnection.GetConnection();
                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }


                sqlText = @"Select * from ApplicantLanguage where ApplicantId=@Id";
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
                objComm.Parameters.AddWithValue("@Id", Id);
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    vm = new ApplicantLanguageVM();
                    vm.Language = dr["Language"].ToString();
                    vm.Reading = dr["Reading"].ToString();
                    vm.Writing = dr["Writing"].ToString();
                    vm.Speaking = dr["Speaking"].ToString();
                    vm.Id = Convert.ToInt32(dr["Id"].ToString());
                    vm.ApplicantId = dr["ApplicantId"].ToString();
                    vms.Add(vm);
                }
                dr.Close();
            }
            catch (Exception ex)
            {

            }
            {
                if (currConn != null && currConn.State == ConnectionState.Open)
                {
                    currConn.Close();
                }
            }

            return vms;
        }

        public List<ApplicantEmployeementHistoryVM> ApplicantTEH(string Id)
        {
            SqlConnection currConn = null;
            string sqlText = "";
            List<ApplicantEmployeementHistoryVM> vms = new List<ApplicantEmployeementHistoryVM>();
            ApplicantEmployeementHistoryVM vm = new ApplicantEmployeementHistoryVM();

            try
            {
                currConn = _dbsqlConnection.GetConnection();
                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }


                sqlText = @"Select * from ApplicantEmployeementHistory where ApplicantId=@Id";
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
                objComm.Parameters.AddWithValue("@Id", Id);
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    vm = new ApplicantEmployeementHistoryVM();
                    vm.CompanyName = dr["CompanyName"].ToString();
                    vm.CompanyBusiness = dr["CompanyBusiness"].ToString();
                    vm.ApplicantDesignation = dr["ApplicantDesignation"].ToString();
                    vm.ApplicantDepartment = dr["ApplicantDepartment"].ToString();
                    vm.EmploymentPeriod = dr["EmploymentPeriod"].ToString();
                    vm.ToDate = dr["ToDate"].ToString();
                    vm.CurrentlyWorking = dr["CurrentlyWorking"].ToString();
                    vm.Responsibilities = dr["Responsibilities"].ToString();
                    vm.AreaOfExperience = dr["AreaOfExperience"].ToString();
                    vm.CompanyLocation = dr["CompanyLocation"].ToString();
                    vm.CompanyLocation = dr["CompanyLocation"].ToString();
                    vm.Id = Convert.ToInt32(dr["Id"].ToString());
                    vm.ApplicantId = dr["ApplicantId"].ToString();
                    vms.Add(vm);
                }
                dr.Close();
            }
            catch (Exception ex)
            {

            }
            {
                if (currConn != null && currConn.State == ConnectionState.Open)
                {
                    currConn.Close();
                }
            }

            return vms;
        }

        public List<ApplicantSkillVM> ApplicantSK(string Id)
        {
            SqlConnection currConn = null;
            string sqlText = "";
            List<ApplicantSkillVM> vms = new List<ApplicantSkillVM>();
            ApplicantSkillVM vm = new ApplicantSkillVM();

            try
            {
                currConn = _dbsqlConnection.GetConnection();
                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }


                sqlText = @"Select * from ApplicantSkill where ApplicantId=@Id";
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
                objComm.Parameters.AddWithValue("@Id", Id);
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    vm = new ApplicantSkillVM();
                    vm.Skill = dr["Skill"].ToString();
                    vm.SkillDescription = dr["SkillDescription"].ToString();
                    vm.ExtraCurricular = dr["ExtraCurricular"].ToString();
                    vm.Id = Convert.ToInt32(dr["Id"].ToString());
                    vm.ApplicantId = dr["ApplicantId"].ToString();
                    vms.Add(vm);
                }
                dr.Close();
            }
            catch (Exception ex)
            {

            }
            {
                if (currConn != null && currConn.State == ConnectionState.Open)
                {
                    currConn.Close();
                }
            }

            return vms;
        }

        public List<EducationVM> ApplicantED(string Id)
        {
            SqlConnection currConn = null;
            string sqlText = "";
            List<EducationVM> vms = new List<EducationVM>();
            EducationVM vm = new EducationVM();

            try
            {
                currConn = _dbsqlConnection.GetConnection();
                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }


                sqlText = @"Select * from ApplicantEducation where ApplicantId=@Id";
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
                objComm.Parameters.AddWithValue("@Id", Id);
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    vm = new EducationVM();
                    vm.ExamTitle = dr["ExamTitle"].ToString();
                    vm.Major = dr["Major"].ToString();
                    vm.Institute = dr["Institute"].ToString();
                    vm.PassYear = dr["PassYear"].ToString();
                    vm.Duration = dr["Duration"].ToString();
                    vm.Achievment = dr["Achievment"].ToString();
                    vm.Id = Convert.ToInt32(dr["Id"].ToString());
                    vm.ApplicantId = dr["ApplicantId"].ToString();
                    vms.Add(vm);
                }
                dr.Close();
            }
            catch (Exception ex)
            {

            }
            {
                if (currConn != null && currConn.State == ConnectionState.Open)
                {
                    currConn.Close();
                }
            }

            return vms;
        }

        public string[] EditApplicantStatus(string Id, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {
            #region Initializ
            string sqlText = "";          
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = Id.ToString();// Return Id
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "ApplicantApplyEdit"; //Method Name
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

               
                    if (Id != null)
                    {
                        sqlText = "  ";
                        sqlText += @"Update ApplicantInfo  set   
                          IsShortlisted=@IsShortlisted
                          ";
                        sqlText += " where Id=@Id ";

                        SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                        cmdInsert.Parameters.AddWithValue("@Id", Id);
                        cmdInsert.Parameters.AddWithValue("@IsShortlisted", true);
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
                retResults[1] = "Applicant Shortlisted";
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

        public string[] EditApplicantStatusReject(string Id, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {
            #region Initializ
            string sqlText = "";
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = Id.ToString();// Return Id
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "ApplicantApplyEdit"; //Method Name
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


                if (Id != null)
                {
                    sqlText = "  ";
                    sqlText += @"Update ApplicantInfo  set   
                          IsArchive=@IsArchive
                          ,IsActive=@IsActive
                          ,IsShortlisted=@IsShortlisted
                          ";
                    sqlText += " where Id=@Id ";

                    SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                    cmdInsert.Parameters.AddWithValue("@Id", Id);
                    cmdInsert.Parameters.AddWithValue("@IsArchive", true);
                    cmdInsert.Parameters.AddWithValue("@IsActive", false);
                    cmdInsert.Parameters.AddWithValue("@IsShortlisted", false);
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
                retResults[1] = " Applicant Rejected";
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

        public string[] InsertApplicantMarks(ApplicantInfoVM vm, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {
            ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
            vm.CreatedAt = identity.FullName;
            #region Initializ
            string sqlText = "";
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message          
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "ApplicantApplyEdit"; //Method Name
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


                if (vm.Id != null)
                {
                    sqlText = "  ";
                    sqlText += @"INSERT INTO ApplicantMarks
                           (ApplicantId
                           ,UserName
                           ,Marks)
                             VALUES
                           (@ApplicantId 
                           ,@UserName
                           ,@Marks)
                           ";
                 
                    SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                    cmdInsert.Parameters.AddWithValue("@ApplicantId", vm.Id);
                    cmdInsert.Parameters.AddWithValue("@UserName", vm.CreatedAt);
                    cmdInsert.Parameters.AddWithValue("@Marks", vm.ApplicantMarksVM.Marks);                    
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
                retResults[1] = " Save Success";
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
                        retResults[1] = "Unexpected error to update ApplicantMarks.";
                        return retResults;
                        // throw new ArgumentNullException("Unexpected error to update ApplicantMarks.", "ApplicantMarks");
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

        public string[] InsertApplicantSalary(ApplicantInfoVM vm, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {
            ShampanIdentity identity = (ShampanIdentity)Thread.CurrentPrincipal.Identity;
            vm.CreatedAt = identity.FullName;
            #region Initializ
            string sqlText = "";
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message          
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "ApplicantApplyEdit"; //Method Name
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


                if (vm.Id != null)
                {
                    sqlText = "  ";
                    sqlText += @"INSERT INTO ApplicantSalary
                           (ApplicantId
                           ,Date
                           ,Salary)
                             VALUES
                           (@ApplicantId 
                           ,@Date
                           ,@Salary)
                           ";

                    SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                    cmdInsert.Parameters.AddWithValue("@ApplicantId", vm.Id);
                    cmdInsert.Parameters.AddWithValue("@Date",DateTime.Now);
                    cmdInsert.Parameters.AddWithValue("@Salary", vm.ApplicantSalaryVM.Salary);
                    cmdInsert.ExecuteNonQuery();
                }


                #endregion Date Create

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
                retResults[1] = "Your Salary Save Successfully";
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
                        retResults[1] = "Unexpected error to update InsertApplicantSalary.";
                        return retResults;
                        // throw new ArgumentNullException("Unexpected error to update InsertApplicantSalary.", "InsertApplicantSalary");
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
    }
}
