namespace DurackServer.Model.DataType
{
    public class CardType
    {
        public Rank rank;
        public Suit suit;

        public CardType(Rank _rank, Suit _suit)
        {
            rank = _rank;
            suit = _suit;
        }
        
        
    }
}