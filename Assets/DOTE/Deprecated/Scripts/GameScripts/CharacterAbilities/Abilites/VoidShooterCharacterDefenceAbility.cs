using System;
using UnityEngine.TextCore.Text;

[Serializable]
public class VoidShooterCharacterDefenceAbility : BaseCharacterAbility
{
    private VoidShooterCharacterDefenceAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (VoidShooterCharacterDefenceAbilityData)characterAbilityData;
        SetCardSelectBehaviour(new EmptySelectBehaviour(""));
        SetUseCardBehaviour(new EmptyUseAbilityBehaviour());

        m_cardSelectBehaviour.OnCancelSelection += OnCancelSelection;
        m_cardSelectBehaviour.OnSelected += OnSelected;
        m_useCardBehaviour.OnCardUse += OnCardUse;
    }

    private void OnSelected()
    {
        abilityOwner.PhysDefence += abilityData.physDefenceAmount;
        abilityOwner.MagDefence += abilityData.magDefenceAmount;
        abilityOwner.InstantiateEffectOnCharacter(abilityData.useEffects["buff"]);

        UseCard(abilityOwner.gameObject);
    }

    private void OnCardUse()
    {
        OnCancelSelection();
    }

    private void OnCancelSelection()
    {

    }
}

[Serializable]
public class VoidShooterCharacterDefenceAbilityData : BaseCharacterAbilityData
{
    public float physDefenceAmount;

    public float magDefenceAmount;
}