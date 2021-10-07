using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.ApplicationCore;
using MISA.ApplicationCore.Const;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Interface;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Z.Dapper.Plus;

namespace MISA.Infrastructure.Repository
{
    public class BaseRepository<MISAEntity> : IBaseRepository<MISAEntity> where MISAEntity : BaseEntity
    {
        #region DECLARE
        protected IConfiguration _configuration;
        protected string _connectionString = string.Empty;
        protected IDbConnection _dbConnection = null;
        string _tableName;
        #endregion
        #region Constructor
        public BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration; // Appseting
            _connectionString = _configuration.GetConnectionString("LocalhostConnectionString"); // Chuỗi kết nối
            // Kết nối database
            _dbConnection = new MySqlConnection(_connectionString);
            // Lấy tên bảng trong database
            _tableName = typeof(MISAEntity).Name;
        }
        #endregion
        #region Methods
        /// <summary>
        /// Xóa dữ liệu theo tên định danh
        /// </summary>
        /// Created By TBN (26/7/2021)
        /// <param name="misaEntityId">Mã định danh</param>
        /// <returns>Số bản ghi bị ảnh hưởng sau khi xóa dữ liệu</returns>
        public virtual int DeleteMISAEntityById(Guid misaEntityId)
        {
            var rowAffects = 0;

            _dbConnection.Open();

            try
            {
                // Câu try vấn
                var sqlCommand = string.Format(StoredProcedure.DeleteSpecificEntityById, _tableName);   //$"Proc_Delete{_tableName}ById";
                                                                                                        // Tạo tham số đầu vào
                var parameters = new DynamicParameters();
                parameters.Add($"{_tableName}Id", misaEntityId, direction: ParameterDirection.Input);
                // Gửi câu truy vấn lên db và nhận lại kết quả
                rowAffects = _dbConnection.Execute(sqlCommand, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception)
            {
            }

            return rowAffects;
        }
        /// <summary>
        /// Lấy dữ liệu theo code
        /// </summary>
        /// <param name="misaEntitycode">Mã code</param>
        /// Created By TBN (18/8/2021)
        /// <returns>Dữ liệu tương ứng với mã code nếu có</returns>
        public virtual MISAEntity GetMISAEntityByCode(string misaEntitycode)
        {
            // Câu try vấn
            var sqlCommand = $"Proc_Get{_tableName}ByCode";
            var parameters = new DynamicParameters();
            parameters.Add($"{_tableName}Code", misaEntitycode, direction: ParameterDirection.Input);
            // Gửi câu truy vấn lên db và nhận lại kết quả
            var misaEntities = _dbConnection.Query<MISAEntity>(sqlCommand, parameters, commandType: CommandType.StoredProcedure);
            return misaEntities.FirstOrDefault();
        }
        /// <summary>
        /// Lấy dữ liệu theo tiêu chí cụ thể
        /// </summary>
        /// <param name="misaEntity"></param>
        /// <param name="property"></param>
        /// <returns>Dữ liệu theo tiêu chí</returns>
        public virtual MISAEntity GetEntityBySpesProperty(MISAEntity misaEntity, PropertyInfo property)
        {
            var propertyName = property.Name;
            var propertyValue = property.GetValue(misaEntity);
            var sqlCommand = string.Empty;
            var keyValue = misaEntity.GetType().GetProperty($"{_tableName}Id").GetValue(misaEntity);
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@PropertyValue", propertyValue);
            dynamicParameters.Add("@KeyValue", keyValue);
            string additionCondition = "";
            if(_tableName == "Employee")
            {
                additionCondition = " AND Status = 1";
            }
            if (property.IsDefined(typeof(CheckExist), false))
            {
                sqlCommand = $"SELECT * FROM {_tableName} WHERE {_tableName}Id = @PropertyValue";
            }
            else
            {
                // Nếu không phải trường kiểm tra tồn tại
                if (propertyValue.GetType() == typeof(string) || propertyValue.GetType() == typeof(Guid))
                {
                    if (misaEntity.EntityState == MISAEntityState.Insert)
                    {
                        sqlCommand = $"SELECT * FROM {_tableName} WHERE {propertyName} = @PropertyValue";
                    }
                    else if (misaEntity.EntityState == MISAEntityState.Update)
                    {
                        sqlCommand = $"SELECT * FROM {_tableName} WHERE {propertyName} = @PropertyValue AND {_tableName}Id <> @KeyValue";
                    }

                }
                else
                {
                    if (misaEntity.EntityState == MISAEntityState.Insert)
                    {
                        sqlCommand = $"SELECT * FROM {_tableName} WHERE {propertyName} = @PropertyValue";
                    }
                    else if (misaEntity.EntityState == MISAEntityState.Update)
                    {
                        sqlCommand = $"SELECT * FROM {_tableName} WHERE {propertyName} = @PropertyValue AND {_tableName}Id <> @KeyValue ";
                    }
                }
            }
            if (sqlCommand == "") return null;
            else
            {
                sqlCommand += additionCondition;
            }
            var checkEntity = _dbConnection.Query<MISAEntity>(sqlCommand, dynamicParameters, commandType: CommandType.Text);
            return checkEntity.FirstOrDefault();
        }

        /// <summary>
        /// Lấy toàn bộ dữ liệu
        /// </summary>
        /// Created By TBN (26/7/2021)
        /// <returns>Danh sách dữ liệu</returns>
        public virtual IEnumerable<MISAEntity> GetMISAEntities()
        {
            // Câu truy vấn
            var sqlCommand = string.Format(StoredProcedure.GetEntity, _tableName); //$"Proc_Get{_tableName}s";
            // Gửi truy vấn lên db và nhận lại kết quả
            var misaEntities = _dbConnection.Query<MISAEntity>(sqlCommand, commandType: CommandType.StoredProcedure);
            return misaEntities;
        }
        /// <summary>
        /// Lấy dữ liệu theo mã định danh
        /// </summary>
        /// Created By TBN (26/7/2021)
        /// <param name="misaEntityId">Mã định danh</param>
        /// <returns>Dữ liệu theo mã định danh</returns>
        public virtual MISAEntity GetMISAEntityById(Guid misaEntityId)
        {
            // Câu try vấn
            var sqlCommand = $"Proc_Get{_tableName}ById";
            var parameters = new DynamicParameters();
            parameters.Add($"{_tableName}Id", misaEntityId, direction: ParameterDirection.Input);
            // Gửi câu truy vấn lên db và nhận lại kết quả
            var misaEntities = _dbConnection.Query<MISAEntity>(sqlCommand, parameters, commandType: CommandType.StoredProcedure);
            return misaEntities.FirstOrDefault();
        }
        /// <summary>
        /// Thêm mới dữ liệu
        /// </summary>
        /// Created By TBN(26/7/2021)
        /// <param name="misaEntity">Dữ liệu thêm mới</param>
        /// <returns>Số bản ghi bị ảnh hưởng sau khi thêm mới</returns>
        public virtual int InsertMISAEntity(MISAEntity misaEntity)
        {
            var rowAffects = 0;
            _dbConnection.Open();
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    // Câu try vấn
                    var sqlCommand = string.Format(StoredProcedure.InsertEntity, _tableName);   //$"Proc_Insert{_tableName}";
                    rowAffects = _dbConnection.Execute(sqlCommand, misaEntity, transaction, commandType: CommandType.StoredProcedure);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
                
            }
            return rowAffects;
        }
        /// <summary>
        /// Cập nhật dữ liệu theo mã định danh
        /// </summary>
        /// Created By TBN (26/7/021)
        /// <param name="misaEntity">Dữ liệu cập nhật</param>
        /// <param name="misaEntityId">Mã định danh</param>
        /// <returns>Số bản ghi bị ảnh hưởng sau khi cập nhật</returns>
        public virtual int UpdateMISAEnityById(MISAEntity misaEntity, Guid misaEntityId)
        {
            var rowAffects = 0;
            _dbConnection.Open();
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    // Câu try vấn
                    var sqlCommand = string.Format(StoredProcedure.UpdateEntity, _tableName);    //$"Proc_Update{_tableName}";
                                                                                                 // Gửi câu truy vấn lên db và nhận lại kết quả
                    rowAffects = _dbConnection.Execute(sqlCommand, misaEntity,transaction, commandType: CommandType.StoredProcedure);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }

            return rowAffects;
        }
        public int DeleteMISAEntitiesById(List<Guid> listMISAEnitityIds)
        {
            var rowAffects = 0;
            _dbConnection.Open();
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    // Câu try vấn
                    var sqlCommand = string.Format(StoredProcedure.DeleteSpecificEntityById, _tableName);   //$"Proc_Delete{_tableName}ById";
                    foreach (Guid misaEntityId in listMISAEnitityIds)
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add($"{_tableName}Id", misaEntityId, direction: ParameterDirection.Input);
                        // Gửi câu truy vấn lên db và nhận lại kết quả
                        rowAffects += _dbConnection.Execute(sqlCommand, parameters, transaction, commandType: CommandType.StoredProcedure);
                    }
                    if (rowAffects < listMISAEnitityIds.Count)
                    {
                        transaction.Rollback();
                    }
                    else
                    {
                        transaction.Commit();
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();
                }
            }
            return rowAffects;
        }
        #endregion
    }
}
