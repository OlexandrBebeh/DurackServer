namespace DurackServer.Model.DataType
{
    public class CardType
    {
        public Rank Rank { get; set; }
        public Suit Suit { get; set; }

        public CardType(Rank rank, Suit suit)
        {
            Rank = rank;
            Suit = suit;
        }
        
        
    }
}