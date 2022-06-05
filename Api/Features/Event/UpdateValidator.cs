using Couple.Shared.Models.Calendar;
using FluentValidation;

namespace Couple.Api.Features.Event;

public class UpdateValidator : AbstractValidator<UpdateEventDto>
{
    public UpdateValidator()
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
