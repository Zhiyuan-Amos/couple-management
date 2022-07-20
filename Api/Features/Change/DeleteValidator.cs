using Couple.Shared.Models.Change;
using FluentValidation;

namespace Couple.Api.Features.Change;

public class DeleteValidator : AbstractValidator<DeleteChangesDto>
{
    public DeleteValidator()
    {
        RuleFor(dto => dto.Guids).NotEmpty();
        RuleForEach(dto => dto.Guids).NotEmpty();
    }
}
