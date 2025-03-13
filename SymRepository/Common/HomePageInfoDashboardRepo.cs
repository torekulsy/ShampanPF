using SymServices.Common;
using SymViewModel.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SymRepository.Common
{
    public class HomePageInfoDashboardRepo
    {
        public AdminInfoDashboardVM GetAdminInfoDashboard()
        {
            try
            {
                return new HomePageInfoDashboardDAL().GetAdminInfoDashboard();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public HrmInfoDashboardVM GetHrmInfoDashboard()
        {
            try
            {
                return new HomePageInfoDashboardDAL().GetHrmInfoDashboard();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public PayrollInfoDashboardVM GetPayrollInfoDashboard()
        {
            try
            {
                return new HomePageInfoDashboardDAL().GetPayrollInfoDashboard();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public TaxInfoDashboardVM GetTaxInfoDashboard()
        {
            try
            {
                return new HomePageInfoDashboardDAL().GetTaxInfoDashboard();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public PfInfoDashboardVM GetPfInfoDashboard()
        {
            try
            {
                return new HomePageInfoDashboardDAL().GetPfInfoDashboard();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public GfInfoDashboardVM GetGfInfoDashboard()
        {
            try
            {
                return new HomePageInfoDashboardDAL().GetGfInfoDashboard();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
