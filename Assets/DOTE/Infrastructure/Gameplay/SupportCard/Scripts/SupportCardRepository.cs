using DOTE.Gameplay.Domain.SupportCard;
using System.Collections.Generic;

namespace DOTE.Gameplay.Infrastructure.SupportCard
{
    public class SupportCardRepository : ISupportCardRepository
    {
        private Dictionary<string, ASupportCard> supportCardsMap;

        public SupportCardRepository()
        {
            supportCardsMap = new();
        }

        public void AddSupportCard(ASupportCard supportCard)
        {
            if (!supportCardsMap.ContainsKey(supportCard.SupportCardId))
            {
                supportCardsMap.Add(supportCard.SupportCardId, supportCard);
            }
        }
        public void RemoveSupportCard(string supportCardId)
        {
            if (supportCardsMap.ContainsKey(supportCardId))
            {
                supportCardsMap.Remove(supportCardId);
            }
        }
        public ASupportCard GetSupportCardById(string supportCardId)
        {
            supportCardsMap.TryGetValue(supportCardId, out ASupportCard supportCard);
            return supportCard;
        }
    }
}