using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xero.AspNet.Core.Data;
using Xero.Refactor.Data;
using Xero.Refactor.Data.Models;
using Xero.Refactor.Services.Exceptions;

namespace Xero.Refactor.Services
{
    public interface IProductOptionServices
    {
        /// <summary>
        /// Retrieves all the product options of a specific product
        /// </summary>
        /// <param name="productId">The id of the product</param>
        /// <returns>An empty list is returned if there are no productsOptions match the given productId</returns>
        Task<IEnumerable<ProductOptionDto>> GetByProductIdAsync(Guid productId);
        /// <summary>
        /// Gets a single product option given the productId & id
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="id">Teh product option Id</param>
        /// <returns></returns>
        Task<ProductOptionDto> GetByIdAsync(Guid productId, Guid id);
        /// <summary>
        /// Creates an new productOption option
        /// </summary>
        /// <param name="productOption"></param>
        /// <returns>Returs the newly created productOption</returns>
        Task<ProductOptionDto> CreateAsync(ProductOptionDto productOption);
        /// <summary>
        /// Updates an existing ProductOption
        /// </summary>
        /// <param name="productOption"></param>
        /// <returns>The updated productOption</returns>
        Task<ProductOptionDto> UpdateAsync(ProductOptionDto productOption);
        /// <summary>
        /// Deletes a ProductOption by id
        /// </summary>
        /// <param name="id">The id of the ProductOption to be deleted</param>
        /// <returns>True is success, false if the ProductOption cannot be found</returns>
        Task<bool> DeleteByIdAsync(Guid id);
    }

    public class ProductOptionServices : DbServiceBase<RefactorDb>, IProductOptionServices
    {
        private readonly IRepository<ProductOption> _productOptionRepository;
        public ProductOptionServices(IUnitOfWork<RefactorDb> unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<ProductOptionDto> CreateAsync(ProductOptionDto productOption)
        {
            if (productOption.Id == Guid.Empty)
            {
                productOption.Id = Guid.NewGuid();
            }
            var efentity = AutoMapper.Mapper.Map<ProductOption>(productOption);
            _productOptionRepository.Add(efentity);
            await UoW.CommitAsync();
            return AutoMapper.Mapper.Map<ProductOptionDto>(efentity);
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            var entityToDelete = await _productOptionRepository.GetAsync(x => x.Id == id);
            if (entityToDelete == null)
            {
                return false;
            }
            _productOptionRepository.Delete(entityToDelete);
            await UoW.CommitAsync();
            return true;
        }

        public async Task<ProductOptionDto> GetByIdAsync(Guid productId, Guid id)
        {
            var result = await _productOptionRepository.GetAsync(x => x.Id == id && x.ProductId == productId);
            return AutoMapper.Mapper.Map<ProductOptionDto>(result);
        }

        public async Task<IEnumerable<ProductOptionDto>> GetByProductIdAsync(Guid productId)
        {
            var result = await _productOptionRepository.GetManyAsync(x => x.ProductId == productId);
            return AutoMapper.Mapper.Map<IEnumerable<ProductOptionDto>>(result);
        }

        public async Task<ProductOptionDto> UpdateAsync(ProductOptionDto productOption)
        {
            var entityToUpdate = AutoMapper.Mapper.Map<ProductOption>(productOption);
            //Check if the entity exists
            if (!await _productOptionRepository.ExistsAsync(x => x.Id == productOption.Id))
            {
                throw new EntityNotFoundException($"The ProductOption with Id:{productOption.Id} could not be found in order to update it");
            }
            _productOptionRepository.Update(entityToUpdate);
            await UoW.CommitAsync();
            return AutoMapper.Mapper.Map<ProductOptionDto>(entityToUpdate);

        }
    }
}
