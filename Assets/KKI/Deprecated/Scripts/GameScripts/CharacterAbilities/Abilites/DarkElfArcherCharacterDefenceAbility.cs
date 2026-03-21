using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[Serializable]
public class DarkElfArcherCharacterDefenceAbility : BaseCharacterAbility
{
    private DarkElfArcherCharacterDefenceAbilityData abilityData;

    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (DarkElfArcherCharacterDefenceAbilityData)characterAbilityData;
        SetCardSelectBehaviour(new EmptySelectBehaviour(""));
        SetUseCardBehaviour(new EmptyUseAbilityBehaviour());

        m_cardSelectBehaviour.OnCancelSelection += OnCancelSelection;
        m_cardSelectBehaviour.OnSelected += OnSelected;
        m_useCardBehaviour.OnCardUse += OnCardUse;
    }

    private void OnSelected()
    {
        abilityOwner.CanBeDamagedByClassesDict[abilityData.classToDefence] = false;
        abilityOwner.InstantiateEffectOnCharacter(abilityData.useEffects["shield"]);

        abilityOwner.OnDamaged += OnDamaged;
        UseCard(abilityOwner.gameObject);
    }

    private void OnDamaged(Character character, string arg2, float arg3)
    {
        if (character.LastAttackedCharacter.Class == abilityData.classToDefence)
        {
            abilityOwner.CanBeDamagedByClassesDict[abilityData.classToDefence] = true;
            abilityOwner.OnDamaged -= OnDamaged;
        }       
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
public class DarkElfArcherCharacterDefenceAbilityData : BaseCharacterAbilityData
{
    public Enums.Classes classToDefence;
}
