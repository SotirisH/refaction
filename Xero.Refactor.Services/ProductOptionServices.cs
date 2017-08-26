using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xero.AspNet.Core.Data;
using Xero.Refactor.Data;

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
        Task<ProductOptionDto> GetById(Guid productId, Guid id);
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
        public ProductOptionServices(IUnitOfWork<RefactorDb> unitOfWork) : base(unitOfWork)
        {
        }

        public Task<ProductOptionDto> CreateAsync(ProductOptionDto productOption)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductOptionDto> GetById(Guid productId, Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductOptionDto>> GetByProductIdAsync(Guid productId)
        {
            throw new NotImplementedException();
        }

        public Task<ProductOptionDto> UpdateAsync(ProductOptionDto productOption)
        {
            throw new NotImplementedException();
        }
    }
}
