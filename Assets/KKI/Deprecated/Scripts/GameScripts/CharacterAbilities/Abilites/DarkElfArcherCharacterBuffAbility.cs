using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DarkElfArcherCharacterBuffAbility : BaseCharacterAbility
{
    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData baseCharacterAbility)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        SetCardSelectBehaviour(new EmptySelectBehaviour(""));
        SetUseCardBehaviour(new EmptyUseAbilityBehaviour());

        m_cardSelectBehaviour.OnCancelSelection += OnCancelSelection;
        m_cardSelectBehaviour.OnSelected += OnSelected;
        m_useCardBehaviour.OnCardUse += OnCardUse;
    }

    private void OnSelected()
    {
        abilityOwner.Heal(abilityOwner.Health*0.5f, abilityOwner.CharacterName);
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

public class DarkElfArcherCharacterBuffAbilityData : BaseCharacterAbilityData
{

}