using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Entities
{
    /// <summary>
    /// Thuộc tính kiểm tra số dương
    /// </summary>
    /// Created By TBN (29/7/2021)
    [AttributeUsage(AttributeTargets.Property)]
    public class PositiveValue : Attribute
    {

    }
    /// <summary>
    /// Thuộc tính kiểm tra trùng lặp
    /// </summary>
    /// Created By TBN (29/7/2021)
    [AttributeUsage(AttributeTargets.Property)]
    public class CheckExist : Attribute
    {

    }
    /// <summary>
    /// Thuộc tính yêu cầu nhập
    /// </summary>
    /// Created By TBN (29/7/2021)
    [AttributeUsage(AttributeTargets.Property)]
    public class Required:Attribute
    {

    }
    /// <summary>
    /// Thuộc tính kiểm tra trùng lặp
    /// </summary>
    /// Created By TBN (29/7/2021)
    [AttributeUsage(AttributeTargets.Property)]
    public class CheckDuplication:Attribute
    {

    }
    /// <summary>
    /// Thuộc tính khóa chính
    /// </summary>
    /// Created By TBN (29/7/2021)
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKey : Attribute
    {

    }
    /// <summary>
    /// Thuộc tính kiểm tra định dạng email
    /// </summary>
    /// Created By TBN (29/7/2021)
    [AttributeUsage(AttributeTargets.Property)]
    public class EmailValidation : Attribute
    {

    }
    /// <summary>
    /// Thuộc tính kiểm tra định dạng số điện thoại
    /// </summary>
    /// Created By TBN (29/7/2021)
    [AttributeUsage(AttributeTargets.Property)]
    public class PhoneNumberValidation : Attribute
    {

    }
    /// <summary>
    /// Thuộc tính kiểm tra định dạng sô điện thoại Mỹ
    /// </summary>
    /// Created By TBN (18/8/2021)
    [AttributeUsage(AttributeTargets.Property)]
    public class USPhoneNumberValidation : Attribute
    {

    }
    /// <summary>
    /// Thuộc tính kiểm tra độ dài 
    /// </summary>
    /// Created By TBN(29/7/2021)
    [AttributeUsage(AttributeTargets.Property)]
    public class MaxLengthValidation : Attribute
    {
        public int MaxLength = 0;
        public MaxLengthValidation(int maxLength)
        {
            this.MaxLength = maxLength;
        }
    }
    [AttributeUsage(AttributeTargets.Property)]
    public class IdentityNumberValidation: Attribute
    {

    }
    /// <summary>
    /// Thuộc tính chung
    /// </summary>
    /// Created By TBN (29/7/2021)
    public class BaseEntity
    {
        /// <summary>
        /// Trạng thái của thuộc tính
        /// </summary>
        /// Created By TBN (29/7/2021)
        public MISAEntityState EntityState { get; set; } = MISAEntityState.Insert;
        /// <summary>
        /// Ngày tạo
        /// </summary>
        /// Created By TBN (29/7/2021)
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Người tạo
        /// </summary>
        /// Created By TBN (29/7/2021)
        [MaxLengthValidation(100)]
        [DisplayName("Người tạo")]
        public string CreatedBy { get; set; }
        /// <summary>
        /// Ngày chỉnh sửa 
        /// </summary>
        /// Created By TBN (29/7/2021)
        public DateTime? ModifiedDate { get; set; }
        /// <summary>
        /// Người chỉnh sửa
        /// </summary>
        /// Created By TBN (29/7/2021)
        [MaxLengthValidation(100)]
        [DisplayName("Người chỉnh sửa")]
        public string ModifiedBy { get; set; }
    }
}
