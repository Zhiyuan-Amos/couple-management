using Api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Api.Features
{
    public class DatabaseInitializerFunction
    {
        private readonly EventContext _eventContext;

        public DatabaseInitializerFunction(EventContext eventContext)
        {
            _eventContext = eventContext;
        }

        //[FunctionName("DatabaseInitializerFunction")]
        public async Task<ActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "DatabaseInitializer")] HttpRequest req,
            ILogger log)
        {
            await _eventContext.Database.EnsureDeletedAsync();
            await _eventContext.Database.EnsureCreatedAsync();
            return new OkResult();
        }
    }
}
