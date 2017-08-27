using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Xero.Refactor.Services;
using Xero.Refactor.Services.Exceptions;
using Xero.Refactor.WebApi.Hypermedia;
using Xero.Refactor.WebApi.Modeling;
namespace Xero.Refactor.WebApi.Controllers
{
    /// <summary>
    /// API for Products
    /// </summary>
    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        private readonly IProductServices _productServices;
        private readonly IProductOptionServices _productOptionServices;
        private readonly ILinkGenerator _linkGenerator;

        public ProductsController(IProductServices productServices,
                                  IProductOptionServices productOptionServices,
                                  ILinkGenerator linkGenerator)
        {
            _productServices = productServices;
            _productOptionServices = productOptionServices;
            _linkGenerator = linkGenerator;
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <returns></returns>
        /// 
        [Route]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<ProductApiModel>))]
        public async Task<IHttpActionResult> GetAll()
        {
            var result = await _productServices.GetAllAsync();
            return Ok(AutoMapper.Mapper.Map<IEnumerable<ProductApiModel>>(result));
        }

        /// <summary>
        /// Gets all products with a specific name
        /// </summary>
        /// <param name="name">the name of the product to be searched</param>
        /// <returns></returns>
        [Route]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<ProductApiModel>))]
        public async Task<IHttpActionResult> SearchByName(string name)
        {
            var result = await _productServices.GetByNameAsync(name);
            if (!result.Any())
            {
                return NotFound();
            }
            var response = AutoMapper.Mapper.Map<IEnumerable<ProductApiModel>>(result);
            return Ok(response);
        }

        /// <summary>
        /// Gets a specific product by its id
        /// </summary>
        /// <param name="id">The id of the product</param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpGet]
        [ResponseType(typeof(ProductApiModel))]
        public async Task<IHttpActionResult> GetProduct(Guid id)
        {
            var result = await _productServices.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            var response = AutoMapper.Mapper.Map<ProductApiModel>(result);
            _linkGenerator.PopulateLinksOnBasicVerbs(response, Url, "product", id);
            return Ok(response);
        }

        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [Route]
        [HttpPost]
        [ResponseType(typeof(ProductApiModel))]
        public async Task<IHttpActionResult> Create(ProductApiModel product)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productServices.CreateAsync(AutoMapper.Mapper.Map<ProductDto>(product));

            return CreatedAtRoute("DefaultApi", new { id = result.Id }, AutoMapper.Mapper.Map<ProductApiModel>(result));
        }
        /// <summary>
        /// Updates an existing product
        /// </summary>
        /// <param name="id">The id of the product</param>
        /// <param name="product">The new values</param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpPut]
        [ResponseType(typeof(ProductApiModel))]
        public async Task<IHttpActionResult> Update(Guid id, ProductApiModel product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id == Guid.Empty)
            {
                return BadRequest("The id cannot be empty!");
            }

            product.Id = id;
            try
            {
                var result = await _productServices.UpdateAsync(AutoMapper.Mapper.Map<ProductDto>(product));
                return Ok(AutoMapper.Mapper.Map<ProductApiModel>(result));
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict();
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Deletes a product
        /// </summary>
        /// <param name="id">The id of the product that we want to delete</param>
        /// <returns></returns>
        [Route("{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            var result = await _productServices.DeleteByIdAsync(id);
            if (result)
            {
                // Return a response message with status code 204 (No Content)
                // To indicate that the operation was successful
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return NotFound();
            }
        }

        [Route("{productId}/options")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<ProductOptionApiModel>))]
        public async Task<IHttpActionResult> GetOptions(Guid productId)
        {
            var result = await _productOptionServices.GetByProductIdAsync(productId);
            if (!result.Any())
            {
                return NotFound();
            }
            return Ok(AutoMapper.Mapper.Map<IEnumerable<ProductOptionApiModel>>(result));
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        [ResponseType(typeof(ProductOptionApiModel))]
        public async Task<IHttpActionResult> GetOption(Guid productId, Guid id)
        {
            var result = await _productOptionServices.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(AutoMapper.Mapper.Map<ProductOptionApiModel>(result));
        }

        [Route("{productId}/options")]
        [HttpPost]
        [ResponseType(typeof(ProductOptionApiModel))]
        public async Task<IHttpActionResult> CreateOption(Guid productId, ProductOptionApiModel option)
        {
            option.ProductId = productId;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productOptionServices.CreateAsync(AutoMapper.Mapper.Map<ProductOptionDto>(option));

            return CreatedAtRoute("DefaultApi", new { id = result.Id }, AutoMapper.Mapper.Map<ProductOptionApiModel>(result));
        }

        [Route("{productId}/options/{id}")]
        [HttpPut]
        [ResponseType(typeof(ProductOptionApiModel))]
        public async Task<IHttpActionResult> UpdateOption(Guid id, ProductOptionApiModel option)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id == Guid.Empty)
            {
                return BadRequest("The id cannot be empty!");
            }

            option.Id = id;
            try
            {
                var result = await _productOptionServices.UpdateAsync(AutoMapper.Mapper.Map<ProductOptionDto>(option));
                return Ok(AutoMapper.Mapper.Map<ProductOptionApiModel>(result));
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteOption(Guid id)
        {
            var result = await _productOptionServices.DeleteByIdAsync(id);
            if (result)
            {
                // Return a response message with status code 204 (No Content)
                // To indicate that the operation was successful
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return NotFound();
            }
        }
    }
}