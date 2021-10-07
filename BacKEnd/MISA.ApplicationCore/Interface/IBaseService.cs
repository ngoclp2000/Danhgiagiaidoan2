using MISA.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.ApplicationCore.Interface
{
    public interface IBaseService<MISAEntity>
    {
        #region Methods
        /// <summary>s
        /// Lây toàn bộ dữ liệu
        /// Create By TBN (26/7/2021)
        /// </summary>
        /// <returns>Danh sách dữ liệu</returns>
        IEnumerable<MISAEntity> GetMISAEntities();
        /// <summary>
        /// Lấy dữ liệu theo mã định danh
        /// </summary>
        /// Create By TBN (26/7/2021)
        /// <param name="misaEntityId"> Mã định danh của thực thể</param>
        /// <returns>Danh sách dữ liệu theo mã định danh</returns>
        MISAEntity GetMISAEntityById(Guid misaEntityId);
        /// <summary>
        /// Lấy dữ liệu theo code
        /// </summary>
        /// <param name="misaEntitycode">Mã code</param>
        /// Created By TBN (19/8/2021)
        /// <returns>Dữ liệu tương ứng với mã code nếu có</returns>
        MISAEntity GetMISAEntityByCode(string misaEntitycode);
        /// <summary>
        /// Xóa dữ liệu theo mã định danh
        /// </summary>
        /// Create By TBN (26/7/2021)
        /// <param name="misaEntityId">Mã định danh của thực thể</param>
        /// <returns>Thông báo kết quả yêu cầu</returns>
        ServiceResult DeleteMISAEntityById(Guid misaEntityId);
        /// <summary>
        /// Cập nhật dữ liệu theo mã định danh
        /// </summary>
        /// Create By TBN (26/7/2021)
        /// <param name="misaEntity"></param>
        /// <param name="misaEntityId"></param>
        /// <returns>Thông báo kết quả yêu cầu</returns>
        ServiceResult UpdateMISAEntityById(MISAEntity misaEntity, Guid misaEntityId);
        /// <summary>
        /// Thêm mới dữ liệu
        /// </summary>
        /// Create by TBN (26/7/2021)
        /// <param name="misaEntity">Mã định danh của thực thể</param>
        /// <returns>Thông báo kết quả yêu cầu</returns>
        ServiceResult InsertMISAEntity(MISAEntity misaEntity);
        /// <summary>
        /// Xóa nhiều dữ liệu theo mã định danh
        /// </summary>
        /// Created By TBN (10/8/2021)
        /// <param name="listMISAEnitityIds">Danh sách mã định danh</param>
        /// <returns>Trạng thái của yêu cầu</returns>
        ServiceResult DeleteMISAEntitiesById(List<Guid> listMISAEnitityIds);
        #endregion
    }
}
