using Couple.Shared.Model.Calendar;
using FluentValidation;

namespace Couple.Api.Validators
{
    public class ToDoDtoValidator : AbstractValidator<ToDoDto>
    {
        public ToDoDtoValidator()
        {
            RuleFor(dto => dto.Id).NotEmpty();
            RuleFor(dto => dto.Text).NotEmpty();
            RuleFor(dto => dto.Category).NotEmpty();
            RuleFor(dto => dto.CreatedOn).NotEmpty();
        }
    }
}
