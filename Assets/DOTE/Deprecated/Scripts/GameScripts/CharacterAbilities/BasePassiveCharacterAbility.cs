using System;
using UnityEngine;

public abstract class BasePassiveCharacterAbility : MonoBehaviour
{
    public BasePassiveCharacterAbilityData baseAbilityData;
    protected Character abilityOwner;
    protected BattleSystem battleSystem;
    public virtual void Init(BattleSystem battleSystem, Character owner)
    {
        this.battleSystem = battleSystem;
        this.abilityOwner = owner;
    }

    public abstract void AbilityStart(Character character);

    public abstract void AbilityEnd(Character character);

}
