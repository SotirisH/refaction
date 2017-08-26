﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using System.Web.Http;
using Xero.Refactor.Services;
using Xero.Refactor.Services.Exceptions;
using Xero.Refactor.WebApi.Modeling;

namespace refactor_me.Controllers
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
            var result = await _productServices.GetAllAsync();
            return Ok(AutoMapper.Mapper.Map<IEnumerable<ProductApiModel>>(result));
        }

        [Route]
        [HttpGet]
        public async Task<IHttpActionResult> SearchByName(string name)
        {
            var result = await _productServices.GetByNameAsync(name);
            if (result == null)
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

            return CreatedAtRoute("DefaultApi", new { id = result.Id }, result);
        }

        [Route("{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> Update(Guid id, ProductApiModel product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
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
            try
            {
                var result = await _productServices.DeleteByIdAsync(id);
                return Ok();
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

        //[Route("{productId}/options")]
        //[HttpGet]
        //public ProductOptions GetOptions(Guid productId)
        //{
        //    return new ProductOptions(productId);
        //}

        //[Route("{productId}/options/{id}")]
        //[HttpGet]
        //public ProductOption GetOption(Guid productId, Guid id)
        //{
        //    var option = new ProductOption(id);
        //    if (option.IsNew)
        //        throw new HttpResponseException(HttpStatusCode.NotFound);

        //    return option;
        //}

        //[Route("{productId}/options")]
        //[HttpPost]
        //public void CreateOption(Guid productId, ProductOption option)
        //{
        //    option.ProductId = productId;
        //    option.Save();
        //}

        //[Route("{productId}/options/{id}")]
        //[HttpPut]
        //public void UpdateOption(Guid id, ProductOption option)
        //{
        //    var orig = new ProductOption(id)
        //    {
        //        Name = option.Name,
        //        Description = option.Description
        //    };

        //    if (!orig.IsNew)
        //        orig.Save();
        //}

        //[Route("{productId}/options/{id}")]
        //[HttpDelete]
        //public void DeleteOption(Guid id)
        //{
        //    var opt = new ProductOption(id);
        //    opt.Delete();
        //}
    }
}
