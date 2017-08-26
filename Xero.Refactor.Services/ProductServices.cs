using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xero.AspNet.Core.Data;
using Xero.Refactor.Data;
using Xero.Refactor.Data.Models;
using Xero.Refactor.Services.Exceptions;

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
        /// <returns>Returns the newly created product</returns>
        Task<ProductDto> CreateAsync(ProductDto product);
        /// <summary>
        /// Updates an existing product
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Returns the updated product</returns>
        Task<ProductDto> UpdateAsync(ProductDto product);
        /// <summary>
        /// Deletes a product by id
        /// </summary>
        /// <param name="id">The id of the product to be deleted</param>
        /// <returns>True is success, false if the product cannot be found</returns>
        Task<bool> DeleteByIdAsync(Guid id);
    }

    public class ProductServices : DbServiceBase<RefactorDb>, IProductServices
    {
        private readonly IRepository<Product> _productRepository;
        public ProductServices(IUnitOfWork<RefactorDb> unitOfWork) : base(unitOfWork)
        {
            _productRepository = UoW.DbFactory.GetGenericRepositoryOf<Product>();
        }

        public async Task<ProductDto> CreateAsync(ProductDto product)
        {
            if (product.Id == Guid.Empty)
            {
                product.Id = Guid.NewGuid();
            }
            var efProduct = AutoMapper.Mapper.Map<Product>(product);
            _productRepository.Add(efProduct);
            await UoW.CommitAsync();
            return AutoMapper.Mapper.Map<ProductDto>(efProduct);
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            var productToDelete = await _productRepository.GetAsync(x => x.Id == id);
            if (productToDelete == null)
            {
                return false;
            }
            _productRepository.Delete(productToDelete);
            await UoW.CommitAsync();
            return true;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var result = await _productRepository.GetAllAsync();
            return AutoMapper.Mapper.Map<IEnumerable<ProductDto>>(result);
        }

        public async Task<ProductDto> GetByIdAsync(Guid id)
        {
            var result = await _productRepository.GetAsync(x => x.Id == id);
            return AutoMapper.Mapper.Map<ProductDto>(result);
        }

        public async Task<IEnumerable<ProductDto>> GetByNameAsync(string name)
        {
            var result = await _productRepository.GetAsync(x => x.Name == name);
            return AutoMapper.Mapper.Map<IEnumerable<ProductDto>>(result);
        }

        public async Task<ProductDto> UpdateAsync(ProductDto product)
        {
            var efProduct = AutoMapper.Mapper.Map<Product>(product);
            if (!await _productRepository.ExistsAsync(x => x.Id == efProduct.Id))
            {
                throw new EntityNotFoundException($"The Product with Id:{efProduct.Id} could not be found in order to update it");
            }
            _productRepository.Update(efProduct);
            await UoW.CommitAsync();
            return AutoMapper.Mapper.Map<ProductDto>(efProduct);
        }
    }
}
