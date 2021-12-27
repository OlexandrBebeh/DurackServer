using DurackServer.Exceptions;
using DurackServer.Model.Game;
using DurackServer.networking;

namespace DurackServer.Controller
{
    public class Controller
    {
        private Game game;
        
        public void StartGameRound()
        {
            int player = game.GetCurrentPlayerId();
            try
            {
                game.StartTransaction();
                
                var action = game.GetPlayer(player).GetAction();
                ProcessAction(action);
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

        private void ProcessAction(PlayerAction action)
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
                game.PutCards(game.GetPlayer(game.GetCurrentPlayerId()).UseCards());
                game.NextPlayer();
            }
            else if (action == PlayerAction.BeatCards)
            {
                game.BeatCards(game.GetPlayer(game.GetCurrentPlayerId()).UseCards());
                game.NextPlayer();
            }
        }
    }
}