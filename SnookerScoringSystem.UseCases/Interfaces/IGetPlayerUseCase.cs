using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnookerScoringSystem.Domain;

namespace SnookerScoringSystem.UseCases.Interfaces
{
    public interface IGetPlayerUseCase
    {
        Task<List<Player>> ExecuteAsync();
    }
}
