using MISA.ApplicationCore.Const;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Services
{
    /// <summary>
    /// Lớp thực thi xử lý nghiệp vụ chung cho toàn bộ thực thể
    /// </summary>
    /// Created By TBN (4/8/2021)
    /// <typeparam name="MISAEntity">Kiểu thực thể</typeparam>
    public class BaseService<MISAEntity> : IBaseService<MISAEntity> where MISAEntity : BaseEntity
    {
        #region DECLARE
        IBaseRepository<MISAEntity> _baseRepository;
        string _tableName;
        protected ServiceResult _serviceResult;
        string locale;
        ResourceManager resourceManager;
        #endregion
        #region Constructor
        public BaseService(IBaseRepository<MISAEntity> baseRepository)
        {
            _baseRepository = baseRepository;
            _tableName = typeof(MISAEntity).Name;
            _serviceResult = new ServiceResult();
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
        }
        #endregion
        #region Methods
        /// <summary>
        /// Phương thức xóa dữ liệu theo mã định danh
        /// </summary>
        /// Created By TBN (31/7/2021)
        /// <param name="misaEntityId">Mã định danh của dữ liệu</param>
        /// <returns>Trạng thái của yêu cầu(Fail, Success)</returns>
        public virtual ServiceResult DeleteMISAEntityById(Guid misaEntityId)
        {
            var rowAffects = _baseRepository.DeleteMISAEntityById(misaEntityId);
            _serviceResult.Data = rowAffects;
            // Kiểm tra số dòng bị ảnh hưởng
            if (rowAffects <= 0)
            {
                // Xóa thất bại
                _serviceResult.Message = new
                {
                    devMsg = resourceManager.GetString("DeleteFail"),
                    usrMsg = resourceManager.GetString("DeleteFail")
                };
                _serviceResult.MISACode = MISACode.Fail;
                return _serviceResult;
            }
            else
            {
                // Xóa thành công
                _serviceResult.Message = new
                {
                    devMsg = resourceManager.GetString("DeleteSuccess"),
                    usrMsg = resourceManager.GetString("DeleteSuccess")
                };
                _serviceResult.MISACode = MISACode.Success;
                return _serviceResult;
            }
        }
        /// <summary>
        /// Phương thức lấy toàn bộ dữ liệu
        /// </summary>
        /// Created By TBN (31/7/2021)
        /// <returns>Toàn bộ dữ liệu</returns>
        public virtual IEnumerable<MISAEntity> GetMISAEntities()
        {
            return _baseRepository.GetMISAEntities();
        }
        /// <summary>
        /// Lấy dữ liệu theo mã định danh
        /// </summary>
        /// Created By TBN (31/7/2021)
        /// <param name="misaEntityId">Mã định danh của dữ liệu</param>
        /// <returns>Dữ liệu tương ứng với mã định danh</returns>
        public virtual MISAEntity GetMISAEntityById(Guid misaEntityId)
        {
            return _baseRepository.GetMISAEntityById(misaEntityId);
        }
        /// <summary>
        /// Lấy dữ liệu theo code
        /// </summary>
        /// <param name="misaEntitycode">Mã code</param>
        /// Created By TBN (19/8/2021)
        /// <returns>Dữ liệu tương ứng với mã code nếu có</returns>
        public virtual MISAEntity GetMISAEntityByCode(string misaEntitycode)
        {
            return _baseRepository.GetMISAEntityByCode(misaEntitycode);
        }
        /// <summary>
        /// Thêm mới dữ liệu
        /// </summary>
        /// Created By TBN (31/7/2021)
        /// <param name="misaEntityList"></param>
        /// <returns>Trả về trạng thái của yêu cầu</returns>
        public virtual ServiceResult InsertMISAEntity(MISAEntity misaEntity)
        {
            // Khởi tạo thông tin ban đầu cho dữ liệu cần được thêm
            misaEntity.EntityState = MISAEntityState.Insert;

            // Kiểm tra định dạng của dữ liệu
            var isValid = ValidateEntity(misaEntity);

            if (isValid)
            {
                var rowAffects = _baseRepository.InsertMISAEntity(misaEntity);
                _serviceResult.Data = rowAffects;
                if (rowAffects <= 0)
                {
                    // Request thành công nhưng thực hiện thêm thất bại
                    _serviceResult.Message = new
                    {
                        devMsg = resourceManager.GetString("AddFail"),
                        usrMsg = resourceManager.GetString("AddFail")
                    };
                    _serviceResult.MISACode = MISACode.Fail;
                    return _serviceResult;
                }
                else
                {
                    // Request thành công, thực hiện thêm thành công
                    _serviceResult.Message = new
                    {
                        devMsg = resourceManager.GetString("AddSuccess"),
                        usrMsg = resourceManager.GetString("AddSuccess")
                    };
                    _serviceResult.MISACode = MISACode.Success;
                    return _serviceResult;
                }
            }
            else
            {
                _serviceResult.MISACode = MISACode.NotValid;
                return _serviceResult;
            }

        }
        /// <summary>
        /// Cập nhật dữ liệu theo mã định danh
        /// </summary>
        /// Created By TBN (31/7/2021)
        /// <param name="misaEntity">Dữ liệu muốn cập nhật</param>
        /// <param name="misaEntityId">Mã định danh của dữ liệu</param>
        /// <returns>Trả về trạng thái </returns>
        public virtual ServiceResult UpdateMISAEntityById(MISAEntity misaEntity, Guid misaEntityId)
        {
            misaEntity.EntityState = MISAEntityState.Update;
            // Kiểm tra định dạng dữ liệu
            var isValid = ValidateEntity(misaEntity);

            if (isValid)
            {
                
                var serviceResult = new ServiceResult();
                var rowAffects = _baseRepository.UpdateMISAEnityById(misaEntity, misaEntityId);
                serviceResult.Data = rowAffects;
                if (rowAffects <= 0)
                {
                    // Chỉnh sửa thất bại
                    serviceResult.Message = new
                    {
                        devMsg = resourceManager.GetString("UpdateFail"),
                        usrMsg = resourceManager.GetString("UpdateFail")
                    };
                    serviceResult.MISACode = MISACode.Fail;
                    return serviceResult;
                }
                else
                {
                    // Chỉnh sửa thành công
                    serviceResult.Message = new
                    {
                        devMsg = resourceManager.GetString("UpdateSuccess"),
                        usrMsg = resourceManager.GetString("UpdateSuccess")
                    };
                    serviceResult.MISACode = MISACode.Success;
                    return serviceResult;
                }
            }
            else
            {
                // Dữ liệu không đúng định dạng
                _serviceResult.MISACode = MISACode.NotValid;
                return _serviceResult;
            }

        }
        /// <summary>
        /// Xóa nhiều dữ liệu theo mã định danh 
        /// </summary>
        /// Created By TBN (10/8/2021)
        /// <param name="listMISAEnitityIds">Danh sách mã định danh</param>
        /// <returns></returns>
        public ServiceResult DeleteMISAEntitiesById(List<Guid> listMISAEnitityIds)
        {
            int rowAffects = _baseRepository.DeleteMISAEntitiesById(listMISAEnitityIds);
            if (rowAffects == listMISAEnitityIds.Count)
            {
                return new ServiceResult
                {
                    Data = rowAffects,
                    Message = new
                    {
                        devMsg = resourceManager.GetString("DeleteSuccess"),
                        usrMsg = resourceManager.GetString("DeleteSuccess")
                    },
                    MISACode = MISACode.Success
                };
            }
            else
            {
                return new ServiceResult
                {
                    Data = rowAffects,
                    Message = new
                    {
                        devMsg = resourceManager.GetString("DeleteFail"),
                        usrMsg = resourceManager.GetString("DeleteFail")
                    },
                    MISACode = MISACode.Fail
                };
            }
        }
        /// <summary>
        /// Phương thức kiểm tra dữ liệu
        /// </summary>
        /// <param name="misaEntity">Thực thể</param>
        /// <returns>Trạng thái đúng sai của dữ liệu</returns>
        private bool ValidateEntity(MISAEntity misaEntity)
        {
            var isValid = true;
            var errorMessageArr = new List<object>();
            // Lấy các thuộc tính của dữ liệu
            var properties = misaEntity.GetType().GetProperties();
            foreach (var property in properties)
            {
                PropertyInfo prop = typeof(MISAEntity).GetProperty(property.Name);
                // Lấy giá trị của thuộc tính tự tạo
                // Tên của thuộc tính

                var displayName = resourceManager.GetString(property.Name); // Tên của thuộc tính

                // Độ dài lớn nhất của thuộc tính nếu có
                var maxLengthProp = property.GetCustomAttributes(typeof(MaxLengthValidation), true);
                var maxLengthValue = 0;
                if (maxLengthProp.Length > 0)
                    if (maxLengthProp.Length > 0)
                        maxLengthValue = (maxLengthProp[0] as MaxLengthValidation).MaxLength;
                // Kiêm tra thuộc tính cần kiểm tra
                if (property.IsDefined(typeof(Required), false))
                {
                    // Kiểm tra bắt buộc nhập
                    var propertyValue = property.GetValue(misaEntity);
                    if (String.IsNullOrEmpty(propertyValue.ToString()))
                    {
                        errorMessageArr.Add(new
                        {
                            fieldName = property.Name,
                            type = "empty",
                            devMsg = string.Format(resourceManager.GetString("NonEmptyFieldDev"), displayName),
                            ursMsg = string.Format(resourceManager.GetString("NonEmptyFieldUsr"), displayName)
                        });
                        _serviceResult.MISACode = MISACode.NotValid;
                        isValid = false;
                    }
                }
                // Kiểm tra trùng dữ liệu
                if (property.IsDefined(typeof(CheckDuplication), false) && property.GetValue(misaEntity) != null)
                {
                    var propertyName = property.Name;
                    var propertyValue = property.GetValue(misaEntity);

                    if (!String.IsNullOrEmpty(propertyValue.ToString()))
                    {
                        var misaEntityDuplicate = _baseRepository.GetEntityBySpesProperty(misaEntity, property);
                        if (misaEntityDuplicate != null)
                        {
                            errorMessageArr.Add(new
                            {
                                fieldName = property.Name,
                                type = "duplicate",
                                devMsg = string.Format(resourceManager.GetString("DuplicateFieldDev"), displayName, propertyValue, propertyValue.GetType().Name),
                                usrMsg = string.Format(resourceManager.GetString("DuplicateFieldUsr"), displayName)
                            });
                            isValid = false;
                            _serviceResult.MISACode = MISACode.NotValid;
                        }
                    }


                }
                // Kiểm tra dữ liệu tồn tại
                if (property.IsDefined(typeof(CheckExist), false) && misaEntity.EntityState != MISAEntityState.Insert)
                {
                    var propertyName = property.Name;
                    var propertyValue = property.GetValue(misaEntity);
                    var misaEntityDuplicate = _baseRepository.GetEntityBySpesProperty(misaEntity, property);
                    if (misaEntityDuplicate == null)
                    {
                        errorMessageArr.Add(new
                        {
                            fieldName = property.Name,
                            type = "duplicate",
                            devMsg = string.Format(resourceManager.GetString("NonExistIdDev"), displayName, propertyValue, propertyValue.GetType().Name),
                            usrMsg = string.Format(resourceManager.GetString("NonExistIdUsr"), displayName)
                        });
                        isValid = false;
                        _serviceResult.MISACode = MISACode.NotValid;
                    }
                }
                // Kiểm tra định dạng email
                if (property.IsDefined(typeof(EmailValidation), false) && property.GetValue(misaEntity) != null)
                {
                    var propertyValue = property.GetValue(misaEntity);
                    if (!String.IsNullOrEmpty(propertyValue.ToString()))
                    {
                        bool isValidEmail = Validator.EmailValidation(propertyValue.ToString());
                        if (!isValidEmail)
                        {
                            errorMessageArr.Add(new
                            {
                                fieldName = property.Name,
                                type = "invalid",
                                devMsg = string.Format(resourceManager.GetString("InvalidFormatDev"), displayName, propertyValue, propertyValue.GetType().Name),
                                usrMsg = string.Format(resourceManager.GetString("InvalidFormatUsr"), displayName)
                            });
                            isValid = false;
                        }
                    }
                }
                // Kiểm tra định dạng số điện thoại
                if (property.IsDefined(typeof(PhoneNumberValidation), false) && property.GetValue(misaEntity) != null)
                {
                    var propertyValue = property.GetValue(misaEntity);

                    if (!String.IsNullOrEmpty(propertyValue.ToString()))
                    {
                        bool isValidVietnamesePhoneNumber = Validator.VietNamesePhoneNumberValidation(propertyValue.ToString());
                        if (!isValidVietnamesePhoneNumber)
                        {
                            errorMessageArr.Add(new
                            {
                                fieldName = property.Name,
                                type = "invlaid",
                                devMsg = string.Format(resourceManager.GetString("InvalidFormatDev"), displayName, propertyValue, propertyValue.GetType().Name),
                                usrMsg = string.Format(resourceManager.GetString("InvalidFormatUsr"), displayName)
                            });
                            isValid = false;
                        }
                    }
                }
                // Kiểm tra độ dài của thuộc tính
                if (property.IsDefined(typeof(MaxLengthValidation), false) && property.GetValue(misaEntity) != null)
                {
                    var propertyValue = property.GetValue(misaEntity);
                    var propertyLength = propertyValue.ToString().Length;
                    if (propertyLength > maxLengthValue)
                    {
                        errorMessageArr.Add(new
                        {
                            fieldName = property.Name,
                            type = "invalidLength",
                            devMsg = string.Format(resourceManager.GetString("InvalidLengthDev"), displayName, propertyValue, propertyLength, maxLengthValue),
                            usrMsg = string.Format(resourceManager.GetString("InvalidLengthUsr"), displayName)
                        });
                        isValid = false;
                    }
                }
                // Kiểm tra số CMND/ Căn cước
                if (property.IsDefined(typeof(IdentityNumberValidation), false) && property.GetValue(misaEntity) != null)
                {
                    string propertyValue = property.GetValue(misaEntity).ToString();

                    if (!String.IsNullOrEmpty(propertyValue.ToString()))
                    {
                        if (!Validator.NumberValidation(propertyValue) || (propertyValue.Length != 9 && propertyValue.Length != 12))
                        {
                            errorMessageArr.Add(new
                            {
                                fieldName = property.Name,
                                type = "invalid",
                                devMsg = string.Format(resourceManager.GetString("InvalidFormatDev"), displayName, propertyValue, propertyValue.GetType().Name),
                                usrMsg = string.Format(resourceManager.GetString("InvalidFormatUsr"), displayName)
                            });
                            isValid = false;
                        }
                    }


                }
                // Kiểm tra số điện thoại cố định
                if (property.IsDefined(typeof(USPhoneNumberValidation), false) && property.GetValue(misaEntity) != null)
                {
                    string propertyValue = property.GetValue(misaEntity).ToString();
                    if (!String.IsNullOrEmpty(propertyValue.ToString()))
                    {
                        if (!Validator.USPhoneNumberValidation(propertyValue))
                        {
                            errorMessageArr.Add(new
                            {
                                fieldName = property.Name,
                                type = "invalid",
                                devMsg = string.Format(resourceManager.GetString("InvalidFormatDev"), displayName, propertyValue, propertyValue.GetType().Name),
                                usrMsg = string.Format(resourceManager.GetString("InvalidFormatUsr"), displayName)
                            });
                            isValid = false;
                        }
                    }

                }
                // Kiểm tra số dương
                if(property.IsDefined(typeof(PositiveValue), false))
                {
                    double propertyValue = ((IConvertible) property.GetValue(misaEntity)).ToDouble(null);
                    if(propertyValue < 0)
                    {
                        errorMessageArr.Add(new
                        {
                            fieldName = property.Name,
                            type = "invalid",
                            devMsg = string.Format(resourceManager.GetString("PositiveValueDev"), displayName, propertyValue, propertyValue.GetType().Name),
                            usrMsg = string.Format(resourceManager.GetString("PositiveValueUsr"), displayName)
                        });
                        isValid = false;
                    }
                }

                
            }
            _serviceResult.Message = errorMessageArr;
            if (isValid)
            {
                isValid = ValidateCustom(misaEntity, errorMessageArr);
            }
            
            return isValid;
        }
        /// <summary>
        /// Phương thức để các class kế thừa sẽ thực hiện
        /// </summary>
        /// <param name="misaEntity">Thực thể</param>
        /// Created By TBN (29/7/2021)
        /// <returns>Trạng thái đúng sai của dữ liệu</returns>
        protected virtual bool ValidateCustom(MISAEntity misaEntity, List<object> errorMessage)
        {
            return true;
        }
        #endregion
    }
}
