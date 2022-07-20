using Couple.Shared.Models.Issue;
using FluentValidation;

namespace Couple.Api.Features.Issue;

public class UpdateValidator : AbstractValidator<UpdateIssueDto>
{
    public UpdateValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty();
        RuleFor(dto => dto.Title).NotEmpty();
        RuleFor(dto => dto.For).IsInEnum();
        RuleFor(dto => dto.Tasks).NotNull();
        RuleForEach(dto => dto.Tasks)
            .ChildRules(tasks =>
                tasks.RuleFor(task => task.Content).NotEmpty());
        RuleFor(dto => dto.CreatedOn).NotEmpty();
    }
}
