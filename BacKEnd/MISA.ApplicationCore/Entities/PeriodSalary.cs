using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Entities
{
    public class PeriodSalary : BaseEntity
    {
        #region Properties
        /// <summary>
        /// Mã định danh kỳ
        /// </summary>
        /// Created By TBN (20/9/2021)
        [PrimaryKey]
        public Guid PeriodSalaryId { get; set; }
        /// <summary>
        /// Tên kỳ
        /// </summary>
        /// Created By TBN (20/9/2021)
        [Required]
        [MaxLengthValidation(255)]
        public string PeriodName { get; set; }
        /// <summary>
        /// Tháng
        /// </summary>
        /// Created By TBN (20/9/2021)
        public string Month { get; set; }
        /// <summary>
        /// Năm 
        /// </summary>
        /// Created By TBN (20/9/2021)
        public string Year { get; set; }
        /// <summary>
        /// Ghi chú
        /// </summary>
        /// Created By TBN (20/9/2021)
        [MaxLengthValidation(255)]
        public string Note { get; set; }
        /// <summary>
        /// Tổng số nhân viên
        /// </summary>
        /// Created By TBN (20/9/2021)
        public int NumberOfEmployee { get; set; }
        /// <summary>
        /// Tổng số tiền lương
        /// </summary>
        /// Created By TBN (20/9/2021)
        public int TotalSalary { get; set; }
        /// <summary>
        /// Danh sách nhân viên bị xóa ( chỉ phục cho việc cập nhật thông tin của kỳ lương)
        /// </summary>
        /// Created By TBN (1/10/2021)
        public string EmployeeDeleteList { get; set; }
        /// <summary>
        /// Danh sách nhân viên được cập nhật thông tin ( chỉ phục cho việc cập nhật thông tin của kỳ lương)
        /// </summary>
        /// Created By TBN (1/10/2021)
        public string EmployeeUpdateList { get; set; }
        /// <summary>
        /// Danh sách nhân viên được thêm mới ( Phục vụ cho việc cập nhật và thêm mới kì lương)
        /// </summary>
        /// Created By TBN (1/10/2021)
        public string EmployeeNewList { get; set; }
        #endregion
    }
}
