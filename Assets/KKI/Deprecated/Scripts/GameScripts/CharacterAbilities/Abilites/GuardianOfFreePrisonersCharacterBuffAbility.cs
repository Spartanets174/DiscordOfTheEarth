using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[Serializable]
public class GuardianOfFreePrisonersCharacterBuffAbility : BaseCharacterAbility, ITurnCountable
{
    public int TurnCount { get => abilityData.turnCount; set => abilityData.turnCount = value; }

    public bool IsBuff { get => abilityData.isBuff; }

    public event Action<ITurnCountable> OnReturnToNormal;

    private Character character;

    private GuardianOfFreePrisonersCharacterBuffAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (GuardianOfFreePrisonersCharacterBuffAbilityData)characterAbilityData;
        SetCardSelectBehaviour(new SelectAllPlayerUnitsBehaviour("Выберите союзного персонажа для услиения", battleSystem));
        SetSelectCharacterBehaviour(new EmptySelectCharacterBehaviour(""));
        SetUseCardBehaviour(new EmptyUseAbilityBehaviour());

        m_cardSelectBehaviour.OnCancelSelection += OnCancelSelection;
        m_cardSelectBehaviour.OnSelected += OnSelected;
        m_selectCharacterBehaviour.OnSelectCharacter += OnSelectCharacter;

        m_useCardBehaviour.OnCardUse += OnCardUse;
    }

    private void OnCardUse()
    {
        OnCancelSelection();
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

        character.PhysAttack+= abilityData.buffAttackAmount;
        character.IsFreezed = true;
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
        character.PhysAttack -= abilityData.buffAttackAmount;
        character.IsFreezed = false;

        OnReturnToNormal?.Invoke(this);
        character = null;
    }
}
[Serializable]
public class GuardianOfFreePrisonersCharacterBuffAbilityData : BaseCharacterAbilityData
{
    public string selectAbilityText;

    public float buffAttackAmount;

    public int turnCount;

    [Header("Не трогать!!!")]
    public bool isBuff;
}