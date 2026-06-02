using DOTE.Gameplay.Domain.Character;
using DOTE.SharedKernel.Infrastructure;

namespace DOTE.Gameplay.Infrastructure
{
    public interface ICharacterActiveAbilityBuilder
    {
        public abstract ACharacterActiveAbility Build(ACharacterActiveAbilityConfig config);
    }
    public abstract class ACharacterActiveAbilityBuilder<TAbility, TConfig>: ICharacterActiveAbilityBuilder where TAbility : ACharacterActiveAbility where TConfig : ACharacterActiveAbilityConfig
    {
        public abstract TAbility Build(TConfig config);

        ACharacterActiveAbility ICharacterActiveAbilityBuilder.Build(ACharacterActiveAbilityConfig config)
        {
            return Build(config as TConfig);
        }
    }
}