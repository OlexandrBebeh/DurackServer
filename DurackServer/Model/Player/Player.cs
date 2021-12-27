using System;
using System.Collections.Generic;
using DurackServer.Model.DataType;
using DurackServer.Model.Game;
using DurackServer.networking.PlayerIO;

namespace DurackServer.Model
{
    public abstract class Player
    {
        private int id;
        private String name;

        public List<CardType> hand = new();
        
        public bool HasCards()
        {
            return hand.Count > 0;
        }
        abstract public PlayerAction GetAction(Command cmd);

        abstract public List<CardType> UseCards();
        
        abstract public void TakeCards(List<CardType> cards);
        
        abstract public void SetCardsToUse(List<CardType> cards);

        abstract public void SetAction(PlayerAction action);

    }
}