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
    public class PeriodSalaryRepository : BaseRepository<PeriodSalary>, IPeriodSalaryRepository
    {
        public PeriodSalaryRepository(IConfiguration configuration) : base(configuration)
        {
        }
        /// <summary>
        /// Phương thức kiểm tra trùng lặp kỳ lương
        /// </summary>
        /// <param name="month">Tháng</param>
        /// <param name="year">Năm</param>
        /// <returns>True=Trùng, False = Không trùng</returns>
        public bool CheckDuplidatePeriodSalary(string month, string year,Guid periodSalaryId)
        {
            var sqlCommand = Function.CheckDuplicatePeriodSalary;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Month", month);
            parameters.Add("@Year", year);
            parameters.Add("@PeriodSalaryId", periodSalaryId);
            var checkValue = _dbConnection.Query(sql: sqlCommand, parameters, commandType: CommandType.Text);

            return ((IConvertible)checkValue.SingleOrDefault().Result).ToBoolean(null);
        }

        /// <summary>
        /// Lấy danh sách nhân viên theo một số tiêu chí
        /// Created By TBN ( 27/7/2021)
        /// </summary>
        /// <param name="pageSize">Kích thước bản ghi trong 1 trang</param>
        /// <param name="pageIndex">Vị trí trang</param>
        /// <param name="periodSalaryFilter">Giá trị lọc</param>
        /// <returns>Danh sách nhân viên tương ứng với các tiêu chí trên</returns>
        public object GetPeriodSalaryFilterPaging(int? pageSize, int? pageIndex, string periodSalaryFilter)
        {
            // Câu lệnh truy vấn
            var sqlCommand = string.Format(StoredProcedure.GetEntityFilterPaging, "PeriodSalary"); //"Proc_GetEmployeesFilterPaging";
            // Tạo params truyền vào procedure
            var parameters = new DynamicParameters();
            parameters.Add("PeriodSalaryFilter", periodSalaryFilter, direction: ParameterDirection.Input);
            parameters.Add("PageIndex", pageIndex, direction: ParameterDirection.Input);
            parameters.Add("PageSize", pageSize, direction: ParameterDirection.Input);
            parameters.Add("TotalRecord", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("TotalPage", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var periodSalaries = _dbConnection.Query<PeriodSalary>(sqlCommand, parameters, commandType: CommandType.StoredProcedure);
            return new
            {
                TotalPage = parameters.Get<int>("TotalPage"),
                TotalRecord = parameters.Get<int>("TotalRecord"),
                Data = periodSalaries
            };
        }
    }
}
