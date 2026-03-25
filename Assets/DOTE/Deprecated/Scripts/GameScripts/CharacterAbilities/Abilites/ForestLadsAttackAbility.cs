using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class ForestLadsAttackAbility : BaseCharacterAbility
{
    private SelectCellsWithCharactersInRangeBehaviour selectCellsToAttackInRangeBehaviour;
    private FormulaAttackSelected—haracterBehaviour formulaAttackSelected—haracterBehaviour;
    private ForestLadsAttackAbilityData abilityData;
    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (ForestLadsAttackAbilityData)characterAbilityData;
        SetCardSelectBehaviour(new SelectCellsWithCharactersInRangeBehaviour(abilityData.selectAbilityText, battleSystem, abilityOwner, abilityData.range, "attack"));
        SetSelectCharacterBehaviour(new SetCurrentEnemyCharacterBehaviour("", battleSystem));
        SetUseCardBehaviour(new FormulaAttackSelected—haracterBehaviour(abilityOwner.PhysAttack * (1 + abilityData.increaseDamageAmount), battleSystem, abilityOwner, $"\"{abilityData.abilityName}\""));

        selectCellsToAttackInRangeBehaviour = (SelectCellsWithCharactersInRangeBehaviour)CardSelectBehaviour;
        formulaAttackSelected—haracterBehaviour = (FormulaAttackSelected—haracterBehaviour)UseCardBehaviour;

        m_cardSelectBehaviour.OnCancelSelection += OnCancelSelection;

        m_cardSelectBehaviour.OnSelected += OnSelected;
        m_selectCharacterBehaviour.OnSelectCharacter += OnSelectCharacter;

        m_useCardBehaviour.OnCardUse += OnCardUse;
    }

    private void OnSelected()
    {
        if (battleSystem.State is PlayerTurn)
        {
            foreach (var enemyCharacter in selectCellsToAttackInRangeBehaviour.charactersOnCells)
            {
                enemyCharacter.OnClick += SelectCharacter;
            }
        }
    }

    private void OnSelectCharacter()
    {
        Character firstCharacter;
        if (battleSystem.State is PlayerTurn)
        {
            firstCharacter = battleSystem.EnemyController.CurrentEnemyCharacter;
        }
        else
        {
            firstCharacter = battleSystem.PlayerController.CurrentPlayerCharacter;
        }
        Enums.Directions direction = selectCellsToAttackInRangeBehaviour.charactersDirectionsOnCells.Where(x => x.Value == firstCharacter).First().Key;
        Character secondCharacter = GetNextCharacterByDirection(firstCharacter.PositionOnField, direction, 1);
        UseCard(firstCharacter.gameObject);
        if (secondCharacter!=null)
        {
            formulaAttackSelected—haracterBehaviour.damage = abilityOwner.PhysAttack * (1 - abilityData.decreaseDamageAmount);
            UseCard(secondCharacter.gameObject);
            battleSystem.PointsOfAction.Value += 11;
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
            enemyCharacter.OnClick -= SelectCharacter;
        }
    }


    private Character GetNextCharacterByDirection(Vector2 pos, Enums.Directions direction, int localRange)
    {
        int newI = (int)pos.x;
        int newJ = (int)pos.y;

        for (int i = 0; i < localRange; i++)
        {
            switch (direction)
            {
                case Enums.Directions.top:
                    newI--;
                    break;
                case Enums.Directions.bottom:
                    newJ--;
                    break;
                case Enums.Directions.right:
                    newI++;
                    break;
                case Enums.Directions.left:
                    newJ++;
                    break;
            }

            if (newI >= 7 || newI < 0)
            {
                break;
            }
            if (newJ >= 11 || newJ < 0)
            {
                break;
            }

            Cell cell = battleSystem.FieldController.GetCell(newI, newJ);
            Character enemy;
            if (battleSystem.State is PlayerTurn)
            {
                enemy = cell.GetComponentInChildren<Enemy>();
            }
            else
            {
                enemy = cell.GetComponentInChildren<PlayerCharacter>();
            }


            KostilEnemy kostilEnemy = cell.GetComponentInChildren<KostilEnemy>();
            if (cell.transform.childCount > 1)
            {

                if (enemy != null && enemy is not KostilEnemy)
                {
                    return enemy;
                }
                if (kostilEnemy != null)
                {
                    return enemy;
                }
            }

        }
        return null;
    }
}
[Serializable]
public class ForestLadsAttackAbilityData : BaseCharacterAbilityData
{
    public string selectAbilityText;

    public float increaseDamageAmount;
    public float decreaseDamageAmount;

    public int range;
}