using Couple.Shared.Model.Event;
using FluentValidation;

namespace Couple.Api.Validators
{
    public class ToDoDtoValidator : AbstractValidator<ToDoDto>
    {
        public ToDoDtoValidator()
        {
            RuleFor(dto => dto.Id).NotEmpty();
            RuleFor(dto => dto.Name).NotEmpty();
            RuleFor(dto => dto.For).NotNull();
            RuleFor(dto => dto.ToDos).NotNull();
            RuleForEach(dto => dto.ToDos)
                .ChildRules(toDos =>
                    toDos.RuleFor(toDo => toDo.Content).NotEmpty());
            RuleFor(dto => dto.CreatedOn).NotEmpty();
        }
    }
}
