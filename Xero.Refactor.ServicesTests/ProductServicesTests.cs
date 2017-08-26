using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data.Common;
using System.Threading.Tasks;
using Xero.AspNet.Core.Data;
using Xero.Refactor.Data;

namespace Xero.Refactor.Services.Tests
{
    [TestClass()]
    public class ProductServicesTests
    {
        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            Effort.Provider.EffortProviderConfiguration.RegisterProvider();

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutoMapperDtoProfile>();
            });
        }


        [TestMethod()]
        public void CreateAsyncTest()
        {
            var mockICurrentUserService = new Mock<ICurrentUserService>();
            mockICurrentUserService.Setup(p => p.GetCurrentUser()).Returns("TestUser");

            DbConnection memoryConnection = Effort.DbConnectionFactory.CreateTransient();
            var memDB = new Data.RefactorDb(memoryConnection);
            // Mocking up dbFactory
            var mockdbFactory = new Mock<DbFactory<RefactorDb>>();
            mockdbFactory.Setup(m => m.DBContext).Returns(memDB);
            IUnitOfWork<RefactorDb> UoW = new UnitOfWork<RefactorDb>(mockdbFactory.Object, mockICurrentUserService.Object);

            var target = new ProductServices(UoW);
            var mockProduct = new ProductDto() {Name="Test",Description="Test",Price=10, DeliveryPrice=10 };
            var result=Task.FromResult(target.CreateAsync(mockProduct)).Result;

            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Result);
            // The row version is returned
            Assert.IsNotNull(result.Result.RowVersion);
            Assert.IsTrue(result.Result.RowVersion.Length>0);

        }
    }
}