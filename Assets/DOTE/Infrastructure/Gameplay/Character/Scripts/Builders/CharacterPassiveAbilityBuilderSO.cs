using DOTE.Gameplay.Domain.Character;
using DOTE.SharedKernel.Domain;
using DOTE.SharedKernel.Infrastructure.Character;
using System;
using UnityEngine;

namespace DOTE.Gameplay.Infrastructure.Character
{
    public abstract class CharacterPassiveAbilityBuilderSO : ScriptableObject
    {
        public abstract CharacterPassiveAbilityBuilder CreateBuilder();
    }

    public abstract class GCharacterPassiveAbilityBuilderSO<TConfig, TBuilder> : CharacterPassiveAbilityBuilderSO where TConfig : CharacterPassiveAbilityConfig where TBuilder: CharacterPassiveAbilityBuilder
    {
        [SerializeField]
        protected TConfig passiveAbilityConfig;

        public override CharacterPassiveAbilityBuilder CreateBuilder()
        {
            return CreateBuilderHook();
        }

        protected abstract TBuilder CreateBuilderHook();
    }

    public abstract class CharacterPassiveAbilityBuilder : ABuilder<ACharacterPassiveAbility>
    {
        protected CharacterPassiveAbilityBuilder(Type type) : base(type)
        {
        }
    }

    public abstract class GCharacterPassiveAbilityBuilder<TAbility, TConfig> : CharacterPassiveAbilityBuilder where TAbility : ACharacterPassiveAbility where TConfig : CharacterPassiveAbilityConfig
    {
        private TConfig passiveAbilityConfig;
        protected GCharacterPassiveAbilityBuilder(Type type, TConfig passiveAbilityConfig) : base(type)
        {
            this.passiveAbilityConfig = passiveAbilityConfig;
        }
        public override ACharacterPassiveAbility Build()
        {
            return BuildHook(passiveAbilityConfig);
        }

        protected abstract TAbility BuildHook(TConfig passiveAbilityConfig);
    }

}