using AutoMapper;
using System.Web.Http;
using Xero.Refactor.Services;
using Xero.Refactor.WebApi;

namespace refactor_me
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            UnityConfig.RegisterComponents();

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutoMapperDtoProfile>();
                cfg.AddProfile<AutoMapperApiModelProfile>();
            });
        }
    }
}
