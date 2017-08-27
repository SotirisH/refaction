using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xero.Refactor.Services;
using Xero.Refactor.Services.Exceptions;
using Xero.Refactor.WebApi.Modeling;

namespace Xero.Refactor.WebApi.Controllers
{
    /// <summary>
    /// API for Products
    /// </summary>
    [Produces("application/json")]
    [Route("api/products")]
    public class ProductsController : Controller
    {
        private readonly IProductServices _productServices;
        private readonly IProductOptionServices _productOptionServices;
        //private readonly ILinkGenerator _linkGenerator;

        public ProductsController(IProductServices productServices,
                                  IProductOptionServices productOptionServices)
        {
            _productServices = productServices;
            _productOptionServices = productOptionServices;
            //  _linkGenerator = linkGenerator;
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _productServices.GetAllAsync();
            return Ok(AutoMapper.Mapper.Map<IEnumerable<ProductApiModel>>(result));
        }

        /// <summary>
        /// Gets all products with a specific name
        /// </summary>
        /// <param name="name">the name of the product to be searched</param>
        /// <returns></returns>
        [Route("SearchByName")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductApiModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SearchByName(string name)
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

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductApiModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProductApiModel), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var result = await _productServices.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            var response = AutoMapper.Mapper.Map<ProductApiModel>(result);
            // _linkGenerator.PopulateLinksOnBasicVerbs(response, Url, "product", id);
            return Ok(response);
        }

        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>

        [HttpPost]
        [ProducesResponseType(typeof(ProductApiModel), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProductApiModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] ProductApiModel product)
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
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProductApiModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProductApiModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProductApiModel), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ProductApiModel), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Update(Guid id, ProductApiModel product)
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
                return StatusCode((int)HttpStatusCode.Conflict);
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
        [ProducesResponseType(typeof(ProductApiModel), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProductApiModel), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _productServices.DeleteByIdAsync(id);
            if (result)
            {
                // Return a response message with status code 204 (No Content)
                // To indicate that the operation was successful
                return new NoContentResult(); ;
            }
            else
            {
                return NotFound();
            }
        }

        [Route("{productId}/options")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductOptionApiModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(IEnumerable<ProductOptionApiModel>), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetOptions(Guid productId)
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
        [ProducesResponseType(typeof(ProductOptionApiModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProductOptionApiModel), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetOption(Guid productId, Guid id)
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
        [ProducesResponseType(typeof(ProductOptionApiModel), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ProductOptionApiModel), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateOption(Guid productId, ProductOptionApiModel option)
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
        [ProducesResponseType(typeof(ProductOptionApiModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ProductOptionApiModel), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ProductOptionApiModel), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(ProductOptionApiModel), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateOption(Guid id, ProductOptionApiModel option)
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
                return StatusCode((int)HttpStatusCode.Conflict);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        [ProducesResponseType(typeof(ProductOptionApiModel), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ProductOptionApiModel), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteOption(Guid id)
        {
            var result = await _productOptionServices.DeleteByIdAsync(id);
            if (result)
            {
                // Return a response message with status code 204 (No Content)
                // To indicate that the operation was successful
                return new NoContentResult();
            }
            else
            {
                return NotFound();
            }
        }
    }
}