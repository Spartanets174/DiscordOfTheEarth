using DOTE.Gameplay.Domain.SupportCard;
using DOTE.SharedKernel.Infrastructure;

namespace DOTE.Gameplay.Infrastructure
{
    public class EmptySupportCardBuilder : ASupportCardBuilder<EmptySupportCard, EmptySupportCardConfig>
    {
        public override EmptySupportCard Build(string id, EmptySupportCardConfig config)
        {
            return new EmptySupportCard(id,
                new SupportCardInformation(config.GetSupportCardName(),
                config.GetSupportCardAbilityDescription(),
                config.GetSupportCardType(),
                config.GetSupportCardRarity()));
        }
    }
}