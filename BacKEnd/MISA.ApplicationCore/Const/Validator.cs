using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Const
{
    /// <summary>
    /// Lớp chứa phương thức cho việc kiểm tra định dạng dữ liệu
    /// Created By TBN (4/8/2021)
    /// </summary>
    public static class Validator
    {
        #region DECLARE
        /// <summary>
        /// Khai báo regex cho định dạng email
        /// Created By TBN (4/8/2021)
        /// </summary>
        static private Regex ValidEmailRegex = CreateValidEmailRegex();
        /// <summary>
        /// Khai báo regex cho định dạng kiểu số
        /// Created By TBN (4/8/2021)
        /// </summary>
        static private Regex ValidNumberRegex = CreateValidNumberRegex();
        /// <summary>
        /// Khai baso regex cho định dạng số điện thoại Việt Nam
        /// </summary>
        /// Created By TBN (4/8/2021)
        static private Regex ValidVietnamesePhoneNumberRegex = CreateValidVietnamesePhoneNumberRegex();
        /// <summary>
        /// Khai báo regex cho định dạng số điện thoại ở Mỹ
        /// </summary>
        static private Regex ValidateUSPhoneNumberRegrex = CreateValidUSPhoneNumberRegex();
        #endregion
        #region CreatedRegex
        /// <summary>
        /// Phương thức khởi tạo email regex
        /// </summary>
        /// Created By TBN (4/8/2021)
        /// <returns>Email Regex</returns>
        private static Regex CreateValidEmailRegex()
        {
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// Phương thức khởi tạo regex cho số
        /// </summary>
        /// Created By TBN (4/8/2021)
        /// <returns>Regex cho số</returns>
        private static Regex CreateValidNumberRegex()
        {
            string validNumberPattern = @"^\d+$";
            return new Regex(validNumberPattern);
        }
        /// <summary>
        /// Phương thức khởi tạo regex cho số điện thoại ở Việt Name
        /// </summary>
        /// Created By TBN (4/8/2021)
        /// <returns>Regex cho số điện thoại Việt Nam</returns>
        private static Regex CreateValidVietnamesePhoneNumberRegex()
        {
            string validVietnamesePhoneNumberPattern = "^(0)(3[2-9]{1}|5[689]{1}|7[06-9]{1}|8[0-689]{1}|9[0-46-9]{1})[0-9]{7}$";
            return new Regex(validVietnamesePhoneNumberPattern);
        }
        /// <summary>
        /// Phương thức khởi tạo regex cho số điện thoại ở US
        /// </summary>
        /// Created by TBN (18/8/2021)
        /// <returns>Trả về Regex tương ứng</returns>
        private static Regex CreateValidUSPhoneNumberRegex()
        {
            string validUSPhoneNumberPattern = @"\([0-9]{3}\) [0-9]{3}-[0-9]{4}$";
            return new Regex(validUSPhoneNumberPattern);
        }
        #endregion
        #region Methods
        /// <summary>
        /// Phương thức kiểm tra định dạng email
        /// </summary>
        /// Created by TBN (4/8/2021)
        /// <param name="email">Địa chỉ email cần kiểm tra</param>
        /// <returns>True= Đúng định dạng, False = Sai định dạng</returns>
        public static bool EmailValidation(string email)
        {
            return ValidEmailRegex.IsMatch(email);
        }
        /// <summary>
        /// Phương thức kiểm tra định dạng số
        /// </summary>
        /// Created By TBN (4/8/2021)
        /// <param name="number">Chuỗi cần kiểm tra</param>
        /// <returns>True= Chuỗi dạng số, False = Chuỗi không phải dạng số</returns>
        public static bool NumberValidation(string number)
        {
            return ValidNumberRegex.IsMatch(number);
        }
        /// <summary>
        /// Phương thức kiểm tra định dạng số điện thoại Việt Nam
        /// </summary>
        /// Created By TBN (4/8/2021)
        /// <param name="phoneNumber">Chuỗi số điện thoại cần kiểm tra</param>
        /// <returns>True= Đúng định dạng , False = Sai định dạng</returns>
        public static bool VietNamesePhoneNumberValidation(string phoneNumber)
        {
            return ValidVietnamesePhoneNumberRegex.IsMatch(phoneNumber);
        }
        /// <summary>
        /// Phương thức kiểm tra định dạng số điện thoại Mỹ
        /// </summary>
        /// Created By TBN (18/8/2021)
        /// <param name="phoneNumber">Số điện thoại cần kiểm tra</param>
        /// <returns>True/False</returns>
        public static bool USPhoneNumberValidation(string phoneNumber)
        {
            return ValidateUSPhoneNumberRegrex.IsMatch(phoneNumber);
        }
        #endregion
    }
}
