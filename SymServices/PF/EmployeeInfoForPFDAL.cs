using Excel;
using SymOrdinary;
using SymServices.Common;
using SymViewModel.Common;
using SymViewModel.PF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymServices.PF
{
    public class EmployeeInfoForPFDAL
    {
        #region Global Variables
        private DBSQLConnection _dbsqlConnection = new DBSQLConnection();
        CommonDAL _cDal = new CommonDAL();
        #endregion
        public string FieldDelimeter { get; set; }
        public string[] InsertEmployeeInfoForPF(EmployeeInfoForPFVM vm, SqlConnection VcurrConn, SqlTransaction Vtransaction)
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
            retResults[5] = "InsertEmployeeInfoForPF"; //Method Name
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            #endregion
            #region Try
            try
            {
                #region Validation
                //if (string.IsNullOrEmpty(EmployeeInfoForPFVM.Id))
                //{
                //    retResults[1] = "Please Input InsertEmployeeInfoForPF";
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
                    currConn = _dbsqlConnection.GetConnectionPF();
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
                        sqlText += @" Update EmployeeInfo set
                                     Code=@Code
                                     ,Name=@Name
                                     ,Department=@Department
                                     ,Designation =@Designation
                                     ,Project=@Project
                                     ,Section=@Section
                                     ,DateOfBirth=@DateOfBirth   
                                     ,JoinDate=@JoinDate 
                                     ,ResignDate=@ResignDate                            
                                     ,IsActive=@IsActive 
                                     ,IsArchive=@IsArchive 
                                     ,CreatedBy=@CreatedBy 
                                     ,CreatedAt=@CreatedAt 
                                     ,CreatedFrom=@CreatedFrom 
                                     ,LastUpdateBy=@LastUpdateBy 
                                     ,LastUpdateFrom=@LastUpdateFrom 
                                     ,PhotoName=@PhotoName  
                                     ,Remarks=@Remarks 
        
                                     ,NomineeDateofBirth=@NomineeDateofBirth
                                     ,NomineeName=@NomineeName
                                     ,NomineeRelation=@NomineeRelation
                                     ,NomineeAddress=@NomineeAddress
                                     ,NomineeDistrict=@NomineeDistrict
                                     ,NomineeDivision=@NomineeDivision
                                     ,NomineeCountry=@NomineeCountry
                                     ,NomineeCity=@NomineeCity
                                     ,NomineePostalCode=@NomineePostalCode
                                     ,NomineePhone=@NomineePhone
                                     ,NomineeMobile=@NomineeMobile
                                     ,NomineeBirthCertificateNo=@NomineeBirthCertificateNo
                                     ,NomineeFax=@NomineeFax
                                     ,NomineeFileName=@NomineeFileName
                                     ,NomineeRemarks=@NomineeRemarks
                                     ,NomineeNID=@NomineeNID
                                     ,GrossSalary=@GrossSalary
                                     ,BasicSalary=@BasicSalary 
                                     ,Email=@Email
                                     ,ContactNo=@ContactNo
                                     where Id=@Id   
                                 ";
                        SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                        cmdInsert.Parameters.AddWithValue("@Id", vm.Id);
                        cmdInsert.Parameters.AddWithValue("@Code", vm.Code);
                        cmdInsert.Parameters.AddWithValue("@Name", vm.Name);
                        cmdInsert.Parameters.AddWithValue("@Department", vm.Department);
                        cmdInsert.Parameters.AddWithValue("@Designation", vm.Designation);
                        cmdInsert.Parameters.AddWithValue("@Project", vm.Project);
                        cmdInsert.Parameters.AddWithValue("@Section", vm.Section);
                        cmdInsert.Parameters.AddWithValue("@DateOfBirth", vm.DateOfBirth);
                        cmdInsert.Parameters.AddWithValue("@JoinDate", vm.JoinDate);
                        cmdInsert.Parameters.AddWithValue("@ResignDate", vm.ResignDate);
                        cmdInsert.Parameters.AddWithValue("@IsActive", vm.IsActive);
                        cmdInsert.Parameters.AddWithValue("@IsArchive", vm.IsArchive);
                        cmdInsert.Parameters.AddWithValue("@CreatedBy", vm.CreatedBy);
                        cmdInsert.Parameters.AddWithValue("@CreatedAt", vm.CreatedAt);
                        cmdInsert.Parameters.AddWithValue("@CreatedFrom", vm.CreatedFrom ?? "");
                        cmdInsert.Parameters.AddWithValue("@LastUpdateBy", vm.LastUpdateBy ?? "");
                        cmdInsert.Parameters.AddWithValue("@LastUpdateFrom", vm.LastUpdateFrom ?? "");
                        cmdInsert.Parameters.AddWithValue("@PhotoName", vm.PhotoName ?? "");
                        cmdInsert.Parameters.AddWithValue("@Remarks", vm.Remarks ?? "");

                        cmdInsert.Parameters.AddWithValue("@NomineeDateofBirth", vm.NomineeDateofBirth);
                        cmdInsert.Parameters.AddWithValue("@NomineeName", vm.NomineeName);
                        cmdInsert.Parameters.AddWithValue("@NomineeRelation", vm.NomineeRelation);
                        cmdInsert.Parameters.AddWithValue("@NomineeAddress", vm.NomineeAddress);
                        cmdInsert.Parameters.AddWithValue("@NomineeDistrict", vm.NomineeDistrict);
                        cmdInsert.Parameters.AddWithValue("@NomineeDivision", vm.NomineeDivision);
                        cmdInsert.Parameters.AddWithValue("@NomineeCountry", vm.NomineeCountry);
                        cmdInsert.Parameters.AddWithValue("@NomineeCity", vm.NomineeCity);
                        cmdInsert.Parameters.AddWithValue("@NomineePostalCode", vm.NomineePostalCode);
                        cmdInsert.Parameters.AddWithValue("@NomineePhone", vm.NomineePhone);
                        cmdInsert.Parameters.AddWithValue("@NomineeMobile", vm.NomineeMobile);
                        cmdInsert.Parameters.AddWithValue("@NomineeBirthCertificateNo", vm.NomineeBirthCertificateNo);
                        cmdInsert.Parameters.AddWithValue("@NomineeFax", vm.NomineeFax);
                        cmdInsert.Parameters.AddWithValue("@NomineeFileName", vm.NomineeFileName);
                        cmdInsert.Parameters.AddWithValue("@NomineeRemarks", vm.NomineeRemarks);
                        cmdInsert.Parameters.AddWithValue("@NomineeNID", vm.NomineeNID);
                        cmdInsert.Parameters.AddWithValue("@GrossSalary", vm.GrossSalary);
                        cmdInsert.Parameters.AddWithValue("@BasicSalary", vm.BasicSalary);
                        cmdInsert.Parameters.AddWithValue("@ContactNo", vm.ContactNo);
                        cmdInsert.Parameters.AddWithValue("@Email", vm.Email);
                        cmdInsert.ExecuteNonQuery();
                    }
                    else
                    {

                        sqlText = "  ";
                        sqlText += @" INSERT INTO EmployeeInfo(
                                   [Code]
                                  ,[Name]
                                  ,[Department]
                                  ,[Designation]
                                  ,[Project]
                                  ,[Section]
                                  ,[DateOfBirth]
                                  ,[JoinDate]
                                  ,[ResignDate]
                                  ,[Remarks]
                                  ,[IsActive]
                                  ,[IsArchive]
                                  ,[CreatedBy]
                                  ,[CreatedAt]
                                  ,[CreatedFrom]                         
                                  ,[LastUpdateAt]
                                  ,[LastUpdateFrom]
                                  ,[PhotoName] 
                                  ,[NomineeDateofBirth]
                                  ,[NomineeName]
                                  ,[NomineeRelation]
                                  ,[NomineeAddress]
                                  ,[NomineeDistrict]
                                  ,[NomineeDivision]
                                  ,[NomineeCountry]
                                  ,[NomineeCity]
                                  ,[NomineePostalCode]
                                  ,[NomineePhone]
                                  ,[NomineeMobile]
                                  ,[NomineeBirthCertificateNo]
                                  ,[NomineeFax]
                                  ,[NomineeFileName]
                                  ,[NomineeRemarks]
                                  ,[NomineeNID]
                                  ,[GrossSalary]
                                  ,[BasicSalary] 
                                  ,[Email]
                                  ,[ContactNo]                                          
                                ) 
                                   VALUES (
                                   @Code
                                  ,@Name
                                  ,@Department
                                  ,@Designation
                                  ,@Project
                                  ,@Section
                                  ,@DateOfBirth  
                                  ,@JoinDate
                                  ,@ResignDate
                                  ,@Remarks
                                  ,@IsActive
                                  ,@IsArchive
                                  ,@CreatedBy 
                                  ,@CreatedAt
                                  ,@CreatedFrom                        
                                  ,@LastUpdateAt
                                  ,@LastUpdateFrom
                                  ,@PhotoName 
        
                                  ,@NomineeDateofBirth
                                  ,@NomineeName
                                  ,@NomineeRelation
                                  ,@NomineeAddress
                                  ,@NomineeDistrict
                                  ,@NomineeDivision
                                  ,@NomineeCountry
                                  ,@NomineeCity
                                  ,@NomineePostalCode
                                  ,@NomineePhone
                                  ,@NomineeMobile
                                  ,@NomineeBirthCertificateNo
                                  ,@NomineeFax
                                  ,@NomineeFileName
                                  ,@NomineeRemarks
                                  ,@NomineeNID
                                  ,@GrossSalary
                                  ,@BasicSalary     
                                  ,@Email
                                  ,@ContactNo     
                                ) 
                                 ";
                        SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                        cmdInsert.Parameters.AddWithValue("@Code", vm.Code);
                        cmdInsert.Parameters.AddWithValue("@Name", vm.Name);
                        cmdInsert.Parameters.AddWithValue("@Department", vm.Department);
                        cmdInsert.Parameters.AddWithValue("@Designation", vm.Designation);
                        cmdInsert.Parameters.AddWithValue("@Project", vm.Project);
                        cmdInsert.Parameters.AddWithValue("@Section", vm.Section);
                        cmdInsert.Parameters.AddWithValue("@DateOfBirth", vm.DateOfBirth);
                        cmdInsert.Parameters.AddWithValue("@JoinDate", vm.JoinDate);
                        cmdInsert.Parameters.AddWithValue("@ResignDate", vm.ResignDate);
                        cmdInsert.Parameters.AddWithValue("@Remarks", vm.Remarks ?? "");
                        cmdInsert.Parameters.AddWithValue("@IsActive", vm.IsActive);
                        cmdInsert.Parameters.AddWithValue("@IsArchive", vm.IsArchive);
                        cmdInsert.Parameters.AddWithValue("@CreatedBy", vm.CreatedBy);
                        cmdInsert.Parameters.AddWithValue("@CreatedAt", vm.CreatedAt);
                        cmdInsert.Parameters.AddWithValue("@CreatedFrom", vm.CreatedFrom);
                        cmdInsert.Parameters.AddWithValue("@LastUpdateAt", vm.LastUpdateAt ?? "");
                        cmdInsert.Parameters.AddWithValue("@LastUpdateFrom", vm.LastUpdateFrom ?? "");
                        cmdInsert.Parameters.AddWithValue("@PhotoName", vm.PhotoName ?? "");
                        cmdInsert.Parameters.AddWithValue("@NomineeDateofBirth", vm.NomineeDateofBirth);
                        cmdInsert.Parameters.AddWithValue("@NomineeName", vm.NomineeName);
                        cmdInsert.Parameters.AddWithValue("@NomineeRelation", vm.NomineeRelation);
                        cmdInsert.Parameters.AddWithValue("@NomineeAddress", vm.NomineeAddress);
                        cmdInsert.Parameters.AddWithValue("@NomineeDistrict", vm.NomineeDistrict);
                        cmdInsert.Parameters.AddWithValue("@NomineeDivision", vm.NomineeDivision);
                        cmdInsert.Parameters.AddWithValue("@NomineeCountry", vm.NomineeCountry);
                        cmdInsert.Parameters.AddWithValue("@NomineeCity", vm.NomineeCity);
                        cmdInsert.Parameters.AddWithValue("@NomineePostalCode", vm.NomineePostalCode);
                        cmdInsert.Parameters.AddWithValue("@NomineePhone", vm.NomineePhone);
                        cmdInsert.Parameters.AddWithValue("@NomineeMobile", vm.NomineeMobile);
                        cmdInsert.Parameters.AddWithValue("@NomineeBirthCertificateNo", vm.NomineeBirthCertificateNo);
                        cmdInsert.Parameters.AddWithValue("@NomineeFax", vm.NomineeFax);
                        cmdInsert.Parameters.AddWithValue("@NomineeFileName", vm.NomineeFileName);
                        cmdInsert.Parameters.AddWithValue("@NomineeRemarks", vm.NomineeRemarks);
                        cmdInsert.Parameters.AddWithValue("@NomineeNID", vm.NomineeNID);
                        cmdInsert.Parameters.AddWithValue("@GrossSalary", vm.GrossSalary);
                        cmdInsert.Parameters.AddWithValue("@BasicSalary", vm.BasicSalary);
                        cmdInsert.Parameters.AddWithValue("@ContactNo", vm.ContactNo);
                        cmdInsert.Parameters.AddWithValue("@Email", vm.Email);
                        cmdInsert.ExecuteNonQuery();
                    }
                }
                else
                {
                    retResults[1] = "This EmployeeInfoForPF already used";
                    throw new ArgumentNullException("Please Input EmployeeInfoForPF Value", "");
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
                        retResults[1] = "Unexpected error to update InsertEmployeeInfoForPF";
                        return retResults;
                        // throw new ArgumentNullException("Unexpected error to update EmployeeInfoForPF.", "EmployeeInfoForPF");
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

        public List<EmployeeInfoForPFVM> SelectAllEmployeeInfoForPF()
        {
            #region Variables

            SqlConnection currConn = null;
            string sqlText = "";
            List<EmployeeInfoForPFVM> employeeInfoForPFVMs = new List<EmployeeInfoForPFVM>();
            EmployeeInfoForPFVM EmployeeInfoForPFVM;
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
                sqlText = @"SELECT ve.EmployeeId, ve.Code, ve.EmpName, ve.DateOfBirth, ve.JoinDate, ve.LeftDate, ve.Branch, ve.Grade, ISNULL(ve.GrossSalary, 0) AS GrossSalary, ISNULL(ve.BasicSalary, 0) AS BasicSalary, ve.PhotoName, 
                         ve.IsActive, ve.IsArchive, ve.LastUpdateAt, ve.LastUpdateBy, ve.LastUpdateFrom, ve.Other1, ve.Remarks, ve.Department, ve.Designation, ve.Section, ve.Project
                         FROM ViewEmployeeInformation AS ve";

                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.CommandType = CommandType.Text;
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    EmployeeInfoForPFVM = new EmployeeInfoForPFVM();
                    EmployeeInfoForPFVM.Id = Convert.ToInt32(dr["EmployeeId"]);
                    EmployeeInfoForPFVM.Code = dr["Code"].ToString();
                    EmployeeInfoForPFVM.Name = dr["EmpName"].ToString();
                    EmployeeInfoForPFVM.Department = dr["Department"].ToString();
                    EmployeeInfoForPFVM.Designation = dr["Designation"].ToString();
                    EmployeeInfoForPFVM.Project = dr["Project"].ToString();
                    EmployeeInfoForPFVM.Section = dr["Section"].ToString();
                    EmployeeInfoForPFVM.DateOfBirth = dr["DateOfBirth"].ToString();
                    EmployeeInfoForPFVM.JoinDate = dr["JoinDate"].ToString();    
                    employeeInfoForPFVMs.Add(EmployeeInfoForPFVM);

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

            return employeeInfoForPFVMs;
        }

        public EmployeeInfoForPFVM SelectById(int Id)
        {
            #region Variables

            SqlConnection currConn = null;
            string sqlText = "";
            EmployeeInfoForPFVM EmployeeInfoForPFVM = new EmployeeInfoForPFVM();
            #endregion
            try
            {
                #region open connection and transaction

                currConn = _dbsqlConnection.GetConnectionPF();
                if (currConn.State != ConnectionState.Open)
                {
                    currConn.Open();
                }

                #endregion open connection and transaction

                #region sql statement

                sqlText = @"SELECT * FROM EmployeeInfo EF               
                WHERE 1=1";
                sqlText += @" and EF.Id=@Id";
                SqlCommand objComm = new SqlCommand();
                objComm.Connection = currConn;
                objComm.CommandText = sqlText;
                objComm.Parameters.AddWithValue("@Id", Id);
                objComm.CommandType = CommandType.Text;
                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    EmployeeInfoForPFVM = new EmployeeInfoForPFVM();
                    EmployeeInfoForPFVM.Id = Convert.ToInt32(dr["Id"]);
                    EmployeeInfoForPFVM.Code = dr["Code"].ToString();
                    EmployeeInfoForPFVM.Name = dr["Name"].ToString();
                    EmployeeInfoForPFVM.Department = dr["Department"].ToString();
                    EmployeeInfoForPFVM.Designation = dr["Designation"].ToString();
                    EmployeeInfoForPFVM.Project = dr["Project"].ToString();
                    EmployeeInfoForPFVM.Section = dr["Section"].ToString();
                    EmployeeInfoForPFVM.DateOfBirth = dr["DateOfBirth"].ToString();
                    EmployeeInfoForPFVM.JoinDate = dr["JoinDate"].ToString();
                    EmployeeInfoForPFVM.ResignDate = dr["ResignDate"].ToString();
                    EmployeeInfoForPFVM.BasicSalary = Convert.ToDecimal(dr["BasicSalary"]);
                    EmployeeInfoForPFVM.GrossSalary = Convert.ToDecimal(dr["GrossSalary"]);
                    EmployeeInfoForPFVM.NomineeName = dr["NomineeName"].ToString();
                    EmployeeInfoForPFVM.NomineeDateofBirth = dr["NomineeDateofBirth"].ToString();
                    EmployeeInfoForPFVM.NomineeRelation = dr["NomineeRelation"].ToString();
                    EmployeeInfoForPFVM.NomineeAddress = dr["NomineeAddress"].ToString();
                    EmployeeInfoForPFVM.NomineeCountry = dr["NomineeCountry"].ToString();
                    EmployeeInfoForPFVM.NomineeDivision = dr["NomineeDivision"].ToString();
                    EmployeeInfoForPFVM.NomineeDistrict = dr["NomineeDistrict"].ToString();
                    EmployeeInfoForPFVM.NomineeCity = dr["NomineeCity"].ToString();
                    EmployeeInfoForPFVM.NomineePostalCode = dr["NomineePostalCode"].ToString();
                    EmployeeInfoForPFVM.NomineePhone = dr["NomineePhone"].ToString();
                    EmployeeInfoForPFVM.NomineeMobile = dr["NomineeMobile"].ToString();
                    EmployeeInfoForPFVM.NomineeBirthCertificateNo = dr["NomineeBirthCertificateNo"].ToString();
                    EmployeeInfoForPFVM.NomineeFax = dr["NomineeFax"].ToString();
                    EmployeeInfoForPFVM.NomineeFileName = dr["NomineeFileName"].ToString();
                    EmployeeInfoForPFVM.NomineeNID = dr["NomineeNID"].ToString();
                    EmployeeInfoForPFVM.NomineeRemarks = dr["NomineeRemarks"].ToString();
                    EmployeeInfoForPFVM.Remarks = dr["Remarks"].ToString();
                    EmployeeInfoForPFVM.IsActive = Convert.ToBoolean(dr["IsActive"].ToString());
                    EmployeeInfoForPFVM.ContactNo = dr["ContactNo"].ToString();
                    EmployeeInfoForPFVM.Email = dr["Email"].ToString();
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

            return EmployeeInfoForPFVM;
        }
        public string[] DeleteEmployeeInfoForPF(int Id, SqlConnection VcurrConn, SqlTransaction Vtransaction)
        {

            #region Initializ
            string sqlText = "";
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = Id.ToString();// Return Id
            retResults[3] = sqlText; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = "DeleteEmployeeInfoForPF"; //Method Name
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
                    currConn = _dbsqlConnection.GetConnectionPF();
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
                    sqlText += @"Update EmployeeInfo  set                        
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
                retResults[1] = "DeleteEmployeeInfoForPF has been Deleted Successfully";
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
                        retResults[1] = "Unexpected error to update DeleteEmployeeInfoForPF.";
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
        public string[] InsertExportData(EmployeeInfoForPFVM paramVM, SqlConnection VcurrConn, SqlTransaction Vtransaction)
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
            retResults[5] = "ImportExcelFile"; //Method Name

            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            #endregion

            #region try
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                #region Excel Reader

                string FileName = paramVM.File.FileName;
                string Fullpath = AppDomain.CurrentDomain.BaseDirectory + "Files\\Export\\" + FileName;
                File.Delete(Fullpath);
                if (paramVM.File != null && paramVM.File.ContentLength > 0)
                {
                    paramVM.File.SaveAs(Fullpath);
                }


                FileStream stream = File.Open(Fullpath, FileMode.Open, FileAccess.Read);
                IExcelDataReader reader = null;
                if (FileName.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (FileName.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                reader.IsFirstRowAsColumnNames = true;
                ds = reader.AsDataSet();


                dt = ds.Tables[0];
                reader.Close();

                File.Delete(Fullpath);
                #endregion

                #region open connection and transaction
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
                string Code = "";

                EmployeeInfoForPFVM vEmployeeInfoVM = new EmployeeInfoForPFVM();
             
                #region Assign Data
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    vEmployeeInfoVM.Code = dr["Code"].ToString();
                    vEmployeeInfoVM.Name = dr["Name"].ToString();
                    //vEmployeeInfoVM.DateOfBirth = dr["DateOfBirth"].ToString();
                    //vEmployeeInfoVM.JoinDate = dr["JoinDate"].ToString();
                    //vEmployeeInfoVM.ResignDate = dr["ResignDate"].ToString();
                    vEmployeeInfoVM.Department = dr["Department"].ToString();
                    vEmployeeInfoVM.Designation = dr["Designation"].ToString();
                    vEmployeeInfoVM.Section = dr["Section"].ToString();
                    vEmployeeInfoVM.Project = dr["Project"].ToString();
                    vEmployeeInfoVM.BasicSalary = Convert.ToDecimal(dr["BasicSalary"].ToString());
                    vEmployeeInfoVM.GrossSalary = Convert.ToDecimal(dr["GrossSalary"].ToString());
                    vEmployeeInfoVM.Remarks = dr["Remarks"].ToString();                   
                    retResults = Insert(vEmployeeInfoVM, currConn, transaction);
                }
                #endregion

                #region Data Insert

               
                if (retResults[0] == "Fail")
                {
                    throw new ArgumentNullException("", retResults[1]);
                }
                #endregion
                #endregion
                #region Commit
                if (transaction != null)
                {
                    transaction.Commit();
                }
                #endregion Commit

                #region SuccessResult
                retResults[0] = "Success";
                retResults[1] = "Data Save Successfully.";
                //retResults[2] = vm.Id.ToString();
                #endregion SuccessResult
            }
            #endregion try
            #region Catch and Finall
            catch (Exception ex)
            {
                retResults[4] = ex.Message.ToString(); //catch ex
                transaction.Rollback();
                return retResults;
            }
            finally
            {
            }
            #endregion
            #region Results
            return retResults;
            #endregion
           
        }



        public string[] Insert(EmployeeInfoForPFVM vEmployeeInfoVM, SqlConnection VcurrConn, SqlTransaction Vtransaction)
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
            retResults[5] = "InsertEmplyeeInfoPF"; //Method Name
            EmployeeInfoForPFDAL _dal = new EmployeeInfoForPFDAL();

            #endregion
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
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
                //vm.Id = cdal.NextId("BonusProcess", currConn, transaction).ToString();
            
                    #region SqlText

                    sqlText = "  ";
                    sqlText += @" INSERT INTO EmployeeInfo
                                (
                                 Code                                
                                ,Name                                
                                ,Department
                                ,Designation
                                ,Project
                                ,Section 
                                ,BasicSalary
                                ,GrossSalary                          
                                ,Remarks
                                ,IsActive
                                ,IsArchive
                                ,CreatedBy
                                ,CreatedAt
                                ,CreatedFrom
                                ,LastUpdateBy
                                ,LastUpdateAt
                                ,LastUpdateFrom
                                ) VALUES (
                                 @Code
                                ,@Name                                
                                ,@Department
                                ,@Designation
                                ,@Project
                                ,@Section 
                                ,@BasicSalary
                                ,@GrossSalary 
                                ,@Remarks
                                ,@IsActive
                                ,@IsArchive
                                ,@CreatedBy
                                ,@CreatedAt
                                ,@CreatedFrom
                                ,@LastUpdateBy
                                ,@LastUpdateAt
                                ,@LastUpdateFrom
                                 ) ";
                    #endregion

                    #region SqlExecution
                                    

                        SqlCommand cmdInsert = new SqlCommand(sqlText, currConn, transaction);
                        cmdInsert.Parameters.AddWithValue("@Code", vEmployeeInfoVM.Code);
                        cmdInsert.Parameters.AddWithValue("@Name", vEmployeeInfoVM.Name);
                        cmdInsert.Parameters.AddWithValue("@Department", vEmployeeInfoVM.Department);
                        cmdInsert.Parameters.AddWithValue("@Designation", vEmployeeInfoVM.Designation);
                        cmdInsert.Parameters.AddWithValue("@Project", vEmployeeInfoVM.Project);
                        cmdInsert.Parameters.AddWithValue("@Section", vEmployeeInfoVM.Section);
                        cmdInsert.Parameters.AddWithValue("@BasicSalary", vEmployeeInfoVM.BasicSalary);
                        cmdInsert.Parameters.AddWithValue("@GrossSalary", vEmployeeInfoVM.GrossSalary);
                        cmdInsert.Parameters.AddWithValue("@Remarks", vEmployeeInfoVM.Remarks);
                        cmdInsert.Parameters.AddWithValue("@IsActive", true);
                        cmdInsert.Parameters.AddWithValue("@IsArchive", false);
                        cmdInsert.Parameters.AddWithValue("@CreatedBy", "");
                        cmdInsert.Parameters.AddWithValue("@CreatedAt", "");
                        cmdInsert.Parameters.AddWithValue("@CreatedFrom", "");
                        cmdInsert.Parameters.AddWithValue("@LastUpdateBy", "");
                        cmdInsert.Parameters.AddWithValue("@LastUpdateAt", "");
                        cmdInsert.Parameters.AddWithValue("@LastUpdateFrom", "");
                      
                        cmdInsert.ExecuteNonQuery();
                  

                    #endregion
              

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
