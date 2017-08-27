using FluentValidation.WebApi;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Xero.Refactor.WebApi.ErrorHandling;

namespace Xero.Refactor.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var formatters = GlobalConfiguration.Configuration.Formatters;
            formatters.Remove(formatters.XmlFormatter);
            formatters.JsonFormatter.Indent = true;

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // Register FluentValidationModelValidator here
            FluentValidationModelValidatorProvider.Configure(config);

            // Register the global error handler here
            //config.Services.Add(typeof(IExceptionHandler), new GlobalExceptionHandler());
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
        }
    }
}