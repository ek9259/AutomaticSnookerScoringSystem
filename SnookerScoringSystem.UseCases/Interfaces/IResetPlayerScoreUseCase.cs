using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnookerScoringSystem.UseCases.Interfaces
{
    public interface IResetPlayerScoreUseCase
    {
        Task<List<int>> ExecuteAsync();
    }
}
