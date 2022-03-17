using bikekeeper.Repository.Abstract;
using Biker_Keeper_Data;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace bikekeeper.Repository
{
    public class BaseRepository<TEntity>: IBaseRepository <TEntity> where TEntity: class, IEntityBase
    {
        private BaseContext context;
        private DbSet<TEntity> dbSet;

        public BaseRepository(BaseContext Context)
        {
            context = Context;
            dbSet = Context.Set<TEntity>();
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            predicate = predicate.And(x => x.IsEnabled == true);
            return context.Set<TEntity>().Any(predicate);
        }

        public bool Exist(int id)
        {
            return context.Set<TEntity>().Any(x => x.Id == id);
        }

        public int Count()
        {
            return context.Set<TEntity>().Count(x => x.IsEnabled == true);
        }

        public Task<int> CountAsync()
        {
            return context.Set<TEntity>().CountAsync(x => x.IsEnabled == true);
        }

        public List<TEntity> GetAll()
        {
            return context.Set<TEntity>().Where(x => x.IsEnabled == true).ToList();
        }

        public TEntity GetByID(int id)
        {
            return context.Set<TEntity>().SingleOrDefault(x => x.Id == id && x.IsEnabled == true);
        }

        public TEntity GetSingle(Expression<Func<TEntity, bool>> predicate)
        {
            predicate = predicate.And(x => x.IsEnabled == true);
            return context.Set<TEntity>().FirstOrDefault(predicate);
        }

        public TEntity GetSingleWithNoIsEnable(Expression<Func<TEntity, bool>> predicate)
        {
            return context.Set<TEntity>().FirstOrDefault(predicate);
        }

        public virtual void Insert(TEntity entity)
        {
            entity.IsEnabled = true;
            dbSet.Add(entity);
        }

        public virtual void Delete(int id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityDelete)
        {
            if (context.Entry(entityDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityDelete);
            }
            dbSet.Remove(entityDelete);
        }

        public virtual void DeleteWhere(Expression<Func<TEntity, bool>> predicate)
        {
            IEnumerable<TEntity> entities = context.Set<TEntity>().Where(predicate);
            foreach (var entity in entities)
            {
                context.Entry<TEntity>(entity).State = EntityState.Deleted;
            }
        }

        public virtual void Update(TEntity entityUpdate)
        {
            entityUpdate.IsEnabled = true;
            dbSet.Attach(entityUpdate);
            context.Entry(entityUpdate).State = EntityState.Modified;
        }

        public void InsertAndUpdate(TEntity entity)
        {
            if (entity.Id == 0)
                Insert(entity);
            else
                Update(entity);
        }
    }
}
