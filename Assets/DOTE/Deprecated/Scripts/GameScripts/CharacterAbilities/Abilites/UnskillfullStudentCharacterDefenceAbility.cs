using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnskillfullStudentCharacterDefenceAbility : BaseCharacterAbility, ITurnCountable
{
    public int TurnCount { get => abilityData.turnCount; set => abilityData.turnCount = value; }

    public bool IsBuff { get => abilityData.isBuff; }

    private Character character;
    private UnskillfullStudentCharacterDefenceAbilityData abilityData;

    public event Action<ITurnCountable> OnReturnToNormal; 

    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData baseCharacterAbility)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (UnskillfullStudentCharacterDefenceAbilityData)baseCharacterAbility;
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
        character.PhysDefence += abilityData.physDefenceAmount;
        OnCancelSelection();
        UseCard(character.gameObject);
    }


    private void OnCancelSelection()
    {
        battleSystem.EnemyController.SetEnemiesStates(true, false);
        battleSystem.PlayerController.SetPlayerStates(true, false, x =>
        {
            x.OnClick -= SelectCharacter;
        });
    }

    public void ReturnToNormal()
    {
        character.PhysDefence -= abilityData.physDefenceAmount;

        OnReturnToNormal?.Invoke(this);
        character = null;
    }
}
[Serializable]
public class UnskillfullStudentCharacterDefenceAbilityData : BaseCharacterAbilityData
{
    public string selectAbilityText;

    public float physDefenceAmount;

    public int turnCount;

    [Header("НЕ трогать!")]
    public bool isBuff;
}