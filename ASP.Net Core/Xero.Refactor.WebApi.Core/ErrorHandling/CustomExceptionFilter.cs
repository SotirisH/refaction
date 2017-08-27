using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace Xero.Refactor.WebApi.ErrorHandling
{
    /// <summary>
    /// Global exception handler for all unhandled exceptions
    /// </summary>
    /// <remarks>http://www.talkingdotnet.com/global-exception-handling-in-aspnet-core-webapi/</remarks>
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            HttpStatusCode status = HttpStatusCode.InternalServerError;
            string message = context.Exception.Message;

            var exceptionType = context.Exception.GetType();
            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                message = "Unauthorized Access";
                status = HttpStatusCode.Unauthorized;
            }
            else if (exceptionType == typeof(NotImplementedException))
            {
                message = "A server error occurred.";
                status = HttpStatusCode.NotImplemented;
            }
            ////else if (exceptionType == typeof(MyAppException))
            ////{
            ////    message = context.Exception.ToString();
            ////    status = HttpStatusCode.InternalServerError;
            ////}
            //else
            //{
            //    message = context.Exception.Message;
            //    status = HttpStatusCode.NotFound;
            //}
            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)status;
            response.ContentType = "application/json";
            var errorApiModel = new ErrorApiModel();
            errorApiModel.Code = status.ToString();
            errorApiModel.Message = message;
            response.WriteAsync (Newtonsoft.Json.JsonConvert.SerializeObject(errorApiModel));
        }
    }
}