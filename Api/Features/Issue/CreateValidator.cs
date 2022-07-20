using Couple.Shared.Models.Issue;
using FluentValidation;

namespace Couple.Api.Features.Issue;

public class CreateValidator : AbstractValidator<CreateIssueDto>
{
    public CreateValidator()
    {
        RuleFor(dto => dto.Id).NotEmpty();
        RuleFor(dto => dto.Title).NotEmpty();
        RuleFor(dto => dto.For).IsInEnum();
        RuleFor(dto => dto.Tasks).NotEmpty();
        RuleForEach(dto => dto.Tasks)
            .ChildRules(tasks =>
                tasks.RuleFor(task => task.Content).NotEmpty());
        RuleFor(dto => dto.CreatedOn).NotEmpty();
    }
}
