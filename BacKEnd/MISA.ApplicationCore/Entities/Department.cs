using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Entities
{
    /// <summary>
    /// Thuộc tính của phòng ban
    /// </summary>
    /// Created By TBN (5/8/2021)
    public class Department: BaseEntity
    {
        /// <summary>
        /// Mã định danh phòng ban
        /// </summary>
        /// Created By TBN (29/7/2021)
        [PrimaryKey]
        public Guid DepartmentId { get; set; }
        /// <summary>
        /// Tên đơn vị
        /// </summary>
        /// Created By TBN (29/7/2021)
        [CheckDuplication]
        [DisplayName("Tên đơn vị")]
        [MaxLengthValidation(255)]
        public string DepartmentName { get; set; }
        /// <summary>
        /// Mã code đơn vị
        /// </summary>
        /// Created By TBN (29/7/2021)
        [CheckDuplication]
        [DisplayName("Mã đơn vị")]
        [MaxLengthValidation(20)]
        public string DepartmentCode { get; set; }
        /// <summary>
        /// Mô tả phòng ban
        /// </summary>
        /// Created By TBN (29/7/2021)
        [MaxLengthValidation(255)]
        [DisplayName("Mô tả")]
        public string Description { get; set; }
    }
}
