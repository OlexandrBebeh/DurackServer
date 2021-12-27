using System;
using System.Collections.Generic;
using DurackServer.Model.DataType;
using DurackServer.Model.Game;

namespace DurackServer.Model
{
    public abstract class Player
    {
        private int id;
        private String name;

        public List<CardType> hand;
        
        public bool HasCards()
        {
            return hand.Count > 0;
        }
        abstract public PlayerAction GetAction();

        abstract public List<CardType> UseCards();
        
        abstract public void TakeCards(List<CardType> cards);
        
        abstract public void SetCardsToUse(List<CardType> cards);

        abstract public void SetAction(PlayerAction action);

    }
}