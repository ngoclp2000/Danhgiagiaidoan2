using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.ApplicationCore;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;

namespace MISA.CukCuk.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BaseEntityController<MISAEntity> : ControllerBase
    {
        #region DECLARE
        IBaseService<MISAEntity> _baseService;
        string locale;
        ResourceManager resourceManager;
        #endregion
        #region Constructor
        public BaseEntityController(IBaseService<MISAEntity> baseService)
        {
            // Thiếp lập ngôn ngữ cho hệ thống
            string lang = "vi-VN";
            System.Threading.Thread.CurrentThread.CurrentCulture =
                new System.Globalization.CultureInfo(lang);
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
            _baseService = baseService;
        }
        #endregion
        #region Methods
        /// <summary>
        /// Lấy toàn bộ dữ liệu
        /// </summary>
        /// Created by TBN( 26/7/2021 )
        /// <returns>
        ///  Danh sách dữ liệu dưới dạng mảng object
        /// </returns>
        [HttpGet]
        public virtual IActionResult GetMISAEntities()
        {
            try
            {
                //var lang = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
                var misaEntities = _baseService.GetMISAEntities();
                return Ok(misaEntities);
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
                }); ;
            }
        }
        /// <summary>
        /// Lấy dữ liệu theo mã định danh 
        /// </summary>
        /// Created By TBN (26/7/2021)
        /// <param name="id">Mã định danh</param>
        /// <returns>Trả về dữ liệu nhân viên theo mã định danh</returns>
        [HttpGet("{id}")]
        public virtual IActionResult GetMISAEntityById(Guid id)
        {
            try
            {
                var misaEntity = _baseService.GetMISAEntityById(id);
                if (misaEntity != null)
                {
                    return Ok(misaEntity);
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
                });
            }

        }
        /// <summary>
        /// Lấy dữ liệu theo code
        /// </summary>
        /// <param name="misaEntitycode">Mã code</param>
        /// Created By TBN (18/8/2021)
        /// <returns>Dữ liệu tương ứng với mã code nếu có</returns>
        [HttpGet("code/{code}")]
        public virtual IActionResult GetMISAEntityByCode(string code)
        {
            try
            {
                var misaEntity = _baseService.GetMISAEntityByCode(code);
                if (misaEntity != null)
                {
                    return Ok(misaEntity);
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
                });
            }
        }
        /// <summary>
        /// Xóa dữ liệu theo mã định danh
        /// </summary>
        /// <param name="id"> Mã nhân viên </param>
        /// Created By TBN (26/7/2021)
        /// <returns> Thông tin sau khi thực hiện yêu cầu </returns>
        [HttpDelete("{id}")]
        public virtual IActionResult DeleteMISAEntityById(Guid id)
        {
            try
            {
                // Gọi xuống service để thực hiện kiểm tra nghiệp vụ
                var serviceResult = _baseService.DeleteMISAEntityById(id);
                return Ok(serviceResult);
            }
            catch (Exception ex)
            {
                // Nếu xảy ra lỗi hệ thống
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
        /// Thêm mới dữ liệu
        /// </summary>
        /// <param name="misaEntity">Dữ liệu</param>
        /// Created By TBN (26/7/2021)
        /// <returns> Thông tin sau khi thực hiện yêu cầu </returns>
        [HttpPost]
        public virtual IActionResult InsertMISAEntity(MISAEntity misaEntity)
        {
            try
            {
                // Chạy phương thức thêm mới
                var serviceResult = _baseService.InsertMISAEntity(misaEntity);
                // Kiểm tra các mã trả về
                if (serviceResult.MISACode == MISACode.NotValid)
                {
                    return Ok(serviceResult);
                }
                else
                {
                    return StatusCode(201,serviceResult);
                }
            }
            catch(Exception ex)
            {
                
                return StatusCode(500,new ServiceResult
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
        /// Cập thông tin dữ liệu
        /// </summary>
        /// <param name="customer">Dữ liêụ</param>
        /// <param name="id">Mã định danh</param>
        /// Created By TBN (26/7/2021)
        /// <returns> Thông tin sau khi thực hiện yêu cầu</returns>
        [HttpPut("{id}")]
        public virtual IActionResult UpdateMISAEntityById(MISAEntity misaEntity, Guid id)
        {
            try
            {
                var serviceResult = _baseService.UpdateMISAEntityById(misaEntity, id);
                return Ok(serviceResult);
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
        [HttpPost("Delete_List")]
        public virtual IActionResult DeleteMISAEntitiesById([FromBody] List<Guid> listEntityIds)
        {
            try
            {
                ServiceResult serviceResult = _baseService.DeleteMISAEntitiesById(listEntityIds);
                return Ok(serviceResult);
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
