using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class СursedCharacterBuffAbility : BaseCharacterAbility, ITurnCountable
{
    public int TurnCount { get => abilityData.turnCount; set => abilityData.turnCount = value; }
    public bool IsBuff { get => abilityData.isBuff; }

    public event Action<ITurnCountable> OnReturnToNormal;

    private float currentCritAmount;

    private СursedCharacterBuffAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (СursedCharacterBuffAbilityData)characterAbilityData;
        SetCardSelectBehaviour(new EmptySelectBehaviour("Используйте карту"));

        m_cardSelectBehaviour.OnSelected += OnSelected;
    }

    private void OnSelected()
    {
        if (abilityOwner.CritChance < abilityData.critAmount)
        {
            currentCritAmount = abilityData.critAmount - abilityOwner.CritChance;

            abilityOwner.CritChance += currentCritAmount;
            abilityOwner.InstantiateEffectOnCharacter(abilityData.useEffects["buff"]);

        }

        m_cardSelectBehaviour.OnSelected -= OnSelected;
        UseCard(abilityOwner.gameObject);

    }

    public void ReturnToNormal()
    {
       abilityOwner.CritChance -= currentCritAmount;

        OnReturnToNormal?.Invoke(this);
    }

}
[Serializable]
public class СursedCharacterBuffAbilityData : BaseCharacterAbilityData
{
    public float critAmount;

    public int turnCount;

    [Header("НЕ трогать!")]
    public bool isBuff;
}