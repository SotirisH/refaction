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

        public ProductServices(RefactorDb db) : base(db)
        {

        }

        public async Task<ProductDto> CreateAsync(ProductDto product)
        {
            if (product.Id == Guid.Empty)
            {
                product.Id = Guid.NewGuid();
            }
            var efProduct = AutoMapper.Mapper.Map<Product>(product);
            DbContext.Products.Add(efProduct);
            await DbContext.SaveChangesAsync();
            var newP= await DbContext.Products.FindAsync(product.Id);
            return AutoMapper.Mapper.Map<ProductDto>(newP);
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            var productToDelete = await DbContext.Products.FindAsync(id);
            if (productToDelete == null)
            {
                return false;
            }
            DbContext.Products.Remove(productToDelete);
            await DbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var result = await DbContext.Products.ToArrayAsync();
            return AutoMapper.Mapper.Map<IEnumerable<ProductDto>>(result);
        }

        public async Task<ProductDto> GetByIdAsync(Guid id)
        {
            var result = await DbContext.Products.FindAsync(id);
            return AutoMapper.Mapper.Map<ProductDto>(result);
        }

        public async Task<IEnumerable<ProductDto>> GetByNameAsync(string name)
        {
            var result = await DbContext.Products.Where(x => x.Name == name).ToArrayAsync();
            return AutoMapper.Mapper.Map<IEnumerable<ProductDto>>(result);
        }

        public async Task<ProductDto> UpdateAsync(ProductDto product)
        {
            var efProduct = AutoMapper.Mapper.Map<Product>(product);
            if (!await DbContext.Products.AnyAsync(x => x.Id == efProduct.Id))
            {
                throw new EntityNotFoundException($"The Product with Id:{efProduct.Id} could not be found in order to update it");
            }
            DbContext.Products.Update(efProduct);
            await DbContext.SaveChangesAsync();
            // get the new values of the timestamp
            var t = await DbContext.Products.FindAsync(product.Id);
            return AutoMapper.Mapper.Map<ProductDto>(t);
        }
    }
}
