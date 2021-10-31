using Couple.Shared.Model.Event;
using FluentValidation;

namespace Couple.Api.Validators
{
    public class EventDtoValidator : AbstractValidator<EventDto>
    {
        public EventDtoValidator()
        {
            RuleFor(dto => dto.Id).NotEmpty();
            RuleFor(dto => dto.Title).NotEmpty();
            RuleFor(dto => dto.Start).NotEmpty();
            RuleFor(dto => dto.End).NotEmpty();
            RuleFor(dto => dto.ToDos).NotNull();
            RuleForEach(dto => dto.ToDos).SetValidator(new IssueDtoValidator());
        }
    }
}
