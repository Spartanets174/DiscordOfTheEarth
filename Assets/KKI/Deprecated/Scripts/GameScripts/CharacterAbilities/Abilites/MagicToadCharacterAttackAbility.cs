using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MagicToadCharacterAttackAbility : BaseCharacterAbility
{
    private SelectCellsWithCharactersInRangeBehaviour selectCellsToAttackInRangeBehaviour;
    private MagicToadCharacterAttackAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (MagicToadCharacterAttackAbilityData)characterAbilityData;
        SetCardSelectBehaviour(new SelectCellsWithCharactersInRangeBehaviour(abilityData.selectAbilityText, battleSystem, abilityOwner, abilityData.range, "attack"));
        SetUseCardBehaviour(new FormulaAttackSelectedСharacterBehaviour(abilityData.damage, battleSystem, abilityOwner, $"\"{abilityData.abilityName}\""));

        selectCellsToAttackInRangeBehaviour = (SelectCellsWithCharactersInRangeBehaviour)CardSelectBehaviour;

        m_cardSelectBehaviour.OnCancelSelection += OnCancelSelection;
        m_cardSelectBehaviour.OnSelected += OnSelected;
        m_useCardBehaviour.OnCardUse += OnCardUse;
    }

    private void OnSelected()
    {
        if (battleSystem.State is PlayerTurn)
        {
            foreach (var enemyCharacter in selectCellsToAttackInRangeBehaviour.charactersOnCells)
            {
                enemyCharacter.OnClick += UseCard;
            }
        }
    }


    private void OnCardUse()
    {
        OnCancelSelection();
    }

    private void OnCancelSelection()
    {
        foreach (var enemyCharacter in selectCellsToAttackInRangeBehaviour.charactersOnCells)
        {
            enemyCharacter.OnClick -= UseCard;
        }
        battleSystem.PlayerController.SetPlayerStates(true, false);
    }
}
[Serializable]
public class MagicToadCharacterAttackAbilityData : BaseCharacterAbilityData
{
    public string selectAbilityText;

    public float damage;

    public int range;
}