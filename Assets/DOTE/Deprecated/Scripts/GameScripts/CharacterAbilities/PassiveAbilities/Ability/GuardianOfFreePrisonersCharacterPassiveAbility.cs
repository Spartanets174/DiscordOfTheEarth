using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianOfFreePrisonersCharacterPassiveAbility : BasePassiveCharacterAbility
{
    private GuardianOfFreePrisonersCharacterPassiveAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner)
    {
        base.Init(battleSystem, owner);
        this.abilityData = (GuardianOfFreePrisonersCharacterPassiveAbilityData)baseAbilityData;
        AbilityStart(abilityOwner);

        abilityOwner.OnDeath += AbilityEnd;

    }
    public override void AbilityStart(Character character)
    {
        abilityOwner.ChanceToAvoidDamage = abilityData.chance;
    }

    public override void AbilityEnd(Character character)
    {
        abilityOwner.ChanceToAvoidDamage = 0;
        abilityOwner.OnDeath -= AbilityEnd;
    }
}