using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Interface
{
    public interface IEmployeeService: IBaseService<Employee>
    {
        #region Methods
        /// <summary>
        /// Lấu mã nhân viên mới
        /// Created By TBN ( 27/7/2021)
        /// </summary>
        /// <returns>Mã nhân viên </returns>
        string GetNewEmployeeCode();
        /// <summary>
        /// Lấy nhân viên theo mã nhân viên
        /// </summary>
        /// Created By TBN (27/7/2021)
        /// <param name="employeeCode">Mã nhân viên</param>
        /// <returns>Nhân viên tương ứng với mã code</returns>
        Employee GetEmployeeByCode(string employeeCode);
        /// <summary>
        /// Lấy danh sách nhân viên theo một số tiêu chí
        /// </summary>
        /// Created By TBN ( 27/7/2021)
        /// <param name="pageSize">Kích thước bản ghi trong 1 trang</param>
        /// <param name="pageIndex">Vị trí trang</param>
        /// <param name="employeeFilter">Giá trị lọc</param>
        /// <returns>Danh sách nhân viên tương ứng với các tiêu chí trên</returns>
        object GetEmployeesFilterPaging(int? pageSize, int? pageIndex, string employeeFilter);
        #endregion
    }
}
