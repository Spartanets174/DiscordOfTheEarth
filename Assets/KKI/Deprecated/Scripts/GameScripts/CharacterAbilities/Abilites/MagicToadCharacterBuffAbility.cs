using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[Serializable]
public class MagicToadCharacterBuffAbility : BaseCharacterAbility, ITurnCountable
{
    public int TurnCount { get => abilityData.turnCount; set => abilityData.turnCount = value; }
    public bool IsBuff { get => abilityData.isBuff; }


    private float healAmount;
    public event Action<ITurnCountable> OnReturnToNormal;

    private MagicToadCharacterBuffAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (MagicToadCharacterBuffAbilityData)characterAbilityData;
        SetCardSelectBehaviour(new EmptySelectBehaviour("Используйте карту"));

        m_cardSelectBehaviour.OnSelected += OnSelected;
    }

    private void OnSelected()
    {
        abilityOwner.PhysDefence += abilityData.physDefenceAmount;
        abilityOwner.MagDefence += abilityData.magDefenceAmount;
        abilityOwner.Heal(abilityData.healthAmount, abilityOwner.CharacterName);
        abilityOwner.InstantiateEffectOnCharacter(abilityData.useEffects["buff"]);

        healAmount = abilityOwner.LastHealAmount;

        m_cardSelectBehaviour.OnSelected -= OnSelected;
        UseCard(abilityOwner.gameObject);
    }

    public void ReturnToNormal()
    {
        abilityOwner.PhysDefence -= abilityData.physDefenceAmount;
        abilityOwner.MagDefence -= abilityData.magDefenceAmount;
        abilityOwner.Damage(healAmount, $"{abilityData.abilityName}");
        OnReturnToNormal?.Invoke(this);
    }
}
[Serializable]
public class MagicToadCharacterBuffAbilityData : BaseCharacterAbilityData
{
    public float healthAmount;

    public float magDefenceAmount;

    public float physDefenceAmount;

    public int turnCount;

    [Header("Не трогать!!!")]
    public bool isBuff;
}