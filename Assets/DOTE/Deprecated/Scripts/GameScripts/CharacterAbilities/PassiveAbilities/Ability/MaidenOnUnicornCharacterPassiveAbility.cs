using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaidenOnUnicornCharacterPassiveAbility : BasePassiveCharacterAbility
{
    private MaidenOnUnicornCharacterPassiveAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner)
    {
        base.Init(battleSystem, owner);
        this.abilityData = (MaidenOnUnicornCharacterPassiveAbilityData)baseAbilityData;
        AbilityStart(abilityOwner);

        abilityOwner.OnDeath += AbilityEnd;

    }
    public override void AbilityStart(Character character)
    {
        foreach (var item in abilityOwner.DamageMultiplierByRacesDict.Keys.ToList())
        {
            foreach (var second in abilityData.racesToIgnoreDamage)
            {
                if (second == item)
                {
                    abilityOwner.DamageMultiplierByRacesDict[item] -= abilityData.physDamageIgnore;
                }
            }           
        }
    }

    public override void AbilityEnd(Character character)
    {
        foreach (var item in abilityOwner.DamageMultiplierByRacesDict.Keys.ToList())
        {
            foreach (var second in abilityData.racesToIgnoreDamage)
            {
                if (second == item)
                {
                    abilityOwner.DamageMultiplierByRacesDict[item] += abilityData.physDamageIgnore;
                }
            }
        }
        abilityOwner.OnDeath -= AbilityEnd;
    }
}