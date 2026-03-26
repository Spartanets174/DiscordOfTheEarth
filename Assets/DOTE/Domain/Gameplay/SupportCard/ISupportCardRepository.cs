namespace DOTE.Gameplay.Domain.SupportCard
{
    public interface ISupportCardRepository
    {
        public void AddSupportCard(ASupportCard supportCard);
        public void RemoveSupportCard(string supportCardId);
        public ASupportCard GetSupportCardById(string supportCardId);
    }
}