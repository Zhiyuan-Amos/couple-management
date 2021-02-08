using Api.Data;
using Api.Infrastructure;
using Couple.Shared.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Features.ToDo
{
    public class DeleteToDoFunction
    {
        private readonly EventContext _context;
        private readonly IDateTimeService _dateTimeService;
        private readonly ICurrentUserService _currentUserService;

        public DeleteToDoFunction(EventContext context,
                                  IDateTimeService dateTimeService,
                                  ICurrentUserService currentUserService)
        {
            _context = context;
            _dateTimeService = dateTimeService;
            _currentUserService = currentUserService;
        }

        [FunctionName("DeleteToDoFunction")]
        public async Task<ActionResult> DeleteToDo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "ToDos/{id}")] HttpRequest req,
            Guid id,
            ILogger log)
        {
            if (_currentUserService.PartnerId == null)
            {
                return new BadRequestResult();
            }

            var toCreate = new Change
            {
                Id = Guid.NewGuid(),
                Function = Function.Delete,
                DataType = DataType.ToDo,
                UserId = _currentUserService.PartnerId,
                Timestamp = _dateTimeService.Now,
                Content = JsonSerializer.Serialize(id),
            };

            _context
                .Changes
                .Add(toCreate);
            await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}
