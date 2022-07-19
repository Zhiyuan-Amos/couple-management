using Couple.Shared;
using Couple.Shared.Extensions;
using Couple.Shared.Models.Image;
using FluentValidation;

namespace Couple.Api.Features.Image;

public class CreateValidator : AbstractValidator<CreateImageDto>
{
    public CreateValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty();
        RuleFor(dto => dto.TakenOn).NotEmpty();
        RuleFor(dto => dto.Data).Custom((data, context) =>
        {
            if (data == null)
            {
                context.AddFailure("Data cannot be null");
                return;
            }

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
