using DOTE.Domain.SharedKernel;
using System.Collections.Generic;
using UnityEngine;

namespace DOTE.Domain.Gameplay.Character
{
    public class Character
    {
        public string CharacterId { get; private set; }
        public CharacterInformation CharacterInformation { get; private set; }

        public FloatLimitedCharacterCharacteristic Health { get; private set; }
        public IntLimitedCharacterCharacteristic Speed { get; private set; }

        public FloatCharacterCharacteristic PhysicalAttack { get; private set; }
        public FloatCharacterCharacteristic MagicalAttack { get; private set; }

        public FloatCharacterCharacteristic PhysicalDefence { get; private set; }
        public FloatCharacterCharacteristic MagicalDefence { get; private set; }

        public FloatLimitedCharacterCharacteristic CriticalDamageChance { get; private set; }
        public FloatCharacterCharacteristic CriticalDamageValue { get; private set; }
        public FloatCharacterCharacteristic PhysicalDamageMultiplier { get; private set; }
        public FloatCharacterCharacteristic MagicalDamageMultiplier { get; private set; }

        public FloatLimitedCharacterCharacteristic FreeAttackChance { get; private set; }
        public FloatLimitedCharacterCharacteristic AvoidDamageChance { get; private set; }

        public IntCharacterCharacteristic AttackRange { get; private set; }
        public FloatCharacterCharacteristic UseAbilityCost { get; private set; }

        public bool IgnoreCellMoveCost { get; private set; }
        public bool IgnoreSwampCellMoveCost { get; private set; }
        public bool CanUseAbilities { get; private set; }
        public bool CanAttack { get; private set; }
        public bool CanBeDamaged{ get; private set; }
        public Vector2 PositionOnField { get; }

        private ACharacterActiveAbility attackAbility;
        private ACharacterActiveAbility protectiveAbility;
        private ACharacterActiveAbility buffAbility;
        private ACharacterPassiveAbility passiveAbility;

        private bool ignorePysicalDamage;
        private bool ignoreMagicalDamage;

        private Dictionary<Class, float> attackMultiplierByClassMap;
        private Dictionary<Race, float> attackMultiplierByRaceMap;
        private Dictionary<Class, float> damageMultiplierByClassMap;
        private Dictionary<Race, float> damageMultiplierByRaceMap;

        private Dictionary<Class, bool> blockDamageFromClassMap;
        private Dictionary<Race, bool> blockDamageFromRaseMap;

        public Character(string characterId, CharacterInformation characterInformation, FloatLimitedCharacterCharacteristic health, IntLimitedCharacterCharacteristic speed, FloatCharacterCharacteristic physicalAttack, FloatCharacterCharacteristic magicalAttack, FloatCharacterCharacteristic physicalDefence, FloatCharacterCharacteristic magicalDefence, FloatLimitedCharacterCharacteristic criticalDamageChance, FloatCharacterCharacteristic criticalDamageValue, FloatCharacterCharacteristic physicalDamageMultiplier, FloatCharacterCharacteristic magicalDamageMultiplier, FloatLimitedCharacterCharacteristic freeAttackChance, FloatLimitedCharacterCharacteristic avoidDamageChance, IntCharacterCharacteristic attackRange, FloatCharacterCharacteristic useAbilityCost, ACharacterActiveAbility attackAbility, ACharacterActiveAbility protectiveAbility, ACharacterActiveAbility buffAbility, ACharacterPassiveAbility passiveAbility)
        {
            CharacterId = characterId;
            CharacterInformation = characterInformation;
            Health = health;
            Speed = speed;
            PhysicalAttack = physicalAttack;
            MagicalAttack = magicalAttack;
            PhysicalDefence = physicalDefence;
            MagicalDefence = magicalDefence;
            CriticalDamageChance = criticalDamageChance;
            CriticalDamageValue = criticalDamageValue;
            PhysicalDamageMultiplier = physicalDamageMultiplier;
            MagicalDamageMultiplier = magicalDamageMultiplier;
            FreeAttackChance = freeAttackChance;
            AvoidDamageChance = avoidDamageChance;
            AttackRange = attackRange;
            UseAbilityCost = useAbilityCost;
            this.attackAbility = attackAbility;
            this.protectiveAbility = protectiveAbility;
            this.buffAbility = buffAbility;
            this.passiveAbility = passiveAbility;

            SetCanAttack(true);
            SetCanUseAbilities(true);
            SetCanBeDamaged(true);

            attackMultiplierByClassMap = new();
            attackMultiplierByRaceMap = new();

            damageMultiplierByClassMap = new();
            damageMultiplierByRaceMap = new();

            blockDamageFromClassMap = new();
            blockDamageFromRaseMap = new();

            foreach (Race characterRace in Race.GetValues(typeof(Race)))
            {
                attackMultiplierByRaceMap.Add(characterRace, 1);
                damageMultiplierByRaceMap.Add(characterRace, 1);
                blockDamageFromRaseMap.Add(characterRace, false);
            }
            foreach (Class characterClass in Class.GetValues(typeof(Class)))
            {
                attackMultiplierByClassMap.Add(characterClass, 1);
                damageMultiplierByClassMap.Add(characterClass, 1);
                blockDamageFromClassMap.Add(characterClass, false);
            }
        }

        public void TakeDamageFromCharacter(float damage, Character attacker)
        {

        }

        public void TakeCleanDamage(float damage, string attackerName)
        {

        }

        public void Heal(float amount, bool ignoreMax)
        {

        }

        public void Move(Vector2 position, int moveCost)
        {

        }

        public void ResetCharacter()
        {

        }

        public void RemoveDebuffs()
        {

        }

        public void UseAtackAbility()
        {

        }

        public void UseProtectiveAbility()
        {

        }

        public void UseBuffAbility()
        {

        }

        public void CancelUsingCurrentAbility()
        {

        }

        public void EquipItem(string ItemID)
        {

        }

        public void RemoveItem(string ItemID)
        {

        }

        public void SetCanAttack(bool value) => CanAttack = value;
        public void SetCanUseAbilities(bool value) => CanUseAbilities = value;
        public void SetCanBeDamaged(bool value) => CanBeDamaged = value;
        public void SetIgnorePysicalDamage(bool value) => ignorePysicalDamage = value;
        public void SetIgnoreMagicalDamage(bool value) => ignoreMagicalDamage = value;
        public void SetIgnoreCellMoveCost(bool value) => IgnoreCellMoveCost = value;
        public void SetIgnoreSwampCellMoveCost(bool value) => IgnoreSwampCellMoveCost = value;


        public void IncreaseAttackMultiplierByClass(Class characterClass, float increaseValue)
        {
            if (attackMultiplierByClassMap.ContainsKey(characterClass))
            {
                attackMultiplierByClassMap[characterClass] = Mathf.Clamp(attackMultiplierByClassMap[characterClass] + increaseValue, 0, float.MaxValue);
            }
        }

        public void IncreaseAttackMultiplierByRaceMap(Race race, float increaseValue)
        {
            if (attackMultiplierByRaceMap.ContainsKey(race))
            {
                attackMultiplierByRaceMap[race] = Mathf.Clamp(attackMultiplierByRaceMap[race] + increaseValue, 0, float.MaxValue);
            }
        }

        public void IncreaseDamageMultiplierByClassMap(Class characterClass, float increaseValue)
        {
            if (damageMultiplierByClassMap.ContainsKey(characterClass))
            {
                damageMultiplierByClassMap[characterClass] = Mathf.Clamp(damageMultiplierByClassMap[characterClass] + increaseValue, 0, float.MaxValue);
            } 
        }

        public void IncreaseDamageMultiplierByRaceMap(Race race, float increaseValue)
        {
            if (damageMultiplierByRaceMap.ContainsKey(race))
            {
                damageMultiplierByRaceMap[race] = Mathf.Clamp(damageMultiplierByRaceMap[race] + increaseValue, 0, float.MaxValue);
            }
        }

        public void SetBlockDamageFromClass(Class characterClassace, bool value)
        {
            if (blockDamageFromClassMap.ContainsKey(characterClassace))
            {
                blockDamageFromClassMap[characterClassace] = value;
            }
        }

        public void SetBlockDamageFromRaseMap(Race race, bool value)
        {
            if (blockDamageFromRaseMap.ContainsKey(race))
            {
                blockDamageFromRaseMap[race] = value;
            }
        }

        private bool IsDamageAvoided()
        {
            float chance = Random.Range(0f, 1f);
            if (chance < AvoidDamageChance.CurrentValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private float IsCrit()
        {
            float chance = Random.Range(0f, 1f);
            if (chance < CriticalDamageChance.CurrentValue)
            {
                return CriticalDamageValue.CurrentValue;
            }
            else
            {
                return 1;
            }
        }
    }
}

