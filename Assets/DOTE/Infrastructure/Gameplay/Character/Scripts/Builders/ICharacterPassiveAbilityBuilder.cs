using DOTE.Gameplay.Domain.Character;
using DOTE.SharedKernel.Infrastructure;

namespace DOTE.Gameplay.Infrastructure
{
    public interface ICharacterPassiveAbilityBuilder
    {
        public ACharacterPassiveAbility Build(ACharacterPassiveAbilityConfig config);
    }

    public abstract class ACharacterPassiveAbilityBuilder<TAbility, TConfig> : ICharacterPassiveAbilityBuilder where TAbility : ACharacterPassiveAbility where TConfig : ACharacterPassiveAbilityConfig
    {
        public abstract TAbility Build(TConfig config);

        ACharacterPassiveAbility ICharacterPassiveAbilityBuilder.Build(ACharacterPassiveAbilityConfig config)
        {
            return Build(config as TConfig);
        }
    }
}