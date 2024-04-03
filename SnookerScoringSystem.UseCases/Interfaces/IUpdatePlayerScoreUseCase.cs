using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnookerScoringSystem.UseCases.Interfaces
{
    public interface IUpdatePlayerScoreUseCase
    {
        Task ExecuteAsync(int player1Score, int player1Foul, int player2Score, int player2Foul);
    }
}
