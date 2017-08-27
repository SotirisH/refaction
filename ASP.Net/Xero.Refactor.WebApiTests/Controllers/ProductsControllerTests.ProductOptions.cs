using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Xero.Refactor.Services;
using Xero.Refactor.Services.Exceptions;
using Xero.Refactor.Test.Common;
using Xero.Refactor.WebApi.Modeling;

namespace Xero.Refactor.WebApiTests
{
    /// <summary>
    /// Partial class that contains tests for the Product options realted actions
    /// </summary>
    public partial class ProductsControllerTests
    {
        [TestMethod()]
        public async Task ProductOption_GetOptions_success()
        {
            var mockDto = FixturesDto.ProductOptionBuildOne(null);
            var retMock = (new List<ProductOptionDto>() { mockDto }).AsEnumerable();


            mockIProductOptionServices.Setup(m => m.GetByProductIdAsync(mockDto.ProductId)).Returns(Task.FromResult(retMock));
            var result = await target.GetOptions(mockDto.ProductId) as OkNegotiatedContentResult<IEnumerable<ProductOptionApiModel>>;
            AssertX.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<IEnumerable<ProductOptionApiModel>>));
            Assert.AreEqual(retMock.First().ProductId, result.Content.First().Id);
            Assert.AreEqual(retMock.First().Name, result.Content.First().Name);
            Assert.AreEqual(retMock.First().Description, result.Content.First().Description);

        }

        [TestMethod()]
        public async Task ProductOption_GetOptions_NotFound()
        {
            var retMock = (new List<ProductOptionDto>()).AsEnumerable();
            mockIProductOptionServices.Setup(m => m.GetByProductIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(retMock));
            var result = await target.GetOptions(Guid.Empty) as NotFoundResult;
            AssertX.IsInstanceOfType(result, typeof(NotFoundResult));

        }

        [TestMethod()]
        public async Task ProductOption_GetOption_success()
        {
            var mockDto = FixturesDto.ProductOptionBuildOne(null);
            mockDto.Id = Guid.NewGuid();
            mockIProductOptionServices.Setup(m => m.GetByIdAsync(mockDto.Id)).Returns(Task.FromResult(mockDto));

            var result = await target.GetOption(mockDto.ProductId, mockDto.Id) as OkNegotiatedContentResult<ProductOptionApiModel>;
            AssertX.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<ProductOptionApiModel>));
            Assert.AreEqual(mockDto.ProductId, result.Content.ProductId);
            Assert.AreEqual(mockDto.Name, result.Content.Name);
            Assert.AreEqual(mockDto.Description, result.Content.Description);

        }

        [TestMethod()]
        public async Task ProductOption_GetOption_NotFound()
        {
            var mockDto = FixturesDto.ProductOptionBuildOne(null);
            mockDto.Id = Guid.NewGuid();
            mockIProductOptionServices.Setup(m => m.GetByIdAsync(mockDto.Id)).Returns(Task.FromResult(mockDto));

            var result = await target.GetOption(mockDto.ProductId, Guid.NewGuid()) as NotFoundResult;
            AssertX.IsInstanceOfType(result, typeof(NotFoundResult));

        }

        [TestMethod()]
        public async Task ProductOption_CreateOption_Ok()
        {
            var mockDto = FixturesDto.ProductOptionBuildOne(null);
            var mockApi = AutoMapper.Mapper.Map<ProductOptionApiModel>(mockDto);

            mockIProductOptionServices.Setup(m => m.CreateAsync(It.IsAny<ProductOptionDto>())).Returns(Task.FromResult(mockDto));

            var result = await target.CreateOption(mockDto.ProductId, mockApi) as CreatedAtRouteNegotiatedContentResult<ProductOptionApiModel>;
            AssertX.IsInstanceOfType(result, typeof(CreatedAtRouteNegotiatedContentResult<ProductOptionApiModel>));
            Assert.AreEqual(mockApi.ProductId, result.Content.ProductId);
            Assert.AreEqual(mockApi.Name, result.Content.Name);
            Assert.AreEqual(mockApi.Description, result.Content.Description);

        }

        [TestMethod()]
        public async Task ProductOption_CreateOption_InvalidModel()
        {
            var mockDto = FixturesDto.ProductOptionBuildOne(null);
            var mockApi = AutoMapper.Mapper.Map<ProductOptionApiModel>(mockDto);

            mockIProductOptionServices.Setup(m => m.CreateAsync(It.IsAny<ProductOptionDto>())).Returns(Task.FromResult(mockDto));
            target.ModelState.AddModelError("Name", "No Value");
            var result = await target.CreateOption(mockDto.ProductId, mockApi) as InvalidModelStateResult;
            AssertX.IsInstanceOfType(result, typeof(InvalidModelStateResult));
            CollectionAssert.Contains(result.ModelState.Keys.ToArray(), "Name");
        }

        [TestMethod()]
        public async Task ProductOption_Delete_success()
        {
            mockIProductOptionServices.Setup(m => m.DeleteByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));

            var result = await target.DeleteOption(Guid.NewGuid()) as StatusCodeResult;
            AssertX.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);

        }

        [TestMethod()]
        public async Task ProductOption_Delete_NotFound()
        {
            mockIProductOptionServices.Setup(m => m.DeleteByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(false));

            var result = await target.DeleteOption(Guid.NewGuid()) as NotFoundResult;
            AssertX.IsInstanceOfType(result, typeof(NotFoundResult));
        }


        [TestMethod()]
        public async Task ProductOption_Update_BadRequest_scenario()
        {
            var mockDto = FixturesDto.ProductOptionBuildOne(null);
            mockDto.Id = Guid.NewGuid();
            var mockApi = AutoMapper.Mapper.Map<ProductOptionApiModel>(mockDto);

            mockIProductOptionServices.Setup(m => m.UpdateAsync(It.IsAny<ProductOptionDto>())).Returns(Task.FromResult(mockDto));

            target.ModelState.AddModelError("Name", "Name is not present!");
            IHttpActionResult result = await target.UpdateOption(mockApi.Id, mockApi) as InvalidModelStateResult;
            AssertX.IsInstanceOfType(result, typeof(InvalidModelStateResult));
            CollectionAssert.Contains(((InvalidModelStateResult)result).ModelState.Keys.ToArray(), "Name");

            target.ModelState.Clear();
            result = await target.UpdateOption(Guid.Empty, mockApi) as BadRequestErrorMessageResult;
            AssertX.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));

            mockIProductOptionServices.Setup(m => m.UpdateAsync(It.IsAny<ProductOptionDto>())).Throws(new EntityNotFoundException("Not found!"));
            result = await target.UpdateOption(mockApi.Id, mockApi) as NotFoundResult;
            AssertX.IsInstanceOfType(result, typeof(NotFoundResult));

            mockIProductOptionServices.Setup(m => m.UpdateAsync(It.IsAny<ProductOptionDto>())).Throws(new DbUpdateConcurrencyException());
            result = await target.UpdateOption(mockApi.Id, mockApi) as ConflictResult;
            AssertX.IsInstanceOfType(result, typeof(ConflictResult));

        }


        [TestMethod()]
        public async Task ProductOption_Update_success()
        {
            var mockDto = FixturesDto.ProductOptionBuildOne(null);
            mockDto.Id = Guid.NewGuid();
            var mockApi = AutoMapper.Mapper.Map<ProductOptionApiModel>(mockDto);
            mockApi.Name = "New Value";
            var transformedDto= AutoMapper.Mapper.Map<ProductOptionDto>(mockApi);
            mockIProductOptionServices.Setup(m => m.UpdateAsync(It.IsAny<ProductOptionDto>())).Returns(Task.FromResult(transformedDto));

            var result = await target.UpdateOption(mockApi.Id, mockApi) as OkNegotiatedContentResult<ProductOptionApiModel>;
            AssertX.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<ProductOptionApiModel>));
            Assert.AreEqual(mockDto.Id, result.Content.Id);
            Assert.AreEqual(mockDto.Id, result.Content.Id);
            Assert.AreEqual(mockApi.Name, result.Content.Name);
            Assert.AreNotEqual(mockDto.Name, result.Content.Name);
        }
    }
}
