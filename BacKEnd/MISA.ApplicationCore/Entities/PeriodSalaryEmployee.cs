using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Entities
{
    public class PeriodSalaryEmployee : BaseEntity
    {
        /// <summary>
        /// Mã định danh kỳ lương nhân viên
        /// </summary>
        /// Created By TBN(20/9/2021)
        [PrimaryKey]
        public Guid PeriodSalaryEmployeeId { get; set; }
        /// <summary>
        /// Mã định danh nhân viên
        /// </summary>
        /// Create By TBN(20/9/2021)
        public Guid EmployeeId { get; set; }
        /// <summary>
        /// Tên nhân viên
        /// </summary>
        /// Create By TBN(20/9/2021)
        public string EmployeeName { get; set; }
        /// <summary>
        /// Mã nhân viên
        /// </summary>
        /// Create By TBN(20/9/2021)
        public string EmployeeCode { get; set; }
        /// <summary>
        /// Mã định danh kỳ lương
        /// </summary>
        /// Created By TBN(20/9/2021)
        public Guid PeriodSalaryId { get; set; }
        /// <summary>
        /// Lương
        /// </summary>
        /// Created By TBN (20/9/2021)
        [PositiveValue]
        public int Salary { get; set; }
        /// <summary>
        ///  Phí bảo hiểm xã hội
        /// </summary>
        /// Created By TBN (20/9/2021)
        [PositiveValue]
        public int SocialInsuranceCost { get; set; }
        /// <summary>
        /// Tiền trợ cấp
        /// </summary>
        /// Created By TBN (20/9/2021)
        [PositiveValue]
        public int Subsidy { get; set; }
        /// <summary>
        /// Chỉ số lương nhân viên
        /// </summary>
        /// Created By TBN (20/9/2021)
        [PositiveValue]
        public double CoefficientsSalary { get; set; }

    }
}
