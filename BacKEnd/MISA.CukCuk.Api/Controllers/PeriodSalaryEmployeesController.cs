using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.ApplicationCore;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;

namespace MISA.CukCuk.Api.Controllers
{
    public class PeriodSalaryEmployeesController : BaseEntityController<PeriodSalaryEmployee>
    {
        #region DECLARE
        IPeriodSalaryEmployeeService _periodSalaryEmployeeService;
        string locale;
        ResourceManager resourceManager;

        #endregion
        #region Constructor
        public PeriodSalaryEmployeesController(IPeriodSalaryEmployeeService periodSalaryEmployeeService) : base(periodSalaryEmployeeService)
        {
            _periodSalaryEmployeeService = periodSalaryEmployeeService;
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
        /// Phương thức lấy danh sách kỳ lương của nhân viên theo kỳ lương cho trước
        /// </summary>
        /// <param name="periodSalaryId">Mã định danh kỳ lương</param>
        /// <returns>Danh sách kỳ lương của nhân viên</returns>
        [HttpGet("PeriodSalaryId")]
        public IActionResult GetPeriodSalaryEmployeeByPeriodSalaryId(Guid periodSalaryId)
        {
            try
            {
                //var lang = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
                var periodSalaryEmployees = _periodSalaryEmployeeService.GetPeriodSalaryEmployeeByPeriodSalaryId(periodSalaryId);
                if (periodSalaryEmployees.Any())
                {
                    return Ok(periodSalaryEmployees);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult
                {
                    Data = ex.Message,
                    Message = new
                    {
                        devMsg = resourceManager.GetString("SystemErrorDev"),
                        usrMsg = resourceManager.GetString("SystemErrorUsr")
                    },
                    MISACode = MISACode.Exception
                }); ;
            }
        }
        #endregion
    }
}
