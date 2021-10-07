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
using MISA.CukCuk.Api.Controllers;
using MISA.ApplicationCore.Interface;

namespace MISA.CukCuk.Api.Api
{
    
    public class DepartmentsController : BaseEntityController<Department>
    {
        #region DECLARE
        IDepartmentService _departmentService;
        #endregion
        #region Constructor
        public DepartmentsController(IDepartmentService departmentService) : base(departmentService)
        {
            _departmentService = departmentService;
        }
        #endregion
    }
}
