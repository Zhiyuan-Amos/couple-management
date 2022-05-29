using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Couple.Api.Features.Utility;

// Called by Application Insights to warm this Function App: https://yourazurecoach.com/2019/05/22/the-dev-ops-way-to-avoid-cold-starts-of-your-azure-functions
public class Ping
{
    [Function("Ping")]
    public HttpResponseData Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")]
        HttpRequestData req) =>
        req.CreateResponse(HttpStatusCode.OK);
}
