using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.ApplicationCore.Const;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Infrastructure.Repository
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        #region Constructor
        public EmployeeRepository(IConfiguration configuration) : base(configuration)
        {

        }
        #endregion
        #region Methods
        /// <summary>
        /// Lấy nhân viên theo mã code
        /// Created By TBN (25/7/2021)
        /// </summary>
        /// <param name="employeeCode">Mã nhân viên</param>
        /// <returns>Nhân viên tương ứng với mã </returns>
        public Employee GetEmployeeByCode(string employeeCode)
        {
            // Câu lệnh truy vấn
            var sqlCommand = string.Format(StoredProcedure.GetEntityByCode,"Employee"); //"Proc_GetEmployeeByCode";
            // Kết nối db nhận kết quả
            var customer = _dbConnection.Query<Employee>(sqlCommand, new { employeeCode }, commandType: CommandType.StoredProcedure);
            return customer.FirstOrDefault();
        }
        /// <summary>
        /// Lấy danh sách nhân viên theo một số tiêu chí
        /// Created By TBN ( 27/7/2021)
        /// </summary>
        /// <param name="pageSize">Kích thước bản ghi trong 1 trang</param>
        /// <param name="pageIndex">Vị trí trang</param>
        /// <param name="employeeFilter">Giá trị lọc</param>
        /// <param name="departmentId">Mã phòng ban</param>
        /// <param name="positionId">Mã vị trí</param>
        /// <returns>Danh sách nhân viên tương ứng với các tiêu chí trên</returns>
        public object GetEmployeesFilterPaging(int? pageSize, int? pageIndex, string employeeFilter)
        {
            // Câu lệnh truy vấn
            var sqlCommand = string.Format(StoredProcedure.GetEntityFilterPaging,"Employee"); //"Proc_GetEmployeesFilterPaging";
            // Tạo params truyền vào procedure
            var parameters = new DynamicParameters();
            parameters.Add("EmployeeFilter", employeeFilter, direction: ParameterDirection.Input);
            parameters.Add("PageIndex", pageIndex, direction: ParameterDirection.Input);
            parameters.Add("PageSize", pageSize, direction: ParameterDirection.Input);
            parameters.Add("TotalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("TotalPage", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var employees = _dbConnection.Query<Employee>(sqlCommand, parameters, commandType: CommandType.StoredProcedure);
            return new
            {
                TotalPage = parameters.Get<int>("TotalPage"),
                TotalRecord = parameters.Get<int>("TotalRecord"),
                Data = employees
            };
        }
        /// <summary>
        /// Lấu mã nhân viên
        /// Created By TBN ( 25/7/2021)
        /// </summary>
        /// <returns>Mã nhân viên </returns>
        public string GetNewEmployeeCode()
        {
            // Câu truy vấn
            var sqlCommand = string.Format(StoredProcedure.GetNewEntityCode,"Employee"); //"Proc_GetNewEmployeeCode";
            // Gửi truy vấn lên db và nhận lại kết quả
            var newEmployeeCode = _dbConnection.Query<string>(sqlCommand, commandType: CommandType.StoredProcedure);
            // Trả về kết quả
            if (newEmployeeCode != null)
            {
                return newEmployeeCode.First().ToString();
            }
            else
            {
                return null;
            }
        }
        #endregion
    }

}
