using System;
using UnityEngine;

[Serializable]
public class FallenElfCharacterAttackAbility : BaseCharacterAbility
{
    private FallenElfCharacterAttackAbilityData abilityData;
    private SelectCellsWithCharactersInRangeBehaviour selectCellsToAttackInRangeBehaviour;
    private FormulaAttackSelected—haracterBehaviour formulaAttackSelected—haracterBehaviour;

    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (FallenElfCharacterAttackAbilityData)characterAbilityData;
        SetCardSelectBehaviour(new SelectCellsWithCharactersInRangeBehaviour(abilityData.selectAbilityText, battleSystem, abilityOwner, abilityData.range, "attack"));
        SetUseCardBehaviour(new FormulaAttackSelected—haracterBehaviour(abilityData.damage, battleSystem, abilityOwner, $"\"{abilityData.abilityName}\""));

        selectCellsToAttackInRangeBehaviour = (SelectCellsWithCharactersInRangeBehaviour)CardSelectBehaviour;
        formulaAttackSelected—haracterBehaviour = (FormulaAttackSelected—haracterBehaviour)UseCardBehaviour;

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
        abilityOwner.Heal(formulaAttackSelected—haracterBehaviour.attackedCharacter.LastDamageAmount * abilityData.healPercent, abilityOwner.CharacterName);
        OnCancelSelection();
    }

    private void OnCancelSelection()
    {
        foreach (var enemyCharacter in selectCellsToAttackInRangeBehaviour.charactersOnCells)
        {
            enemyCharacter.OnClick -= UseCard;
        }
        selectCellsToAttackInRangeBehaviour.charactersDirectionsOnCells.Clear();
        battleSystem.PlayerController.SetPlayerStates(true, false);
    }
}
[Serializable]
public class FallenElfCharacterAttackAbilityData : BaseCharacterAbilityData
{
    public string selectAbilityText;

    public float damage;

    public int range;

    public float healPercent;
}