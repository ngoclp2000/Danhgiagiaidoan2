using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Entities
{
    public class ServiceResult
    {
        /// <summary>
        /// Dữ liệu trả về
        /// </summary>
        /// Created By TBN (29/7/2021)
        public object Data { get; set; }
        /// <summary>
        /// Tin nhắn trả về
        /// </summary>
        /// Created By TBN (29/7/2021)
        public object Message { get; set; }
        /// <summary>
        /// Mã code
        /// </summary>
        /// Created By TBN (29/7/2021)
        public MISACode? MISACode { get; set; }
    }
}
