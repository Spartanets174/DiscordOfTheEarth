using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FallenElfCharacterDefenceAbility : BaseCharacterAbility, ITurnCountable
{
    public int TurnCount { get => abilityData.turnCount; set => abilityData.turnCount = value; }
    public bool IsBuff { get => abilityData.isBuff; }

    public event Action<ITurnCountable> OnReturnToNormal;

    private FallenElfCharacterDefenceAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData baseCharacterAbility)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (FallenElfCharacterDefenceAbilityData)baseCharacterAbility;
        SetCardSelectBehaviour(new EmptySelectBehaviour("Используйте карту"));

        m_cardSelectBehaviour.OnSelected += OnSelected;
    }

    private void OnSelected()
    {
        abilityOwner.PhysDefence += abilityData.physDefenceAmount;
        abilityOwner.MagDefence += abilityData.magDefenceAmount;
        abilityOwner.InstantiateEffectOnCharacter(abilityData.useEffects["buff"]);

        m_cardSelectBehaviour.OnSelected -= OnSelected;
        UseCard(abilityOwner.gameObject);

    }

    public void ReturnToNormal()
    {
        abilityOwner.PhysDefence -= abilityData.physDefenceAmount;
        abilityOwner.MagDefence -= abilityData.magDefenceAmount;

        OnReturnToNormal?.Invoke(this);
    }

}
[Serializable]
public class FallenElfCharacterDefenceAbilityData : BaseCharacterAbilityData
{
    public float magDefenceAmount;

    public float physDefenceAmount;

    public int turnCount;

    [Header("Не трогать!!!")]
    public bool isBuff;
}