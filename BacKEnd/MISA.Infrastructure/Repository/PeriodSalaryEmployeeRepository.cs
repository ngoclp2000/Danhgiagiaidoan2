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
    public class PeriodSalaryEmployeeRepository : BaseRepository<PeriodSalaryEmployee>, IPeriodSalaryEmployeeRepository
    {
        public PeriodSalaryEmployeeRepository(IConfiguration configuration) : base(configuration)
        {
        }
        /// <summary>
        /// Phương thức lấy danh sách kỳ lương của nhân viên theo mã định danh kỳ lương
        /// </summary>
        /// <param name="periodSalaryId">Mã định danh kỳ lương</param>
        /// Created By TBN (20/9/2021)
        /// <returns>Danh sách thông tin kỳ lương của nhân viên tương ứng với kỳ lương cần tìm</returns>
        public IEnumerable<PeriodSalaryEmployee> GetPeriodSalaryEmployeeByPeriodSalaryId(Guid periodSalaryId)
        {
            var sqlCommand = StoredProcedure.GetPeriodSalaryEmployeeByPeriodSalaryId;
            // Tạo params cho stored procedure
            var parameters = new DynamicParameters();
            parameters.Add("PeriodSalaryId", periodSalaryId);
            var result = _dbConnection.Query<PeriodSalaryEmployee>(sqlCommand,parameters,commandType: CommandType.StoredProcedure);
            return result;
        }
    }
}
