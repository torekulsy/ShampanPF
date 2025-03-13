using SymOrdinary;
using SymViewModel.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymServices.Common
{
    public class AppraisalMarksDAL
    {
        private const string FieldDelimeter = DBConstant.FieldDelimeter;
        private DBSQLConnection _dbsqlConnection = new DBSQLConnection();

        public List<AppraisalMarksDetailVM> SelectAllQuestionByDepartment(string Id, SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
        {
            #region Variables
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            string sqlText = "";
            List<AppraisalMarksDetailVM> VMs = new List<AppraisalMarksDetailVM>();
            AppraisalMarksDetailVM vm;
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
                sqlText = @"                       
	                     Select ISNULL(aqsd.CategoryId,0)CategoryId,ac.CategoryName,aqsd.QuestionId,qb.Question,aqsd.IsOwn,aqsd.IsTeamLead,aqsd.IsHR,aqsd.IsCOO,aqsd.IsMd 
                    ,IsP1,IsP2,IsP3,IsP4,IsP5
                    from AppraisalQuestionsSet aqs
                    Left Outer Join AppraisalQuestionsSetDetails aqsd on aqsd.QuestionsetId =aqs.id
                    Left Outer Join AppraisalCategory ac on ac.id=aqsd.CategoryId
                    Left Outer Join AppraisalQuestionBank qb on qb.id=aqsd.QuestionId
                    where 1=1
                        ";

                if (Id != "")
                {
                    sqlText += @"  and aqs.DepartmentId=@dId";
                }


                SqlCommand objComm = new SqlCommand(sqlText, currConn, transaction);

                if (Id != "")
                {
                    objComm.Parameters.AddWithValue("@dId", Id);
                }
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    vm = new AppraisalMarksDetailVM();
                    vm.CategoryId = dr["CategoryId"].ToString();
                    vm.CategoryName = dr["CategoryName"].ToString();
                    vm.QuestionId = dr["QuestionId"].ToString();
                    vm.QuestionName = dr["Question"].ToString();
                    vm.IsOwn = dr["IsOwn"].ToString();
                    vm.IsTeamLead = dr["IsTeamLead"].ToString();
                    vm.IsHR = dr["IsHR"].ToString();
                    vm.IsCOO = dr["IsCOO"].ToString();
                    vm.IsMD = dr["IsMd"].ToString();
                    vm.IsP1 = dr["IsP1"].ToString();
                    vm.IsP2 = dr["IsP2"].ToString();
                    vm.IsP3 = dr["IsP3"].ToString();
                    vm.IsP4 = dr["IsP4"].ToString();
                    vm.IsP5 = dr["IsP5"].ToString();
                    VMs.Add(vm);
                }
                dr.Close();
                if (Vtransaction == null && transaction != null)
                {
                    transaction.Commit();
                }
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
                if (VcurrConn == null && currConn != null && currConn.State == ConnectionState.Open)
                {
                    currConn.Close();
                }
            }
            #endregion
            return VMs;
        }

        public List<AppraisalMarksDetailVM> SelectAllQuestionByEmployeeExist(string AssignFrom, string dId, string EmployeeCode, SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
        {
            #region Variables
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            string sqlText = "";
            List<AppraisalMarksDetailVM> VMs = new List<AppraisalMarksDetailVM>();
            AppraisalMarksDetailVM vm;
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
                sqlText = @"                       
	                  
                  Select ISNULL(aqsd.CategoryId,0)CategoryId,ac.CategoryName,aqsd.QuestionId,qb.Question,aqsd.IsOwn,aqsd.IsTeamLead,aqsd.IsHR,aqsd.IsCOO,aqsd.IsMd 
                    ,IsP1,IsP2,IsP3,IsP4,IsP5
                    from AppraisalAssignToEmployee aqs
                    Left Outer Join AppraisalAssignToEmployeeDetails aqsd on aqsd.QuestionsetId =aqs.id
                    Left Outer Join AppraisalCategory ac on ac.id=aqsd.CategoryId
                    Left Outer Join AppraisalQuestionBank qb on qb.id=aqsd.QuestionId
                    where 1=1 AND (
                            aqsd.IsCOO = 1 
		                    OR aqsd.IsHR = 1 
                            OR aqsd.IsMd = 1 
                            OR aqsd.IsP1 = 1 
                            OR aqsd.IsP2 = 1 
                            OR aqsd.IsP3 = 1 
                            OR aqsd.IsP4 = 1 
                            OR aqsd.IsP5 = 1
                        )";

                if (dId != "")
                {
                    sqlText += @"  and aqs.DepartmentId=@DepartmentId";
                }
                if (EmployeeCode != "")
                {
                    sqlText += @"  and aqs.EmployeeCode=@EmployeeCode";
                }
                if (AssignFrom != "")
                {
                    sqlText += @"  and aqsd.Is" + AssignFrom + "=@AssignToId";
                }                

                SqlCommand objComm = new SqlCommand(sqlText, currConn, transaction);

                if (dId != "")
                {
                    objComm.Parameters.AddWithValue("@DepartmentId", dId);
                }
                if (EmployeeCode != "")
                {
                    objComm.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                }
                if (AssignFrom != "")
                {
                    objComm.Parameters.AddWithValue("@AssignToId", 1);
                }
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    if (dr["CategoryId"].ToString() == "0")
                    {

                    }
                    else
                    {
                        vm = new AppraisalMarksDetailVM();
                        vm.CategoryId = dr["CategoryId"].ToString();
                        vm.CategoryName = dr["CategoryName"].ToString();
                        vm.QuestionId = dr["QuestionId"].ToString();
                        vm.QuestionName = dr["Question"].ToString();
                        vm.IsOwn = dr["IsOwn"].ToString();
                        vm.IsTeamLead = dr["IsTeamLead"].ToString();
                        vm.IsHR = dr["IsHR"].ToString();
                        vm.IsCOO = dr["IsCOO"].ToString();
                        vm.IsMD = dr["IsMd"].ToString();
                        vm.IsP1 = dr["IsP1"].ToString();
                        vm.IsP2 = dr["IsP2"].ToString();
                        vm.IsP3 = dr["IsP3"].ToString();
                        vm.IsP4 = dr["IsP4"].ToString();
                        vm.IsP5 = dr["IsP5"].ToString();
                        VMs.Add(vm);
                    }
                }
                dr.Close();
                if (Vtransaction == null && transaction != null)
                {
                    transaction.Commit();
                }
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
                if (VcurrConn == null && currConn != null && currConn.State == ConnectionState.Open)
                {
                    currConn.Close();
                }
            }
            #endregion
            return VMs;
        }

        public List<AppraisalMarksDetailVM> SelectMarksByEmployeeExist(string dId, string EmployeeCode, string EvaluationForId, string AssignFrom, SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
        {
            #region Variables
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            string sqlText = "";
            List<AppraisalMarksDetailVM> VMs = new List<AppraisalMarksDetailVM>();
            AppraisalMarksDetailVM vm;
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
                sqlText = @"                       
	                   Select ISNULL(aqs.CategoryId,0)CategoryId,aqs.CategoryName,aqs.QuestionId,aqs.QuestionName,aqs.Own,aqs.TeamLead,aqs.HR,aqs.COO,aqs.Md 
                    ,P1,P2,P3,P4,P5
                    from AppraisalMarkSetups aqs               
                    where 1=1  
                            ";

                if (dId != "")
                {
                    sqlText += @"  and aqs.DepartmentId=@DepartmentId";
                }
                if (EmployeeCode != "")
                {
                    sqlText += @"  and aqs.EmployeeCode=@EmployeeCode";
                }
                if (EvaluationForId != "")
                {
                    sqlText += @"  and aqs.EvaluationForId=@EvaluationForId";
                }
                if (AssignFrom != "")
                {
                    sqlText += @"  and aqs.AssignFrom=@AssignFrom";
                }

                SqlCommand objComm = new SqlCommand(sqlText, currConn, transaction);

                if (dId != "")
                {
                    objComm.Parameters.AddWithValue("@DepartmentId", dId);
                }
                if (EmployeeCode != "")
                {
                    objComm.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                }
                if (EvaluationForId != "")
                {
                    objComm.Parameters.AddWithValue("@EvaluationForId", EvaluationForId);
                }
                if (EvaluationForId != "")
                {
                    objComm.Parameters.AddWithValue("@AssignFrom", AssignFrom);
                }
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    if (dr["CategoryId"].ToString() == "0")
                    {

                    }
                    else
                    {
                        vm = new AppraisalMarksDetailVM();
                        vm.CategoryId = dr["CategoryId"].ToString();
                        vm.CategoryName = dr["CategoryName"].ToString();
                        vm.QuestionId = dr["QuestionId"].ToString();
                        vm.QuestionName = dr["QuestionName"].ToString();
                        vm.IsOwn = dr["Own"].ToString();
                        vm.IsTeamLead = dr["TeamLead"].ToString();
                        vm.IsHR = dr["HR"].ToString();
                        vm.IsCOO = dr["COO"].ToString();
                        vm.IsMD = dr["Md"].ToString();
                        vm.IsP1 = dr["P1"].ToString();
                        vm.IsP2 = dr["P2"].ToString();
                        vm.IsP3 = dr["P3"].ToString();
                        vm.IsP4 = dr["P4"].ToString();
                        vm.IsP5 = dr["P5"].ToString();
                        VMs.Add(vm);
                    }
                }
                dr.Close();
                if (Vtransaction == null && transaction != null)
                {
                    transaction.Commit();
                }
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
                if (VcurrConn == null && currConn != null && currConn.State == ConnectionState.Open)
                {
                    currConn.Close();
                }
            }
            #endregion
            return VMs;
        }

        public List<AppraisalQuestionSetDetailVM> GetUesrForAppraisal(SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
        {
            #region Variables
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            string sqlText = "";
            List<AppraisalQuestionSetDetailVM> VMs = new List<AppraisalQuestionSetDetailVM>();
            AppraisalQuestionSetDetailVM vm;
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
                sqlText = @"                       
	                 Select Id, FullName,Logid,
                       CAST(0 AS BIT) AS IsOwn,
                       CAST(0 AS BIT) AS IsTeamLead,
                       CAST(0 AS BIT) AS IsHR,
                       CAST(0 AS BIT) AS IsCOO,
                       CAST(0 AS BIT) AS IsMD,
                       CAST(0 AS BIT) AS IsP1,
                       CAST(0 AS BIT) AS IsP2,
                       CAST(0 AS BIT) AS IsP3,
                       CAST(0 AS BIT) AS IsP4,
                       CAST(0 AS BIT) AS IsP5
                    from [user] order by Logid";

                SqlCommand objComm = new SqlCommand(sqlText, currConn, transaction);
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    vm = new AppraisalQuestionSetDetailVM();
                    vm.Id = dr["Id"].ToString();
                    vm.FullName = dr["FullName"].ToString();
                    vm.Logid = dr["Logid"].ToString();
                    vm.IsOwn = Convert.ToBoolean(dr["IsOwn"].ToString());
                    vm.IsTeamLead = Convert.ToBoolean(dr["IsTeamLead"].ToString());
                    vm.IsHR = Convert.ToBoolean(dr["IsHR"].ToString());
                    vm.IsCOO = Convert.ToBoolean(dr["IsCOO"].ToString());
                    vm.IsMD = Convert.ToBoolean(dr["IsMD"].ToString());
                    vm.IsP1 = Convert.ToBoolean(dr["IsP1"].ToString());
                    vm.IsP2 = Convert.ToBoolean(dr["IsP2"].ToString());
                    vm.IsP3 = Convert.ToBoolean(dr["IsP3"].ToString());
                    vm.IsP4 = Convert.ToBoolean(dr["IsP4"].ToString());
                    vm.IsP5 = Convert.ToBoolean(dr["IsP5"].ToString());
                    VMs.Add(vm);
                }
                dr.Close();
                if (Vtransaction == null && transaction != null)
                {
                    transaction.Commit();
                }
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
                if (VcurrConn == null && currConn != null && currConn.State == ConnectionState.Open)
                {
                    currConn.Close();
                }
            }
            #endregion
            return VMs;
        }

        public List<AppraisalQuestionSetDetailVM> GetUesrForAppraisalExist(SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
        {
            #region Variables
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            string sqlText = "";
            List<AppraisalQuestionSetDetailVM> VMs = new List<AppraisalQuestionSetDetailVM>();
            AppraisalQuestionSetDetailVM vm;
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
                sqlText = @"                       
	                 Select Id, FullName,Logid,
                       IsOwn,
                       IsTeamLead,
                       IsHR,
                       IsCOO,
                       IsMD,
                       IsP1,
                       IsP2,
                       IsP3,
                       IsP4,
                       IsP5
                    from UserRoleForAppraisal order by Logid";

                SqlCommand objComm = new SqlCommand(sqlText, currConn, transaction);
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    vm = new AppraisalQuestionSetDetailVM();
                    vm.Id = dr["Id"].ToString();
                    vm.FullName = dr["FullName"].ToString();
                    vm.Logid = dr["Logid"].ToString();
                    vm.IsOwn = Convert.ToBoolean(dr["IsOwn"].ToString());
                    vm.IsTeamLead = Convert.ToBoolean(dr["IsTeamLead"].ToString());
                    vm.IsHR = Convert.ToBoolean(dr["IsHR"].ToString());
                    vm.IsCOO = Convert.ToBoolean(dr["IsCOO"].ToString());
                    vm.IsMD = Convert.ToBoolean(dr["IsMD"].ToString());
                    vm.IsP1 = Convert.ToBoolean(dr["IsP1"].ToString());
                    vm.IsP2 = Convert.ToBoolean(dr["IsP2"].ToString());
                    vm.IsP3 = Convert.ToBoolean(dr["IsP3"].ToString());
                    vm.IsP4 = Convert.ToBoolean(dr["IsP4"].ToString());
                    vm.IsP5 = Convert.ToBoolean(dr["IsP5"].ToString());
                    VMs.Add(vm);
                }
                dr.Close();
                if (Vtransaction == null && transaction != null)
                {
                    transaction.Commit();
                }
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
                if (VcurrConn == null && currConn != null && currConn.State == ConnectionState.Open)
                {
                    currConn.Close();
                }
            }
            #endregion
            return VMs;
        }

        public string[] Insert(AppraisalQuestionSetVM vm, SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
        {
            #region Initializ
            string sqlText = "";
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message          
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "InsertInvestmentName"; //Method Name
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
                CommonDAL _cDal = new CommonDAL();

                if (vm != null)
                {
                    try
                    {
                        sqlText = "  ";
                        sqlText += @" 
                                Delete from UserRoleForAppraisal 
                               ";
                        SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                        var exeRes = cmdInsert.ExecuteNonQuery();
                        transResult = Convert.ToInt32(exeRes);
                    }

                    catch (Exception ex)
                    {

                        throw;
                    }

                    sqlText = "  ";
                    sqlText += @" 
                    INSERT INTO UserRoleForAppraisal(
                     Id
                    ,FullName    
                    ,LogId		
                    ,IsOwn
                    ,IsTeamLead
                    ,IsHR
                    ,IsCOO
                    ,IsMD
                    ,IsP1
                    ,IsP2
                    ,IsP3
                    ,IsP4
                    ,IsP5                   
                    ) VALUES (
                     @Id
                    ,@FullName    
                    ,@LogId		
                    ,@IsOwn
                    ,@IsTeamLead
                    ,@IsHR
                    ,@IsCOO
                    ,@IsMD
                    ,@IsP1
                    ,@IsP2
                    ,@IsP3
                    ,@IsP4
                    ,@IsP5                                    
                    )";
                    foreach (AppraisalQuestionSetDetailVM vms in vm.AppraisalQuestionSetDetaiVMs)
                    {
                        vm.Id = _cDal.NextId("UserRoleForAppraisal", currConn, transaction);

                        SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                        cmdInsert.Parameters.AddWithValue("@Id", vm.Id);
                        cmdInsert.Parameters.AddWithValue("@FullName", vms.FullName);
                        cmdInsert.Parameters.AddWithValue("@LogId", vms.Logid);
                        cmdInsert.Parameters.AddWithValue("@IsOwn", vms.IsOwn);
                        cmdInsert.Parameters.AddWithValue("@IsTeamLead", vms.IsTeamLead);
                        cmdInsert.Parameters.AddWithValue("@IsHR", vms.IsHR);
                        cmdInsert.Parameters.AddWithValue("@IsCOO", vms.IsCOO);
                        cmdInsert.Parameters.AddWithValue("@IsMD", vms.IsMD);
                        cmdInsert.Parameters.AddWithValue("@IsP1", vms.IsP1);
                        cmdInsert.Parameters.AddWithValue("@IsP2", vms.IsP2);
                        cmdInsert.Parameters.AddWithValue("@IsP3", vms.IsP3);
                        cmdInsert.Parameters.AddWithValue("@IsP4", vms.IsP4);
                        cmdInsert.Parameters.AddWithValue("@IsP5", vms.IsP5);

                        var exeRes = cmdInsert.ExecuteNonQuery();
                        transResult = Convert.ToInt32(exeRes);
                    }
                    if (transResult <= 0)
                    {
                        retResults[3] = sqlText;
                        throw new ArgumentNullException("Unexpected error to update InvestmentNames.", "");
                    }
                }
                else
                {
                    retResults[1] = "This InvestmentName already used!";
                    throw new ArgumentNullException("Please Input InvestmentName Value", "");
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
                retResults[2] = vm.Id.ToString();
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

        public string[] InsertAdminMarks(AppraisalMarksVM vms, SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
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
            retResults[5] = "QuestionSetVM"; //Method Name
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

                if (vms.AppraisalMarksDetailVMs != null)
                {
                    sqlText = "  ";
                    sqlText += @"Delete  from  AppraisalMarkSetups where EmployeeCode=@EmployeeCode and AssignFrom=@AssignFrom and EvaluationForId=@EvaluationForId";
                    SqlCommand objCommd = new SqlCommand(sqlText, currConn, transaction);
                    objCommd.Parameters.AddWithValue("@EmployeeCode", vms.EmployeeCode);
                    objCommd.Parameters.AddWithValue("@AssignFrom", vms.AssignToName);
                    objCommd.Parameters.AddWithValue("@EvaluationForId", vms.EvaluationForId);
                    var exed = objCommd.ExecuteNonQuery();

                    #region SqlText
                    sqlText = "  ";
                    sqlText += @" 
                    INSERT INTO AppraisalMarkSetups (
                     EmployeeCode
                    ,QuestionId
                    ,QuestionName
                    ,Own
                    ,TeamLead
                    ,AssignFrom
                    ,AssignFromCode
                    ,HR
                    ,COO
                    ,MD                     
                    ,P1
                    ,P2
                    ,P3
                    ,P4
                    ,P5
                    ,CategoryId
                    ,CategoryName
                    ,EvaluationForId
                    ,DepartmentId
                    ) VALUES (
                     @EmployeeCode
                    ,@QuestionId
                    ,@QuestionName
                    ,@IsOwn
                    ,@IsTeamLead
                    ,@AssignFrom
                    ,@AssignFromCode
                    ,@IsHR
                    ,@IsCOO
                    ,@IsMD                  
                    ,@IsP1
                    ,@IsP2
                    ,@IsP3
                    ,@IsP4
                    ,@IsP5
                    ,@CategoryId
                    ,@CategoryName
                    ,@EvaluationForId
                    ,@DepartmentId
                    ) 
                    ";
                    #endregion SqlText

                    #region SqlExecution
                    foreach (AppraisalMarksDetailVM vm in vms.AppraisalMarksDetailVMs)
                    {
                        bool shouldExecute =
                      (int.Parse(vm.IsOwn ?? "0") > 0) ||
                      (int.Parse(vm.IsTeamLead ?? "0") > 0) ||
                      (int.Parse(vm.IsHR ?? "0") > 0) ||
                      (int.Parse(vm.IsCOO ?? "0") > 0) ||
                      (int.Parse(vm.IsMD ?? "0") > 0) ||
                      (int.Parse(vm.IsP1 ?? "0") > 0) ||
                      (int.Parse(vm.IsP2 ?? "0") > 0) ||
                      (int.Parse(vm.IsP3 ?? "0") > 0) ||
                      (int.Parse(vm.IsP4 ?? "0") > 0) ||
                      (int.Parse(vm.IsP5 ?? "0") > 0);

                        if (shouldExecute)
                        {
                            SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                            cmdInsert.Parameters.AddWithValue("@EmployeeCode", vms.EmployeeCode);
                            cmdInsert.Parameters.AddWithValue("@QuestionId", vm.QuestionId);
                            cmdInsert.Parameters.AddWithValue("@QuestionName", vm.QuestionName);
                            cmdInsert.Parameters.AddWithValue("@IsOwn", vm.IsOwn ?? "0");
                            cmdInsert.Parameters.AddWithValue("@IsTeamLead", vm.IsTeamLead ?? "0");
                            cmdInsert.Parameters.AddWithValue("@AssignFrom", vms.AssignToName);
                            cmdInsert.Parameters.AddWithValue("@AssignFromCode", vms.AssignToId);
                            cmdInsert.Parameters.AddWithValue("@IsHR", vm.IsHR ?? "0");
                            cmdInsert.Parameters.AddWithValue("@IsCOO", vm.IsCOO ?? "0");
                            cmdInsert.Parameters.AddWithValue("@IsMD", vm.IsMD ?? "0");
                            cmdInsert.Parameters.AddWithValue("@IsP1", vm.IsP1 ?? "0");
                            cmdInsert.Parameters.AddWithValue("@IsP2", vm.IsP2 ?? "0");
                            cmdInsert.Parameters.AddWithValue("@IsP3", vm.IsP3 ?? "0");
                            cmdInsert.Parameters.AddWithValue("@IsP4", vm.IsP4 ?? "0");
                            cmdInsert.Parameters.AddWithValue("@IsP5", vm.IsP5 ?? "0");
                            cmdInsert.Parameters.AddWithValue("@CategoryId", vm.CategoryId);
                            cmdInsert.Parameters.AddWithValue("@CategoryName", vm.CategoryName);
                            cmdInsert.Parameters.AddWithValue("@EvaluationForId", vms.EvaluationForId);
                            cmdInsert.Parameters.AddWithValue("@DepartmentId", vms.DepartmentId);

                            var exeRes = cmdInsert.ExecuteNonQuery();
                            transResult = Convert.ToInt32(exeRes);
                        }
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

        public DataTable AppraisalEvaluationReport(string codeFrom, SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
        {
            #region Variables
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            string sqlText = "";
            DataTable dt = new DataTable();
            #endregion

            try
            {



                #region open connection and transaction
                currConn = VcurrConn ?? _dbsqlConnection.GetConnection();

                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }

                transaction = Vtransaction ?? currConn.BeginTransaction();
                #endregion

                sqlText = @"
                      SELECT 
                        [EmployeeCode],
                        ve.EmpName,
                        ve.Designation,
                        ve.Department,
                        [QuestionId],    
                        am.[CategoryId],
                        [CategoryName],
                        [QuestionName],
                        SUM([Own]) AS Own,
                        SUM([TeamLead]) AS TeamLead,
                        SUM([HR]) AS HR,
                        SUM([COO]) AS COO,
                        SUM([Md]) AS Md,
                        SUM([P1]) AS P1,
                        SUM([P2]) AS P2,
                        SUM([P3]) AS P3,
                        SUM([P4]) AS P4,
                        SUM([P5]) AS P5,
                        ae.EvaluationName
                        FROM AppraisalMarkSetups am
                        LEFT OUTER JOIN AppraisalEvaluation ae ON ae.id = am.EvaluationForId
                        Left Outer Join ViewEmployeeInformation ve on ve.Code=am.EmployeeCode
                        WHERE EmployeeCode in "+codeFrom+" GROUP BY  [EmployeeCode],ve.EmpName,ve.Designation,ve.Department,[QuestionId],   am.[CategoryId],[CategoryName],[QuestionName],ae.EvaluationName order by [CategoryName] ";

                using (SqlCommand objComm = new SqlCommand(sqlText, currConn, transaction))
                {
                   // objComm.Parameters.AddWithValue("@EmployeeCode", codeFrom);

                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(objComm))
                    {
                        dataAdapter.Fill(dt);
                    }
                }
            }
            catch (SqlException sqlex)
            {
                throw new Exception("SQL Error: " + sqlText + " " + sqlex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
                if (VcurrConn == null && currConn != null && currConn.State == ConnectionState.Open)
                {
                    currConn.Close();
                }
            }

            return dt;
        }


        public DataTable GetAppraisalWeightage(SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
        {
            #region Variables
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            string sqlText = "";
            DataTable dt = new DataTable();
            #endregion

            try
            {
                #region open connection and transaction
                currConn = VcurrConn ?? _dbsqlConnection.GetConnection();

                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }

                transaction = Vtransaction ?? currConn.BeginTransaction();
                #endregion

                sqlText = @"
                      Select * from AppraisalAssignTo
                    ";

                using (SqlCommand objComm = new SqlCommand(sqlText, currConn, transaction))
                {
                    using (SqlDataAdapter dataAdapter = new SqlDataAdapter(objComm))
                    {
                        dataAdapter.Fill(dt);
                    }
                }
            }
            catch (SqlException sqlex)
            {
                throw new Exception("SQL Error: " + sqlText + " " + sqlex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
                if (VcurrConn == null && currConn != null && currConn.State == ConnectionState.Open)
                {
                    currConn.Close();
                }
            }

            return dt;
        }

        public DataTable PrintSheet(AppraisalMarksVM vm, SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
        {
            #region Variables
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            string sqlText = "";
            DataTable dt = new DataTable();
            #endregion

            try
            {
                #region open connection and transaction
                currConn = VcurrConn ?? _dbsqlConnection.GetConnection();

                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }

                transaction = Vtransaction ?? currConn.BeginTransaction();
                #endregion

                sqlText = @"
                       Select ISNULL(aqs.CategoryId,0)CategoryId,aqs.CategoryName,aqs.QuestionId,aqs.QuestionName,aqs.Own,aqs.TeamLead,aqs.HR,aqs.COO,aqs.Md 
                    ,P1,P2,P3,P4,P5
                    from AppraisalMarkSetups aqs               
                    where 1=1  
                    ";
                if (vm.DepartmentId != "")
                {
                    sqlText += @"  and aqs.DepartmentId=@DepartmentId";
                }
                if (vm.EmployeeCode != "")
                {
                    sqlText += @"  and aqs.EmployeeCode=@EmployeeCode";
                }
                if (vm.EvaluationForId != "")
                {
                    sqlText += @"  and aqs.EvaluationForId=@EvaluationForId";
                }
              
                SqlCommand objComm = new SqlCommand(sqlText, currConn, transaction);

                if (vm.DepartmentId != "")
                {
                    objComm.Parameters.AddWithValue("@DepartmentId", vm.DepartmentId);
                }
                if (vm.EmployeeCode != "")
                {
                    objComm.Parameters.AddWithValue("@EmployeeCode", vm.EmployeeCode);
                }
                if (vm.EvaluationForId != "")
                {
                    objComm.Parameters.AddWithValue("@EvaluationForId", vm.EvaluationForId);
                }
              
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(objComm))
                {
                    dataAdapter.Fill(dt);
                }
            }
            catch (SqlException sqlex)
            {
                throw new Exception("SQL Error: " + sqlText + " " + sqlex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
                if (VcurrConn == null && currConn != null && currConn.State == ConnectionState.Open)
                {
                    currConn.Close();
                }
            }

            return dt;
        }

        public List<AppraisalHeightMarksVM> GetAppraisalHeightMarks(SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
        {
            #region Variables
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            string sqlText = "";
            List<AppraisalHeightMarksVM> VMs = new List<AppraisalHeightMarksVM>();
            AppraisalHeightMarksVM vm;
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
                sqlText = @"                       
	               		SELECT 
                        ve.Department,
                        [EmployeeCode],
                        ve.EmpName,
                        SUM([Own]) AS TotalOwn,
                        SUM([TeamLead]) AS TotalTeamLead,
                        SUM([HR]) AS TotalHR,
                        SUM([COO]) AS TotalCOO,
                        SUM([Md]) AS TotalMd,  
                        SUM([Own]) + SUM([TeamLead]) + SUM([HR]) + SUM([COO]) + SUM([Md]) AS TotalMarks,
                        -- Count only non-zero values for NOQ
                        SUM(CASE WHEN [Own] <> 0 THEN 1 ELSE 0 END) +
                        SUM(CASE WHEN [TeamLead] <> 0 THEN 1 ELSE 0 END) +
                        SUM(CASE WHEN [HR] <> 0 THEN 1 ELSE 0 END) +
                        SUM(CASE WHEN [COO] <> 0 THEN 1 ELSE 0 END) +
                        SUM(CASE WHEN [Md] <> 0 THEN 1 ELSE 0 END) AS NOQ
                    FROM 
                        [ShampanHRM_DB].[dbo].[AppraisalMarkSetups] am
                        LEFT OUTER JOIN ViewEmployeeInformation ve ON ve.Code = am.EmployeeCode
                    GROUP BY 
                        [EmployeeCode], ve.EmpName, ve.Department
                    ORDER BY 
                        ve.Department, 
                        (SUM([Own]) + SUM([TeamLead]) + SUM([HR]) + SUM([COO]) + SUM([Md])) DESC
                    ";

                SqlCommand objComm = new SqlCommand(sqlText, currConn, transaction);
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    vm = new AppraisalHeightMarksVM();

                    vm.Department = dr["Department"].ToString();
                    vm.EmployeeCode = dr["EmployeeCode"].ToString();
                    vm.EmpName = dr["EmpName"].ToString();
                    vm.TotalMarks = dr["TotalMarks"].ToString();
                    vm.NOQ = dr["NOQ"].ToString();

                    VMs.Add(vm);
                }
                dr.Close();
                if (Vtransaction == null && transaction != null)
                {
                    transaction.Commit();
                }
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
                if (VcurrConn == null && currConn != null && currConn.State == ConnectionState.Open)
                {
                    currConn.Close();
                }
            }
            #endregion
            return VMs;
        }
    }
}


















