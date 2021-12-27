using DurackServer.Exceptions;
using DurackServer.Model;
using DurackServer.Model.Game;
using DurackServer.networking;
using DurackServer.networking.PlayerIO;
using DurackServer.networking.Session;

namespace DurackServer.Controller
{
    public class Controller
    {
        private Game game = new();
        
        public void StartGameRound(GameSession session, Command cmd)
        {
            var playerId = game.GetCurrentPlayerId();
            try
            {
                game.StartTransaction();
                var curPlayer = game.GetPlayer(playerId);
                var action = curPlayer.GetAction(cmd);
                ProcessAction(action, cmd);
            }
            catch (GameException e)
            {
                game.UndoTransaction();
                throw new GameException("make another move -_-");
            }

            if (game.CheckWin() > -1)
            {
                throw new GameException("Game end, someone win(?)");
            }
        }

        public void AddPlayer(Player player)
        {
            game.gameState.Players.Add(player);
            game.PostRaund();
        }

        public int GetNextPlayerId()
        {
            return game.GetNextPlayerId();
        }
        
        public int GetCurrentPlayerId()
        {
            return game.GetCurrentPlayerId();
        }
        
        public Player GetCurrentPlayer()
        {
            return game.GetPlayer(GetCurrentPlayerId());
        }

        private void ProcessAction(PlayerAction action, Command cmd)
        {
            if (action == PlayerAction.Pass)
            {
                game.Pass();
                game.PostRaund();
                game.NextPlayer();
            }
            else if (action == PlayerAction.TakeCards)
            {
                game.NextPlayer();
                game.TakeCards();
                game.PostRaund();
                game.NextPlayer();
            }
            else if (action == PlayerAction.ThrowCards)
            {
                game.GetPlayer(game.GetCurrentPlayerId()).SetCardsToUse(cmd.Cards);
                game.PutCards(game.GetPlayer(game.GetCurrentPlayerId()).UseCards());
                game.NextPlayer();
            }
            else if (action == PlayerAction.BeatCards)
            {
                game.GetPlayer(game.GetCurrentPlayerId()).SetCardsToUse(cmd.Cards);
                game.BeatCards(game.GetPlayer(game.GetCurrentPlayerId()).UseCards());
                game.NextPlayer();
            }
        }

        public GameState GetGameState()
        {
            return game.gameState;
        }
    }
}