using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace Xero.Refactor.WebApi.ErrorHandling
{
    /// <summary>
    /// Global exception handler for all unhandled exceptions
    /// </summary>
    /// <remarks>https://docs.microsoft.com/en-us/aspnet/web-api/overview/error-handling/web-api-global-error-handling</remarks>
    public class GlobalExceptionHandler: ExceptionHandler
    {
        public override Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            return base.HandleAsync(context, cancellationToken);
        }
        public override void Handle(ExceptionHandlerContext context)
        {
            var errorModel = new ErrorApiModel();
            errorModel.Message = context.Exception.Message;
            errorModel.Code = context.Exception.HResult.ToString();
            context.Result = new ErrorResult
            {
                Request = context.ExceptionContext.Request,
                Content = Newtonsoft.Json.JsonConvert.SerializeObject(errorModel)
            };
        }

        internal class ErrorResult : IHttpActionResult
        {
            public HttpRequestMessage Request { get; set; }

            public string Content { get; set; }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                HttpResponseMessage response =
                                 new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                response.Content = new StringContent(Content);
                response.RequestMessage = Request;
                return Task.FromResult(response);
            }
        }
    }
}