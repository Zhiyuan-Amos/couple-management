using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Couple.Api.Data;
using Couple.Api.Infrastructure;
using Couple.Shared;
using Couple.Shared.Model;
using Couple.Shared.Model.Image;
using Couple.Shared.Utility;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Couple.Api.Features.Image
{
    public class CreateImageFunction
    {
        private readonly ChangeContext _context;
        private readonly IDateTimeService _dateTimeService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public CreateImageFunction(ChangeContext context,
            IDateTimeService dateTimeService,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _context = context;
            _dateTimeService = dateTimeService;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        [FunctionName("CreateImageFunction")]
        public async Task<ActionResult> CreateImage(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Images")]
            HttpRequest req,
            ILogger log,
            IBinder binder)
        {
            var form = await req.GetJsonBody<CreateImageDto, Validator>();

            if (!form.IsValid)
            {
                log.LogWarning("{ErrorMessage}", form.ErrorMessage());
                return form.ToBadRequest();
            }

            if (_currentUserService.PartnerId == null)
            {
                return new BadRequestResult();
            }

            var dto = form.Value;
            var toCreate = new Model.Change
            {
                Id = Guid.NewGuid(),
                Command = Command.CreateImage,
                UserId = _currentUserService.PartnerId,
                Timestamp = _dateTimeService.Now,
                Content = JsonSerializer.Serialize(_mapper.Map<Model.Image>(dto)),
            };

            _context
                .Changes
                .Add(toCreate);
            await _context.SaveChangesAsync();

            // Late binding is required to name the file using the Id
            var blobAttribute = new BlobAttribute($"images/{dto.Id}", FileAccess.Write)
            {
                Connection = "ImagesConnectionString"
            };
            await using var stream = binder.Bind<Stream>(blobAttribute);
            await stream.WriteAsync(dto.Data.AsMemory(0, dto.Data.Length));

            return new OkResult();
        }

        private class Validator : AbstractValidator<CreateImageDto>
        {
            public Validator()
            {
                RuleFor(dto => dto.Id).NotEmpty();
                RuleFor(dto => dto.TakenOn).NotEmpty();
                RuleFor(dto => dto.Data).Custom((data, context) =>
                {
                    if (!ImageExtensions.IsImage(new MemoryStream(data)))
                    {
                        context.AddFailure("Invalid file type");
                    }

                    if (data.Length > Constants.MaxFileSize)
                    {
                        context.AddFailure("File exceeded size limit");
                    }
                });
            }
        }
    }
}
