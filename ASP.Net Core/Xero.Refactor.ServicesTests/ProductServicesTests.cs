using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xero.Refactor.ServicesTests;
using Xero.Refactor.Test.Common;

namespace Xero.Refactor.Services.Tests
{
    [TestClass()]
    public class ProductServicesTests
    {
        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutoMapperDtoProfile>();
            });
        }

        [TestInitialize]
        public void TestInit()
        {

            DbMemoryHelper.Get();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            DbMemoryHelper.Dispose();
        }


        [TestMethod()]
        public void BasicCreateAsyncTest()
        {


            var target = new ProductServices(DbMemoryHelper.Get(), Mapper.Instance);
            var mockProduct = FixturesDto.ProductsBuildOne();
            var result = Task.FromResult(target.CreateAsync(mockProduct)).Result;

            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Result);
        }

        [TestMethod()]
        public async Task Delete_product_sucess()
        {
            var target = new ProductServices(DbMemoryHelper.Get(), Mapper.Instance);
            var mockProduct = FixturesDto.ProductsBuildOne();
            var result = Task.FromResult(target.CreateAsync(mockProduct)).Result;
            var newProductId = result.Result.Id;
            var deleteResult = await target.DeleteByIdAsync(newProductId);
            Assert.IsTrue(deleteResult);


        }
        [TestMethod()]
        public async Task Delete_product_fail()
        {
            var target = new ProductServices(DbMemoryHelper.Get(), Mapper.Instance);
            var mockProduct = FixturesDto.ProductsBuildOne();
            var result = Task.FromResult(target.CreateAsync(mockProduct)).Result;
            var newProductId = Guid.NewGuid(); ;
            var deleteResult = await target.DeleteByIdAsync(newProductId);
            Assert.IsFalse(deleteResult);
        }

        [TestMethod()]
        public async Task Get_all_products_expected_two()
        {
            var target = new ProductServices(DbMemoryHelper.Get(), Mapper.Instance);
            var mockProduct1 = FixturesDto.ProductsBuildOne();
            await target.CreateAsync(mockProduct1);
            var mockProduct2 = FixturesDto.ProductsBuildOne();
            await target.CreateAsync(mockProduct2);

            var result = await target.GetAllAsync();
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count(), "Expected two products to be found!");
        }

        [TestMethod()]
        public async Task Discover_product_by_id()
        {
            var target = new ProductServices(DbMemoryHelper.Get(), Mapper.Instance);
            var mockProduct1 = FixturesDto.ProductsBuildOne();
            await target.CreateAsync(mockProduct1);
            var mockProduct2 = FixturesDto.ProductsBuildOne();
            await target.CreateAsync(mockProduct2);

            var result = await target.GetByIdAsync(mockProduct2.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual(mockProduct2.Id, result.Id);
        }

        [Ignore("The memory providers don't support timestamp!")]
        [TestMethod()]
        public async Task Discover_product_by_name_expect_two()
        {
            var target = new ProductServices(DbMemoryHelper.Get(), Mapper.Instance);
            var mockProduct1 = FixturesDto.ProductsBuildOne();
            // Bogus doesn't ensure unique names
            mockProduct1.Name = "Prod1";
            await target.CreateAsync(mockProduct1);
            var mockProduct2 = FixturesDto.ProductsBuildOne();

            await target.CreateAsync(mockProduct2);

            var mockProduct3 = FixturesDto.ProductsBuildOne();
            mockProduct3.Name = mockProduct2.Name;
            await target.CreateAsync(mockProduct3);


            var result = await target.GetByNameAsync(mockProduct2.Name);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count(), $"Expected two products with name '{ mockProduct2.Name}' to be found!");
            Assert.AreEqual(mockProduct2.Name, result.First().Name);
        }

        [Ignore("The memory providers don't support timestamp!")]
        [TestMethod()]
        public async Task Update_product_success()
        {
            var target = new ProductServices(DbMemoryHelper.Get(), Mapper.Instance);
            var mockProduct1 = FixturesDto.ProductsBuildOne();
            await target.CreateAsync(mockProduct1);
            var mockProduct2 = FixturesDto.ProductsBuildOne();
            await target.CreateAsync(mockProduct2);

            var mockProduct3 = FixturesDto.ProductsBuildOne();
            var prodtoUpdate = await target.CreateAsync(mockProduct3);

            prodtoUpdate.Name = "UpdatedProd";
            var result = await target.UpdateAsync(prodtoUpdate);
            Assert.IsNotNull(result);
            Assert.AreEqual(prodtoUpdate.Name, result.Name);
            Assert.AreNotEqual(mockProduct3.Name, result.Name);
        }

        [TestMethod()]
        public async Task Update_product_conflict()
        {
            var target = new ProductServices(DbMemoryHelper.Get(), Mapper.Instance);
            var mockProduct1 = FixturesDto.ProductsBuildOne();
            await target.CreateAsync(mockProduct1);
            var mockProduct2 = FixturesDto.ProductsBuildOne();
            await target.CreateAsync(mockProduct2);

            var mockProduct3 = FixturesDto.ProductsBuildOne();
            var prodtoUpdate = await target.CreateAsync(mockProduct3);

            prodtoUpdate.Name = "UpdatedProd";
            // Initial update
            var result = await target.UpdateAsync(prodtoUpdate);
            // need to revise
            prodtoUpdate.Name = "Confict";
            var result2 = Task.FromResult(target.UpdateAsync(prodtoUpdate)).Result;
            Assert.IsNotNull(result2.Exception);
            Assert.IsInstanceOfType(result2.Exception.InnerException, typeof(DbUpdateConcurrencyException));
        }
    }
}