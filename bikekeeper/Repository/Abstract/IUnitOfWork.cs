using Biker_Keeper_Data;
using Biker_Keeper_Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bikekeeper.Repository.Abstract
{
    public interface IUnitOfWork
    {
        IBaseRepository<T> RepositoryCRUD<T>() where T : class, IEntityBase;
        void Commit();
        Task<int> CommitAsync();
    }
}
