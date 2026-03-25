using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkElfArcherCharacterPassiveAbility : BasePassiveCharacterAbility
{
    private DarkElfArcherCharacterPassiveAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner)
    {
        base.Init(battleSystem, owner);
        this.abilityData = (DarkElfArcherCharacterPassiveAbilityData)baseAbilityData;
        AbilityStart(abilityOwner);

        abilityOwner.OnDeath += AbilityEnd;

    }
    public override void AbilityStart(Character character)
    {
        abilityOwner.AttackMultiplierByClassesDict[abilityData.classToIncreaseDamage] += abilityData.damageIncrease;
    }

    public override void AbilityEnd(Character character)
    {
        abilityOwner.AttackMultiplierByClassesDict[abilityData.classToIncreaseDamage] += abilityData.damageIncrease;

        abilityOwner.OnDeath -= AbilityEnd;
    }
}