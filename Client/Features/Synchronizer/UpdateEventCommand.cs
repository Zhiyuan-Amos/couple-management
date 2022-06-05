using Couple.Client.Shared.Data;
using Couple.Shared.Models.Calendar;

namespace Couple.Client.Features.Synchronizer;

public class UpdateEventCommand : ICommand
{
    private readonly AppDbContext _dbContext;
    private readonly UpdateEventDto _model;

    public UpdateEventCommand(AppDbContext dbContext, UpdateEventDto model) => (_dbContext, _model) = (dbContext, model);

    public async Task Execute()
    {
        var @event = (await _dbContext.Events.FindAsync(_model.Id))!;
        _dbContext.Attach(@event);
        @event.Title = _model.Title;
        @event.For = _model.For;
        @event.Start = _model.Start;
        @event.End = _model.End;
        await _dbContext.SaveChangesAsync();
    }
}
