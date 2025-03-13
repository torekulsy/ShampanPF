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
    public class AppraisalEvaluationDAL
    {
        #region Global Variables
        private const string FieldDelimeter = DBConstant.FieldDelimeter;
        private DBSQLConnection _dbsqlConnection = new DBSQLConnection();

        #endregion

        //A

        public List<AppraisalEvaluationFor> SelectAll()
        {
            #region Variables

            SqlConnection currConn = null;
            string sqlText = "";
            List<AppraisalEvaluationFor> appraisalCategoryVMs = new List<AppraisalEvaluationFor>();
            AppraisalEvaluationFor appraisalCategoryVM;
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

                sqlText = @"SELECT * FROM AppraisalEvaluation WHERE IsActive=1 ";
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    appraisalCategoryVM = new AppraisalEvaluationFor();
                    appraisalCategoryVM.Id = Convert.ToInt32(dr["Id"]);
                    appraisalCategoryVM.EvaluationName = dr["EvaluationName"].ToString();
               
                    appraisalCategoryVMs.Add(appraisalCategoryVM);

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

            return appraisalCategoryVMs;
        }

        public string[] Insert(AppraisalEvaluationFor vm, SqlConnection VcurrConn, SqlTransaction Vtransaction)
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
            retResults[5] = "Insert Question"; //Method Name

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

                #region Save

                if (1 == 1)
                {

                    sqlText = "  ";
                    sqlText += @" INSERT INTO AppraisalEvaluation(
                                        EvaluationName,IsActive,CreatedBy,CreatedDate,CreateFrom) 
                                VALUES (@EvaluationName,@IsActive,@CreatedBy,@CreatedDate,@CreateFrom) 
                                        SELECT SCOPE_IDENTITY()";

                    SqlCommand cmdInsert = new SqlCommand(sqlText, currConn);

                    cmdInsert.Parameters.AddWithValue("@EvaluationName", vm.EvaluationName);                 
                    cmdInsert.Parameters.AddWithValue("@IsActive", true);
                    cmdInsert.Parameters.AddWithValue("@CreatedBy", vm.CreatedBy);
                    cmdInsert.Parameters.AddWithValue("@CreatedDate", vm.CreatedDate);
                    cmdInsert.Parameters.AddWithValue("@CreateFrom", vm.CreatedFrom);                   
                    cmdInsert.Transaction = transaction;
                    var exeRes = cmdInsert.ExecuteScalar();
                    Id = Convert.ToInt32(exeRes);

                    if (Id <= 0)
                    {
                        retResults[1] = "Please Input Question Value";
                        retResults[3] = sqlText;
                        throw new ArgumentNullException("Please Input Question Value", "");
                    }
                }
                else
                {
                    retResults[1] = "This Question already used";
                    throw new ArgumentNullException("Please Input Branch Value", "");
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
                retResults[2] = Id.ToString();

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

        public AppraisalEvaluationFor SelectById(int id)
        {
            #region Variables

            SqlConnection currConn = null;
            string sqlText = "";
            AppraisalEvaluationFor appraisalCategoryVM = new AppraisalEvaluationFor();
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

                sqlText = @"SELECT * FROM AppraisalEvaluation WHERE Id=@Id";
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.Parameters.AddWithValue("@Id", id);
                objComm.CommandType = CommandType.Text;
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    appraisalCategoryVM.Id = Convert.ToInt32(dr["Id"]);
                    appraisalCategoryVM.EvaluationName = dr["EvaluationName"].ToString();                 
                    appraisalCategoryVM.IsActive = Convert.ToBoolean(dr["IsActive"].ToString());

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

            return appraisalCategoryVM;
        }

        public string[] Edit(AppraisalEvaluationFor vm, SqlConnection VcurrConn, SqlTransaction Vtransaction)
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

                sqlText = "  ";
                sqlText += @" Update AppraisalEvaluation set 
                                 EvaluationName=@EvaluationName,IsActive=@IsActive,UpdateBy=@UpdatedBy,UpdateDate=@UpdatedDate,CreateFrom=@CreateFrom
                                where Id=@Id";
                SqlCommand cmdEdit = new SqlCommand(sqlText, currConn);
                cmdEdit.Parameters.AddWithValue("@EvaluationName", vm.EvaluationName);  
                cmdEdit.Parameters.AddWithValue("@IsActive", vm.IsActive);
                cmdEdit.Parameters.AddWithValue("@UpdatedBy", vm.CreatedBy);
                cmdEdit.Parameters.AddWithValue("@UpdatedDate", vm.CreatedDate);
                cmdEdit.Parameters.AddWithValue("@CreateFrom", vm.CreatedFrom);
                cmdEdit.Parameters.AddWithValue("@Id", vm.Id);
                cmdEdit.Transaction = transaction;
                var exeRes = cmdEdit.ExecuteScalar();

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
                retResults[2] = Id.ToString();

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

        public string[] Delete(AppraisalEvaluationFor vm, string[] Ids, SqlConnection VcurrConn, SqlTransaction Vtransaction)
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

                if (Ids.Length >= 1)
                {
                    for (int i = 0; i < Ids.Length - 1; i++)
                    {
                        sqlText = "  ";
                        sqlText += @" Update AppraisalEvaluation set 
                                 IsActive=@IsActive,UpdateBy=@UpdatedBy,UpdateDate=@UpdatedDate,CreateFrom=@CreateFrom
                                where Id=@Id";
                        SqlCommand cmdEdit = new SqlCommand(sqlText, currConn);
                        cmdEdit.Parameters.AddWithValue("@IsActive", vm.IsActive);
                        cmdEdit.Parameters.AddWithValue("@UpdatedBy", vm.UpdatedBy);
                        cmdEdit.Parameters.AddWithValue("@UpdatedDate", vm.UpdatedDate);
                        cmdEdit.Parameters.AddWithValue("@CreateFrom", vm.CreatedFrom);
                        cmdEdit.Parameters.AddWithValue("@Id", Ids[i]);
                        cmdEdit.Transaction = transaction;
                        var exeRes = cmdEdit.ExecuteScalar();
                    }
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
