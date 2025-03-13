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
    public class AppraisalScheduleDAL
    {
        #region Global Variables
        private const string FieldDelimeter = DBConstant.FieldDelimeter;
        private DBSQLConnection _dbsqlConnection = new DBSQLConnection();

        #endregion

        public List<AppraisalScheduleVM> SelectAll( int Id)
        {
            #region Variables

            SqlConnection currConn = null;
            string sqlText = "";
            List<AppraisalScheduleVM> Schedules = new List<AppraisalScheduleVM>();
            AppraisalScheduleVM Schedule;
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
                           Select * from AppraisalSchedule sc
                            Left Outer Join AppraisalQuestionsSet aqs on aqs.Id=sc.QusestionSetId";
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
                    Schedule = new AppraisalScheduleVM();
                    Schedule.Id = Convert.ToInt32(dr["Id"]);
                    Schedule.QuestionSetName = dr["QuestionSetName"].ToString();
                    Schedule.ScheduleName = dr["ScheduleName"].ToString();
                    Schedule.StartDate = dr["StartDate"].ToString();
                    Schedule.EndDate = dr["EndDate"].ToString();

                    Schedules.Add(Schedule);

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

            return Schedules;
        }

        public string[] Insert(AppraisalScheduleVM vm, SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
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
                vm.Id = _cDal.NextId("AppraisalQuestionsSet", currConn, transaction);
                if (vm != null)
                {
                    sqlText = "  ";
                    sqlText += @" 
                    INSERT INTO AppraisalQuestionsSet(
                    QuestionSetName
                    ,DepartmentId
                    ,CategoryId
                    ,AssignToId
                    ,CreateDate
                    ,CreateBy
                    ,UpdateDate
                    ,UpdateBy
                    ,CreateFrom
                    ) VALUES (
                     @QuestionSetName
                    ,@DepartmentId
                    ,@CategoryId
                    ,@AssignToId
                    ,@CreateDate
                    ,@CreateBy
                    ,@UpdateDate
                    ,@UpdateBy
                    ,@CreateFrom
                    )";
                    SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                    cmdInsert.Parameters.AddWithValue("@QuestionSetName", vm.QuestionSetName);                    
                    cmdInsert.Parameters.AddWithValue("@CreateDate", vm.CreateDate);
                    cmdInsert.Parameters.AddWithValue("@CreateBy", vm.CreateBy);
                    cmdInsert.Parameters.AddWithValue("@UpdateDate", vm.UpdateDate);
                    cmdInsert.Parameters.AddWithValue("@UpdateBy", vm.UpdateBy);
                    cmdInsert.Parameters.AddWithValue("@CreateFrom", vm.CreateFrom);

                    var exeRes = cmdInsert.ExecuteNonQuery();
                    transResult = Convert.ToInt32(exeRes);
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
    }
}
