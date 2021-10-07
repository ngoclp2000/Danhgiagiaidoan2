using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Interface
{
    public interface IPeriodSalaryEmployeeService : IBaseService<PeriodSalaryEmployee> 
    {
        /// <summary>
        /// Phương thức lấy danh sách kỳ lương của nhân viên theo mã định danh kỳ lương
        /// </summary>
        /// <param name="periodSalaryId">Mã định danh kỳ lương</param>
        /// Created By TBN (20/9/2021)
        /// <returns>Danh sách thông tin kỳ lương của nhân viên tương ứng với kỳ lương cần tìm</returns>
        public IEnumerable<PeriodSalaryEmployee> GetPeriodSalaryEmployeeByPeriodSalaryId(Guid periodSalaryId);

    }
}
