using Biker_Keeper_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace bikekeeper.Repository.Abstract
{
    public interface IBaseRepository<TEntity> where TEntity: class, IEntityBase
    {
        int Count();
        Task<int> CountAsync();
        bool Any(Expression<Func<TEntity, bool>> predicate);
        bool Exist(int id);
        List<TEntity> GetAll();
        TEntity GetByID(int id);
        TEntity GetSingle(Expression<Func<TEntity, bool>> predicate);
        TEntity GetSingleWithNoIsEnable(Expression<Func<TEntity, bool>> predicate);
        void Delete(TEntity entityToDelete);
        void Delete(int id);
        void DeleteWhere(Expression<Func<TEntity, bool>> predicate);
        void Insert(TEntity entity);
        void Update(TEntity entityToUpdate);
        void InsertAndUpdate(TEntity entity);
    }
}
