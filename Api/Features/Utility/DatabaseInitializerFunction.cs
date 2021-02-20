using Couple.Api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Couple.Api.Features.Utility
{
    public class DatabaseInitializerFunction
    {
        private readonly ChangeContext _eventContext;

        public DatabaseInitializerFunction(ChangeContext eventContext)
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
