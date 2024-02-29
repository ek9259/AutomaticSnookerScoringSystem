using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Player = SnookerScoringSystem.Domain.Player;

namespace SnookerScoringSystem.UseCases.PluginInterfaces
{
    public interface IPlayerRepository
    {
        Task AddPlayerAsync(Player player);
        Task<List<Player>> GetPlayerAsync();
    }
}
