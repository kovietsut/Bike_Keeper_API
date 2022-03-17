using AutoMapper;
using bikekeeper.Repository.Abstract;
using bikekeeper.Services.Abstract;
using Biker_Keeper_Data;
using Infrastructure;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace bikekeeper.Services
{
    public class GeneralService<TCreateViewModel, TModel> : IGeneralService<TCreateViewModel, TModel>
                                                                            where TCreateViewModel : class, IEntityBase
                                                                            where TModel : class, IEntityBase
    {
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IMapper mapper;

        public GeneralService(IUnitOfWork UnitOfWork, IMapper Mapper)
        {
            unitOfWork = UnitOfWork;
            mapper = Mapper;
        }

        public virtual ResponseModel Get(int id)
        {
            var entity = unitOfWork.RepositoryCRUD<TModel>().GetByID(id);

            if (entity != null)
                return ResponseModel.CreateSuccess(mapper.Map<TCreateViewModel>(entity));

            return ResponseModel.CreateSuccess();
        }
        // To Get All Records Without Paging
        public virtual ResponseModel GetAll()
        {
            var entity = unitOfWork.RepositoryCRUD<TModel>().GetAll();

            if (entity != null)
                return ResponseModel.CreateSuccess(entity);

            return ResponseModel.CreateError();
        }
        // To Get All Records With Paging
        public virtual ResponseModel GetAllPaging(int? pageNumber, string searchText, int? pageSize)
        {
            // Count the size of a page
            var totalRecords = unitOfWork.RepositoryCRUD<TModel>().Count();
            var pagingData = unitOfWork.RepositoryCRUD<TModel>().GetAll()
                .Skip(((int)pageNumber - 1) * (int)pageSize)
                .Take((int)pageSize);
            // Search Text           
            return ResponseModel.CreateSuccess(pagingData, "Successful", totalRecords);
        }

        public virtual async Task<ResponseModel> Create(TCreateViewModel viewModel)
        {
            try
            {
                TModel model = mapper.Map<TModel>(viewModel);
                unitOfWork.RepositoryCRUD<TModel>().Insert(model);
                await unitOfWork.CommitAsync();
                return ResponseModel.CreateSuccess(mapper.Map<TModel>(viewModel));
            }
            catch (Exception ex)
            {
                return ResponseModel.CreateError(ex.Message);
            }
        }

        public virtual async Task<ResponseModel> Update(TCreateViewModel viewModel)
        {
            try
            {
                TModel model = unitOfWork.RepositoryCRUD<TModel>().GetByID(viewModel.Id);
                model = mapper.Map(viewModel, model);
                unitOfWork.RepositoryCRUD<TModel>().Update(model);
                await unitOfWork.CommitAsync();
                return ResponseModel.CreateSuccess(mapper.Map<TModel>(viewModel));
            }
            catch(Exception ex)
            {
                return ResponseModel.CreateError(ex.Message);
            }
        }

        public virtual async Task<ResponseModel> Delete(TCreateViewModel viewModel)
        {
            var entity = unitOfWork.RepositoryCRUD<TModel>().GetByID(viewModel.Id);
            if (entity != null)
            {
                try
                {
                    unitOfWork.RepositoryCRUD<TModel>().Update(entity);
                    entity.IsEnabled = false;
                    await unitOfWork.CommitAsync();
                    return ResponseModel.CreateSuccess(viewModel);
                }
                catch (Exception ex)
                {
                    return ResponseModel.CreateError(ex.Message);
                }
            }
            return ResponseModel.CreateError(MessageUtils.General.NotExist);
        }

    }
}