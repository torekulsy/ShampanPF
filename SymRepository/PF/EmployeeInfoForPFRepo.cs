using SymServices.Common;
using SymServices.PF;
using SymViewModel.Common;
using SymViewModel.PF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymRepository.PF
{
    public class EmployeeInfoForPFRepo
    {
        public string[] InsertEmployeeInfoForPF(EmployeeInfoForPFVM vm)
        {
            try
            {
                return new EmployeeInfoForPFDAL().InsertEmployeeInfoForPF(vm, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<EmployeeInfoForPFVM> SelectAllEmployeeInfoForPF()
        {
            try
            {
                return new EmployeeInfoForPFDAL().SelectAllEmployeeInfoForPF();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public EmployeeInfoForPFVM SelectById(int id)
        {
            try
            {
                return new EmployeeInfoForPFDAL().SelectById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[] DeleteEmployeeInfoForPF(int Id)
        {
            try
            {
                return new EmployeeInfoForPFDAL().DeleteEmployeeInfoForPF(Id, null, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string[] ImportExcelFile(EmployeeInfoForPFVM vm)
        {
            throw new NotImplementedException();
        }
    }
}
