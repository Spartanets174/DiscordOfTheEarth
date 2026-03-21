using System;
using UnityEngine;

[Serializable]
public class LadyOnPonyCharacterAttackAbility : BaseCharacterAbility
{
    private SelectCellsWithCharactersInRangeBehaviour selectCellsToAttackInRangeBehaviour;
    private LadyOnPonyCharacterAttackAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData baseAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        this.abilityData = (LadyOnPonyCharacterAttackAbilityData)baseAbilityData;
        SetCardSelectBehaviour(new SelectCellsWithCharactersInRangeBehaviour(abilityData.selectAbilityText, battleSystem, abilityOwner, abilityData.range, "attack"));
        SetUseCardBehaviour(new FormulaAttackSelected—haracterBehaviour(abilityData.damage, battleSystem, abilityOwner, $"\"{abilityData.abilityName}\""));

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
public class LadyOnPonyCharacterAttackAbilityData : BaseCharacterAbilityData
{
    public string selectAbilityText;

    public float damage;

    public int range;
}