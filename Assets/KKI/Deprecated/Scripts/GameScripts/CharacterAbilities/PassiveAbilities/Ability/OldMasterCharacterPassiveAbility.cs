using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OldMasterCharacterPassiveAbility : BasePassiveCharacterAbility
{
    private OldMasterCharacterPassiveAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner)
    {
        base.Init(battleSystem, owner);
        this.abilityData = (OldMasterCharacterPassiveAbilityData)baseAbilityData;
        AbilityStart(abilityOwner);

        abilityOwner.OnDeath += AbilityEnd;

    }
    public override void AbilityStart(Character character)
    {
        foreach (var item in abilityOwner.AttackMultiplierByRacesDict.Keys.ToList())
        {
            foreach (var second in abilityData.racesToIncreaseDamage)
            {
                if (second == item)
                {
                    abilityOwner.AttackMultiplierByRacesDict[item] += abilityData.damageIncrease;
                }
            }
        }
    }

    public override void AbilityEnd(Character character)
    {
        foreach (var item in abilityOwner.AttackMultiplierByRacesDict.Keys.ToList())
        {
            foreach (var second in abilityData.racesToIncreaseDamage)
            {
                if (second == item)
                {
                    abilityOwner.AttackMultiplierByRacesDict[item] -= abilityData.damageIncrease;
                }
            }
        }
        abilityOwner.OnDeath -= AbilityEnd;
    }
}