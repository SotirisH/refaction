using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;
using Xero.AspNet.Core.Data;
using Xero.Refactor.Data;
using Xero.Refactor.Services;

namespace Xero.Refactor.WebApi
{
    public class CurrentUserService : ICurrentUserService
    {
        public string GetCurrentUser()
        {
            return "XeroUser";
        }
    }

    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<ICurrentUserService, CurrentUserService>();
            container.RegisterType<DbFactory<RefactorDb>, RefactorDbFactory>();

            container.RegisterType<IUnitOfWork<RefactorDb>, UnitOfWork<RefactorDb>>();
            container.RegisterType<IProductServices, ProductServices>();
            container.RegisterType<IProductOptionServices, ProductOptionServices>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}