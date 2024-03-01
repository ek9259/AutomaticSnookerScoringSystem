namespace SnookerScoringSystem.UseCases.Interfaces
{
    // This interface defines the contract for adding a player to the system.
    public interface IAddPlayerUseCase
    {
        // The method is asynchronous, meaning it returns a Task that represents the ongoing operation.
        // Executing the addition of a player to the system.

        Task ExecuteAsync(Domain.Player player);
    }
}
