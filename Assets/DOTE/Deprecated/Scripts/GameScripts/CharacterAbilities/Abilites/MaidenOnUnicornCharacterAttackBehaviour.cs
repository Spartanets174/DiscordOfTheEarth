using System;
using UnityEngine;

[Serializable]
public class MaidenOnUnicornCharacterAttackBehaviour : BaseCharacterAbility
{
    private SelectCellsToAttackInAreaRangedBehaviour selectCellsToAttackInAreaRangedBehaviour;
    private AttackAllCharactersInAreaBehaviour attackAllCharactersInAreaBehaviour;
    private MaidenOnUnicornCharacterAttackBehaviourData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (MaidenOnUnicornCharacterAttackBehaviourData)characterAbilityData;
        SetCardSelectBehaviour(new SelectCellsToAttackInAreaRangedBehaviour(battleSystem, abilityOwner, abilityData.area, abilityData.direction));
        SetUseCardBehaviour(new AttackAllCharactersInAreaBehaviour(abilityData.damage, battleSystem, "\"Большой брат\""));

        selectCellsToAttackInAreaRangedBehaviour = (SelectCellsToAttackInAreaRangedBehaviour)CardSelectBehaviour;
        attackAllCharactersInAreaBehaviour = (AttackAllCharactersInAreaBehaviour)UseCardBehaviour;

        m_cardSelectBehaviour.OnSelected += OnSelected;
        m_useCardBehaviour.OnCardUse += OnCardUse;
    }
    private void OnSelected()
    {
        attackAllCharactersInAreaBehaviour.cellsToAttack = selectCellsToAttackInAreaRangedBehaviour.cells;
        UseCard(abilityOwner.gameObject);
    }


    private void OnCardUse()
    {
        selectCellsToAttackInAreaRangedBehaviour.cells.Clear();
        attackAllCharactersInAreaBehaviour.cellsToAttack.Clear();
    }
}
[Serializable]
public class MaidenOnUnicornCharacterAttackBehaviourData : BaseCharacterAbilityData
{
    public float damage;

    public Vector2 area;

    public Enums.Directions direction;
}