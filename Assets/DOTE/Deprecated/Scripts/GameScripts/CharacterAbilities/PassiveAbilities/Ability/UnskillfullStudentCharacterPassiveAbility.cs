using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnskillfullStudentCharacterPassiveAbility : BasePassiveCharacterAbility
{
    private UnskillfullStudentCharacterPassiveAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner)
    {
        base.Init(battleSystem, owner);
        this.abilityData = (UnskillfullStudentCharacterPassiveAbilityData)baseAbilityData;
        AbilityStart(abilityOwner);

        abilityOwner.OnDeath += AbilityEnd;

    }
    public override void AbilityStart(Character character)
    {
        abilityOwner.ChanceToFreeAttack = abilityData.chance;
    }

    public override void AbilityEnd(Character character)
    {
        abilityOwner.ChanceToFreeAttack = 0;
        abilityOwner.OnDeath -= AbilityEnd;
    }
}