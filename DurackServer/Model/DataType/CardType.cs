using System;

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

        public override bool Equals(object? obj)
        {
            if (obj is not CardType otherCard) return false;
            return Rank == otherCard.Rank && Suit == otherCard.Suit;
        }

        protected bool Equals(CardType other)
        {
            return Rank == other.Rank && Suit == other.Suit;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int) Rank, (int) Suit);
        }
    }
}