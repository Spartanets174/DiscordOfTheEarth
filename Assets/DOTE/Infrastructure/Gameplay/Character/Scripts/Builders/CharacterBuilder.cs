using DOTE.Gameplay.Domain.Character;
using DOTE.SharedKernel.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DOTE.Gameplay.Infrastructure
{
    public class CharacterBuilder
    {
        private Dictionary<Type, ICharacterPassiveAbilityBuilder> passiveAbilityBuildersMap;
        private Dictionary<Type, ICharacterActiveAbilityBuilder> activeAbilityBuildersMap;

        public CharacterBuilder()
        {
            BuildPassiveAbilityBuildersMap();
            BuildActiveAbilityBuildersMap();
        }

        public PlayableCharacter BuildCharacter(CharacterConfig characterConfig, string ownerId, Dictionary<Type, int> defaultMoveCostByCellType)
        {
            CharacterInformationConfig characterInformationConfig = characterConfig.GetCharacterInformationConfig();

            return new(characterInformationConfig.GetCharacterId(),
                ownerId,
                new CharacterInformation(characterInformationConfig.GetCharacterName(),
                characterInformationConfig.GetCharacterDescription(),
                characterInformationConfig.GetCharacterRace(),
                characterInformationConfig.GetCharacterClass(),
                characterInformationConfig.GetCharacterRarity()),
                new FloatLimitedCharacterCharacteristic(characterInformationConfig.GetHealth()),
                new IntLimitedCharacterCharacteristic(characterInformationConfig.GetSpeed()),
                new FloatCharacterCharacteristic(characterInformationConfig.GetPhysicalAttack()),
                new FloatCharacterCharacteristic(characterInformationConfig.GetMagicalAttack()),
                new FloatCharacterCharacteristic(characterInformationConfig.GetPhysicalDefence()),
                new FloatCharacterCharacteristic(characterInformationConfig.GetMagicalDefence()),
                new FloatLimitedCharacterCharacteristic(1, characterInformationConfig.GetCriticalDamageChance()),
                new FloatCharacterCharacteristic(characterInformationConfig.GetCriticalDamageValue()),
                new FloatCharacterCharacteristic(1),
                new FloatCharacterCharacteristic(1),
                new FloatLimitedCharacterCharacteristic(1, 0),
                new FloatLimitedCharacterCharacteristic(1, 0),
                new IntCharacterCharacteristic(characterInformationConfig.GetAttackRange()),
                new IntCharacterCharacteristic(characterConfig.GetDefaultAbilityUsingCost()),
                new IntCharacterCharacteristic(characterConfig.GetDefaultAttackCost()),
                BuildActiveAbility(characterConfig.GetAttackAbilityConfig()),
                BuildActiveAbility(characterConfig.GetProtectiveAbilityConfig()),
                BuildActiveAbility(characterConfig.GetEnchancingAbilityConfig()),
                BuildPassiveAbility(characterConfig.GetPassiveAbilityConfig()),
                defaultMoveCostByCellType);
        }

        private ACharacterPassiveAbility BuildPassiveAbility(ACharacterPassiveAbilityConfig passiveAbilityConfig)
        {
            if (passiveAbilityBuildersMap.TryGetValue(passiveAbilityConfig.GetType(), out ICharacterPassiveAbilityBuilder builder))
            {
                return builder.Build(passiveAbilityConfig);
            }
            else
            {
                return null;
            }
        }

        private ACharacterActiveAbility BuildActiveAbility(ACharacterActiveAbilityConfig attackAbilityConfig)
        {
            if (activeAbilityBuildersMap.TryGetValue(attackAbilityConfig.GetType(), out ICharacterActiveAbilityBuilder builder))
            {
                return builder.Build(attackAbilityConfig);
            }
            else
            {
                return null;
            }
        }

        private void BuildPassiveAbilityBuildersMap()
        {
            passiveAbilityBuildersMap = new();
            List<Type> derivedTypes = Assembly.GetExecutingAssembly()
             .GetTypes()
             .Where(t => t.GetInterfaces().Contains(typeof(ICharacterPassiveAbilityBuilder)) && !t.IsAbstract)
             .ToList();
            foreach (var type in derivedTypes)
            {
                ICharacterPassiveAbilityBuilder builder = (ICharacterPassiveAbilityBuilder)Activator.CreateInstance(type);
                passiveAbilityBuildersMap.TryAdd(builder.GetType().GetGenericArguments()[1], builder);
            }
        }

        private void BuildActiveAbilityBuildersMap()
        {
            activeAbilityBuildersMap = new();
            List<Type> derivedTypes = Assembly.GetExecutingAssembly()
             .GetTypes()
             .Where(t => t.GetInterfaces().Contains(typeof(ICharacterActiveAbilityBuilder)) && !t.IsAbstract)
             .ToList();
            foreach (var type in derivedTypes)
            {
                ICharacterActiveAbilityBuilder builder = (ICharacterActiveAbilityBuilder)Activator.CreateInstance(type);
                activeAbilityBuildersMap.TryAdd(builder.GetType().GetGenericArguments()[1], builder);
            }
        }
    }
}