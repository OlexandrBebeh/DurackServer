namespace DurackServer.Model.DataType
{
    public class CardCouplet
    {
        public CardType firstCard;
        public CardType? beatCard;

        public CardCouplet(CardType card)
        {
            firstCard = card;
        }

        public void AddBeatCard(CardType card)
        {
            beatCard = card;
        }
    }
}