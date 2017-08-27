using AutoMapper;
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
using Xero.Refactor.WebApi.Controllers;
using Xero.Refactor.WebApi.Core;
using Xero.Refactor.WebApi.Modeling;

namespace Xero.Refactor.WebApiTests
{
    [TestClass()]
    public partial class ProductsControllerTests
    {
        private Mock<IProductServices> mockIProductServices = new Mock<IProductServices>();
        private Mock<IProductOptionServices> mockIProductOptionServices = new Mock<IProductOptionServices>();
        private ProductsController target;

        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutoMapperApiModelProfile>();
            });
        }

        [TestInitialize]
        public void TestInitialize()
        {
            mockIProductServices = new Mock<IProductServices>();
            mockIProductOptionServices = new Mock<IProductOptionServices>();
            target = new ProductsController(mockIProductServices.Object, mockIProductOptionServices.Object, Mapper.Instance);
        }

        [TestMethod()]
        public async Task Product_searchByName_success()
        {

            var mockProductDto = FixturesDto.ProductsBuildOne();
            var retMock = (new List<ProductDto>() { mockProductDto }).AsEnumerable();
            mockIProductServices.Setup(m => m.GetByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(retMock));

            var result = await target.SearchByName("Any") as OkObjectResult;
            AssertX.IsInstanceOfType(result, typeof(OkObjectResult));
            var resultValue = result.Value as IEnumerable<ProductApiModel>;

            AssertX.IsInstanceOfType(resultValue, typeof(IEnumerable<ProductApiModel>));
        }

        [TestMethod()]
        public async Task Product_SearchByName_notFound()
        {

            var mockProductDto = FixturesDto.ProductsBuildOne();
            var retMock = (new List<ProductDto>() { mockProductDto }).AsEnumerable();
            mockIProductServices.Setup(m => m.GetByNameAsync("Me")).Returns(Task.FromResult(retMock));

            var result = await target.SearchByName("Any") as IActionResult;
            AssertX.IsInstanceOfType(result, typeof(NotFoundResult));

        }

        [TestMethod()]
        public async Task Product_GetById_notFound()
        {

            var mockProductDto = FixturesDto.ProductsBuildOne();
            mockProductDto.Id = Guid.NewGuid();
            mockIProductServices.Setup(m => m.GetByIdAsync(mockProductDto.Id)).Returns(Task.FromResult(mockProductDto));

            var result = await target.GetProduct(Guid.NewGuid()) as IActionResult;
            AssertX.IsInstanceOfType(result, typeof(NotFoundResult));

        }

        [TestMethod()]
        public async Task Product_GetById_success()
        {

            var mockProductDto = FixturesDto.ProductsBuildOne();
            mockProductDto.Id = Guid.NewGuid();

            mockIProductServices.Setup(m => m.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(mockProductDto));

            var result = await target.GetProduct(mockProductDto.Id) as OkObjectResult;
            var resultValue = result.Value as ProductApiModel;
            AssertX.AreEqual(mockProductDto.Id, resultValue.Id);

        }

        [TestMethod()]
        public async Task Product_Create_success()
        {
            var mockProductDto = FixturesDto.ProductsBuildOne();
            var mockApiModel = AutoMapper.Mapper.Map<ProductApiModel>(mockProductDto);
            mockProductDto.Id = Guid.NewGuid();
            mockIProductServices.Setup(m => m.CreateAsync(It.IsAny<ProductDto>())).Returns(Task.FromResult(mockProductDto));

            var result = await target.Create(mockApiModel) as CreatedAtRouteResult;

            AssertX.IsInstanceOfType(result, typeof(CreatedAtRouteResult));
            var resultValue = result.Value as ProductApiModel;
            Assert.AreEqual(mockProductDto.Id, resultValue.Id);
            Assert.AreEqual(mockProductDto.Id, result.RouteValues["id"]);

        }

        [TestMethod()]
        public async Task Product_Create_invalid_model()
        {
            var mockProductDto = FixturesDto.ProductsBuildOne();
            var mockApiModel = AutoMapper.Mapper.Map<ProductApiModel>(mockProductDto);
            mockApiModel.Price = -100;
            mockProductDto.Id = Guid.NewGuid();
            mockIProductServices.Setup(m => m.CreateAsync(It.IsAny<ProductDto>())).Returns(Task.FromResult(mockProductDto));

            target.ModelState.AddModelError("Price", "Price should be a positive value!");
            var result = await target.Create(mockApiModel) as BadRequestObjectResult;
            
            AssertX.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var resultValue = result.Value as SerializableError;
            CollectionAssert.Contains(resultValue.Keys.ToArray(), "Price");

        }

        [TestMethod()]
        public async Task Product_Update_BadRequest_scenario()
        {
            var mockProductDto = FixturesDto.ProductsBuildOne();
            var mockApiModel = AutoMapper.Mapper.Map<ProductApiModel>(mockProductDto);
            mockApiModel.Price = -100;
            mockProductDto.Id = Guid.NewGuid();
            mockIProductServices.Setup(m => m.UpdateAsync(It.IsAny<ProductDto>())).Returns(Task.FromResult(mockProductDto));

            target.ModelState.AddModelError("Price", "Price should be a positive value!");
            IActionResult result = await target.Update(mockProductDto.Id, mockApiModel) as BadRequestObjectResult;
            AssertX.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var resultValue = ((BadRequestObjectResult)result).Value as SerializableError;
            CollectionAssert.Contains(resultValue.Keys.ToArray(), "Price");

            target.ModelState.Clear();
            result = await target.Update(Guid.Empty, mockApiModel) as BadRequestObjectResult;
            AssertX.IsInstanceOfType(result, typeof(BadRequestObjectResult));

            mockIProductServices.Setup(m => m.UpdateAsync(It.IsAny<ProductDto>())).Throws(new EntityNotFoundException("Not found!"));
            result = await target.Update(mockProductDto.Id, mockApiModel) as NotFoundResult;
            AssertX.IsInstanceOfType(result, typeof(NotFoundResult));

            //mockIProductServices.Setup(m => m.UpdateAsync(It.IsAny<ProductDto>())).Throws(new DbUpdateConcurrencyException("Error",null));
            //result = await target.Update(mockProductDto.Id, mockApiModel) as StatusCodeResult;
            //AssertX.IsInstanceOfType(result, typeof(StatusCodeResult));

        }


        [TestMethod()]
        public async Task Product_Update_success()
        {
            var mockProductDto = FixturesDto.ProductsBuildOne();
            mockProductDto.Id = Guid.NewGuid();
            var mockApiModel = AutoMapper.Mapper.Map<ProductApiModel>(mockProductDto);
            mockIProductServices.Setup(m => m.UpdateAsync(It.IsAny<ProductDto>())).Returns(Task.FromResult(mockProductDto));

            var result = await target.Update(mockProductDto.Id, mockApiModel) as OkObjectResult;
            AssertX.IsInstanceOfType(result, typeof(OkObjectResult));
            var resultValue = result.Value as ProductApiModel;
            Assert.AreEqual(mockProductDto.Id, resultValue.Id);
        }

        [TestMethod()]
        public async Task Product_Delete_success()
        {
            var mockProductDto = FixturesDto.ProductsBuildOne();
            mockProductDto.Id = Guid.NewGuid();
            mockIProductServices.Setup(m => m.DeleteByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));

            var result = await target.Delete(mockProductDto.Id) as StatusCodeResult;
            AssertX.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }

        [TestMethod()]
        public async Task Product_Delete_NotFound()
        {
            mockIProductServices.Setup(m => m.DeleteByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(false));

            var result = await target.Delete(Guid.NewGuid()) as NotFoundResult;
            AssertX.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}