using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Xero.Refactor.Services;
using Xero.Refactor.Services.Exceptions;
using Xero.Refactor.WebApi.Modeling;

namespace Xero.Refactor.WebApi.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        private readonly IProductServices _productServices;
        private readonly IProductOptionServices _productOptionServices;

        public ProductsController(IProductServices productServices,
                                IProductOptionServices productOptionServices)
        {
            _productServices = productServices;
            _productOptionServices = productOptionServices;
        }

        [Route]
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            throw new Exception("Oh my god!");
           // var result = await _productServices.GetAllAsync();
            //return Ok(AutoMapper.Mapper.Map<IEnumerable<ProductApiModel>>(result));
        }

        [Route]
        [HttpGet]
        public async Task<IHttpActionResult> SearchByName(string name)
        {
            var result = await _productServices.GetByNameAsync(name);
            if (!result.Any())
            {
                return NotFound();
            }
            return Ok(AutoMapper.Mapper.Map<IEnumerable<ProductApiModel>>(result));
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetProduct(Guid id)
        {
            var result = await _productServices.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(AutoMapper.Mapper.Map<ProductApiModel>(result));
        }

        [Route]
        [HttpPost]
        public async Task<IHttpActionResult> Create(ProductApiModel product)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _productServices.CreateAsync(AutoMapper.Mapper.Map<ProductDto>(product));

            return CreatedAtRoute("DefaultApi", new { id = result.Id }, AutoMapper.Mapper.Map<ProductApiModel>(result));
        }

        [Route("{id}")]
        [HttpPut]
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