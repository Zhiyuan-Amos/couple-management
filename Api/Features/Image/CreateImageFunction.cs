using Couple.Api.Data;
using Couple.Api.Infrastructure;
using Couple.Shared.Model;
using Couple.Shared.Model.Image;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;

namespace Couple.Api.Features.Image
{
    public class CreateImageFunction
    {
        private readonly ChangeContext _context;
        private readonly IDateTimeService _dateTimeService;
        private readonly ICurrentUserService _currentUserService;

        public CreateImageFunction(ChangeContext context,
            IDateTimeService dateTimeService,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _dateTimeService = dateTimeService;
            _currentUserService = currentUserService;
        }

        [FunctionName("CreateImageFunction")]
        public async Task<ActionResult> CreateImage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Images")]
            HttpRequest req,
            //[Blob("images/foo.jpeg", FileAccess.Write, Connection = "AzureWebJobsStorage")]
            //Stream outputFile,
            ILogger log)
        {
            var formData = await req.ReadFormAsync();
            string data = formData["data"];
            // Use HttpRequestExtensions

            if (!form.IsValid)
            {
                log.LogWarning("{ErrorMessage}", form.ErrorMessage());
                return form.ToBadRequest();
            }

            var file = formData.Files["image"];
            var fileExtension = Path.GetExtension(file.FileName);
            if (fileExtension != ".jpg")
            {
                return new BadRequestObjectResult(request.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage
                }));
            }

            if (_currentUserService.PartnerId == null)
            {
                return new BadRequestResult();
            }

            var toCreate = new Model.Change
            {
                Id = Guid.NewGuid(),
                Command = Command.CreateImage,
                UserId = _currentUserService.PartnerId,
                Timestamp = _dateTimeService.Now,
                Content = form.Json,
            };

            _context
                .Changes
                .Add(toCreate);
            await _context.SaveChangesAsync();

            //await outputFile.WriteAsync(output, 0, output.Length);

            return new OkResult();
        }

        private class Validator : AbstractValidator<CreateImageDto>
        {
            public Validator()
            {
                RuleFor(dto => dto.Id).NotEmpty();
                RuleFor(dto => dto.TakenOn).NotEmpty();
            }
        }
    }
}
