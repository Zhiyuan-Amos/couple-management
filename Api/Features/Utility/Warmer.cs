using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Couple.Api.Features.Utility
{
    public class Warmer
    {
        [Function("Warmer")]
        public HttpResponseData Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}
