using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class ProtectedDonkeyCharacterAttackAbility : BaseCharacterAbility
{
    private SelectCellsInRangeBehaviour selectCellsInRangeBehaviour;
    private FormulaAttackEnemyCharactersInAreaBehaviour formulaAttackAllCharactersInAreaBehaviour;
    private ProtectedDonkeyCharacterAttackAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (ProtectedDonkeyCharacterAttackAbilityData)characterAbilityData;
        SetCardSelectBehaviour(new SelectCellsInRangeBehaviour(abilityData.selectAbilityText, battleSystem, abilityOwner, abilityData.damageArea, abilityData.range, "attack"));
        SetUseCardBehaviour(new FormulaAttackEnemyCharactersInAreaBehaviour(abilityData.damage, battleSystem, abilityOwner, $"\"{abilityData.abilityName}\""));

        selectCellsInRangeBehaviour = (SelectCellsInRangeBehaviour)CardSelectBehaviour;
        formulaAttackAllCharactersInAreaBehaviour = (FormulaAttackEnemyCharactersInAreaBehaviour)UseCardBehaviour;

        m_cardSelectBehaviour.OnSelected += OnSelected;
        m_useCardBehaviour.OnCardUse += OnCardUse;
    }
    private void OnSelected()
    {
        if (battleSystem.State is PlayerTurn)
        {
            formulaAttackAllCharactersInAreaBehaviour.cellsToAttack = selectCellsInRangeBehaviour.highlightedCells.Where(x => x.GetComponentInChildren<Enemy>() != null).ToList();
        }
        else
        {
            formulaAttackAllCharactersInAreaBehaviour.cellsToAttack = selectCellsInRangeBehaviour.highlightedCells.Where(x => x.GetComponentInChildren<PlayerCharacter>() != null).ToList();
        }
        if (formulaAttackAllCharactersInAreaBehaviour.cellsToAttack.Count == 0)
        {
            SelectCard();
        }
        else
        {
            UseCard(abilityOwner.gameObject);
        }
    }


    private void OnCardUse()
    {
        selectCellsInRangeBehaviour.highlightedCells.Clear();
        formulaAttackAllCharactersInAreaBehaviour.cellsToAttack.Clear();
    }
}
[Serializable]
public class ProtectedDonkeyCharacterAttackAbilityData : BaseCharacterAbilityData
{
    public string selectAbilityText;

    public float damage;

    public int range;

    public Vector2 damageArea;
}