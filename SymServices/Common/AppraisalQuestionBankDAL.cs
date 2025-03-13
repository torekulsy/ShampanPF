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
    public class AppraisalQuestionBankDAL
    {
        #region Global Variables
        private const string FieldDelimeter = DBConstant.FieldDelimeter;
        private DBSQLConnection _dbsqlConnection = new DBSQLConnection();

        #endregion

        //==================SelectAll=================
        public List<AppraisalQuestionBankVM> SelectAll()
        {
            #region Variables

            SqlConnection currConn = null;
            string sqlText = "";
            List<AppraisalQuestionBankVM> appraisalQuestionsVMs = new List<AppraisalQuestionBankVM>();
            AppraisalQuestionBankVM appraisalQuestionsVM;
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
  aq.Id
 ,d.Name  AS DepartmentName
 ,ac.CategoryName  AS CategoryName
 ,aq.Question
 ,aq.Mark
 ,aq.IsActive
   FROM AppraisalQuestionBank AS aq
   JOIN Department AS d ON aq.DepartmentId=d.Id
   JOIN AppraisalCategory AS ac ON ac.id=aq.CategoryId
   WHERE aq.IsActive=1

";
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    appraisalQuestionsVM = new AppraisalQuestionBankVM();
                    appraisalQuestionsVM.Id = Convert.ToInt32(dr["Id"]);
                    appraisalQuestionsVM.DepartmentName = dr["DepartmentName"].ToString();
                    appraisalQuestionsVM.CategoryName = dr["CategoryName"].ToString();
                    appraisalQuestionsVM.Question = dr["Question"].ToString();
                    appraisalQuestionsVM.Mark = dr["Mark"].ToString();
                    appraisalQuestionsVM.IsActive =Convert.ToBoolean(dr["IsActive"].ToString());             
                    
                    appraisalQuestionsVMs.Add(appraisalQuestionsVM);

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

            return appraisalQuestionsVMs;
        }

        public AppraisalQuestionBankVM SelectById(int id)
        {
            #region Variables

            SqlConnection currConn = null;
            string sqlText = "";
            AppraisalQuestionBankVM appraisalQuestionsVM = new AppraisalQuestionBankVM();
           
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
  aq.Id
 ,d.Name  AS DepartmentName
 ,d.Id DepartmentId 
 ,ac.CategoryName  AS CategoryName
 ,ac.Id  AS CategoryId
 ,aq.Question
 ,aq.Mark
 ,aq.IsActive
 ,aq.Remark
   FROM AppraisalQuestionBank AS aq
   JOIN Department AS d ON aq.DepartmentId=d.Id
   JOIN AppraisalCategory AS ac ON ac.id=aq.CategoryId
   WHERE aq.IsActive=1 and aq.Id=@Id

";
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
                objComm.Parameters.AddWithValue("@Id", id);
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    appraisalQuestionsVM = new AppraisalQuestionBankVM();
                    appraisalQuestionsVM.Id = Convert.ToInt32(dr["Id"]);
                    appraisalQuestionsVM.DepartmentName = dr["DepartmentName"].ToString();
                    appraisalQuestionsVM.CategoryName = dr["CategoryName"].ToString();
                    appraisalQuestionsVM.DepartmentId = dr["DepartmentId"].ToString();
                    appraisalQuestionsVM.CategoryId = dr["CategoryId"].ToString();
                    appraisalQuestionsVM.Question = dr["Question"].ToString();
                    appraisalQuestionsVM.Mark = dr["Mark"].ToString();
                    appraisalQuestionsVM.Remark = dr["Remark"].ToString();
                    appraisalQuestionsVM.IsActive = Convert.ToBoolean(dr["IsActive"].ToString());
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

            return appraisalQuestionsVM;
        }
       
        public string[] Insert(AppraisalQuestionBankVM vm, SqlConnection VcurrConn, SqlTransaction Vtransaction)
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
                    sqlText += @" INSERT INTO AppraisalQuestionBank(
                                        DepartmentId,CategoryId,Question,Mark,IsActive,CreatedBy,CreatedDate,CreatedFrom,Remark) 
                                VALUES (@DepartmentId,@CategoryId,@Question,@Mark,@IsActive,@CreatedBy,@CreatedDate,@CreatedFrom,@Remark) 
                                        SELECT SCOPE_IDENTITY()";

                    SqlCommand cmdInsert = new SqlCommand(sqlText, currConn);

                    cmdInsert.Parameters.AddWithValue("@DepartmentId", vm.DepartmentId);
                    cmdInsert.Parameters.AddWithValue("@CategoryId", vm.CategoryId);
                    cmdInsert.Parameters.AddWithValue("@Question", vm.Question);
                    cmdInsert.Parameters.AddWithValue("@Mark", vm.Mark);
                    cmdInsert.Parameters.AddWithValue("@Remark", vm.Remark ?? "");
                    cmdInsert.Parameters.AddWithValue("@IsActive", true);                  
                    cmdInsert.Parameters.AddWithValue("@CreatedBy", vm.CreatedBy);
                    cmdInsert.Parameters.AddWithValue("@CreatedDate", vm.CreatedDate);
                    cmdInsert.Parameters.AddWithValue("@CreatedFrom", vm.CreatedFrom);

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

        public string[] Edit(AppraisalQuestionBankVM vm, SqlConnection VcurrConn, SqlTransaction Vtransaction)
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

                #region Save

                if (1 == 1)
                {

                    sqlText = "  ";
                    sqlText += @" Update AppraisalQuestionBank set 
                                 DepartmentId=@DepartmentId,CategoryId=@CategoryId,Question=@Question,Mark=@Mark,IsActive=@IsActive,UpdatedBy=@UpdatedBy,UpdatedDate=@UpdatedDate,CreatedFrom=@CreatedFrom,Remark=@Remark 
                                where Id=@Id";
                    SqlCommand cmdEdit = new SqlCommand(sqlText, currConn);
                    cmdEdit.Parameters.AddWithValue("@DepartmentId", vm.DepartmentId);
                    cmdEdit.Parameters.AddWithValue("@CategoryId", vm.CategoryId);
                    cmdEdit.Parameters.AddWithValue("@Question", vm.Question);
                    cmdEdit.Parameters.AddWithValue("@Mark", vm.Mark);
                    cmdEdit.Parameters.AddWithValue("@Remark", vm.Remark);
                    cmdEdit.Parameters.AddWithValue("@IsActive", true);
                    cmdEdit.Parameters.AddWithValue("@UpdatedBy", vm.CreatedBy);
                    cmdEdit.Parameters.AddWithValue("@UpdatedDate", vm.CreatedDate);
                    cmdEdit.Parameters.AddWithValue("@CreatedFrom", vm.CreatedFrom);
                    cmdEdit.Parameters.AddWithValue("@Id", vm.Id);
                    cmdEdit.Transaction = transaction;
                    var exeRes = cmdEdit.ExecuteScalar();                  
                }
                else
                {
                    retResults[1] = "This Question already used";
                    throw new ArgumentNullException("Please Input Branch Value", "");
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

        public string[] Delete(AppraisalQuestionBankVM vm, int Id, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {
            #region Initializ
            string sqlText = "";
            vm.Id = Id;
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = Id.ToString();// Return Id
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "Delete Question"; //Method Name

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
                    sqlText += @" Update AppraisalQuestionBank set IsActive=@IsActive
                                where Id=@Id";
                    SqlCommand cmdEdit = new SqlCommand(sqlText, currConn);
                    cmdEdit.Parameters.AddWithValue("@DepartmentId", vm.DepartmentId);
                    cmdEdit.Parameters.AddWithValue("@Question", vm.Question);
                    cmdEdit.Parameters.AddWithValue("@Mark", vm.Mark);
                    cmdEdit.Parameters.AddWithValue("@Remark", vm.Remark);
                    cmdEdit.Parameters.AddWithValue("@IsActive", false);
                    cmdEdit.Parameters.AddWithValue("@CreatedBy", vm.CreatedBy);
                    cmdEdit.Parameters.AddWithValue("@CreatedDate", vm.CreatedDate);
                    cmdEdit.Parameters.AddWithValue("@CreatedFrom", vm.CreatedFrom);
                    cmdEdit.Parameters.AddWithValue("@Id", vm.Id);
                    cmdEdit.Transaction = transaction;
                    var exeRes = cmdEdit.ExecuteScalar();
                }
                else
                {
                    retResults[1] = "This Question already used";
                    throw new ArgumentNullException("Please Input Branch Value", "");
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
                retResults[1] = "Data Deleted Successfully.";
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
    }
}
