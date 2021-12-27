using System;
using System.Collections.Generic;
using System.Linq;

namespace DurackServer.Model.DataType
{
    public class DeckType
    {
        private static int deckSize = 32;
        private List<CardType> deck = new();
        private Suit trump;
        private CardType bottomCard;
        public DeckType()
        {
            foreach (Suit suit in (Suit[]) Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in (Rank[]) Enum.GetValues(typeof(Rank)))
                {
                    deck.Add(new CardType(rank,suit));
                }
            }

            var rand = new Random();
            
            trump = (Suit)(rand.Next() % 4);

            deck = deck.OrderBy(x=> rand.Next()).ToList();

            bottomCard = deck[deck.Count - 1];

            trump = bottomCard.Suit;
        }

        public Suit GetTrump()
        {
            return trump;
        }

        public CardType GetBotomCard()
        {
            return bottomCard;
        }
        
        public int GetCardsAmount()
        {
            return deck.Count;
        }
        public CardType RemoveTop()
        {
            if (deck.Count == 0)
            {
               //exception
            }
            var card = deck[0];

            deck.Remove(card);

            return card;
        }
        
        public List<CardType> RemoveNTop(int n)
        {
            var lst = new List<CardType>();
            for (int i = 0; i < n && deck.Count > 0; i++)
            {
                lst.Add(deck[0]);
                deck.Remove(deck[0]);
            }

            return lst;
        }
        
        public bool TryBeat(CardType first, CardType second)
        {
            if (first.Suit == second.Suit)
            {
                return first.Rank < second.Rank;
            }

            if (second.Suit == GetTrump())
            {
                if (first.Suit == GetTrump())
                {
                    return first.Rank < second.Rank;
                }
                return true;
            }

            return false;
        }
    }
}