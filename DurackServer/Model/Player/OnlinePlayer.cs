using System;
using System.Collections.Generic;
using DurackServer.Model.DataType;
using DurackServer.Model.Game;

namespace DurackServer.Model
{
    public class OnlinePlayer : Player
    {
        public override PlayerAction GetAction()
        {
            return PlayerAction.Pass;
        }

        public override List<CardType> UseCards()
        {
            
            throw new NotImplementedException();
        }

        public override void TakeCards(List<CardType> cards)
        {
            throw new NotImplementedException();
        }
    }
}