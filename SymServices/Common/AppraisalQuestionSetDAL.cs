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
    public class AppraisalQuestionSetDAL
    {
        #region Global Variables
        private const string FieldDelimeter = DBConstant.FieldDelimeter;
        private DBSQLConnection _dbsqlConnection = new DBSQLConnection();

        #endregion

        public List<AppraisalQuestionSetVM> SelectAll(int Id = 0)
        {
            #region Variables

            SqlConnection currConn = null;
            string sqlText = "";
            List<AppraisalQuestionSetVM> QuestionSets = new List<AppraisalQuestionSetVM>();
            List<AppraisalQuestionSetDetailVM> QuestionSetDetails = new List<AppraisalQuestionSetDetailVM>();
            AppraisalQuestionSetVM QuestionSet;
            AppraisalQuestionSetDetailVM QuestionSetDetail;
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

                sqlText = @"
                            Select qs.Id, QuestionSetName, d.Name as DepartmentName, qs.CreateDate, qs.* from AppraisalQuestionsSet qs  
                            Left Outer Join Department d on d.Id=qs.DepartmentId 
                            WHERE 1=1 and IsActive=1";
                if (Id > 0)
                {
                    sqlText += @" and qs.Id=@Id";
                }
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
                if (Id > 0)
                {
                    objComm.Parameters.AddWithValue("@Id", Id);
                }
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    QuestionSet = new AppraisalQuestionSetVM();
                    QuestionSet.Id = Convert.ToInt32(dr["Id"]);
                    QuestionSet.QuestionSetName = dr["QuestionSetName"].ToString();
                    QuestionSet.DepartmentName = dr["DepartmentName"].ToString();
                    QuestionSet.DepartmentId = dr["DepartmentId"].ToString();
                    QuestionSet.AssignToId = dr["AssignToId"].ToString();
                    QuestionSet.Year = dr["Year"].ToString();
                    QuestionSet.ExDate = dr["ExDate"].ToString();
                    QuestionSet.CreateDate = Ordinary.StringToDate(dr["CreateDate"].ToString());
                    QuestionSets.Add(QuestionSet);

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

            return QuestionSets;
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
                    sqlText += @" Select * from AppraisalQuestionsSet where DepartmentId=@DepartmentId ";
                    SqlCommand objComm = new SqlCommand(sqlText, currConn, transaction);
                    objComm.Parameters.AddWithValue("@DepartmentId", vm.DepartmentId);
                    SqlDataReader dr;
                    dr = objComm.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(dr);
                    if (dt.Rows.Count > 0)
                    {
                        vm.Id = Convert.ToInt32(dt.Rows[0]["Id"].ToString());
                        try
                        {
                            sqlText = "  ";
                            sqlText += @" 
                                Update AppraisalQuestionsSet set
                                QuestionSetName=@QuestionSetName
                                ,DepartmentId  =@DepartmentId 
                                ,CreateDate = @CreateDate
                                ,CreateBy = @CreateBy
                                ,UpdateDate = @UpdateDate
                                ,UpdateBy = @UpdateBy
                                ,Year = @Year
                                ,ExDate = @ExDate
                                ,CreateFrom = @CreateFrom where DepartmentId=@DepartmentId
                               ";
                            sqlText += @" 
                                Delete from AppraisalQuestionsSetDetails where QuestionSetId=@Id
                               ";
                            SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                            cmdInsert.Parameters.AddWithValue("@DepartmentId", vm.DepartmentId);
                            cmdInsert.Parameters.AddWithValue("@QuestionSetName", vm.QuestionSetName);
                            cmdInsert.Parameters.AddWithValue("@CreateDate", vm.CreateDate);
                            cmdInsert.Parameters.AddWithValue("@CreateBy", vm.CreateBy);
                            cmdInsert.Parameters.AddWithValue("@UpdateDate", vm.UpdateDate);
                            cmdInsert.Parameters.AddWithValue("@UpdateBy", vm.UpdateBy);
                            cmdInsert.Parameters.AddWithValue("@Year", vm.Year);
                            cmdInsert.Parameters.AddWithValue("@ExDate", vm.ExDate);
                            cmdInsert.Parameters.AddWithValue("@CreateFrom", vm.CreateFrom);
                            cmdInsert.Parameters.AddWithValue("@Id", vm.Id);

                            var exeRes = cmdInsert.ExecuteNonQuery();
                            transResult = Convert.ToInt32(exeRes);

                            if (exeRes == 0)
                            {
                                throw new Exception("Update query did not affect any rows. Check if ID exists.");
                            }

                            if (vm.AppraisalQuestionSetDetaiVMs.Count > 0)
                            {
                                InsertDetails(vm, currConn, transaction);
                            }
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }

                    }
                    else
                    {
                        vm.Id = _cDal.NextId("AppraisalQuestionsSet", currConn, transaction);

                        sqlText = "  ";
                        sqlText += @" 
                    INSERT INTO AppraisalQuestionsSet(
                    QuestionSetName
                    ,DepartmentId    
                    ,CreateDate
                    ,CreateBy
                    ,UpdateDate
                    ,UpdateBy
                    ,CreateFrom
                    ,Year
                    ,ExDate
                    ) VALUES (
                     @QuestionSetName
                    ,@DepartmentId 
                    ,@CreateDate
                    ,@CreateBy
                    ,@UpdateDate
                    ,@UpdateBy
                    ,@CreateFrom
                    ,@Year
                    ,@ExDate
                    )";
                        SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                        cmdInsert.Parameters.AddWithValue("@QuestionSetName", vm.QuestionSetName);
                        cmdInsert.Parameters.AddWithValue("@DepartmentId", vm.DepartmentId);
                        cmdInsert.Parameters.AddWithValue("@CreateDate", vm.CreateDate);
                        cmdInsert.Parameters.AddWithValue("@CreateBy", vm.CreateBy);
                        cmdInsert.Parameters.AddWithValue("@UpdateDate", vm.UpdateDate);
                        cmdInsert.Parameters.AddWithValue("@UpdateBy", vm.UpdateBy);
                        cmdInsert.Parameters.AddWithValue("@CreateFrom", vm.CreateFrom);
                        cmdInsert.Parameters.AddWithValue("@Year", vm.Year);
                        cmdInsert.Parameters.AddWithValue("@ExDate", vm.ExDate);

                        var exeRes = cmdInsert.ExecuteNonQuery();
                        transResult = Convert.ToInt32(exeRes);
                        if (transResult <= 0)
                        {
                            retResults[3] = sqlText;
                            throw new ArgumentNullException("Unexpected error to update InvestmentNames.", "");
                        }

                        if (vm.AppraisalQuestionSetDetaiVMs.Count > 0)
                        {
                            InsertDetails(vm, currConn, transaction);
                        }
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

        public string[] InsertDetails(AppraisalQuestionSetVM QuestionSetVM, SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
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

                if (QuestionSetVM.AppraisalQuestionSetDetaiVMs != null)
                {
                    #region SqlText
                    sqlText = "  ";
                    sqlText += @" 
                    INSERT INTO AppraisalQuestionsSetDetails (
                     QuestionSetId
                    ,QuestionId
                    ,IsOwn
                    ,IsTeamLead
                    ,IsHR
                    ,IsCOO
                    ,IsMD  
                    ,CategoryId
                    ,IsP1
                    ,IsP2
                    ,IsP3
                    ,IsP4
                    ,IsP5
                   ,IsActive
                    ) VALUES (
                     @QuestionSetId
                    ,@QuestionId
                    ,@IsOwn
                    ,@IsTeamLead
                    ,@IsHR
                    ,@IsCOO
                    ,@IsMD
                    ,@CategoryId
                    ,@IsP1
                    ,@IsP2
                    ,@IsP3
                    ,@IsP4
                    ,@IsP5
                    ,@IsActive
                    ) 
                    ";
                    #endregion SqlText

                    #region SqlExecution
                    foreach (AppraisalQuestionSetDetailVM vm in QuestionSetVM.AppraisalQuestionSetDetaiVMs)
                    {
                        SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                        cmdInsert.Parameters.AddWithValue("@QuestionSetId", QuestionSetVM.Id);
                        cmdInsert.Parameters.AddWithValue("@QuestionId", vm.QuestionId);
                        cmdInsert.Parameters.AddWithValue("@IsOwn", vm.IsOwn);
                        cmdInsert.Parameters.AddWithValue("@IsTeamLead", vm.IsTeamLead);
                        cmdInsert.Parameters.AddWithValue("@IsHR", vm.IsHR);
                        cmdInsert.Parameters.AddWithValue("@IsCOO", vm.IsCOO);
                        cmdInsert.Parameters.AddWithValue("@IsMD", vm.IsMD);
                        cmdInsert.Parameters.AddWithValue("@CategoryId", vm.CategoryId);
                        cmdInsert.Parameters.AddWithValue("@IsP1", vm.IsP1);
                        cmdInsert.Parameters.AddWithValue("@IsP2", vm.IsP2);
                        cmdInsert.Parameters.AddWithValue("@IsP3", vm.IsP3);
                        cmdInsert.Parameters.AddWithValue("@IsP4", vm.IsP4);
                        cmdInsert.Parameters.AddWithValue("@IsP5", vm.IsP5);
                        cmdInsert.Parameters.AddWithValue("@IsActive", vm.IsActive);
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

        public List<AppraisalQuestionSetDetailVM> SelectAllDetails(int Id, SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
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
                       
	                   SELECT ac.Id as CategoryId
	                    ,ac.CategoryName
                        ,qsd.Id					
                        ,QuestionSetId
                        ,QuestionId
						,qb.Question
                        ,IsOwn
                        ,IsTeamLead
                        ,IsHR
                        ,IsCOO
                        ,IsMD  
                        ,qsd.IsActive
                         From AppraisalQuestionsSetDetails qsd						
						 Left Outer Join AppraisalCategory ac on ac.Id=qsd.CategoryId
						 Left Outer Join AppraisalQuestionBank qb on qb.Id=qsd.QuestionId
                        WHERE  1=1 
                        ";

                if (Id > 0)
                {
                    sqlText += @" and QuestionSetId=@Id";
                }


                SqlCommand objComm = new SqlCommand(sqlText, currConn, transaction);

                if (Id > 0)
                {
                    objComm.Parameters.AddWithValue("@Id", Id);
                }
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    vm = new AppraisalQuestionSetDetailVM();
                    vm.Id = dr["Id"].ToString();
                    vm.CategoryId = dr["CategoryId"].ToString();
                    vm.CategoryName = dr["CategoryName"].ToString();
                    vm.QuestionSetId = dr["QuestionSetId"].ToString();
                    vm.QuestionId = dr["QuestionId"].ToString();
                    vm.QuestionName = dr["Question"].ToString();
                    vm.IsOwn = Convert.ToBoolean(dr["IsOwn"].ToString());
                    vm.IsTeamLead = Convert.ToBoolean(dr["IsTeamLead"].ToString());
                    vm.IsHR = Convert.ToBoolean(dr["IsHR"].ToString());
                    vm.IsCOO = Convert.ToBoolean(dr["IsCOO"].ToString());
                    vm.IsMD = Convert.ToBoolean(dr["IsMD"].ToString());
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

        public List<AppraisalQuestionSetDetailVM> SelectAllQuestionByDepartment(string Id, SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
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
	                   SELECT 
                        qb.Id
                        ,ac.Id as CategoryId
                        ,ac.CategoryName                  
                        ,qb.Id as QuestionId
                        ,qb.Question				
                        ,0 IsOwn
                        ,0 IsTeamLead
                        ,0 IsHR 
                        ,0 IsCOO 
                        ,0 IsMD
                        ,0 IsP1
                        ,0 IsP2
                        ,0 IsP3
                        ,0 IsP4
                        ,0 IsP5
                        ,0 IsActive
                        From AppraisalQuestionBank qb						
                        Left Outer Join AppraisalCategory ac on ac.Id=qb.CategoryId					
                        WHERE  1=1
                        ";

                if (Id != "")
                {
                    sqlText +="  and DepartmentId=@dId";
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
                    vm = new AppraisalQuestionSetDetailVM();
                    vm.CategoryId = dr["CategoryId"].ToString();
                    vm.CategoryName = dr["CategoryName"].ToString();
                    vm.QuestionId = dr["QuestionId"].ToString();
                    vm.QuestionName = dr["Question"].ToString();
                    vm.Id = dr["Id"].ToString();
                    vm.IsOwn = Convert.ToBoolean(dr["IsOwn"]);
                    vm.IsTeamLead = Convert.ToBoolean(dr["IsTeamLead"]);
                    vm.IsHR = Convert.ToBoolean(dr["IsHR"]);
                    vm.IsCOO = Convert.ToBoolean(dr["IsCOO"]);
                    vm.IsMD = Convert.ToBoolean(dr["IsMD"]);
                    vm.IsP1 = Convert.ToBoolean(dr["IsP1"]);
                    vm.IsP2 = Convert.ToBoolean(dr["IsP2"]);
                    vm.IsP3 = Convert.ToBoolean(dr["IsP3"]);
                    vm.IsP4 = Convert.ToBoolean(dr["IsP4"]);
                    vm.IsP5 = Convert.ToBoolean(dr["IsP5"]);
                    vm.IsActive = Convert.ToBoolean(dr["IsActive"]);
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

        public List<AppraisalQuestionSetDetailVM> SelectAllQuestionByDepartmentExist(string Id, SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
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
	                  
                    Select ISNULL(aqsd.CategoryId,0)CategoryId,ac.CategoryName,aqsd.QuestionId,qb.Question,aqsd.IsOwn,aqsd.IsTeamLead,aqsd.IsHR,aqsd.IsCOO,aqsd.IsMd 
                    ,IsP1,IsP2,IsP3,IsP4,IsP5,aqsd.IsActive
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
                    if (dr["CategoryId"].ToString()== "0")
                    {
                       
                    }
                    else
                    {
                        vm = new AppraisalQuestionSetDetailVM();
                        vm.CategoryId = dr["CategoryId"].ToString();
                        vm.CategoryName = dr["CategoryName"].ToString();
                        vm.QuestionId = dr["QuestionId"].ToString();
                        vm.QuestionName = dr["Question"].ToString();
                        vm.IsOwn = Convert.ToBoolean(dr["IsOwn"].ToString());
                        vm.IsTeamLead = Convert.ToBoolean(dr["IsTeamLead"].ToString());
                        vm.IsHR = Convert.ToBoolean(dr["IsHR"].ToString());
                        vm.IsCOO = Convert.ToBoolean(dr["IsCOO"].ToString());
                        vm.IsMD = Convert.ToBoolean(dr["IsMd"].ToString());
                        vm.IsP1 = Convert.ToBoolean(dr["IsP1"].ToString());
                        vm.IsP2 = Convert.ToBoolean(dr["IsP2"].ToString());
                        vm.IsP3 = Convert.ToBoolean(dr["IsP3"].ToString());
                        vm.IsP4 = Convert.ToBoolean(dr["IsP4"].ToString());
                        vm.IsP5 = Convert.ToBoolean(dr["IsP5"].ToString());
                        vm.IsActive = Convert.ToBoolean(dr["IsActive"].ToString());
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

        public List<AppraisalCheckBoxVM> SelectApprisalCheckBox(SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
        {
            #region Variables
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            string sqlText = "";
            List<AppraisalCheckBoxVM> VMs = new List<AppraisalCheckBoxVM>();
            AppraisalCheckBoxVM vm;
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
	                   SELECT *
                        From AppraisalAssignTo 
                        WHERE  IsActive=1
                        ";

                SqlCommand objComm = new SqlCommand(sqlText, currConn, transaction);                
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    vm = new AppraisalCheckBoxVM();
                    vm.FieldName = dr["AssignToName"].ToString();
                    vm.IsChecked =Convert.ToBoolean(dr["IsActive"].ToString());                  
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
     
        public string[] DeleteAppraisalQuestionSet(int Id, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {

            #region Initializ
            string sqlText = "";
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = Id.ToString();// Return Id
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "AppraisalQuestionSet"; //Method Name
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
                    sqlText += "Delete from AppraisalQuestionsSet  where Id=@Id";                        
                    SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                    cmdInsert.Parameters.AddWithValue("@Id", Id);                          
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
                retResults[1] = "Deleted Successfully";
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
                        retResults[1] = "Unexpected error to update AppraisalAssignTo";
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

        public List<AppraisalQuestionSetDetailVM> SelectAll2()
        {
            #region Variables
            SqlConnection currConn = null;
            string sqlText = "";        
            List<AppraisalQuestionSetDetailVM> QuestionSetDetails = new List<AppraisalQuestionSetDetailVM>();          
            AppraisalQuestionSetDetailVM QuestionSetDetail;
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
                sqlText = @"
                           Select ISNULL(aqsd.CategoryId,0)CategoryId,ac.CategoryName,aqsd.QuestionId,qb.Question,aqsd.IsOwn,aqsd.IsTeamLead,aqsd.IsHR,aqsd.IsCOO,aqsd.IsMd 
                    ,IsP1,IsP2,IsP3,IsP4,IsP5
                    from AppraisalQuestionsSet aqs
                    Left Outer Join AppraisalQuestionsSetDetails aqsd on aqsd.QuestionsetId =aqs.id
                    Left Outer Join AppraisalCategory ac on ac.id=aqsd.CategoryId
                    Left Outer Join AppraisalQuestionBank qb on qb.id=aqsd.QuestionId
                    where 1=1";
              
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
               
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    QuestionSetDetail = new AppraisalQuestionSetDetailVM();
                    QuestionSetDetail.Id = dr["QuestionId"].ToString();
                    QuestionSetDetail.CategoryName = dr["CategoryName"].ToString();
                    QuestionSetDetail.QuestionName = dr["Question"].ToString();      
                    QuestionSetDetails.Add(QuestionSetDetail);
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
            return QuestionSetDetails;
        }

        public List<AppraisalQuestionSetDetailVM> SelectAllByDepartmentId(string DepartmentId)
        {
            #region Variables
            SqlConnection currConn = null;
            string sqlText = "";
            List<AppraisalQuestionSetDetailVM> QuestionSetDetails = new List<AppraisalQuestionSetDetailVM>();
            AppraisalQuestionSetDetailVM QuestionSetDetail;
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
                sqlText = @"
                           Select ISNULL(aqsd.CategoryId,0)CategoryId,ac.CategoryName,aqsd.QuestionId,qb.Question,aqsd.IsOwn,aqsd.IsTeamLead,aqsd.IsHR,aqsd.IsCOO,aqsd.IsMd 
                    ,IsP1,IsP2,IsP3,IsP4,IsP5
                    from AppraisalQuestionsSet aqs
                    Left Outer Join AppraisalQuestionsSetDetails aqsd on aqsd.QuestionsetId =aqs.id
                    Left Outer Join AppraisalCategory ac on ac.id=aqsd.CategoryId
                    Left Outer Join AppraisalQuestionBank qb on qb.id=aqsd.QuestionId
                    where 1=1 and aqs.DepartmentId=@DepartmentId";

                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
                objComm.Parameters.AddWithValue("@DepartmentId", DepartmentId);
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    QuestionSetDetail = new AppraisalQuestionSetDetailVM();
                    QuestionSetDetail.Id = dr["QuestionId"].ToString();
                    QuestionSetDetail.CategoryName = dr["CategoryName"].ToString();
                    QuestionSetDetail.QuestionName = dr["Question"].ToString();
                    QuestionSetDetails.Add(QuestionSetDetail);
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
            return QuestionSetDetails;
        }


    }
}
