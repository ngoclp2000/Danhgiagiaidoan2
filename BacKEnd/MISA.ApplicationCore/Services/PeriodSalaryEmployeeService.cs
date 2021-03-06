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
    public class PeriodSalaryEmployeeService : BaseService<PeriodSalaryEmployee>, IPeriodSalaryEmployeeService
    {
        #region DECLARE
        IPeriodSalaryEmployeeRepository _periodSalaryEmployeeRepository;
        string locale;
        ResourceManager resourceManager;
        #endregion

        #region Constructor
        public PeriodSalaryEmployeeService(IPeriodSalaryEmployeeRepository periodSalaryEmployeeRepository) : base(periodSalaryEmployeeRepository)
        {
            _periodSalaryEmployeeRepository = periodSalaryEmployeeRepository;
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
        /// <summary>
        /// Phương thức lấy danh sách kỳ lương của nhân viên theo mã định danh kỳ lương
        /// </summary>
        /// <param name="periodSalaryId">Mã định danh kỳ lương</param>
        /// Created By TBN (20/9/2021)
        /// <returns>Danh sách thông tin kỳ lương của nhân viên tương ứng với kỳ lương cần tìm</returns>
        public IEnumerable<PeriodSalaryEmployee> GetPeriodSalaryEmployeeByPeriodSalaryId(Guid periodSalaryId)
        {
            return _periodSalaryEmployeeRepository.GetPeriodSalaryEmployeeByPeriodSalaryId(periodSalaryId);
        }
    }
}
