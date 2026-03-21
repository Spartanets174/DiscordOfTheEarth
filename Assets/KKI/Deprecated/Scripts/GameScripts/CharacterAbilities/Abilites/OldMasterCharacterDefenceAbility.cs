using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OldMasterCharacterDefenceAbility : BaseCharacterAbility, ITurnCountable
{
    public int TurnCount { get => abilityData.turnCount; set => abilityData.turnCount = value; }
    public bool IsBuff { get => abilityData.isBuff; }

    public event Action<ITurnCountable> OnReturnToNormal;

    private Character character;
    private OldMasterCharacterDefenceAbilityData abilityData;

    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (OldMasterCharacterDefenceAbilityData)characterAbilityData;
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

        character.PhysAttack += abilityData.physAttackAmount;
        character.PhysDefence -= abilityData.physDefenceAmount;


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
        character.PhysAttack -= abilityData.physAttackAmount;
        character.PhysDefence += abilityData.physDefenceAmount;

        OnReturnToNormal?.Invoke(this);
        character = null;
    }
}
[Serializable]
public class OldMasterCharacterDefenceAbilityData : BaseCharacterAbilityData
{
    public string selectAbilityText;

    public float physAttackAmount;

    public float physDefenceAmount;

    public int turnCount;

    [Header("НЕ трогать!")]
    public bool isBuff;
}