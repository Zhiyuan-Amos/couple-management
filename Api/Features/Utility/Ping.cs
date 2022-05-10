namespace Couple.Api.Features.Utility;

public class Ping
{
    [Function("Ping")]
    public HttpResponseData Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")]
        HttpRequestData req)
    {
        return req.CreateResponse(HttpStatusCode.OK);
    }
}