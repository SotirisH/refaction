using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
            var result = await target.GetOptions(mockDto.ProductId) as OkObjectResult;
            AssertX.IsInstanceOfType(result, typeof(OkObjectResult));
            var resultValue = result.Value as IEnumerable<ProductOptionApiModel>;
            Assert.AreEqual(retMock.First().ProductId, resultValue.First().Id);
            Assert.AreEqual(retMock.First().Name, resultValue.First().Name);
            Assert.AreEqual(retMock.First().Description, resultValue.First().Description);
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

            var result = await target.GetOption(mockDto.ProductId, mockDto.Id) as OkObjectResult;
            AssertX.IsInstanceOfType(result, typeof(OkObjectResult));
            var resultValue = result.Value as ProductOptionApiModel;
            Assert.AreEqual(mockDto.ProductId, resultValue.ProductId);
            Assert.AreEqual(mockDto.Name, resultValue.Name);
            Assert.AreEqual(mockDto.Description, resultValue.Description);
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

            var result = await target.CreateOption(mockDto.ProductId, mockApi) as CreatedAtRouteResult;
            AssertX.IsInstanceOfType(result, typeof(CreatedAtRouteResult));
            var resultValue = result.Value as ProductOptionApiModel;
            Assert.AreEqual(mockApi.ProductId, resultValue.ProductId);
            Assert.AreEqual(mockApi.Name, resultValue.Name);
            Assert.AreEqual(mockApi.Description, resultValue.Description);
        }

        [TestMethod()]
        public async Task ProductOption_CreateOption_InvalidModel()
        {
            var mockDto = FixturesDto.ProductOptionBuildOne(null);
            var mockApi = AutoMapper.Mapper.Map<ProductOptionApiModel>(mockDto);

            mockIProductOptionServices.Setup(m => m.CreateAsync(It.IsAny<ProductOptionDto>())).Returns(Task.FromResult(mockDto));
            target.ModelState.AddModelError("Name", "No Value");
            var result = await target.CreateOption(mockDto.ProductId, mockApi) as BadRequestObjectResult;
            AssertX.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var resultValue = ((BadRequestObjectResult)result).Value as SerializableError;
            CollectionAssert.Contains(resultValue.Keys.ToArray(), "Name");
        }

        [TestMethod()]
        public async Task ProductOption_Delete_success()
        {
            mockIProductOptionServices.Setup(m => m.DeleteByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));

            var result = await target.DeleteOption(Guid.NewGuid()) as StatusCodeResult;
            AssertX.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
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
            IActionResult result = await target.UpdateOption(mockApi.Id, mockApi) as BadRequestObjectResult;
            AssertX.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var resultValue = ((BadRequestObjectResult)result).Value as SerializableError;
            CollectionAssert.Contains(resultValue.Keys.ToArray(), "Name");

            target.ModelState.Clear();
            result = await target.UpdateOption(Guid.Empty, mockApi) as BadRequestObjectResult;
            AssertX.IsInstanceOfType(result, typeof(BadRequestObjectResult));

            mockIProductOptionServices.Setup(m => m.UpdateAsync(It.IsAny<ProductOptionDto>())).Throws(new EntityNotFoundException("Not found!"));
            result = await target.UpdateOption(mockApi.Id, mockApi) as NotFoundResult;
            AssertX.IsInstanceOfType(result, typeof(NotFoundResult));

            //mockIProductOptionServices.Setup(m => m.UpdateAsync(It.IsAny<ProductOptionDto>())).Throws(new DbUpdateConcurrencyException("Conflict", null));
            //result = await target.UpdateOption(mockApi.Id, mockApi) as StatusCodeResult;
            //AssertX.IsInstanceOfType(result, typeof(StatusCodeResult));
        }

        [TestMethod()]
        public async Task ProductOption_Update_success()
        {
            var mockDto = FixturesDto.ProductOptionBuildOne(null);
            mockDto.Id = Guid.NewGuid();
            var mockApi = AutoMapper.Mapper.Map<ProductOptionApiModel>(mockDto);
            mockApi.Name = "New Value";
            var transformedDto = AutoMapper.Mapper.Map<ProductOptionDto>(mockApi);
            mockIProductOptionServices.Setup(m => m.UpdateAsync(It.IsAny<ProductOptionDto>())).Returns(Task.FromResult(transformedDto));

            var result = await target.UpdateOption(mockApi.Id, mockApi) as OkObjectResult;
            AssertX.IsInstanceOfType(result, typeof(OkObjectResult));
            var resultValue = result.Value as ProductOptionApiModel;
            Assert.AreEqual(mockDto.Id, resultValue.Id);
            Assert.AreEqual(mockApi.Name, resultValue.Name);
            Assert.AreNotEqual(mockDto.Name, resultValue.Name);
        }
    }
}