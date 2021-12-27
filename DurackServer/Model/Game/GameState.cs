using System.Collections.Generic;
using DurackServer.Model.DataType;

namespace DurackServer.Model.Game
{
    public class GameState
    {
        public DeckType deckType;
        public List<Player> players = new ();
        public int current_player = 0;
        public List<CardCouplet> fieldState = new();

        public HashSet<Rank> allowedRanks = new();
    }
}