using SymOrdinary;
using SymServices.Common;
using SymViewModel.Attendance;
using SymViewModel.Common;
using SymViewModel.Common;
using SymViewModel.Leave;
using SymViewModel.Payroll;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SymServices.Common
{
    public class RecruitmentRequisitionDAL
    {
        #region Global Variables
        private DBSQLConnection _dbsqlConnection = new DBSQLConnection();
        CommonDAL _cDal = new CommonDAL();
        #endregion
        public string[] InsertRecruitmentRequisition(RecruitmentRequisitionVM vm, SqlConnection VcurrConn, SqlTransaction Vtransaction)
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
            retResults[5] = "InsertRecruitmentRequisition"; //Method Name
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            #endregion
            #region Try
            try
            {
                #region Validation
                //if (string.IsNullOrEmpty(RecruitmentRequisitionVM.Id))
                //{
                //    retResults[1] = "Please Input RecruitmentRequisition";
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
                    if (vm.Id > 0)
                    {
                        sqlText = "  ";
                        sqlText += @"Update RecruitmentRequisition  set
                           Department=@Department
                          ,Designation =@Designation
                          ,Experience=@Experience
                          ,Deadline=@Deadline
                          ,Description=@Description                                               
                          ,IsActive =@IsActive
                          ,IsApproved=@IsApproved
                            where Id=@Id   
                         ";
                        SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                        cmdInsert.Parameters.AddWithValue("@Id", vm.Id);
                        cmdInsert.Parameters.AddWithValue("@Department", vm.Department);
                        cmdInsert.Parameters.AddWithValue("@Designation", vm.Designation);
                        cmdInsert.Parameters.AddWithValue("@Experience", vm.Experience);
                        cmdInsert.Parameters.AddWithValue("@Deadline", vm.Deadline);
                        cmdInsert.Parameters.AddWithValue("@Description", vm.Description);
                        cmdInsert.Parameters.AddWithValue("@IsActive", vm.IsActive);
                        cmdInsert.Parameters.AddWithValue("@IsApproved", vm.IsApproved);
                        cmdInsert.ExecuteNonQuery();
                    }
                    else
                    {

                        sqlText = "  ";
                        sqlText += @" INSERT INTO RecruitmentRequisition(
                           [Department]
                          ,[Designation]
                          ,[Experience]
                          ,[Deadline]
                          ,[Description]
                          ,[IsActive]
                          ,[IsApproved]                                            
                        ) 
                           VALUES (
                           @Department
                          ,@Designation
                          ,@Experience
                          ,@Deadline
                          ,@Description
                          ,@IsActive
                          ,@IsApproved                                               
                        ) 
                         ";
                        SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                        cmdInsert.Parameters.AddWithValue("@Department", vm.Department);
                        cmdInsert.Parameters.AddWithValue("@Designation", vm.Designation);
                        cmdInsert.Parameters.AddWithValue("@Experience", vm.Experience);
                        cmdInsert.Parameters.AddWithValue("@Deadline", vm.Deadline);
                        cmdInsert.Parameters.AddWithValue("@Description", vm.Description);
                        cmdInsert.Parameters.AddWithValue("@IsActive", vm.IsActive);
                        cmdInsert.Parameters.AddWithValue("@IsApproved", vm.IsApproved);

                        cmdInsert.ExecuteNonQuery();
                    }
                }
                else
                {
                    retResults[1] = "This RecruitmentRequisition already used";
                    throw new ArgumentNullException("Please Input RecruitmentRequisition Value", "");
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
                        retResults[1] = "Unexpected error to update InsertRecruitmentRequisition.";
                        return retResults;
                        // throw new ArgumentNullException("Unexpected error to update RecruitmentRequisition.", "RecruitmentRequisition");
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

        public List<RecruitmentRequisitionVM> SelectAllRecruitmentRequisition()
        {
            #region Variables

            SqlConnection currConn = null;
            string sqlText = "";
            List<RecruitmentRequisitionVM> recruitmentRequisitionVMs = new List<RecruitmentRequisitionVM>();
            RecruitmentRequisitionVM RecruitmentRequisitionVM;
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
                sqlText = @"SELECT rr.*, d.Name DepartmentName, dg.Name DesignationName FROM RecruitmentRequisition rr
                Left Outer Join Department d on d.Id=rr.Department
                Left Outer Join Designation dg on dg.Id=rr.Designation where rr.IsActive=1 and rr.IsArchive=0";
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    RecruitmentRequisitionVM = new RecruitmentRequisitionVM();
                    RecruitmentRequisitionVM.Id = Convert.ToInt32(dr["Id"]);
                    RecruitmentRequisitionVM.Department = dr["Department"].ToString();
                    RecruitmentRequisitionVM.DepartmentName = dr["DepartmentName"].ToString();
                    RecruitmentRequisitionVM.DesignationName = dr["DesignationName"].ToString();
                    RecruitmentRequisitionVM.Designation = dr["Designation"].ToString();
                    RecruitmentRequisitionVM.Experience = dr["Experience"].ToString();
                    RecruitmentRequisitionVM.Deadline = dr["Deadline"].ToString();
                    RecruitmentRequisitionVM.Description = dr["Description"].ToString();
                    RecruitmentRequisitionVM.IsActive = Convert.ToBoolean(dr["IsActive"].ToString());
                    RecruitmentRequisitionVM.IsApproved = Convert.ToBoolean(dr["IsApproved"].ToString());
                    recruitmentRequisitionVMs.Add(RecruitmentRequisitionVM);

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

            return recruitmentRequisitionVMs;
        }

        public string FieldDelimeter { get; set; }

        public RecruitmentRequisitionVM SelectById(int Id)
        {
            #region Variables

            SqlConnection currConn = null;
            string sqlText = "";
            RecruitmentRequisitionVM RecruitmentRequisitionVM = new RecruitmentRequisitionVM();
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

                sqlText = @"SELECT rr.*, d.Name DepartmentName, dg.Name DesignationName FROM RecruitmentRequisition rr
                Left Outer Join Department d on d.Id=rr.Department
                Left Outer Join Designation dg on dg.Id=rr.Designation
                WHERE rr.IsActive=1";
                sqlText += @" and rr.Id=@Id ";               
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.Parameters.AddWithValue("@Id", Id);
                objComm.CommandType = CommandType.Text;
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    RecruitmentRequisitionVM = new RecruitmentRequisitionVM();
                    RecruitmentRequisitionVM.Id = Convert.ToInt32(dr["Id"]);
                    RecruitmentRequisitionVM.Department = dr["Department"].ToString();
                    RecruitmentRequisitionVM.Designation = dr["Designation"].ToString();
                    RecruitmentRequisitionVM.Designation = dr["Designation"].ToString();
                    RecruitmentRequisitionVM.Experience = dr["Experience"].ToString();
                    RecruitmentRequisitionVM.Deadline = dr["Deadline"].ToString();
                    RecruitmentRequisitionVM.Description = dr["Description"].ToString();
                    RecruitmentRequisitionVM.IsActive = Convert.ToBoolean(dr["IsActive"].ToString());
                    RecruitmentRequisitionVM.IsApproved = Convert.ToBoolean(dr["IsApproved"].ToString());
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

            return RecruitmentRequisitionVM;
        }

        public string[] ApprovedRecruitmentRequisition(int id, SqlConnection VcurrConn, SqlTransaction Vtransaction)
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
            retResults[5] = "DeleteRecruitmentRequisition"; //Method Name
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            #endregion
            #region Try
            try
            {
                #region Validation
                //if (string.IsNullOrEmpty(RecruitmentRequisitionVM.Id))
                //{
                //    retResults[1] = "Please InputRecruitmentRequisition";
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

                    sqlText = @"SELECT IsApproved FROM RecruitmentRequisition where Id=@Id";
                    SqlCommand objComm = new SqlCommand(sqlText, currConn);
                    objComm.Parameters.AddWithValue("@Id",id);
                    objComm.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(objComm);
                    transaction.Commit();
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if(dt.Rows.Count>0)
                    {
                        if(dt.Rows[0][0].ToString()=="True")
                        {
                            IsApproved = false;
                        }
                        else
                        {
                            IsApproved = true;
                        }
                    }


                    sqlText = "  ";
                    sqlText += @"Update RecruitmentRequisition set
                         
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
                    retResults[1] = "This RecruitmentRequisition already used";
                    throw new ArgumentNullException("Please Input RecruitmentRequisition Value", "");
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
                        retResults[1] = "Unexpected error to update RecruitmentRequisition.";
                        return retResults;
                        // throw new ArgumentNullException("Unexpected error to update RecruitmentRequisition.", "RecruitmentRequisition");
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
        public string[] DeleteRecruitmentRequisition(int Id, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {

            #region Initializ
            string sqlText = "";
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = Id.ToString();// Return Id
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "RecruitmentRequisition"; //Method Name
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
                    sqlText += @"Update RecruitmentRequisition  set                        
                          IsActive =@IsActive,
                          IsArchive =@IsArchive
                          ";
                    sqlText += " where Id=@Id ";

                    SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                    cmdInsert.Parameters.AddWithValue("@Id", Id);
                    cmdInsert.Parameters.AddWithValue("@IsActive", false);
                    cmdInsert.Parameters.AddWithValue("@IsArchive", true);
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
                retResults[1] = "RecruitmentRequisition has been Deleted Successfully";
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
                        retResults[1] = "Unexpected error to update RecruitmentRequisitionInfo.";
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

       
    }
}
