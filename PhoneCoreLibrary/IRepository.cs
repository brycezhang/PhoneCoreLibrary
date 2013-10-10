using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PhoneCoreLibrary
{
    /// <summary>
    /// 仓储泛型接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity>
    {
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="item">Item to add to repository</param>
        void Add(TEntity item);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="item">Item to delete</param>
        void Remove(TEntity item);

        /// <summary>
        /// 删除全部
        /// </summary>
        void RemoveAll();

        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="item">Item to modify</param>
        void Modify(TEntity item);

        /// <summary>
        /// 通过Id获取实体
        /// </summary>
        /// <param name="id">Entity key value</param>
        /// <returns></returns>
        TEntity Get<TKey>(TKey id);

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns>实体列表</returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <param name="pageIndex">页号</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns>实体列表</returns>
        IEnumerable<TEntity> GetAll(int pageIndex, int pageSize);

        /// <summary>
        /// 获取匹配的实体列表
        /// </summary>
        /// <param name="filter">Filter that each element do match</param>
        /// <returns>List of selected elements</returns>
        IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// 获取匹配的实体列表
        /// </summary>
        /// <param name="filter">Filter that each element do match</param>
        /// <param name="pageIndex">页号</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns>List of selected elements</returns>
        IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageSize);

        /// <summary>
        /// 提交更改
        /// </summary>
        void Save();
    }
}
