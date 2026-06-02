using DOTE.Gameplay.Domain.Character;
using DOTE.SharedKernel.Infrastructure;

namespace DOTE.Gameplay.Infrastructure
{
    public class EmptyCharacterPassiveAbilityBuilder : ACharacterPassiveAbilityBuilder<EmptyCharacterPassiveAbility, EmptyCharacterPassiveAbilityConfig>
    {
        public override EmptyCharacterPassiveAbility Build(EmptyCharacterPassiveAbilityConfig config)
        {
            return new EmptyCharacterPassiveAbility(new(config.GetAbilityName(), config.GetAbilityDescription()));
        }
    }
}