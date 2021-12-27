using System;
using System.Collections.Generic;
using DurackServer.Exceptions;
using DurackServer.Model.DataType;

namespace DurackServer.Model.Game
{
    public class Game
    {
        private static int CardsInHand = 6;
        public GameState gameState = new ();
        public int playerTurn;
        public GameState prevGameState;
        public Game()
        {
            gameState.DeckType = new DeckType();
        }

        public DeckType GetDeck()
        {
            return gameState.DeckType;
        }
        
        public void StartTransaction()
        {
            prevGameState = gameState;
        }
        
        public void UndoTransaction()
        {
            gameState = prevGameState;
        }
        public Player GetPlayer(int p)
        {
            if (gameState.Players.Count > p)
                return gameState.Players[p];

            throw new GameException("Invalid player");
        }
        public void NextPlayer()
        {
            gameState.CurrentPlayer += 1;
            gameState.CurrentPlayer = gameState.CurrentPlayer % gameState.Players.Capacity;
        }
        
        public int GetNextPlayerId()
        {
            return (gameState.CurrentPlayer + 1) % gameState.Players.Capacity;
        }
        
        public int GetCurrentPlayerId()
        {
            return gameState.CurrentPlayer;
        }

        public void PutCards(List<CardType> cards)
        {
            if (cards.Count == 0)
            {
                throw new GameException("Not valid amount of cards");
            }
            
            if (gameState.AllowedRanks.Count == 0)
            {
                gameState.AllowedRanks.Add(cards[0].Rank);
            }
            
            if (cards.TrueForAll(x => gameState.AllowedRanks.Contains(x.Rank)))
            {
                CreateCardForAttack(cards);
            }
            else
            {
                throw new GameException("Not valid rank");
            }

            RemoveCardFromHand(GetCurrentPlayerId(), cards);
        }

        public void BeatCards(List<CardType> cards)
        {
            if (cards.Count == 0)
            {
                return;
            }

            if (cards.Count == gameState.FieldState.FindAll(x => x.beatCard == null).Count)
            {
                throw new GameException("Not valid amount of cards");
            }

            int i = 0;
            foreach (var couplet in gameState.FieldState)
            {
                if (couplet.beatCard != null)
                {
                    if (gameState.DeckType.TryBeat(couplet.beatCard,cards[i]))
                    {
                        couplet.AddBeatCard(cards[i]);
                    }
                    else
                    {
                        throw new GameException("Card dont beat");
                    }
                    i++;
                }
            }
            
            RemoveCardFromHand(GetNextPlayerId(), cards);
        }

        public void Pass()
        {
            gameState.FieldState.Clear();
        }
        
        private void CreateCardForAttack(List<CardType> cards)
        {
            foreach (var card in cards)
            {
                gameState.AllowedRanks.Add(card.Rank);
                gameState.FieldState.Add(new CardCouplet(card));
            }
        }

        public void RemoveCardFromHand(int player, List<CardType> cards)
        {
            foreach (var card in cards)
            {
                if (gameState.Players[player].hand.Contains(card))
                {
                    gameState.Players[player].hand.Remove(card);
                }
                else
                {
                    throw new GameException("Try to use card not in hand");
                }
            }
        }

        public void TakeCards()
        {
            List<CardType> cards = new ();
            var b = true;
            
            foreach (var couplet in gameState.FieldState)
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
                throw new GameException("Try to use card not in hand");
            }
            
            gameState.Players[GetCurrentPlayerId()].TakeCards(cards);
        }

        public void PostRaund()
        {
            for (int i = 0; i < gameState.Players.Count && gameState.DeckType.GetCardsAmount() > 0; i++)
            {
                var cardAmount = Math.Min(
                    gameState.DeckType.GetCardsAmount(),
                    CardsInHand - GetPlayer((GetCurrentPlayerId() + i) % gameState.Players.Count).hand.Count);
                
                GetPlayer((GetCurrentPlayerId() + i) % gameState.Players.Count)
                    .hand.AddRange(gameState.DeckType.RemoveNTop(cardAmount));
            }
        }

        public int CheckWin()
        {
            if (gameState.DeckType.GetCardsAmount() == 0)
            {
                for (int i = 0; i < gameState.Players.Count; i++)
                {
                    if (gameState.Players[i].hand.Count == 0)
                    {
                        return i;
                    }
                }  
            }
            
            return -1;
        }
    }
}