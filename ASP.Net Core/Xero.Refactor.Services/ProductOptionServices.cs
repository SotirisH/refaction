using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <param name="id">The product option Id</param>
        /// <returns></returns>
        Task<ProductOptionDto> GetByIdAsync(Guid id);
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
        private readonly IMapper _mapper;
        public ProductOptionServices(RefactorDb db,
                                    IMapper mapper) : base(db)
        {
            _mapper = mapper;
        }

        public async Task<ProductOptionDto> CreateAsync(ProductOptionDto productOption)
        {
            if (productOption.Id == Guid.Empty)
            {
                productOption.Id = Guid.NewGuid();
            }
            var efentity = _mapper.Map<ProductOption>(productOption);
            DbContext.Add(efentity);
            await DbContext.SaveChangesAsync();
            return _mapper.Map<ProductOptionDto>(efentity);
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {

            var entityToDelete = await DbContext.ProductOptions.FindAsync(id);
            if (entityToDelete == null)
            {
                return false;
            }
            DbContext.ProductOptions.Remove(entityToDelete);
            await DbContext.SaveChangesAsync();
            return true;
        }

        public async Task<ProductOptionDto> GetByIdAsync(Guid id)
        {
            var result = await DbContext.ProductOptions.FindAsync(id);
            return _mapper.Map<ProductOptionDto>(result);
        }

        public async Task<IEnumerable<ProductOptionDto>> GetByProductIdAsync(Guid productId)
        {
            var result = await DbContext.ProductOptions.Where(x => x.ProductId == productId).ToArrayAsync();
            return _mapper.Map<IEnumerable<ProductOptionDto>>(result);
        }

        public async Task<ProductOptionDto> UpdateAsync(ProductOptionDto productOption)
        {
            var entityToUpdate = _mapper.Map<ProductOption>(productOption);
            //Check if the entity exists
            if (!await DbContext.ProductOptions.AnyAsync(x => x.Id == productOption.Id))
            {
                throw new EntityNotFoundException($"The ProductOption with Id:{productOption.Id} could not be found in order to update it");
            }
            DbContext.ProductOptions.Update(entityToUpdate);
            await DbContext.SaveChangesAsync(); 
            return _mapper.Map<ProductOptionDto>(entityToUpdate);

        }
    }
}
