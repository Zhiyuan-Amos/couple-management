using Couple.Shared.Models.Issue;
using FluentValidation;

namespace Couple.Api.Features.Issue.Task;

public class CompleteValidator : AbstractValidator<CompleteTaskDto>
{
    public CompleteValidator()
    {
        RuleFor(dto => dto.TaskId).NotEmpty();
        RuleFor(dto => dto.IssueId).NotEmpty();
        RuleFor(dto => dto.CompletedDate).NotEmpty();
    }
}
