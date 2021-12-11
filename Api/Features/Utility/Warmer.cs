using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;

namespace Couple.Api.Features.Utility;

public class Warmer
{
    [Function("Warmer")]
    public HttpResponseData Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")]
        HttpRequestData req)
    {
        return req.CreateResponse(HttpStatusCode.OK);
    }
}
