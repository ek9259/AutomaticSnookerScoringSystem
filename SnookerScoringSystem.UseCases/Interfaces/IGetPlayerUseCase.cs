using SnookerScoringSystem.Domain;

namespace SnookerScoringSystem.UseCases.Interfaces
{
    // This interface defines the contract for getting a player to the system.
    public interface IGetPlayerUseCase
    {
        Task<List<Player>> ExecuteAsync();
    }
}
