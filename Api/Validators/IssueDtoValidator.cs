using Couple.Shared.Model.Event;
using FluentValidation;

namespace Couple.Api.Validators;

public class IssueDtoValidator : AbstractValidator<IssueDto>
{
    public IssueDtoValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty();
        RuleFor(dto => dto.Title).NotEmpty();
        RuleFor(dto => dto.For).NotNull();
        RuleFor(dto => dto.Tasks).NotNull();
        RuleForEach(dto => dto.Tasks)
            .ChildRules(tasks =>
                tasks.RuleFor(task => task.Content).NotEmpty());
        RuleFor(dto => dto.CreatedOn).NotEmpty();
    }
}
