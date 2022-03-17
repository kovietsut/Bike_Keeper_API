using bikekeeper.Repository.Abstract;
using bikekeeper.Services.Abstract;
using Biker_Keeper_Data.Entity;
using Biker_Keeper_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bikekeeper.Services
{
    public class PageService: IPageService
    {
        protected readonly IUnitOfWork unitOfWork;

        public PageService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        
    }
}
