using Couple.Client.Features.Calendar.Models;
using Couple.Client.Shared.Data;

namespace Couple.Client.Features.Synchronizer;

public class CreateEventCommand : ICommand
{
    private readonly AppDbContext _dbContext;
    private readonly EventModel _model;

    public CreateEventCommand(AppDbContext dbContext, EventModel model) => (_dbContext, _model) = (dbContext, model);

    public async Task Execute()
    {
        _dbContext.Events.Add(_model);
        await _dbContext.SaveChangesAsync();
    }
}
