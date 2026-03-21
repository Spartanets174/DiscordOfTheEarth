using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GargoyleCharacterBuffAbility : BaseCharacterAbility, ITurnCountable
{
    public int TurnCount { get => abilityData.turnCount; set => abilityData.turnCount = value; }

    public bool IsBuff { get => abilityData.isBuff; }

    private GargoyleCharacterBuffAbilityData abilityData;

    public event Action<ITurnCountable> OnReturnToNormal;

    private float amount;
    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (GargoyleCharacterBuffAbilityData)characterAbilityData;
        SetCardSelectBehaviour(new EmptySelectBehaviour("Используйте карту"));

        m_cardSelectBehaviour.OnSelected += OnSelected;
    }

    private void OnSelected()
    {
        amount = abilityData.physDefenceToIcrease - abilityOwner.PhysDefence;

        abilityOwner.PhysDefence += amount;
        abilityOwner.InstantiateEffectOnCharacter(abilityData.useEffects["buff"]);


        m_cardSelectBehaviour.OnSelected -= OnSelected;
        UseCard(abilityOwner.gameObject);
    }

    public void ReturnToNormal()
    {
        abilityOwner.PhysDefence -= amount;
        OnReturnToNormal?.Invoke(this);
    }
}

[Serializable]
public class GargoyleCharacterBuffAbilityData : BaseCharacterAbilityData
{
    public float physDefenceToIcrease;

    public int turnCount;

    [Header("Не трогать!!!")]
    public bool isBuff;

}