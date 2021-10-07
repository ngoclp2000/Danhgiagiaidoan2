using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore
{
    /// <summary>
    /// Enum cho mã code trả về FE
    /// @author = "MF914-TBNgoc" (25/7/2021)
    /// </summary>
    public enum MISACode
    {
        /// <summary>
        /// Dữ liệu không hợp lệ
        /// </summary>
        NotValid = 104,
        /// <summary>
        /// Thành công
        /// </summary>
        Success = 200,
        /// <summary>
        /// Thất bại
        /// </summary>
        Fail = 201,
        /// <summary>
        /// Lỗi hệ thống
        /// </summary>
        Exception = 500

    }
    /// <summary>
    /// Enum trả về trạng thái của thực thể
    /// </summary>
    /// Created By TBN (31/7/2021)
    public enum MISAEntityState
    {
        /// <summary>
        /// Trạng thái thêm mới
        /// Created By TBN(31/7/2021)
        /// </summary>
        Insert = 1,
        /// <summary>
        /// Trạng thái cập nhập
        /// Created By TBN (31/7/2021)
        /// </summary>
        Update = 2,
        /// <summary>
        /// Trạng thái xóa
        /// Created By TBN (31/7/2021)
        /// </summary>
        Delete = 3
    }
    
}
