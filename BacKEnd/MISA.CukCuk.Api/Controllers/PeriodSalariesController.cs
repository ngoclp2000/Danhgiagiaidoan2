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
    public class PeriodSalariesController : BaseEntityController<PeriodSalary>
    {
        #region DECLARE
        IPeriodSalaryService _periodSalaryService;
        string locale;
        ResourceManager resourceManager;

        #endregion
        #region Constructor
        public PeriodSalariesController(IPeriodSalaryService periodSalaryService) : base(periodSalaryService)
        {
            _periodSalaryService = periodSalaryService;
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
        /// Lọc dữ liệu theo yêu cầu lọc
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="periodSalaryFilter"></param>
        /// <returns> Trả về danh sách nhân viên theo yêu cầu lọc </returns>
        [HttpGet("PeriodSalaryFilter")]
        public IActionResult GetFilterEmployee(int? pageSize, int? pageIndex, string periodSalaryFilter)
        {
            try
            {
                object res = _periodSalaryService.GetPeriodSalaryFilterPaging(pageSize, pageIndex, periodSalaryFilter);
                if (res.GetType() == typeof(ServiceResult))
                {
                    return BadRequest(res);
                }
                else
                {
                    return Ok(res);
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
                });
            }

        }
        #endregion
    }
}
