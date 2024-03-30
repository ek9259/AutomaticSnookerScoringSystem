// This line is creating an alias for the Player class in the SnookerScoringSystem.Domain namespace.
// Now, we can simply use 'Player' instead of 'SnookerScoringSystem.Domain.Player'.
using Player = SnookerScoringSystem.Domain.Player;

namespace SnookerScoringSystem.UseCases.PluginInterfaces
{
    // This interface defines the contract for a player repository which is responsible for managing the players in the system.
    public interface IPlayerRepository
    {
        // Abstract methods that are needed to be implemented by PlayerRepository
        Task AddPlayerAsync(Player player);
        Task<List<Player>> GetPlayerAsync();
        Task UpdatePlayerScoreAsync(int player1Score, int player2Score);
        Task<List<int>> ResetPlayerScoreAsync();
        void Reset();
    }
}
