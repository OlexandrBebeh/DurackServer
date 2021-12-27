using System;
using System.Collections.Generic;
using DurackServer.Model.DataType;
using DurackServer.Model.Game;

namespace DurackServer.Model
{
    public class OnlinePlayer : Player
    {
        private List<CardType> cardsToUse;
        private PlayerAction action;
        public override PlayerAction GetAction()
        {
            return PlayerAction.Pass;
        }

        public override List<CardType> UseCards()
        {
            return cardsToUse;
        }

        public override void TakeCards(List<CardType> cards)
        {
            hand.AddRange(cards);
        }

        public override void SetCardsToUse(List<CardType> cards)
        {
            cardsToUse = cards;
        }

        public override void SetAction(PlayerAction _action)
        {
            action = _action;
        }
    }
}