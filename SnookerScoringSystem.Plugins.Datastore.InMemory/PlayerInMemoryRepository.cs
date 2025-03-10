﻿using SnookerScoringSystem.UseCases.PluginInterfaces;
using Player = SnookerScoringSystem.Domain.Player;
using System.Collections.Concurrent;

namespace SnookerScoringSystem.Plugins.Datastore.InMemory
{
    public class PlayerInMemoryRepository : IPlayerRepository
    {
        // Use a thread-safe collection
        private static List<Player> _players;

        public PlayerInMemoryRepository()
        {
            _players = new List<Player>();
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

                _players.Add(player);
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
            return Task.FromResult(_players);
        }

        //Update the player score and foul from other components
        public Task UpdatePlayerScoreAsync(int player1Score, int player1Foul, int player2Score, int player2Foul)
        {
            _players[0].Score = player1Score;
            _players[0].Foul = player1Foul;
            _players[1].Score = player2Score;
            _players[1].Foul = player2Foul;
            return Task.CompletedTask;
        }

        //Reset player score and foul only
        public Task<List<int>> ResetPlayerScoreAsync()
        {
            _players[0].Score = 0;
            _players[0].Foul = 0;
            _players[1].Score = 0;
            _players[1].Foul = 0;
            var playerScores = new List<int>
            {
                _players[0].Score,
                _players[1].Score
            };
            return Task.FromResult(playerScores);
        }

        //Reset players when restart the game
        public void Reset()
        {
            _players.Clear();
        }
    }
}