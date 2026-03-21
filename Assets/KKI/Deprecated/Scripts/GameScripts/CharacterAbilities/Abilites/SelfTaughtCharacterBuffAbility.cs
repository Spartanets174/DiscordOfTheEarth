using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SelfTaughtCharacterBuffAbility : BaseCharacterAbility
{
    private SelfTaughtCharacterBuffAbilityData abilityData;
    private Character character;

    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (SelfTaughtCharacterBuffAbilityData)characterAbilityData;
        SetCardSelectBehaviour(new SelectAllPlayerUnitsBehaviour(abilityData.selectAbilityText, battleSystem));
        SetSelectCharacterBehaviour(new EmptySelectCharacterBehaviour(""));

        m_cardSelectBehaviour.OnCancelSelection += OnCancelSelection;
        m_cardSelectBehaviour.OnSelected += OnSelected;
        m_selectCharacterBehaviour.OnSelectCharacter += OnSelectCharacter;
    }
    private void OnSelected()
    {
        if (battleSystem.State is PlayerTurn)
        {
            foreach (var playerCharacter in battleSystem.PlayerController.PlayerCharactersObjects)
            {
                playerCharacter.OnClick += SelectCharacter;
            }
        }

    }
    private void OnSelectCharacter()
    {
        if (battleSystem.State is PlayerTurn)
        {
            character = battleSystem.PlayerController.CurrentPlayerCharacter;
        }
        else
        {
            character = battleSystem.EnemyController.CurrentEnemyCharacter;
        }
        character.InstantiateEffectOnCharacter(abilityData.useEffects["buff"]);

        character.PhysAttack += abilityData.physDamageAmount;

        battleSystem.PlayerController.SetPlayerChosenState(false, x =>
        {
            x.OnClick -= SelectCharacter;
        });

        UseCard(null);
    }

    private void OnCancelSelection()
    {
        foreach (var playerCharacter in battleSystem.PlayerController.PlayerCharactersObjects)
        {
            playerCharacter.OnClick -= SelectCharacter;
        }
    }
}
[Serializable]
public class SelfTaughtCharacterBuffAbilityData : BaseCharacterAbilityData
{
    public string selectAbilityText;

    public float physDamageAmount;
}