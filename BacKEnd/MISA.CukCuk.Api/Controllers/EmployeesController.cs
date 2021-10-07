using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;
using System.Data;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interface;
using MISA.CukCuk.Api.Controllers;
using MISA.ApplicationCore;
using System.Resources;

namespace MISA.CukCuk.Api.Api
{
   public class EmployeesController : BaseEntityController<Employee> {
        #region DECLARE
        IEmployeeService _employeeService;
        string locale;
        ResourceManager resourceManager;
       
        #endregion
        #region Constructor
        public EmployeesController(IEmployeeService employeeService) : base(employeeService)
        {
            _employeeService = employeeService;
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
        /// Lấy  mã nhân viên mới 
        /// </summary>
        /// <returns>
        /// Mã nhân viên mới dưới dạng string
        /// </returns>
        [HttpGet("NewEmployeeCode")]
        public IActionResult GetNewEmployeeCode()
        {
            try
            {
                var res = _employeeService.GetNewEmployeeCode();
                return Ok(res);
            }   
            catch(Exception ex)
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
        /// <summary>
        /// Lọc dữ liệu theo yêu cầu lọc
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="employeeFilter"></param>
        /// <returns> Trả về danh sách nhân viên theo yêu cầu lọc </returns>
        [HttpGet("EmployeeFilter")]
        public IActionResult GetFilterEmployee(int? pageSize, int? pageIndex, string employeeFilter)
        {
            try
            {
                object res = _employeeService.GetEmployeesFilterPaging(pageSize, pageIndex, employeeFilter);
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
        
    }
}
