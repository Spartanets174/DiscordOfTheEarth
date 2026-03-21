using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SentinelCharacterAttackAbility : BaseCharacterAbility
{
    private SentinelCharacterAttackAbilityData abilityData;
    private SelectCellsWithCharactersInRangeBehaviour selectCellsToAttackInRangeBehaviour;
    private FormulaAttackSelected—haractersBehaviour attackSelected—haractersBehaviour;

    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (SentinelCharacterAttackAbilityData)characterAbilityData;
        SetCardSelectBehaviour(new SelectCellsWithCharactersInRangeBehaviour("",battleSystem, abilityOwner, abilityData.range, "attack"));
        SetUseCardBehaviour(new FormulaAttackSelected—haractersBehaviour(abilityData.damage, battleSystem, abilityOwner, $"\"{abilityData.abilityName}\""));

        selectCellsToAttackInRangeBehaviour = (SelectCellsWithCharactersInRangeBehaviour)CardSelectBehaviour;
        attackSelected—haractersBehaviour = (FormulaAttackSelected—haractersBehaviour)UseCardBehaviour;

        m_cardSelectBehaviour.OnSelected += OnSelected;
        m_useCardBehaviour.OnCardUse += OnCardUse;
    }
    private void OnSelected()
    {
        attackSelected—haractersBehaviour.characters.AddRange(selectCellsToAttackInRangeBehaviour.charactersOnCells);
        foreach (var character in attackSelected—haractersBehaviour.characters)
        {
            character.PhysDefence -=abilityData.physDefenceAmount;
        }
        UseCard(abilityOwner.gameObject);       
    }


    private void OnCardUse()
    {
        selectCellsToAttackInRangeBehaviour.charactersOnCells.Clear();
        attackSelected—haractersBehaviour.characters.Clear();
    }
}
[Serializable]
public class SentinelCharacterAttackAbilityData : BaseCharacterAbilityData
{
    public float damage;
    public float physDefenceAmount;
    public int range;
}