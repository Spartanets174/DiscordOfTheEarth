using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class СursedCharacterDefenceAbility : BaseCharacterAbility, ITurnCountable
{
    public int TurnCount { get => abilityData.turnCount; set => abilityData.turnCount = value; }

    public bool IsBuff { get => abilityData.isBuff; }

    private СursedCharacterDefenceAbilityData abilityData;
    public event Action<ITurnCountable> OnReturnToNormal;

    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (СursedCharacterDefenceAbilityData)characterAbilityData;
        SetCardSelectBehaviour(new EmptySelectBehaviour("Используйте карту"));

        m_cardSelectBehaviour.OnSelected += OnSelected;
    }

    private void OnSelected()
    {
        abilityOwner.IgnoreMagDamage = true;
        abilityOwner.InstantiateEffectOnCharacter(abilityData.useEffects["buff"]);

        m_cardSelectBehaviour.OnSelected -= OnSelected;
        UseCard(abilityOwner.gameObject);

    }

    public void ReturnToNormal()
    {
        abilityOwner.IgnoreMagDamage = false;
        OnReturnToNormal?.Invoke(this);
    }

}
[Serializable]
public class СursedCharacterDefenceAbilityData : BaseCharacterAbilityData
{
    public int turnCount;

    [Header("НЕ трогать!")]
    public bool isBuff;
}