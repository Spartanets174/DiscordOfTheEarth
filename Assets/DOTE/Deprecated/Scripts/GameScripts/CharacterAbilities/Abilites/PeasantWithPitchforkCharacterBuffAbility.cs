using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PeasantWithPitchforkCharacterBuffAbility : BaseCharacterAbility
{
    private SelectCellsWithCharactersInRangeBehaviour selectCellsToAttackInRangeBehaviour;
    private PeasantWithPitchforkCharacterBuffAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (PeasantWithPitchforkCharacterBuffAbilityData)characterAbilityData;
        SetCardSelectBehaviour(new SelectCellsWithCharactersInRangeBehaviour("", battleSystem, abilityOwner, abilityData.range, "allowed"));

        selectCellsToAttackInRangeBehaviour = (SelectCellsWithCharactersInRangeBehaviour)CardSelectBehaviour;

        m_cardSelectBehaviour.OnSelected += OnSelected;
    }
    private void OnSelected()
    {
        foreach (var character in selectCellsToAttackInRangeBehaviour.charactersOnCells)
        {
            character.PhysDefence += abilityData.physDefenceAmount;
            character.InstantiateEffectOnCharacter(abilityData.useEffects["buff"]);

        }
        selectCellsToAttackInRangeBehaviour.charactersOnCells.Clear();
        UseCard(abilityOwner.gameObject);
    }

}
[Serializable]
public class PeasantWithPitchforkCharacterBuffAbilityData : BaseCharacterAbilityData
{
    public float physDefenceAmount;

    public int range;
}