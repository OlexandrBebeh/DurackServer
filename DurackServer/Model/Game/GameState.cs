using System.Collections.Generic;
using DurackServer.Model.DataType;

namespace DurackServer.Model.Game
{
    public class GameState
    {
        public DeckType DeckType { get; set; }
        public List<Player> Players { get; set; } = new ();
        public int CurrentPlayer { get; set; } = 0;
        public List<CardCouplet> FieldState { get; set; } = new();
        public HashSet<Rank> AllowedRanks { get; set; }= new();
    }
}