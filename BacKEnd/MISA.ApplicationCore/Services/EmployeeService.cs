using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Services
{
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        #region DECLARE
        IEmployeeRepository _employeeRepository;
        string locale;
        ResourceManager resourceManager;
        #endregion

        #region Constructor
        public EmployeeService(IEmployeeRepository employeeRepository) : base(employeeRepository)
        {
            _employeeRepository = employeeRepository;
            locale = System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
            switch (locale)
            {
                case "vi":
                    resourceManager = new ResourceManager(typeof(ApplicationCore.Resource.ViResource));
                    break;
                case "en":
                    resourceManager = new ResourceManager(typeof(ApplicationCore.Resource.EnResource));
                    break;
            }
            
        }
        #endregion
        #region Methods
        /// <summary>
        /// Lấy dữ liệu nhân viên theo code nhân viên
        /// </summary>
        /// Created By TBN (31/7/2021)
        /// <param name="employeeCode">Code của nhân viên muốn lấy</param>
        /// <returns>Trả về dữ liệu nhân viên tương ứng với mã code nếu có</returns>
        public Employee GetEmployeeByCode(string employeeCode)
        {
            return _employeeRepository.GetEmployeeByCode(employeeCode);
        }
        /// <summary>
        /// Phương thức lấy dữ liệu nhân viên theo tiêu chí
        /// </summary>
        /// Created By TBN(31/7/2021)
        /// <param name="pageSize">Kích thước trang</param>
        /// <param name="pageIndex">Số trang </param>
        /// <param name="employeeFilter">Dữ liệu cần lọc</param>
        /// <returns>Trả về đối tượng
        ///     TotalRecord: Tổng số bản ghi tương ứng với các tiêu chí
        ///     TotalPage: Tổng số trang tương ứng với kích thước cho trước
        ///     Data: Danh sách dữ liệu tại trang hiện tại cần lấy
        /// </returns>
        public object GetEmployeesFilterPaging(int? pageSize, int? pageIndex, string employeeFilter)
        {
            if (pageSize <= 0 || pageIndex < 0)
            {
                var errorMessageArr = new List<object>();
     
                if (pageSize <= 0)
                {
                    errorMessageArr.Add(new
                    {
                       
                        devMsg = string.Format(resourceManager.GetString("InvalidFieldDev"), resourceManager.GetString("PageSize"), pageSize, pageSize.GetType()),
                        usrMsg = string.Format(resourceManager.GetString("InvalidFieldUsr"), resourceManager.GetString("PageSize"), pageSize, pageSize.GetType())
                    });
                }
                if (pageIndex < 0)
                {
                    errorMessageArr.Add(new
                    {
                        
                        devMsg = string.Format(resourceManager.GetString("InvalidFieldDev"), resourceManager.GetString("PageIndex"), pageIndex, pageIndex.GetType()),
                        usrMsg = string.Format(resourceManager.GetString("InvalidFieldUsr"), resourceManager.GetString("PageIndex"), pageIndex, pageIndex.GetType())
                    });
                }
                var serviceResult = new ServiceResult
                {
                    Data = string.Empty,
                    Message = errorMessageArr,
                    MISACode = MISACode.NotValid
                };
                return serviceResult;
            }
            else
            {
                return _employeeRepository.GetEmployeesFilterPaging(pageSize, pageIndex, employeeFilter);
            }
        }
        /// <summary>
        /// Lấy code nhân viên mới nhất
        /// </summary>
        /// Created By TBN (31/7/2021)
        /// <returns>Code nhân viên mới nhất</returns>
        public string GetNewEmployeeCode()
        {
            return _employeeRepository.GetNewEmployeeCode();
        }
        #endregion
    }
}
