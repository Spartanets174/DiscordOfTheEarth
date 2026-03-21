using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

[Serializable]
public class LadyOnPonyCharacterDefenceAbility : BaseCharacterAbility
{
    private SelectCellsWithCharactersInRangeBehaviour selectCellsToAttackInRangeBehaviour;
    private LadyOnPonyCharacterDefenceAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (LadyOnPonyCharacterDefenceAbilityData)characterAbilityData;
        abilityData = (LadyOnPonyCharacterDefenceAbilityData)characterAbilityData;
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
public class LadyOnPonyCharacterDefenceAbilityData : BaseCharacterAbilityData
{
    public int range;

    public float physDefenceAmount;
}