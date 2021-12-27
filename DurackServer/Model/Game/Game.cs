using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DurackServer.Model.DataType;

namespace DurackServer.Model.Game
{
    public class Game
    {
        private static int CardsInHand = 6;
        public GameState gameState;
        public int playerTurn;
        
        public Game()
        {
            gameState.deckType = new DeckType();
        }

        public DeckType GetGeck()
        {
            return gameState.deckType;
        }
        public Player GetPlayer(int p)
        {
            if (gameState.players.Count > p)
                return gameState.players[p];
            
            //exeption
            return gameState.players[0];
        }
        public void NextPlayer()
        {
            gameState.current_player += 1;
            gameState.current_player = gameState.current_player % gameState.players.Capacity;
        }
        
        public int GetNextPlayerId()
        {
            return (gameState.current_player + 1) % gameState.players.Capacity;
        }
        
        public int GetCurrentPlayerId()
        {
            return gameState.current_player;
        }

        public void PutCards(List<CardType> cards)
        {
            if (cards.Count == 0)
            {
                return;
            }
            
            if (gameState.allowedRanks.Count == 0)
            {
                gameState.allowedRanks.Add(cards[0].rank);
            }
            
            if (cards.TrueForAll(x => gameState.allowedRanks.Contains(x.rank)))
            {
                CreateCardForAttack(cards);
            }
            else
            {
                //Add exception
            }

            RemoveCardFromHand(GetCurrentPlayerId(), cards);
        }

        public void BeatCards(List<CardType> cards)
        {
            if (cards.Count == 0)
            {
                return;
            }

            if (cards.Count == gameState.fieldState.FindAll(x => x.beatCard == null).Count)
            {
                // Exeption
                return;
            }

            int i = 0;
            foreach (var couplet in gameState.fieldState)
            {
                if (couplet.beatCard != null)
                {
                    if (gameState.deckType.TryBeat(couplet.beatCard,cards[i]))
                    {
                        couplet.AddBeatCard(cards[i]);
                    }
                    else
                    {
                        //exception
                    }
                    i++;
                }
            }
            
            RemoveCardFromHand(GetNextPlayerId(), cards);
        }

        public void Pass()
        {
            gameState.fieldState.Clear();
        }
        
        private void CreateCardForAttack(List<CardType> cards)
        {
            foreach (var card in cards)
            {
                gameState.allowedRanks.Add(card.rank);
                gameState.fieldState.Add(new CardCouplet(card));
            }
        }

        public void RemoveCardFromHand(int player, List<CardType> cards)
        {
            foreach (var card in cards)
            {
                if (gameState.players[player].hand.Contains(card))
                {
                    gameState.players[player].hand.Remove(card);
                }
                else
                {
                    //exception
                    return;
                }
            }
        }

        public void TakeCards()
        {
            List<CardType> cards = new ();
            var b = true;
            
            foreach (var couplet in gameState.fieldState)
            {
                cards.Add(couplet.firstCard);

                if (couplet.beatCard != null)
                {
                    cards.Add(couplet.firstCard);
                }
                else
                {
                    b = false;
                }
            }

            if (b)
            {
                //exeption
            }
            
            gameState.players[GetCurrentPlayerId()].TakeCards(cards);
        }

        public void PostRaund()
        {
            for (int i = 0; i < gameState.players.Count && gameState.deckType.GetCardsAmount() > 0; i++)
            {
                var cardAmount = Math.Min(
                    gameState.deckType.GetCardsAmount(),
                    CardsInHand - GetPlayer(GetNextPlayerId()).hand.Count);
                
                GetPlayer((GetCurrentPlayerId() + i) % gameState.players.Count)
                    .hand.AddRange(gameState.deckType.RemoveNTop(cardAmount));
            }
        }

        public int CheckWin()
        {
            if (gameState.deckType.GetCardsAmount() == 0)
            {
                for (int i = 0; i < gameState.players.Count; i++)
                {
                    if (gameState.players[i].hand.Count == 0)
                    {
                        return i;
                    }
                }  
            }
            
            return -1;
        }
    }
}