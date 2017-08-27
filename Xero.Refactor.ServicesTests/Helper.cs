using Moq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xero.AspNet.Core.Data;
using Xero.Refactor.Data;

namespace Xero.Refactor.ServicesTests
{
    internal static class Helper
    {
        /// <summary>
        /// Creates a unit of work using a memory database
        /// </summary>
        /// <returns></returns>
        public static IUnitOfWork<RefactorDb> CreateMemoryUow()
        {
            var mockICurrentUserService = new Mock<ICurrentUserService>();
            mockICurrentUserService.Setup(p => p.GetCurrentUser()).Returns("TestUser");

            DbConnection memoryConnection = Effort.DbConnectionFactory.CreateTransient();
            var memDB = new RefactorDb(memoryConnection);
            // Mocking up dbFactory
            var mockdbFactory = new Mock<DbFactory<RefactorDb>>();
            mockdbFactory.Setup(m => m.DBContext).Returns(memDB);
            IUnitOfWork<RefactorDb> UoW = new UnitOfWork<RefactorDb>(mockdbFactory.Object, mockICurrentUserService.Object);
            return UoW;
        }

    }
}
