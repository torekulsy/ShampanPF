using SymOrdinary;
using SymViewModel.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SymServices.Common
{
    public class AppraisalAssingToEmployeeDAL
    {
        private const string FieldDelimeter = DBConstant.FieldDelimeter;
        private DBSQLConnection _dbsqlConnection = new DBSQLConnection();

        public List<AppraisalQuestionSetVM> SelectAll(int Id = 0)
        {
            #region Variables

            SqlConnection currConn = null;
            string sqlText = "";
            List<AppraisalQuestionSetVM> QuestionSets = new List<AppraisalQuestionSetVM>();
            List<AppraisalQuestionSetDetailVM> QuestionSetDetails = new List<AppraisalQuestionSetDetailVM>();
            AppraisalQuestionSetVM QuestionSet;
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
                             Select ae.Id, QuestionSetName, d.Name as DepartmentName,ve.EmpName, ae.CreateDate,ev.EvaluationName, ae.* from AppraisalAssignToEmployee ae  
                            Left Outer Join Department d on d.Id=ae.DepartmentId 
                            Left Outer Join ViewEmployeeInformation ve on ve.Code=ae.EmployeeCode
							Left Outer Join AppraisalEvaluation ev on ev.id=ae.EvaluationFor
                            WHERE 1=1";
                if (Id > 0)
                {
                    sqlText += @" and ae.Id=@Id";
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
                    QuestionSet.EmployeeName = dr["EmpName"].ToString();
                    QuestionSet.EmployeeCode = dr["EmployeeCode"].ToString();
                    QuestionSet.AssignToId = dr["AssignToId"].ToString();
                    QuestionSet.EvaluationFor = dr["EvaluationName"].ToString();
                    QuestionSet.EvaluationForId = dr["EvaluationFor"].ToString();
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
                    DataTable dte = new DataTable();
                    if (vm.EmployeeCode == null)
                    {
                        string sqlTexte = @"SELECT Code, EmpName FROM ViewEmployeeInformation where DepartmentId=@DepartmentId and IsActive=1";
                        SqlCommand objComme = new SqlCommand(sqlTexte, currConn, transaction);
                        objComme.Parameters.AddWithValue("@DepartmentId", vm.DepartmentId);
                        SqlDataReader dre = objComme.ExecuteReader();
                        dte = new DataTable();
                        dte.Load(dre);

                        foreach (DataRow item in dte.Rows)
                        {
                            vm.EmployeeCode = item["Code"].ToString();
                            vm.QuestionSetName = "Appraisal Question For " + item["Code"].ToString() + "~" + item["EmpName"].ToString();
                            sqlText = "  ";
                            sqlText += @" Select * from AppraisalAssignToEmployee where EmployeeCode=@EmployeeCode ";
                            SqlCommand objComm = new SqlCommand(sqlText, currConn, transaction);
                            objComm.Parameters.AddWithValue("@EmployeeCode", vm.EmployeeCode);
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
                                Update AppraisalAssignToEmployee set
                                QuestionSetName=@QuestionSetName
                                ,DepartmentId  =@DepartmentId 
                                ,EmployeeCode  =@EmployeeCode 
                                ,CreateDate = @CreateDate
                                ,CreateBy = @CreateBy
                                ,UpdateDate = @UpdateDate
                                ,UpdateBy = @UpdateBy
                                ,Year = @Year
                                ,ExDate = @ExDate
                                ,EvaluationFor=@EvaluationFor
                                ,CreateFrom = @CreateFrom where EmployeeCode=@EmployeeCode
                               ";
                                    sqlText += @" 
                                Delete from AppraisalAssignToEmployeeDetails where QuestionSetId=@Id
                               ";
                                    SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                                    cmdInsert.Parameters.AddWithValue("@DepartmentId", vm.DepartmentId);
                                    cmdInsert.Parameters.AddWithValue("@EmployeeCode", vm.EmployeeCode);
                                    cmdInsert.Parameters.AddWithValue("@QuestionSetName", vm.QuestionSetName);
                                    cmdInsert.Parameters.AddWithValue("@CreateDate", vm.CreateDate);
                                    cmdInsert.Parameters.AddWithValue("@CreateBy", vm.CreateBy);
                                    cmdInsert.Parameters.AddWithValue("@UpdateDate", vm.UpdateDate);
                                    cmdInsert.Parameters.AddWithValue("@UpdateBy", vm.UpdateBy);
                                    cmdInsert.Parameters.AddWithValue("@Year", vm.Year);
                                    cmdInsert.Parameters.AddWithValue("@ExDate", vm.ExDate);
                                    cmdInsert.Parameters.AddWithValue("@EvaluationFor", vm.EvaluationForId);
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
                                vm.Id = _cDal.NextId("AppraisalAssignToEmployee", currConn, transaction);

                                sqlText = "  ";
                                sqlText += @" 
                        INSERT INTO AppraisalAssignToEmployee(
                        QuestionSetName
                        ,DepartmentId    
                        ,EmployeeCode    
                        ,CreateDate
                        ,CreateBy
                        ,UpdateDate
                        ,UpdateBy
                        ,CreateFrom
                        ,Year
                        ,ExDate
                        ,EvaluationFor
                        ) VALUES (
                         @QuestionSetName
                        ,@DepartmentId    
                        ,@EmployeeCode 
                        ,@CreateDate
                        ,@CreateBy
                        ,@UpdateDate
                        ,@UpdateBy
                        ,@CreateFrom
                        ,@Year
                        ,@ExDate
                        ,@EvaluationFor
                        )";
                                SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                                cmdInsert.Parameters.AddWithValue("@QuestionSetName", vm.QuestionSetName);
                                cmdInsert.Parameters.AddWithValue("@DepartmentId", vm.DepartmentId);
                                cmdInsert.Parameters.AddWithValue("@EmployeeCode", vm.EmployeeCode);
                                cmdInsert.Parameters.AddWithValue("@CreateDate", vm.CreateDate);
                                cmdInsert.Parameters.AddWithValue("@CreateBy", vm.CreateBy);
                                cmdInsert.Parameters.AddWithValue("@UpdateDate", vm.UpdateDate);
                                cmdInsert.Parameters.AddWithValue("@UpdateBy", vm.UpdateBy);
                                cmdInsert.Parameters.AddWithValue("@CreateFrom", vm.CreateFrom);
                                cmdInsert.Parameters.AddWithValue("@Year", vm.Year);
                                cmdInsert.Parameters.AddWithValue("@ExDate", vm.ExDate);
                                cmdInsert.Parameters.AddWithValue("@EvaluationFor", vm.EvaluationForId);

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
                    }
                    else
                    {
                        sqlText = "  ";
                        sqlText += @" Select * from AppraisalAssignToEmployee where EmployeeCode=@EmployeeCode ";
                        SqlCommand objComm = new SqlCommand(sqlText, currConn, transaction);
                        objComm.Parameters.AddWithValue("@EmployeeCode", vm.EmployeeCode);
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
                                Update AppraisalAssignToEmployee set
                                QuestionSetName=@QuestionSetName
                                ,DepartmentId  =@DepartmentId 
                                ,EmployeeCode  =@EmployeeCode 
                                ,CreateDate = @CreateDate
                                ,CreateBy = @CreateBy
                                ,UpdateDate = @UpdateDate
                                ,UpdateBy = @UpdateBy
                                ,Year = @Year
                                ,ExDate = @ExDate
                                ,EvaluationFor=@EvaluationFor
                                ,CreateFrom = @CreateFrom where EmployeeCode=@EmployeeCode
                               ";
                                sqlText += @" 
                                Delete from AppraisalAssignToEmployeeDetails where QuestionSetId=@Id
                               ";
                                SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                                cmdInsert.Parameters.AddWithValue("@DepartmentId", vm.DepartmentId);
                                cmdInsert.Parameters.AddWithValue("@EmployeeCode", vm.EmployeeCode);
                                cmdInsert.Parameters.AddWithValue("@QuestionSetName", vm.QuestionSetName);
                                cmdInsert.Parameters.AddWithValue("@CreateDate", vm.CreateDate);
                                cmdInsert.Parameters.AddWithValue("@CreateBy", vm.CreateBy);
                                cmdInsert.Parameters.AddWithValue("@UpdateDate", vm.UpdateDate);
                                cmdInsert.Parameters.AddWithValue("@UpdateBy", vm.UpdateBy);
                                cmdInsert.Parameters.AddWithValue("@Year", vm.Year);
                                cmdInsert.Parameters.AddWithValue("@ExDate", vm.ExDate);
                                cmdInsert.Parameters.AddWithValue("@EvaluationFor", vm.EvaluationForId);
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
                            vm.Id = _cDal.NextId("AppraisalAssignToEmployee", currConn, transaction);

                            sqlText = "  ";
                            sqlText += @" 
                        INSERT INTO AppraisalAssignToEmployee(
                        QuestionSetName
                        ,DepartmentId    
                        ,EmployeeCode    
                        ,CreateDate
                        ,CreateBy
                        ,UpdateDate
                        ,UpdateBy
                        ,CreateFrom
                        ,Year
                        ,ExDate
                        ,EvaluationFor
                        ) VALUES (
                         @QuestionSetName
                        ,@DepartmentId    
                        ,@EmployeeCode 
                        ,@CreateDate
                        ,@CreateBy
                        ,@UpdateDate
                        ,@UpdateBy
                        ,@CreateFrom
                        ,@Year
                        ,@ExDate
                        ,@EvaluationFor
                        )";
                            SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                            cmdInsert.Parameters.AddWithValue("@QuestionSetName", vm.QuestionSetName);
                            cmdInsert.Parameters.AddWithValue("@DepartmentId", vm.DepartmentId);
                            cmdInsert.Parameters.AddWithValue("@EmployeeCode", vm.EmployeeCode);
                            cmdInsert.Parameters.AddWithValue("@CreateDate", vm.CreateDate);
                            cmdInsert.Parameters.AddWithValue("@CreateBy", vm.CreateBy);
                            cmdInsert.Parameters.AddWithValue("@UpdateDate", vm.UpdateDate);
                            cmdInsert.Parameters.AddWithValue("@UpdateBy", vm.UpdateBy);
                            cmdInsert.Parameters.AddWithValue("@CreateFrom", vm.CreateFrom);
                            cmdInsert.Parameters.AddWithValue("@Year", vm.Year);
                            cmdInsert.Parameters.AddWithValue("@ExDate", vm.ExDate);
                            cmdInsert.Parameters.AddWithValue("@EvaluationFor", vm.EvaluationForId);

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
                    INSERT INTO AppraisalAssignToEmployeeDetails (
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

        public List<AppraisalQuestionSetDetailVM> SelectAllQuestionByDepartmentExist(string dId, SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
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
                    ,IsP1,IsP2,IsP3,IsP4,IsP5
                    from AppraisalQuestionsSet aqs
                    Left Outer Join AppraisalQuestionsSetDetails aqsd on aqsd.QuestionsetId =aqs.id
                    Left Outer Join AppraisalCategory ac on ac.id=aqsd.CategoryId
                    Left Outer Join AppraisalQuestionBank qb on qb.id=aqsd.QuestionId
                    where 1=1
                        ";

                if (dId != "")
                {
                    sqlText += @"  and aqs.DepartmentId=@dId";
                }

                SqlCommand objComm = new SqlCommand(sqlText, currConn, transaction);

                if (dId != "")
                {
                    objComm.Parameters.AddWithValue("@dId", dId);
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

        public List<AppraisalQuestionSetDetailVM> SelectAllQuestionByEmployeeExist(string dId, string EmployeeCode, string EvaluationFor, SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
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
                    ,IsP1,IsP2,IsP3,IsP4,IsP5
                    from AppraisalAssignToEmployee aqs
                    Left Outer Join AppraisalAssignToEmployeeDetails aqsd on aqsd.QuestionsetId =aqs.id
                    Left Outer Join AppraisalCategory ac on ac.id=aqsd.CategoryId
                    Left Outer Join AppraisalQuestionBank qb on qb.id=aqsd.QuestionId
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
                if (EvaluationFor != "")
                {
                    sqlText += @"  and aqs.EvaluationFor=@EvaluationFor";
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
                if (EvaluationFor != "")
                {
                    objComm.Parameters.AddWithValue("@EvaluationFor", EvaluationFor);
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

        public string[] Delete(AppraisalCategoryVM vm, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {
            #region Initializ
            string sqlText = "";
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message         
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "Edit Question"; //Method Name

            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            #endregion

            #region Try

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

                #region Update

                if (vm.Id >= 1)
                {
                    sqlText = "  ";
                    sqlText += @" Delete from  AppraisalAssignToEmployee                                
                                where Id=@Id";
                    sqlText += @" Delete from  AppraisalAssignToEmployeeDetails                                
                                where QuestionSetId=@Id";
                    SqlCommand cmdEdit = new SqlCommand(sqlText, currConn);
                    cmdEdit.Parameters.AddWithValue("@Id", vm.Id);
                    cmdEdit.Transaction = transaction;
                    var exeRes = cmdEdit.ExecuteScalar();
                }

                #endregion Update
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
                retResults[1] = "Data Updated Successfully.";

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
