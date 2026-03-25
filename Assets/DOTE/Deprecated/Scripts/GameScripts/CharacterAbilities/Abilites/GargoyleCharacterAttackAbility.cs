using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GargoyleCharacterAttackAbility : BaseCharacterAbility
{
    private SelectCellsWithCharactersInRangeBehaviour selectCellsToAttackInRangeBehaviour;
    private GargoyleCharacterAttackAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (GargoyleCharacterAttackAbilityData)characterAbilityData;
        SetCardSelectBehaviour(new SelectCellsWithCharactersInRangeBehaviour(abilityData.selectAbilityText, battleSystem, abilityOwner, abilityData.range, "attack"));
        SetUseCardBehaviour(new AttackSelected—haracterBehaviour(UnityEngine.Random.Range(abilityData.damageSpread.x, abilityData.damageSpread.y), battleSystem, "\"ƒ˚ı‡ÌËÂ Ó„Ì∏Ï\""));

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
public class GargoyleCharacterAttackAbilityData : BaseCharacterAbilityData
{
    public string selectAbilityText;

    public Vector2 damageSpread;

    public int range;
}