using DOTE.Gameplay.Domain.Character;
using DOTE.SharedKernel.Domain;
using DOTE.SharedKernel.Infrastructure.Character;
using System;
using UnityEngine;

namespace DOTE.Gameplay.Infrastructure.Character
{
    public abstract class CharacterActiveAbilityBuilderSO 
    {
        public abstract CharacterActiveAbilityBuilder CreateBuilder();
    }

    public abstract class GCharacterActiveAbilityBuilderSO<TConfig, TBuilder> : CharacterActiveAbilityBuilderSO where TConfig : CharacterActiveAbilityConfig where TBuilder : CharacterActiveAbilityBuilder
    {
        [SerializeField]
        protected TConfig activeAbilityConfig;

        public override CharacterActiveAbilityBuilder CreateBuilder()
        {
            return CreateBuilderHook();
        }

        protected abstract TBuilder CreateBuilderHook();
    }

    public abstract class CharacterActiveAbilityBuilder : ABuilder<ACharacterActiveAbility>
    {
        protected CharacterActiveAbilityBuilder(Type type) : base(type)
        {
        }
    }

    public abstract class GCharacterActiveAbilityBuilder<TAbility, TConfig> : CharacterActiveAbilityBuilder where TAbility : ACharacterActiveAbility where TConfig : CharacterPassiveAbilityConfig
    {
        private TConfig activeAbilityConfig;
        protected GCharacterActiveAbilityBuilder(Type type, TConfig activeAbilityConfig) : base(type)
        {
            this.activeAbilityConfig = activeAbilityConfig;
        }

        public override ACharacterActiveAbility Build()
        {
            return BuildHook(activeAbilityConfig);
        }
        protected abstract TAbility BuildHook(TConfig activeAbilityConfig);
    }
}