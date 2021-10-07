using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Const
{
    /// <summary>
    /// Const chứa tên của stored procedure
    /// </summary>
    /// Created By TBN (4/8/2021)
    public class StoredProcedure
    {
        /// <summary>
        /// Const thêm mới dữ liệu
        /// Value: Proc_Insert{0}
        /// </summary>
        /// Created By TBN (31/7/2021)
        public static string InsertEntity => "Proc_Insert{0}";
        /// <summary>
        /// Const xóa dữ liệu theo mã định danh
        /// Value: Proc_Delete{0}ById
        /// </summary>
        /// Created By TBN (31/7/2021)
        public static string DeleteSpecificEntityById => "Proc_Delete{0}ById";
        /// <summary>
        /// Const lấy toàn bộ dữ liệu
        /// Value: Proc_Get{0}s
        /// </summary>
        /// Created By TBN (31/7/2021)
        public static string GetEntity => "Proc_Get{0}";
        /// <summary>
        /// Const lấy dữ liệu theo mã định danh
        /// Value: Proc_Get{0}ById
        /// </summary>
        /// Created By TBN (31/7/2021)
        public static string GetEntityById => "Proc_Get{0}ById";
        /// <summary>
        /// Const cập nhật dữ liệu 
        /// Value: Proc_Update{0}
        /// </summary>
        /// Created By TBN (31/7/2021)
        public static string UpdateEntity => "Proc_Update{0}";
        /// <summary>
        /// Lấy dữ liệu theo tiêu chí phân trang
        /// Value: Proc_Get{0}sFilterPaging
        /// </summary>
        /// Created By TBN (31/7/2021)
        public static string GetEntityFilterPaging => "Proc_Get{0}FilterPaging";
        /// <summary>
        /// Lấy dữ liệu theo mã code
        /// Value:  Proc_Get{0}ByCode
        /// </summary>
        /// Created By TBN (31/7/2021)
        public static string GetEntityByCode => "Proc_Get{0}ByCode";
        /// <summary>
        /// Lấy mã code mới 
        /// Value: Proc_GetNew{0}Code
        /// </summary>
        /// Created By TBN (31/7/2021)
        public static string GetNewEntityCode => "Proc_GetNew{0}Code";
        /// <summary>
        /// Lấy danh sách kỳ lương của nhân viên theo kỳ lương
        /// </summary>
        /// Created By TBN (20/9/2021)
        public static string GetPeriodSalaryEmployeeByPeriodSalaryId => "Proc_GetPeriodSalaryEmployeeByPeriodSalaryId";
    }
}
