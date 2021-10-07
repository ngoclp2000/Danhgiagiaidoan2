using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Interface
{
    public interface IPeriodSalaryService : IBaseService<PeriodSalary>
    {
        /// <summary>
        /// Lấy dữ liệu kỳ lương theo phân trang
        /// </summary>
        /// <param name="pageSize">Kích thước trang</param>
        /// <param name="pageIndex">Vị trí trang</param>
        /// <param name="periodFilter">Nội dung lọc</param>
        /// <returns>Đối tượng bao gồm:
        ///             + Danh sách nhân viên
        ///             + Số lượng trang
        ///             + Số lượng bản ghi
        ///  Nếu lỗi sẽ trả về lỗi.
        /// </returns>
        /// Created By TBN (20/9/2021)
        object GetPeriodSalaryFilterPaging(int? pageSize, int? pageIndex, string periodFilter);
    }
}
