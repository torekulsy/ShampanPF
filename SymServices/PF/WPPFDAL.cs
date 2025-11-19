using Excel;
using SymOrdinary;
using SymServices.Common;

using SymServices.Payroll;
using SymViewModel.Common;
using SymViewModel.HRM;
using SymViewModel.PF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace SymServices.PF
{
    public class WPPFDAL
    {
        #region Global Variables
        private const string FieldDelimeter = DBConstant.FieldDelimeter;
        private DBSQLConnection _dbsqlConnection = new DBSQLConnection();
        CommonDAL _cDal = new CommonDAL();
        #endregion

        #region Methods

        public List<PFHeaderVM> SelectFiscalPeriodHeader(string[] conditionFields = null, string[] conditionValues = null, SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
        {
            #region Variables
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            string sqlText = "";
            List<PFHeaderVM> VMs = new List<PFHeaderVM>();
            PFHeaderVM vm;
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
                string hrmDB = _dbsqlConnection.GetConnection().Database;
                #region sql statement
                #region SqlText
                sqlText = @"
SELECT 
pfd.Id
,pfd.Code
,pfd.FiscalYearDetailId
,p.Name ProjectName
,p.Id ProjectId
,fyd.PeriodName
,fyd.PeriodStart
,pfd.Post 
,pfd.TotalProfitValue TotalPF


FROM WPPFHeader pfd
";

                sqlText += "  LEFT OUTER JOIN [dbo].Project p ON pfd.ProjectId=p.Id";
                sqlText += "  LEFT OUTER JOIN [dbo].[FiscalYearDetail] fyd ON pfd.FiscalYearDetailId=fyd.Id";
                sqlText += @" WHERE  1=1  AND pfd.IsArchive = 0
";
                string cField = "";
                if (conditionFields != null && conditionValues != null && conditionFields.Length == conditionValues.Length)
                {
                    for (int i = 0; i < conditionFields.Length; i++)
                    {
                        if (string.IsNullOrWhiteSpace(conditionFields[i]) || string.IsNullOrWhiteSpace(conditionValues[i]))
                        {
                            continue;
                        }
                        cField = conditionFields[i].ToString();
                        cField = Ordinary.StringReplacing(cField);
                        sqlText += " AND " + conditionFields[i] + "=@" + cField;
                    }
                }
                //  sqlText += "  GROUP BY p.Name,p.Id, pfd.FiscalYearDetailId, fyd.PeriodName, fyd.PeriodStart, fyd.PeriodEnd, pfd.Post ";
                sqlText += " ORDER BY fyd.PeriodStart DESC";

                #endregion SqlText
                #region SqlExecution

                SqlCommand objComm = new SqlCommand(sqlText, currConn, transaction);
                if (conditionFields != null && conditionValues != null && conditionFields.Length == conditionValues.Length)
                {
                    for (int j = 0; j < conditionFields.Length; j++)
                    {
                        if (string.IsNullOrWhiteSpace(conditionFields[j]) || string.IsNullOrWhiteSpace(conditionValues[j]))
                        {
                            continue;
                        }
                        cField = conditionFields[j].ToString();
                        cField = Ordinary.StringReplacing(cField);
                        objComm.Parameters.AddWithValue("@" + cField, conditionValues[j]);
                    }
                }

                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    vm = new PFHeaderVM();
                    vm.Id = Convert.ToInt32(dr["Id"]);
                    vm.FiscalYearDetailId = Convert.ToInt32(dr["FiscalYearDetailId"]);
                    vm.Code = dr["Code"].ToString();
                    vm.ProjectName = dr["ProjectName"].ToString();
                    vm.ProjectId = dr["ProjectId"].ToString();
                    vm.FiscalPeriod = dr["PeriodName"].ToString();
                    vm.PeriodStart = dr["PeriodStart"].ToString();
                    vm.TotalPF = Convert.ToDecimal(dr["TotalPF"]);
                    vm.Post = Convert.ToBoolean(dr["Post"]);

                    VMs.Add(vm);
                }
                dr.Close();
                #endregion SqlExecution

                if (Vtransaction == null && transaction != null)
                {
                    transaction.Commit();
                }
                #endregion
            }
            #region catch

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

        public List<PFHeaderVM> SelectProfitDistribution(string[] conditionFields = null, string[] conditionValues = null, SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
        {
            #region Variables
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            string sqlText = "";
            List<PFHeaderVM> VMs = new List<PFHeaderVM>();
            PFHeaderVM vm;
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
                string hrmDB = _dbsqlConnection.GetConnection().Database;
                #region sql statement
                #region SqlText
                sqlText = @"Select a.Id, b.Code As WPPFCode, c.Name, a.DistributionDate, a.EmployeeProfit, a.Post  From WPPFProfitDistribution a";

                sqlText += "  left join WPPFHeader b on a.WPPFHeaderId = b.Id";
                sqlText += "  left join EmployeeInfo c on a.EmployeeId = c.Id";
                sqlText += @" WHERE  1=1  AND a.IsArchive = 0
";
                string cField = "";
                if (conditionFields != null && conditionValues != null && conditionFields.Length == conditionValues.Length)
                {
                    for (int i = 0; i < conditionFields.Length; i++)
                    {
                        if (string.IsNullOrWhiteSpace(conditionFields[i]) || string.IsNullOrWhiteSpace(conditionValues[i]))
                        {
                            continue;
                        }
                        cField = conditionFields[i].ToString();
                        cField = Ordinary.StringReplacing(cField);
                        sqlText += " AND " + conditionFields[i] + "=@" + cField;
                    }
                }
                //  sqlText += "  GROUP BY p.Name,p.Id, pfd.FiscalYearDetailId, fyd.PeriodName, fyd.PeriodStart, fyd.PeriodEnd, pfd.Post ";
                sqlText += " ORDER BY c.Name";

                #endregion SqlText
                #region SqlExecution

                SqlCommand objComm = new SqlCommand(sqlText, currConn, transaction);
                if (conditionFields != null && conditionValues != null && conditionFields.Length == conditionValues.Length)
                {
                    for (int j = 0; j < conditionFields.Length; j++)
                    {
                        if (string.IsNullOrWhiteSpace(conditionFields[j]) || string.IsNullOrWhiteSpace(conditionValues[j]))
                        {
                            continue;
                        }
                        cField = conditionFields[j].ToString();
                        cField = Ordinary.StringReplacing(cField);
                        objComm.Parameters.AddWithValue("@" + cField, conditionValues[j]);
                    }
                }

                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    vm = new PFHeaderVM();
                    vm.Id = Convert.ToInt32(dr["Id"]);
                    vm.Code = dr["WPPFCode"].ToString();
                    vm.ProjectName = dr["Name"].ToString();
                    vm.FiscalPeriod = dr["DistributionDate"].ToString();
                    vm.TotalPF = Convert.ToDecimal(dr["EmployeeProfit"]);
                    vm.Post = Convert.ToBoolean(dr["Post"]);

                    VMs.Add(vm);
                }
                dr.Close();
                #endregion SqlExecution

                if (Vtransaction == null && transaction != null)
                {
                    transaction.Commit();
                }
                #endregion
            }
            #region catch

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

        public List<PFHeaderVM> SelectWPPF(string[] conditionFields = null, string[] conditionValues = null, SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
        {
            #region Variables
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            string sqlText = "";
            List<PFHeaderVM> VMs = new List<PFHeaderVM>();
            PFHeaderVM vm;
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
                string hrmDB = _dbsqlConnection.GetConnection().Database;
                #region sql statement
                #region SqlText
                sqlText = @"
SELECT 
    pfd.Id,
    pfd.Code,
    pfd.FiscalYearDetailId,
    p.Name AS ProjectName,
    p.Id AS ProjectId,
    fyd.PeriodName,
    fyd.PeriodStart,
    pfd.Post,
    SUM(ISNULL(c.EmployeeProfit,0)) AS TotalPF
FROM WPPFHeader pfd
LEFT OUTER JOIN [dbo].[Project] p 
    ON pfd.ProjectId = p.Id
LEFT OUTER JOIN [dbo].[FiscalYearDetail] fyd 
    ON pfd.FiscalYearDetailId = fyd.Id
LEFT OUTER JOIN [dbo].[WPPFProfitDistribution] c 
    ON pfd.Id = c.WPPFHeaderId
WHERE 1 = 1
  AND pfd.IsArchive = 0
";

                // Condition fields
                string cField = "";
                if (conditionFields != null && conditionValues != null && conditionFields.Length == conditionValues.Length)
                {
                    for (int i = 0; i < conditionFields.Length; i++)
                    {
                        if (string.IsNullOrWhiteSpace(conditionFields[i]) || string.IsNullOrWhiteSpace(conditionValues[i]))
                        {
                            continue;
                        }

                        cField = Ordinary.StringReplacing(conditionFields[i]);
                        sqlText += " AND " + conditionFields[i] + "=@" + cField;
                    }
                }

                // Final GROUP BY (Must come last)
                sqlText += @"
GROUP BY 
    pfd.Id,
    pfd.Code,
    pfd.FiscalYearDetailId,
    p.Name,
    p.Id,
    fyd.PeriodName,
    fyd.PeriodStart,
    pfd.Post
";

                //  sqlText += "  GROUP BY p.Name,p.Id, pfd.FiscalYearDetailId, fyd.PeriodName, fyd.PeriodStart, fyd.PeriodEnd, pfd.Post ";
                

                #endregion SqlText
                #region SqlExecution

                SqlCommand objComm = new SqlCommand(sqlText, currConn, transaction);
                if (conditionFields != null && conditionValues != null && conditionFields.Length == conditionValues.Length)
                {
                    for (int j = 0; j < conditionFields.Length; j++)
                    {
                        if (string.IsNullOrWhiteSpace(conditionFields[j]) || string.IsNullOrWhiteSpace(conditionValues[j]))
                        {
                            continue;
                        }
                        cField = conditionFields[j].ToString();
                        cField = Ordinary.StringReplacing(cField);
                        objComm.Parameters.AddWithValue("@" + cField, conditionValues[j]);
                    }
                }

                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    vm = new PFHeaderVM();
                    vm.Id = Convert.ToInt32(dr["Id"]);
                    vm.FiscalYearDetailId = Convert.ToInt32(dr["FiscalYearDetailId"]);
                    vm.Code = dr["Code"].ToString();
                    vm.ProjectName = dr["ProjectName"].ToString();
                    vm.ProjectId = dr["ProjectId"].ToString();
                    vm.FiscalPeriod = dr["PeriodName"].ToString();
                    vm.PeriodStart = dr["PeriodStart"].ToString();
                    vm.TotalPF =Math.Round(Convert.ToDecimal(dr["TotalPF"]));
                    vm.Post = Convert.ToBoolean(dr["Post"]);

                    VMs.Add(vm);
                }
                dr.Close();
                #endregion SqlExecution

                if (Vtransaction == null && transaction != null)
                {
                    transaction.Commit();
                }
                #endregion
            }
            #region catch

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

        public List<PFHeaderVM> SelectWWF(string[] conditionFields = null, string[] conditionValues = null, SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
        {
            #region Variables
            SqlConnection currConn = null;
            SqlTransaction transaction = null;
            string sqlText = "";
            List<PFHeaderVM> VMs = new List<PFHeaderVM>();
            PFHeaderVM vm;
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
                string hrmDB = _dbsqlConnection.GetConnection().Database;
                #region sql statement
                #region SqlText
                sqlText = @"
SELECT 
pfd.Id
,c.Code
,pfd.FiscalYearDetailId
,p.Name ProjectName
,p.Id ProjectId
,fyd.PeriodName
,fyd.PeriodStart
,pfd.Post 
,pfd.TotalValue TotalPF
FROM WWFHeader pfd
";

                sqlText += "  LEFT OUTER JOIN [dbo].Project p ON pfd.ProjectId=p.Id";
                sqlText += "  LEFT OUTER JOIN [dbo].[FiscalYearDetail] fyd ON pfd.FiscalYearDetailId=fyd.Id";
                sqlText += "  LEFT OUTER JOIN [dbo].[WPPFHeader] c ON pfd.WPPFHeaderId=c.Id";
                sqlText += @" WHERE  1=1  AND pfd.IsArchive = 0
";
                string cField = "";
                if (conditionFields != null && conditionValues != null && conditionFields.Length == conditionValues.Length)
                {
                    for (int i = 0; i < conditionFields.Length; i++)
                    {
                        if (string.IsNullOrWhiteSpace(conditionFields[i]) || string.IsNullOrWhiteSpace(conditionValues[i]))
                        {
                            continue;
                        }
                        cField = conditionFields[i].ToString();
                        cField = Ordinary.StringReplacing(cField);
                        sqlText += " AND " + conditionFields[i] + "=@" + cField;
                    }
                }
                //  sqlText += "  GROUP BY p.Name,p.Id, pfd.FiscalYearDetailId, fyd.PeriodName, fyd.PeriodStart, fyd.PeriodEnd, pfd.Post ";
                sqlText += " ORDER BY fyd.PeriodStart DESC";

                #endregion SqlText
                #region SqlExecution

                SqlCommand objComm = new SqlCommand(sqlText, currConn, transaction);
                if (conditionFields != null && conditionValues != null && conditionFields.Length == conditionValues.Length)
                {
                    for (int j = 0; j < conditionFields.Length; j++)
                    {
                        if (string.IsNullOrWhiteSpace(conditionFields[j]) || string.IsNullOrWhiteSpace(conditionValues[j]))
                        {
                            continue;
                        }
                        cField = conditionFields[j].ToString();
                        cField = Ordinary.StringReplacing(cField);
                        objComm.Parameters.AddWithValue("@" + cField, conditionValues[j]);
                    }
                }

                SqlDataReader dr;
                dr = objComm.ExecuteReader();
                while (dr.Read())
                {
                    vm = new PFHeaderVM();
                    vm.Id = Convert.ToInt32(dr["Id"]);
                    vm.FiscalYearDetailId = Convert.ToInt32(dr["FiscalYearDetailId"]);
                    vm.Code = dr["Code"].ToString();
                    vm.ProjectName = dr["ProjectName"].ToString();
                    vm.ProjectId = dr["ProjectId"].ToString();
                    vm.FiscalPeriod = dr["PeriodName"].ToString();
                    vm.PeriodStart = dr["PeriodStart"].ToString();
                    vm.TotalPF = Convert.ToDecimal(dr["TotalPF"]);
                    vm.Post = Convert.ToBoolean(dr["Post"]);

                    VMs.Add(vm);
                }
                dr.Close();
                #endregion SqlExecution

                if (Vtransaction == null && transaction != null)
                {
                    transaction.Commit();
                }
                #endregion
            }
            #region catch

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

        public string[] PFProcess(decimal? TotalProfit, string FiscalYearDetailId, int? FiscalYear, ShampanIdentityVM auditvm, SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
        {
            string[] ret = new string[6] { "Fail", "Fail", "0", "", "", "PFProcess" };

            SqlConnection currConn = null;
            SqlTransaction transaction = null;

            try
            {
                // Open connection
                currConn = VcurrConn ?? _dbsqlConnection.GetConnection();
                if (currConn.State != ConnectionState.Open)
                    currConn.Open();

                transaction = Vtransaction ?? currConn.BeginTransaction();

                string distributionDate;

                string fySql = @"
    SELECT PeriodEnd 
    FROM FiscalYearDetail 
    WHERE Id = @FiscalYearDetailId";

                using (SqlCommand fyCmd = new SqlCommand(fySql, currConn, transaction))
                {
                    fyCmd.Parameters.AddWithValue("@FiscalYearDetailId", FiscalYearDetailId);

                    object obj = fyCmd.ExecuteScalar();

                    if (obj == null || obj == DBNull.Value)
                    {
                        distributionDate = DateTime.Now.ToString();
                    }
                    else
                    {
                        distributionDate = obj.ToString();
                    }
                }


                string insertSql = @"
INSERT INTO WPPFHeader
(
      Code
    , FiscalYearDetailId
    , Year
    , ProjectId
    , TotalProfitValue
    , Post
    , Remarks
    , IsActive
    , IsArchive
    , CreatedBy
    , CreatedAt
    , CreatedFrom

)
VALUES
(
      @Code
    , @FiscalYearDetailId
    , @Year
    , @ProjectId
    , @TotalProfitValue
    , 0                         
    , @Remarks
    , 1                       
    , 0                        
    , @CreatedBy
    , @CreatedAt                
    , @CreatedFrom


);

SELECT SCOPE_IDENTITY();
";

                string NewCode = new CommonDAL().CodeGenerationPF("PF", "PFContribution", DateTime.Now.ToString(), currConn, transaction);

               
                SqlCommand cmd = new SqlCommand(insertSql, currConn, transaction);
                cmd.Parameters.AddWithValue("@Code", NewCode);
                cmd.Parameters.AddWithValue("@FiscalYearDetailId", FiscalYearDetailId);
                cmd.Parameters.AddWithValue("@TotalProfitValue", TotalProfit);
                cmd.Parameters.AddWithValue("@Year", FiscalYear);  
                cmd.Parameters.AddWithValue("@ProjectId", "1_1");                  
                cmd.Parameters.AddWithValue("@Remarks", "");
                cmd.Parameters.AddWithValue("@CreatedBy", auditvm.CreatedBy);
                cmd.Parameters.AddWithValue("@CreatedAt ", auditvm.CreatedAt);
                cmd.Parameters.AddWithValue("@CreatedFrom", auditvm.CreatedFrom);
               
                decimal newId = Convert.ToDecimal(cmd.ExecuteScalar());

                string empSql = @"
                                SELECT 
                                      E.Id
                                    , E.Code
                                    , E.IsActive
                                FROM EmployeeInfo E
                                WHERE E.IsActive = 1
                                ORDER BY E.Id";

               

                SqlCommand empCmd = new SqlCommand(empSql, currConn, transaction);

                SqlDataAdapter daEmp = new SqlDataAdapter(empCmd);
                DataTable dt = new DataTable();
                daEmp.Fill(dt);

                decimal disPerson = dt.Rows.Count;
                decimal distValue = TotalProfit.Value * 0.05m;   
                decimal disWPPF = distValue * 0.8m;              
                decimal disEmpValue = disWPPF / disPerson;       
                decimal disWFF = distValue * 0.1m;               
                decimal disWWF = distValue * 0.1m;               

                foreach (DataRow dr in dt.Rows)
                {
                    int empId = Convert.ToInt32(dr["Id"]);

                    string insertDetailSql = @"
    INSERT INTO WPPFProfitDistribution
    (
          WPPFHeaderId
        , EmployeeId
        , DistributionDate
        , EmployeeProfit
        , Post
        , IsPaid
        , Remarks
        , IsActive
        , IsArchive
        , CreatedBy
        , CreatedAt
        , CreatedFrom
    )
    VALUES
    (
          @WPPFHeaderId
        , @EmployeeId
        , @DistributionDate              
        , @EmployeeProfit
        , 0                       
        , 0                     
        , ''
        , 1                      
        , 0                       
        , @CreatedBy
        , GETDATE()               
        , @CreatedFrom
    );";

                    SqlCommand detailCmd = new SqlCommand(insertDetailSql, currConn, transaction);

                    detailCmd.Parameters.AddWithValue("@WPPFHeaderId", newId);   
                    detailCmd.Parameters.AddWithValue("@EmployeeId", empId);
                    detailCmd.Parameters.AddWithValue("@DistributionDate", distributionDate);
                    detailCmd.Parameters.AddWithValue("@EmployeeProfit", disEmpValue);

                    detailCmd.Parameters.AddWithValue("@CreatedBy", auditvm.CreatedBy);
                    detailCmd.Parameters.AddWithValue("@CreatedFrom", auditvm.CreatedFrom);

                    detailCmd.ExecuteNonQuery();
                }

                string insertBWWFSql = @"
INSERT INTO BWWFHeader
(
      WPPFHeaderId
    , FiscalYearDetailId
    , Year
    , ProjectId
    , TotalValue
    , Post
    , Remarks
    , IsActive
    , IsArchive
    , CreatedBy
    , CreatedAt
    , CreatedFrom
)
VALUES
(
      @WPPFHeaderId
    , @FiscalYearDetailId
    , @Year
    , @ProjectId
    , @TotalValue
    , 0
    , ''
    , 1
    , 0
    , @CreatedBy
    , GETDATE()
    , @CreatedFrom
);";

                SqlCommand bwfCmd = new SqlCommand(insertBWWFSql, currConn, transaction);

                bwfCmd.Parameters.AddWithValue("@WPPFHeaderId", newId);
                bwfCmd.Parameters.AddWithValue("@FiscalYearDetailId", FiscalYearDetailId);
                bwfCmd.Parameters.AddWithValue("@Year", FiscalYear);
                bwfCmd.Parameters.AddWithValue("@ProjectId", "1_1");     
                bwfCmd.Parameters.AddWithValue("@TotalValue", disWFF);  
                bwfCmd.Parameters.AddWithValue("@CreatedBy", auditvm.CreatedBy);
                bwfCmd.Parameters.AddWithValue("@CreatedFrom", auditvm.CreatedFrom);

                bwfCmd.ExecuteNonQuery();

                string insertWWFSql = @"
INSERT INTO WWFHeader
(
      WPPFHeaderId
    , FiscalYearDetailId
    , Year
    , ProjectId
    , TotalValue
    , Post
    , Remarks
    , IsActive
    , IsArchive
    , CreatedBy
    , CreatedAt
    , CreatedFrom
)
VALUES
(
      @WPPFHeaderId
    , @FiscalYearDetailId
    , @Year
    , @ProjectId
    , @TotalValue
    , 0
    , ''
    , 1
    , 0
    , @CreatedBy
    , GETDATE()
    , @CreatedFrom
);";

                SqlCommand wwfCmd = new SqlCommand(insertWWFSql, currConn, transaction);

                wwfCmd.Parameters.AddWithValue("@WPPFHeaderId", newId);
                wwfCmd.Parameters.AddWithValue("@FiscalYearDetailId", FiscalYearDetailId);
                wwfCmd.Parameters.AddWithValue("@Year", FiscalYear);
                wwfCmd.Parameters.AddWithValue("@ProjectId", "1_1");
                wwfCmd.Parameters.AddWithValue("@TotalValue", disWWF);

                wwfCmd.Parameters.AddWithValue("@CreatedBy", auditvm.CreatedBy);
                wwfCmd.Parameters.AddWithValue("@CreatedFrom", auditvm.CreatedFrom);

                wwfCmd.ExecuteNonQuery();



                if (Vtransaction == null)
                    transaction.Commit();

                ret[0] = "Success";
                ret[1] = "WPPF successfully processed";
                ret[2] = newId.ToString();

                return ret;
            }
            catch (Exception ex)
            {
                if (Vtransaction == null && transaction != null)
                    transaction.Rollback();

                ret[1] = ex.Message;
                return ret;
            }
            finally
            {
                if (VcurrConn == null && currConn != null)
                    currConn.Close();
            }
        }

        public string[] PostHeader(PFHeaderVM vm, SqlConnection VcurrConn = null, SqlTransaction Vtransaction = null)
        {
            #region Variables
            string[] retResults = new string[6];
            retResults[0] = "Fail";//Success or Fail
            retResults[1] = "Fail";// Success or Fail Message
            retResults[2] = "0";
            retResults[3] = "sqlText"; //  SQL Query
            retResults[4] = "ex"; //catch ex
            retResults[5] = " WPPF Post"; //Method Name
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
                if (transaction == null) { transaction = currConn.BeginTransaction("Post WPPF"); }
                #endregion open connection and transaction
                if (vm != null)
                {
                    #region Update Settings
                    sqlText = "";
                    sqlText = "update WPPFHeader set";
                    sqlText += "  Post=@Post";

                    sqlText += @" where Id=@Id
                    update WPPFProfitDistribution set Post=@Post where WPPFHeaderId=@Id
                    UPDATE BWWFHeader SET Post=@Post WHERE WPPFHeaderId=@Id
                    UPDATE WWFHeader SET Post=@Post WHERE WPPFHeaderId=@Id";
                    SqlCommand cmdUpdate = new SqlCommand(sqlText, currConn, transaction);
                    cmdUpdate.Parameters.AddWithValue("@Id", vm.Id);
                    cmdUpdate.Parameters.AddWithValue("@Post", true);

                    var exeRes = cmdUpdate.ExecuteNonQuery();
                    transResult = Convert.ToInt32(exeRes);
                    retResults[2] = vm.Id.ToString();// Return Id
                    retResults[3] = sqlText; //  SQL Query
                    #region Commit
                    if (transResult <= 0)
                    {
                        // throw new ArgumentNullException("Education Update", EEHeadVM.BranchId + " could not updated.");
                    }
                    #endregion Commit
                    #endregion Update Settings
                }
                else
                {
                    throw new ArgumentNullException("Post WPPF", "Could not found any item.");
                }

                if (Vtransaction == null)
                {
                    transaction.Commit();
                }
                retResults[0] = "Success";
                retResults[1] = "Data  Successfully Post.";
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

        #endregion Methods
    }
}
