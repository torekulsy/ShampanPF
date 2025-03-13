using SymOrdinary;
using SymViewModel.HRM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymServices.HRM
{
    public class AppraisalEvaluationDAL
    {
        private const string FieldDelimeter = DBConstant.FieldDelimeter;
        private DBSQLConnection _dbsqlConnection = new DBSQLConnection();

        public List<AppraisalEvaluationVM> SelectQuestionSetByDepartment(string Id)
        {
            #region Variables

            SqlConnection currConn = null;
            string sqlText = "";
            List<AppraisalEvaluationVM> QuestionSets = new List<AppraisalEvaluationVM>();
            AppraisalEvaluationVM QuestionSet;
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
                              Select qs.Id, QuestionSetName, d.Name as DepartmentName, qs.CreateDate, qs.* from AppraisalAssignToEmployee qs  
                            Left Outer Join Department d on d.Id=qs.DepartmentId 
                            WHERE 1=1 ";
                if (Id != "")
                {
                    sqlText += @" and qs.EmployeeCode=@EmployeeCode";
                }
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
                if (Id != "")
                {
                    objComm.Parameters.AddWithValue("@EmployeeCode", Id);
                }
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    QuestionSet = new AppraisalEvaluationVM();
                    QuestionSet.Id = Convert.ToInt32(dr["Id"]);
                    QuestionSet.QuestionSetName = dr["QuestionSetName"].ToString();
                    QuestionSet.DepartmentName = dr["DepartmentName"].ToString();
                    QuestionSet.DepartmentId = dr["DepartmentId"].ToString();
                    QuestionSet.AssignToId = dr["AssignToId"].ToString();
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

        public List<AppraisalEvaluationDetailVM> SelectAllQuestionByDepartmentExist(string Id, SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
        {
            #region Variables
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            string sqlText = "";
            List<AppraisalEvaluationDetailVM> VMs = new List<AppraisalEvaluationDetailVM>();
            AppraisalEvaluationDetailVM vm;
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

                if (Id != "")
                {
                    sqlText += @"  and aqs.EmployeeCode=@EmployeeCode";
                }


                SqlCommand objComm = new SqlCommand(sqlText, currConn, transaction);

                if (Id != "")
                {
                    objComm.Parameters.AddWithValue("@EmployeeCode", Id);
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
                        vm = new AppraisalEvaluationDetailVM();
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

                if (VMs.Count == 0)
                {
                    VMs = new List<AppraisalEvaluationDetailVM>();
                }

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

        public UserRoleForAppraisalVM GetUesrForAppraisalExist(string Code, SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
        {
            #region Variables
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            string sqlText = "";
            UserRoleForAppraisalVM vm = new UserRoleForAppraisalVM();
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
                       IsTeamLead                      
                    from UserRoleForAppraisal where Logid=@LogId order by Logid";

                SqlCommand objComm = new SqlCommand(sqlText, currConn, transaction);
                objComm.Parameters.AddWithValue("@LogId", Code);
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    vm = new UserRoleForAppraisalVM();
                    vm.Id = dr["Id"].ToString();
                    vm.FullName = dr["FullName"].ToString();
                    vm.Logid = dr["Logid"].ToString();
                    vm.IsOwn = Convert.ToBoolean(dr["IsOwn"].ToString());
                    vm.IsTeamLead = Convert.ToBoolean(dr["IsTeamLead"].ToString());
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
            return vm;
        }


        public string[] Insert(EmployeeInfoVM vms, SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
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

                if (vms.AppraisalEvaluationDetailVM != null)
                {
                    if (string.IsNullOrEmpty(vms.employeeJob.EmployeeId))
                    {
                        vms.employeeJob.EmployeeId = vms.Code;
                    }
                    if (vms.employeeJob.EmployeeId == vms.Code)
                    {
                        vms.AppraisalEvaluationVM.AssignToName = "Own";
                    }
                    else
                    {
                        vms.AppraisalEvaluationVM.AssignToName = "TeamLead";
                    }
                    sqlText = "  ";
                    sqlText += @"Delete  from  AppraisalMarkSetups where EmployeeCode=@EmployeeCode and AssignFromCode=@AssignFromCode and EvaluationForId=@EvaluationForId";
                    SqlCommand objCommd = new SqlCommand(sqlText, currConn, transaction);
                    objCommd.Parameters.AddWithValue("@EmployeeCode", vms.employeeJob.EmployeeId ?? vms.Code);
                    objCommd.Parameters.AddWithValue("@AssignFromCode", vms.Code);
                    objCommd.Parameters.AddWithValue("@EvaluationForId", vms.AppraisalEvaluationVM.EvaluationForId);
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
                    ) 
                    ";
                    #endregion SqlText

                    #region SqlExecution
                    foreach (AppraisalEvaluationDetailVM vm in vms.AppraisalEvaluationDetailVM)
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
                            cmdInsert.Parameters.AddWithValue("@EmployeeCode", vms.employeeJob.EmployeeId);
                            cmdInsert.Parameters.AddWithValue("@QuestionId", vm.QuestionId);
                            cmdInsert.Parameters.AddWithValue("@QuestionName", vm.QuestionName);
                            cmdInsert.Parameters.AddWithValue("@IsOwn", vm.IsOwn ?? "0");
                            cmdInsert.Parameters.AddWithValue("@IsTeamLead", vm.IsTeamLead ?? "0");
                            cmdInsert.Parameters.AddWithValue("@AssignFrom", vms.AppraisalEvaluationVM.AssignToName);
                            cmdInsert.Parameters.AddWithValue("@AssignFromCode", vms.Code);
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
                            cmdInsert.Parameters.AddWithValue("@EvaluationForId", vms.AppraisalEvaluationVM.EvaluationForId);
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

     
        public List<AppraisalEvaluationDetailVM> SelectMarksByEmployeeExist(string EmployeeCode, string EvaluationFor, SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
        {
            #region Variables
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            string sqlText = "";
            List<AppraisalEvaluationDetailVM> VMs = new List<AppraisalEvaluationDetailVM>();
            AppraisalEvaluationDetailVM vm;
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
                    where 1=1  and AssignFrom ='Own'
                            ";

               
                if (EmployeeCode != "")
                {
                    sqlText += @"  and aqs.EmployeeCode=@EmployeeCode";
                }
                if (EvaluationFor != "")
                {
                    sqlText += @"  and aqs.EvaluationForId=@EvaluationForId";
                }
               

                SqlCommand objComm = new SqlCommand(sqlText, currConn, transaction);

              
                if (EmployeeCode != "")
                {
                    objComm.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                }
                if (EvaluationFor != "")
                {
                    objComm.Parameters.AddWithValue("@EvaluationForId", EvaluationFor);
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
                        vm = new AppraisalEvaluationDetailVM();
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
    }
}
