using bikekeeper.Repository.Abstract;
using Biker_Keeper_Data;
using Biker_Keeper_Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bikekeeper.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private BaseContext baseContext;
        private Dictionary<string, object> repositoriesCRUD;

        public UnitOfWork(BaseContext BaseContext)
        {
            baseContext = BaseContext;
            repositoriesCRUD = new Dictionary<string, object>();
        }

        public IBaseRepository<T> RepositoryCRUD<T>() where T : class, IEntityBase
        {
            if (repositoriesCRUD == null)
            {
                repositoriesCRUD = new Dictionary<string, object>();
            }
            var type = typeof(T).Name;
            if (!repositoriesCRUD.ContainsKey(type))
            {
                var repositoryType = typeof(BaseRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), baseContext);
                repositoriesCRUD.Add(type, repositoryInstance);
            }
            return (BaseRepository<T>)repositoriesCRUD[type];
        }

        public void Commit()
        {
            baseContext.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await baseContext.SaveChangesAsync();
        }
    }
}
