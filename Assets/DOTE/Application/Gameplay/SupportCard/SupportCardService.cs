using DOTE.Gameplay.Domain.SupportCard;

namespace DOTE.Gameplay.Application.SupportCard
{
    public class SupportCardService
    {
        private ISupportCardRepository supportCardRepository;

        public SupportCardService(ISupportCardRepository supportCardRepository)
        {
            this.supportCardRepository = supportCardRepository;
        }

        public void UseSupportCard(string supportCardId)
        {
            ASupportCard supportCard = supportCardRepository.GetSupportCardById(supportCardId);

            supportCard.UseSupportCard();
        }

        public void CancelUsingSupportCard(string supportCardId)
        {
            ASupportCard supportCard = supportCardRepository.GetSupportCardById(supportCardId);

            supportCard.CancelUsingSupportCard();
        }
    }
}