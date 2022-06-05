using Couple.Shared.Models.Calendar;
using FluentValidation;

namespace Couple.Api.Features.Event;

public class CreateValidator : AbstractValidator<CreateEventDto>
{
    public CreateValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty();
        RuleFor(dto => dto.Title).NotEmpty();
        RuleFor(dto => dto.For).NotNull();
        RuleFor(dto => dto.Start).NotEmpty();
        RuleFor(dto => dto.End).NotEmpty()
            .GreaterThan(d => d.Start);
        RuleFor(dto => dto.CreatedOn).NotEmpty();
    }
}
