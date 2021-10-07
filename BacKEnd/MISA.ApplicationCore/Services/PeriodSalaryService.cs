using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Services
{
    public class PeriodSalaryService : BaseService<PeriodSalary>, IPeriodSalaryService
    {
        #region DECLARE
        IPeriodSalaryRepository _periodRepository;
        string locale;
        ResourceManager resourceManager;
        #endregion

        #region Constructor
        public PeriodSalaryService(IPeriodSalaryRepository periodRepository) : base(periodRepository)
        {
            _periodRepository = periodRepository;
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
        /// Phương thức lấy dữ liệu kỳ lương theo tiêu chí
        /// </summary>
        /// Created By TBN(31/7/2021)
        /// <param name="pageSize">Kích thước trang</param>
        /// <param name="pageIndex">Số trang </param>
        /// <param name="periodFilter">Dữ liệu cần lọc</param>
        /// <returns>Trả về đối tượng
        ///     TotalRecord: Tổng số bản ghi tương ứng với các tiêu chí
        ///     TotalPage: Tổng số trang tương ứng với kích thước cho trước
        ///     Data: Danh sách dữ liệu tại trang hiện tại cần lấy
        /// </returns>
        public object GetPeriodSalaryFilterPaging(int? pageSize, int? pageIndex, string periodFilter)
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
                return _periodRepository.GetPeriodSalaryFilterPaging(pageSize, pageIndex, periodFilter);
            }
        }
        /// <summary>
        /// Phương thức kế thừa để validate trùng lặp ngày tháng
        /// </summary>
        /// <param name="periodSalary"></param>
        /// <param name="errorMessage"></param>
        /// <returns>Trạng thái True, False</returns>
        protected override bool ValidateCustom(PeriodSalary periodSalary, List<object> errorMessage)
        {
            var isValid = true;
            int month = ((IConvertible)Regex.Match(periodSalary.Month, @"\d+").Value).ToInt32(null);
            int year = ((IConvertible)Regex.Match(periodSalary.Year, @"\d+").Value).ToInt32(null);
            // Kiểm tra tính đúng đăn của ngày tháng
            if (month > 12)
            {
                errorMessage.Add(new
                {
                    fieldName = "Month",
                    type = "invalid",
                    devMsg = string.Format(resourceManager.GetString("InvalidFieldDev"), "Tháng",  periodSalary.Month.ToString(), periodSalary.Month.GetType().Name),
                    usrMsg = string.Format(resourceManager.GetString("InvalidFieldUsr"), "Tháng")
                });
                isValid = false;
            }else if(year > 2155 || year < 1901)
            {
                errorMessage.Add(new
                {
                    fieldName = "Year",
                    type = "invalid",
                    devMsg = string.Format(resourceManager.GetString("InvalidFieldDev"), "Năm", periodSalary.Year.ToString(), periodSalary.Year.GetType().Name),
                    usrMsg = string.Format(resourceManager.GetString("InvalidFieldUsr"), "Năm")
                });
                isValid = false;
            }

            if (!isValid) return isValid;

            // Kiểm tra trùng lặp kỳ lương
            if (!_periodRepository.CheckDuplidatePeriodSalary(periodSalary.Month, periodSalary.Year,periodSalary.PeriodSalaryId))
            {
                isValid = true;
            }else
            {
                errorMessage.Add(new
                {
                    fieldName = "Month",
                    type = "duplicate",
                    devMsg = string.Format(resourceManager.GetString("DuplicateFieldDev"), resourceManager.GetString("MonthYear"), periodSalary.Month.ToString() + "-" + " Năm" + periodSalary.Year.ToString(), periodSalary.Month.GetType().Name),
                    usrMsg = string.Format(resourceManager.GetString("DuplicateFieldUsr"), resourceManager.GetString("MonthYear"))
                });
                isValid = false;
            }
            
            return isValid;
        }
        
        #endregion
    }
}
