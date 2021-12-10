using System.Threading.Tasks;

namespace Couple.Client.Services.Synchronizer;

public interface ICommand
{
    Task Execute();
}
