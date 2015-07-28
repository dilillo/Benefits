using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Filters;

namespace BenefitsWeb.Filters
{
    /// <summary>
    /// Handles an exception encountered during a the execution of a WebApi controller action.
    /// </summary>
    public class HandleExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var jsonMediaTypeFormatter = new JsonMediaTypeFormatter();

            jsonMediaTypeFormatter.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Objects;
            jsonMediaTypeFormatter.SerializerSettings.ObjectCreationHandling = ObjectCreationHandling.Replace;

            context.Response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                ReasonPhrase = context.Exception.GetType().Name,
                Content = new ObjectContent<string>(context.Exception.Message, jsonMediaTypeFormatter)
            };
        }
    }
}