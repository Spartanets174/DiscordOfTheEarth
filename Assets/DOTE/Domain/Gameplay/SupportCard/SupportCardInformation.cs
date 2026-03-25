using DOTE.SharedKernel.Domain;

namespace DOTE.Gameplay.Domain.SupportCard
{
    public class SupportCardInformation
    {
        private string supportCardName;
        private string supportCardAbilityDescription;
        private SupportCardType supportCardType;
        private Rarity supportCardRarity;

        public SupportCardInformation(string supportCardName, string supportCardAbilityDescription, SupportCardType supportCardType, Rarity supportCardRarity)
        {
            this.supportCardName = supportCardName;
            this.supportCardAbilityDescription = supportCardAbilityDescription;
            this.supportCardType = supportCardType;
            this.supportCardRarity = supportCardRarity;
        }

        public string GetSupportCardName() => supportCardName;
        public string GetSupportCardAbilityDescription() => supportCardAbilityDescription;
        public SupportCardType GetSupportCardType() => supportCardType;
        public Rarity GetSupportCardRarity() => supportCardRarity;
    }
}