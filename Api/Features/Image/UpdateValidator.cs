using Couple.Shared.Extensions;
using Couple.Shared.Models.Image;
using FluentValidation;

namespace Couple.Api.Features.Image;

public class UpdateValidator : AbstractValidator<UpdateImageDto>
{
    public UpdateValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty();
        RuleFor(dto => dto.TakenOn).NotEmpty();
        RuleFor(dto => dto.Data).Custom((data, context) =>
        {
            if (!ImageExtensions.IsImage(new MemoryStream(data)))
            {
                context.AddFailure("Invalid file type");
            }
        });
    }
}
