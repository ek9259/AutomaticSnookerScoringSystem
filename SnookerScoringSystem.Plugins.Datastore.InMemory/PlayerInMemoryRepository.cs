using SnookerScoringSystem.UseCases.PluginInterfaces;
using Player = SnookerScoringSystem.Domain.Player;

namespace SnookerScoringSystem.Plugins.Datastore.InMemory
{
    // This class implements the IPlayerRepository interface and defines how to manage players in an in-memory data store.
    public class PlayerInMemoryRepository : IPlayerRepository
    {
        // A static list to hold the players in memory.
        public static List<Player> _players;

        public PlayerInMemoryRepository()
        {
            _players = new List<Player>();
        }
        public Task AddPlayerAsync(Player player)
        {
            //Check if current players list has any player added already or not
            //If not, add a first player, else add a second second
            int maxId;
            if (_players.Count != 0)
            {
                maxId = _players.Max(x => x.Id);
            }
            else
            {
                maxId = 0;
            }
            
            player.Id = maxId + 1;
            player.Score = 0;
            player.Foul = 0;

            // If the player's name is null or whitespace, set it to "Player 1 or 2".
            if (string.IsNullOrWhiteSpace(player.Name))
            {
                player.Name = $"Player {player.Id} ";
            }
            _players.Add(player);

            return Task.CompletedTask;
        }

        // This method is responsible for retrieving all players from the in-memory data store.
        public Task<List<Player>> GetPlayerAsync()
        {
            return Task.FromResult(_players);
        }

    }
}
