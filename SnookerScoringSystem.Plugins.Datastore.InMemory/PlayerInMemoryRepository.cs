using SnookerScoringSystem.UseCases.PluginInterfaces;
using Player = SnookerScoringSystem.Domain.Player;

namespace SnookerScoringSystem.Plugins.Datastore.InMemory
{
    // All the code in this file is included in all platforms.
    public class PlayerInMemoryRepository : IPlayerRepository
    {
        public static List<Player> _players;

        public PlayerInMemoryRepository()
        {
            _players = new List<Player>();
        }
        public Task AddPlayerAsync(Player player)
        {
            //Check if current players list has any player added already or not, it not, add a first player, else second
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

            if (string.IsNullOrWhiteSpace(player.Name))
            {
                player.Name = $"Player {player.Id} ";
            }
            _players.Add(player);

            return Task.CompletedTask;
        }

        public Task<List<Player>> GetPlayerAsync()
        {
            return Task.FromResult(_players);
        }
    }
}
