using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xero.AspNet.Core.Data;
using Xero.Refactor.Data;

namespace Xero.Refactor.Services
{
    public interface IProductServices
    {
        /// <summary>
        /// Retrieves all the products, ordered by name
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ProductDto>> GetAllAsync();
        /// <summary>
        /// Retrieves all the products with a specific name
        /// </summary>
        /// <param name="name">The name of the product to be searched</param>
        /// <returns>An empty list is returned if there are no products match the given name</returns>
        Task<IEnumerable<ProductDto>> GetByNameAsync(string name);
        /// <summary>
        /// Gets a product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Null if the product cannot be found</returns>
        Task<ProductDto> GetByIdAsync(Guid id);
        /// <summary>
        /// Creates an new Product
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Returs the newly created product</returns>
        Task<ProductDto> CreateAsync(ProductDto product);
        /// <summary>
        /// Updates an existing product
        /// </summary>
        /// <param name="product"></param>
        /// <returns>The updated product</returns>
        Task<ProductDto> UpdateAsync(ProductDto product);
        /// <summary>
        /// Deletes a product by id
        /// </summary>
        /// <param name="id">The id of the product to be deleted</param>
        /// <returns>True is success, false if the product cannot be found</returns>
        Task<bool> DeleteByIdAsync(Guid id);
    }

    public class ProductServices: DbServiceBase<RefactorDb>, IProductServices
    {

        public ProductServices(IUnitOfWork<RefactorDb> unitOfWork):base(unitOfWork)
        {
        }

        public Task<ProductDto> CreateAsync(ProductDto product)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ProductDto> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductDto>> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<ProductDto> UpdateAsync(ProductDto product)
        {
            throw new NotImplementedException();
        }
    }
}
