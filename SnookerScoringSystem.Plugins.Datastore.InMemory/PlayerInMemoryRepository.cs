using SnookerScoringSystem.UseCases.PluginInterfaces;
using Player = SnookerScoringSystem.Domain.Player;
using System.Collections.Concurrent;

namespace SnookerScoringSystem.Plugins.Datastore.InMemory
{
    public class PlayerInMemoryRepository : IPlayerRepository
    {
        // Use a thread-safe collection
        private static ConcurrentQueue<Player> _players;

        public PlayerInMemoryRepository()
        {
            _players = new ConcurrentQueue<Player>();
        }

        public Task AddPlayerAsync(Player player)
        {
            try
            {
                // Check if the player is null
                if (player == null)
                {
                    throw new ArgumentNullException(nameof(player));
                }

                // Check if current players list has any player added already or not
                // If not, add a first player, else add a second second
                int maxId = _players.Count != 0 ? _players.Max(x => x.Id) : 0;

                player.Id = maxId + 1;
                player.Score = 0;
                player.Foul = 0;

                // If the player's name is null or whitespace, set it to "Player 1 or 2".
                if (string.IsNullOrWhiteSpace(player.Name))
                {
                    player.Name = $"Player {player.Id} ";
                }

                _players.Enqueue(player);
            }
            catch (Exception ex)
            {
                throw new Exception("An error has occured while creating a new player");
            }

            return Task.CompletedTask;
        }

        // This method is responsible for retrieving all players from the in-memory data store.
        public Task<List<Player>> GetPlayerAsync()
        {
            return Task.FromResult(_players.ToList());
        }
    }
}