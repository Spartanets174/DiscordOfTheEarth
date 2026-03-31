using DOTE.Gameplay.Domain.Character;
using DOTE.SharedKernel.Domain;
using DOTE.SharedKernel.Infrastructure.Character;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DOTE.Gameplay.Infrastructure.Character
{
    [CreateAssetMenu(fileName = "CharacterBuilderSO", menuName = "DOTE/Gameplay/Character/CharacterBuilderSO")]
    public class CharacterBuilderSO : ScriptableObject
    {
        [SerializeField]
        private CharacterConfig characterConfig;
        [SerializeField]
        private CharacterActiveAbilityBuilderSO attackAbilityBuilderSO;
        [SerializeField]
        private CharacterActiveAbilityBuilderSO protectiveAbilityBuilderSO;
        [SerializeField]
        private CharacterActiveAbilityBuilderSO enchancingAbilityBuilderSO;
        [SerializeField]
        private CharacterPassiveAbilityBuilderSO passiveAbilityBuilderSO;

        [SerializeField]
        private int defaultAbilityUsingCost = 11;
        [SerializeField]
        private int defaultAttackCost = 2;

        public CharacterBuilder CreateBuilder(string characterId, string ownerId, Dictionary<Type, int> defaultMoveCostByCellType)
        {
            return new(typeof(PlayableCharacter),
                characterId,
                ownerId,
                characterConfig,
                attackAbilityBuilderSO.CreateBuilder(),
                protectiveAbilityBuilderSO.CreateBuilder(),
                enchancingAbilityBuilderSO.CreateBuilder(),
                passiveAbilityBuilderSO.CreateBuilder(),
                defaultAbilityUsingCost,
                defaultAttackCost,
                defaultMoveCostByCellType);
        }
    }

    public class CharacterBuilder : ABuilder<PlayableCharacter>
    {
        private string characterId;
        private string ownerId;

        private CharacterConfig characterConfig;
        private CharacterActiveAbilityBuilder attackAbilityBuilder;
        private CharacterActiveAbilityBuilder protectiveAbilityBuilder;
        private CharacterActiveAbilityBuilder enchancingAbilityBuilder;
        private CharacterPassiveAbilityBuilder passiveAbilityBuilder;

        private int defaultAbilityUsingCost;
        private int defaultAttackCost;

        private Dictionary<Type, int> defaultMoveCostByCellType;

        public CharacterBuilder(Type type, string characterId, string ownerId, CharacterConfig characterConfig, CharacterActiveAbilityBuilder attackAbilityBuilder, CharacterActiveAbilityBuilder protectiveAbilityBuilder, CharacterActiveAbilityBuilder enchancingAbilityBuilder, CharacterPassiveAbilityBuilder passiveAbilityBuilder, int defaultAbilityUsingCost, int defaultAttackCost, Dictionary<Type, int> defaultMoveCostByCellType) : base(type)
        {
            this.characterId = characterId;
            this.ownerId = ownerId;
            this.characterConfig = characterConfig;
            this.attackAbilityBuilder = attackAbilityBuilder;
            this.protectiveAbilityBuilder = protectiveAbilityBuilder;
            this.enchancingAbilityBuilder = enchancingAbilityBuilder;
            this.passiveAbilityBuilder = passiveAbilityBuilder;
            this.defaultAbilityUsingCost = defaultAbilityUsingCost;
            this.defaultAttackCost = defaultAttackCost;
            this.defaultMoveCostByCellType = defaultMoveCostByCellType;
        }

        public override PlayableCharacter Build()
        {
            return new(characterId,
                ownerId,
                new CharacterInformation(characterConfig.GetCharacterName(),
                characterConfig.GetCharacterDescription(),
                characterConfig.GetCharacterRace(),
                characterConfig.GetCharacterClass(),
                characterConfig.GetCharacterRarity()),
                new FloatLimitedCharacterCharacteristic(characterConfig.GetHealth()),
                new IntLimitedCharacterCharacteristic(characterConfig.GetSpeed()),
                new FloatCharacterCharacteristic(characterConfig.GetPhysicalAttack()),
                new FloatCharacterCharacteristic(characterConfig.GetMagicalAttack()),
                new FloatCharacterCharacteristic(characterConfig.GetPhysicalDefence()),
                new FloatCharacterCharacteristic(characterConfig.GetMagicalDefence()),
                new FloatLimitedCharacterCharacteristic(1, characterConfig.GetCriticalDamageChance()),
                new FloatCharacterCharacteristic(characterConfig.GetCriticalDamageValue()),
                new FloatCharacterCharacteristic(1),
                new FloatCharacterCharacteristic(1),
                new FloatLimitedCharacterCharacteristic(1, 0),
                new FloatLimitedCharacterCharacteristic(1, 0),
                new IntCharacterCharacteristic(characterConfig.GetAttackRange()),
                new IntCharacterCharacteristic(defaultAbilityUsingCost),
                new IntCharacterCharacteristic(defaultAttackCost),
                attackAbilityBuilder.Build(),
                protectiveAbilityBuilder.Build(),
                enchancingAbilityBuilder.Build(),
                passiveAbilityBuilder.Build(),
                defaultMoveCostByCellType);
        }

    }
}