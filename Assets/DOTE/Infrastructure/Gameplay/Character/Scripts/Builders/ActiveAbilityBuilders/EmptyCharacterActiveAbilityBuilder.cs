using DOTE.Gameplay.Domain.Character;
using DOTE.SharedKernel.Infrastructure;

namespace DOTE.Gameplay.Infrastructure
{
    public class EmptyCharacterActiveAbilityBuilder : ACharacterActiveAbilityBuilder<EmptyCharacterActiveAbility, EmptyCharacterActiveAbilityConfig>
    {
        public override EmptyCharacterActiveAbility Build(EmptyCharacterActiveAbilityConfig config)
        {
            return new EmptyCharacterActiveAbility(new CharacterActiveAbilityInformation(config.GetAbilityName(),
                config.GetAbilityDescription(),
                config.GetActiveAbilityType()));
        }
    }
}