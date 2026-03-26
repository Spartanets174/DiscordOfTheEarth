using DOTE.Gameplay.Domain.Item;
using DOTE.SharedKernel.Domain;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace DOTE.Gameplay.Domain.Character
{
    public class PlayableCharacter
    {
        public string CharacterId { get; private set; }
        public string OwnerId { get; private set; }
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

        public bool IsCharacterDied;
        public bool IgnoreCellMoveCost { get; private set; }
        public bool IgnoreSwampCellMoveCost { get; private set; }
        public bool CanUseAbilities { get; private set; }
        public bool CanAttack { get; private set; }
        public bool CanBeDamaged { get; private set; }
        public (int, int) PositionOnField { get; private set; }

        private ACharacterPassiveAbility passiveAbility;

        private ACharacterActiveAbility attackAbility;
        private ACharacterActiveAbility protectiveAbility;
        private ACharacterActiveAbility enchancingAbility;

        private ACharacterActiveAbility currentUsingAbility;

        private bool isAttackedOnMove;
        private bool ignorePysicalDamage;
        private bool ignoreMagicalDamage;

        [Inject]
        private IDomainEventBus eventBus;

        private List<string> equipedItemsIds;

        private Dictionary<Class, float> attackMultiplierByClassMap;
        private Dictionary<Race, float> attackMultiplierByRaceMap;
        private Dictionary<Class, float> damageMultiplierByClassMap;
        private Dictionary<Race, float> damageMultiplierByRaceMap;

        private Dictionary<Class, bool> blockDamageFromClassMap;
        private Dictionary<Race, bool> blockDamageFromRaseMap;

        public PlayableCharacter(string characterId, string ownerId, CharacterInformation characterInformation, FloatLimitedCharacterCharacteristic health, IntLimitedCharacterCharacteristic speed, FloatCharacterCharacteristic physicalAttack, FloatCharacterCharacteristic magicalAttack, FloatCharacterCharacteristic physicalDefence, FloatCharacterCharacteristic magicalDefence, FloatLimitedCharacterCharacteristic criticalDamageChance, FloatCharacterCharacteristic criticalDamageValue, FloatCharacterCharacteristic physicalDamageMultiplier, FloatCharacterCharacteristic magicalDamageMultiplier, FloatLimitedCharacterCharacteristic freeAttackChance, FloatLimitedCharacterCharacteristic avoidDamageChance, IntCharacterCharacteristic attackRange, FloatCharacterCharacteristic useAbilityCost, ACharacterActiveAbility attackAbility, ACharacterActiveAbility protectiveAbility, ACharacterActiveAbility enchancingAbility, ACharacterPassiveAbility passiveAbility)
        {
            CharacterId = characterId;
            OwnerId = ownerId;
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
            this.enchancingAbility = enchancingAbility;
            this.passiveAbility = passiveAbility;

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

            this.attackAbility.SetAbilityOwner(this);
            this.protectiveAbility.SetAbilityOwner(this);
            this.enchancingAbility.SetAbilityOwner(this);
            this.passiveAbility.SetAbilityOwner(this);

            SetCanAttack(true);
            SetCanUseAbilities(true);
            SetCanBeDamaged(true);
        }

        public void Attack(PlayableCharacter target)
        {
            if (CanAttack && !isAttackedOnMove)
            {
                target.TakeDamage(this);
                isAttackedOnMove = true;
            }
        }

        public void TakeDamage(float damage, ACharacterActiveAbility characterActiveAbility)
        {
            float finalPhysicalDamage = CalculateFinalDamageByFormula(damage, PhysicalDefence.CurrentValue) * PhysicalDamageMultiplier.CurrentValue;
            float finalMagicalDamage = CalculateFinalDamageByFormula(damage, MagicalDefence.CurrentValue) * MagicalDamageMultiplier.CurrentValue;
            float finalDamage = Mathf.Max(finalPhysicalDamage, finalMagicalDamage) * CalculateCriticalDamageMultiplier(characterActiveAbility.AbilityOwner);

            if (finalDamage == 0)
            {
                finalDamage = Random.Range(0.01f, 0.1f);
            }

            TakeDamage(finalDamage, characterActiveAbility.AbilityInfo.GetAbilityName());
        }

        public void TakeDamage(PlayableCharacter attacker)
        {
            if (blockDamageFromClassMap[attacker.CharacterInformation.GetCharacterClass()])
            {
                eventBus.Publish(new CharacterDamaged(CharacterId, attacker.CharacterInformation.GetCharacterName(), 0));
            }

            float finalPhysicalDamage = ignorePysicalDamage ? 0 : CalculateFinalDamageByFormula(attacker.PhysicalAttack.CurrentValue, PhysicalDefence.CurrentValue) * PhysicalDamageMultiplier.CurrentValue;
            float finalMagicalDamage = ignoreMagicalDamage ? 0 : CalculateFinalDamageByFormula(attacker.MagicalAttack.CurrentValue, MagicalDefence.CurrentValue) * MagicalDamageMultiplier.CurrentValue;
            float finalDamage = Mathf.Max(finalPhysicalDamage, finalMagicalDamage) * CalculateDamageMultipliers(attacker);

            TakeDamage(finalDamage, attacker.CharacterInformation.GetCharacterName());
        }

        public void TakeDamage(float damage, string attackerName)
        {
            if (!CanBeDamaged || IsDamageAvoided())
            {
                eventBus.Publish(new CharacterDamaged(CharacterId, attackerName, 0));
            }

            Health.DecreaseCurrentValue(damage);

            TryDeath();
        }

        private void TryDeath()
        {
            if (Health.CurrentValue <= 0 && !IsCharacterDied)
            {
                IsCharacterDied = true;
                passiveAbility.StopAbility();
                eventBus.Publish(new CharacterDied(CharacterId));
            }
        }

        public void Heal(float amount, bool ignoreMax)
        {
            Health.IncreaseCurrentValue(amount, ignoreMax);
            eventBus.Publish(new CharacterHealed(CharacterId, amount));
        }

        public void Move((int,int) position, int moveCost)
        {
            if (moveCost > Speed.CurrentValue)
            {
                return;
            }
            (int, int) oldPosition = PositionOnField;
            Speed.DecreaseCurrentValue(moveCost);
            PositionOnField = position;
            eventBus.Publish(new CharacterMoved(CharacterId, oldPosition, PositionOnField));
        }

        public void ResetCharacter()
        {
            Speed.ToStartValueIfLower();
            isAttackedOnMove = false;
        }

        public void RemoveDebuffs()
        {
            Speed.SetCanChangeValue(true);
            SetCanAttack(true);
            CanUseAbilities = true;

            PhysicalAttack.ToStartValueIfLower();
            MagicalAttack.ToStartValueIfLower();

            PhysicalDefence.ToStartValueIfLower();
            MagicalDefence.ToStartValueIfLower();

            CriticalDamageChance.ToStartValueIfLower();
            CriticalDamageValue.ToStartValueIfLower();
            PhysicalDamageMultiplier.ToStartValueIfLower();
            MagicalDamageMultiplier.ToStartValueIfLower();

            UseAbilityCost.ToStartValueIfHigher();
            AttackRange.ToStartValueIfLower();
        }

        public void UseAbility(ActiveAbilityType activeAbilityType)
        {
            if (!CanUseAbilities)
            {
                return;
            }

            switch (activeAbilityType)
            {
                case ActiveAbilityType.Attack:
                    UseAbility(attackAbility);
                    break;
                case ActiveAbilityType.Protective:
                    UseAbility(protectiveAbility);
                    break;
                case ActiveAbilityType.Enchancing:
                    UseAbility(enchancingAbility);
                    break;
            }
        }

        public void CancelUsingCurrentActiveAbility()
        {
            if (currentUsingAbility != null)
            {
                currentUsingAbility.CancelUsingAbility();
                currentUsingAbility.OnAbilityUsed -= OnCharacterAbilityUsed;
                eventBus.Publish(new CharacterActiveAbilityUsingCanceled(CharacterId, attackAbility.ActiveAbilityType));
            }
        }

        public void ActivatePassiveAbility()
        {
            passiveAbility.RunAbility();
        }

        public void EquipItem(string itemId)
        {
            equipedItemsIds.Add(itemId);
            eventBus.Publish(new CharacterItemEquiped(CharacterId, itemId));
        }

        public void RemoveItem(string itemId)
        {
            equipedItemsIds.Add(itemId);
            eventBus.Publish(new CharacterItemRemoved(CharacterId, itemId));
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

        private void UseAbility(ACharacterActiveAbility activeAbility)
        {
            activeAbility.UseAbility();
            currentUsingAbility = activeAbility;
            currentUsingAbility.OnAbilityUsed += OnCharacterAbilityUsed;
            eventBus.Publish(new CharacterActiveAbilityUsingStarted(CharacterId, attackAbility.ActiveAbilityType));
        }

        private void OnCharacterAbilityUsed()
        {
            currentUsingAbility.OnAbilityUsed -= OnCharacterAbilityUsed;
            eventBus.Publish(new CharacterActiveAbilityUsed(CharacterId, attackAbility.ActiveAbilityType));
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

        //(0,65 * (урон атакующего / защита атакуемого) ^ 0,4 * (1 * —Ћ”„ћ≈∆ƒ”(1;1,5)) / 8
        private float CalculateFinalDamageByFormula(float attackerDamage, float attackedResistance)
        {
            return (0.65f * Mathf.Pow(attackerDamage / attackedResistance, 0.4f) * (1 * Random.Range(1, 1.5f))) / 8;
        }

        private float CalculateDamageMultipliers(PlayableCharacter attacker)
        {
            return CalculateCriticalDamageMultiplier(attacker) * CalculateRaceAndClassMultiplier(attacker);
        }

        private float CalculateRaceAndClassMultiplier(PlayableCharacter attacker)
        {
            Race attackerRace = attacker.CharacterInformation.GetCharacterRace();
            Class attackerClass = attacker.CharacterInformation.GetCharacterClass();
            Race attackedRace = CharacterInformation.GetCharacterRace();
            Class attackedClass = CharacterInformation.GetCharacterClass();
            return CalculateCriticalDamageMultiplier(attacker) * damageMultiplierByRaceMap[attackerRace] * damageMultiplierByClassMap[attackerClass] * attacker.attackMultiplierByRaceMap[attackedRace] * attacker.attackMultiplierByClassMap[attackedClass];
        }

        private float CalculateCriticalDamageMultiplier(PlayableCharacter character)
        {
            float chance = Random.Range(0f, 1f);
            if (chance < character.CriticalDamageChance.CurrentValue)
            {
                return character.CriticalDamageValue.CurrentValue;
            }
            else
            {
                return 1;
            }
        }
    }
}

