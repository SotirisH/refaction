using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Xero.Refactor.Services;
using Xero.Refactor.Services.Exceptions;
using Xero.Refactor.Test.Common;
using Xero.Refactor.WebApi;
using Xero.Refactor.WebApi.Controllers;
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
            target = new ProductsController(mockIProductServices.Object, mockIProductOptionServices.Object);
        }

        [TestMethod()]
        public async Task Product_searchByName_success()
        {

            var mockProductDto = FixturesDto.ProductsBuildOne();
            var retMock = (new List<ProductDto>() { mockProductDto }).AsEnumerable();
            mockIProductServices.Setup(m => m.GetByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(retMock));

            var result = await target.SearchByName("Any") as OkNegotiatedContentResult<IEnumerable<ProductApiModel>>;
            AssertX.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<IEnumerable<ProductApiModel>>));

        }

        [TestMethod()]
        public async Task Product_SearchByName_notFound()
        {

            var mockProductDto = FixturesDto.ProductsBuildOne();
            var retMock = (new List<ProductDto>() { mockProductDto }).AsEnumerable();
            mockIProductServices.Setup(m => m.GetByNameAsync("Me")).Returns(Task.FromResult(retMock));

            var result = await target.SearchByName("Any") as IHttpActionResult;
            AssertX.IsInstanceOfType(result, typeof(NotFoundResult));

        }

        [TestMethod()]
        public async Task Product_GetById_notFound()
        {

            var mockProductDto = FixturesDto.ProductsBuildOne();
            mockProductDto.Id = Guid.NewGuid();
            mockIProductServices.Setup(m => m.GetByIdAsync(mockProductDto.Id)).Returns(Task.FromResult(mockProductDto));

            var result = await target.GetProduct(Guid.NewGuid()) as IHttpActionResult;
            AssertX.IsInstanceOfType(result, typeof(NotFoundResult));

        }

        [TestMethod()]
        public async Task Product_GetById_success()
        {

            var mockProductDto = FixturesDto.ProductsBuildOne();
            mockProductDto.Id = Guid.NewGuid();

            mockIProductServices.Setup(m => m.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(mockProductDto));

            var result = await target.GetProduct(mockProductDto.Id) as OkNegotiatedContentResult<ProductApiModel>;
            AssertX.AreEqual(mockProductDto.Id, result.Content.Id);

        }

        [TestMethod()]
        public async Task Product_Create_success()
        {
            var mockProductDto = FixturesDto.ProductsBuildOne();
            var mockApiModel = AutoMapper.Mapper.Map<ProductApiModel>(mockProductDto);
            mockProductDto.Id = Guid.NewGuid();
            mockIProductServices.Setup(m => m.CreateAsync(It.IsAny<ProductDto>())).Returns(Task.FromResult(mockProductDto));

            var result = await target.Create(mockApiModel) as CreatedAtRouteNegotiatedContentResult<ProductApiModel>;

            AssertX.IsInstanceOfType(result, typeof(CreatedAtRouteNegotiatedContentResult<ProductApiModel>));
            Assert.AreEqual(mockProductDto.Id, result.Content.Id);
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
            var result = await target.Create(mockApiModel) as InvalidModelStateResult;

            AssertX.IsInstanceOfType(result, typeof(InvalidModelStateResult));
            CollectionAssert.Contains(result.ModelState.Keys.ToArray(), "Price");

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
            IHttpActionResult result = await target.Update(mockProductDto.Id, mockApiModel) as InvalidModelStateResult;
            AssertX.IsInstanceOfType(result, typeof(InvalidModelStateResult));
            CollectionAssert.Contains(((InvalidModelStateResult)result).ModelState.Keys.ToArray(), "Price");

            target.ModelState.Clear();
            result = await target.Update(Guid.Empty, mockApiModel) as BadRequestErrorMessageResult;
            AssertX.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));

            mockIProductServices.Setup(m => m.UpdateAsync(It.IsAny<ProductDto>())).Throws(new EntityNotFoundException("Not found!"));
            result = await target.Update(mockProductDto.Id, mockApiModel) as NotFoundResult;
            AssertX.IsInstanceOfType(result, typeof(NotFoundResult));

            mockIProductServices.Setup(m => m.UpdateAsync(It.IsAny<ProductDto>())).Throws(new DbUpdateConcurrencyException());
            result = await target.Update(mockProductDto.Id, mockApiModel) as ConflictResult;
            AssertX.IsInstanceOfType(result, typeof(ConflictResult));

        }


        [TestMethod()]
        public async Task Product_Update_success()
        {
            var mockProductDto = FixturesDto.ProductsBuildOne();
            mockProductDto.Id = Guid.NewGuid();
            var mockApiModel = AutoMapper.Mapper.Map<ProductApiModel>(mockProductDto);
            mockIProductServices.Setup(m => m.UpdateAsync(It.IsAny<ProductDto>())).Returns(Task.FromResult(mockProductDto));

            var result = await target.Update(mockProductDto.Id, mockApiModel) as OkNegotiatedContentResult<ProductApiModel>;
            AssertX.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<ProductApiModel>));
            Assert.AreEqual(mockProductDto.Id, result.Content.Id);
        }

        [TestMethod()]
        public async Task Product_Delete_success()
        {
            var mockProductDto = FixturesDto.ProductsBuildOne();
            mockProductDto.Id = Guid.NewGuid();
            mockIProductServices.Setup(m => m.DeleteByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(true));

            var result = await target.Delete(mockProductDto.Id) as OkResult;
            AssertX.IsInstanceOfType(result, typeof(OkResult));
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