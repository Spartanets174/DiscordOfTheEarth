using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestLadsPassiveAbility : BasePassiveCharacterAbility
{
    private bool isSetted;
    private ForestLadsPassiveAbilityData abilityData;
    private List<GameObject> objectsOnCells = new();
    public override void Init(BattleSystem battleSystem, Character owner)
    {
        base.Init(battleSystem, owner);
        this.abilityData = (ForestLadsPassiveAbilityData)baseAbilityData;
        foreach (var character in battleSystem.EnemyController.EnemyCharObjects)
        {
            character.OnPositionOnFieldChanged += AbilityStart;
        }
        abilityOwner.OnPositionOnFieldChanged += AbilityStart;

        abilityOwner.OnDeath += AbilityEnd;

        AbilityStart(abilityOwner);

    }
    public override void AbilityStart(Character character)
    {
        GetObjectsOnCells();
        if (objectsOnCells.Count > 0)
        {
            if (!isSetted)
            {
                abilityOwner.PhysDefence += abilityData.physDefenceAmount;
                abilityOwner.MagDefence += abilityData.magDefenceAmount;

                isSetted = true;
            }
        }
        else
        {
            if (isSetted)
            {
                abilityOwner.PhysDefence -= abilityData.physDefenceAmount;
                abilityOwner.MagDefence -= abilityData.magDefenceAmount;
                isSetted = false;
            }
        }
    }

    public override void AbilityEnd(Character character)
    {
        foreach (var enemyCharacter in battleSystem.EnemyController.EnemyCharObjects)
        {
            enemyCharacter.OnPositionOnFieldChanged -= AbilityStart;
        }
        abilityOwner.OnPositionOnFieldChanged -= AbilityStart;

        abilityOwner.OnDeath -= AbilityEnd;
    }

    private void GetObjectsOnCells()
    {
        objectsOnCells.Clear();
        battleSystem.FieldController.TurnOffCells();

        SetAttackableCells(abilityOwner.PositionOnField, Enums.Directions.top);
        SetAttackableCells(abilityOwner.PositionOnField, Enums.Directions.bottom);
        SetAttackableCells(abilityOwner.PositionOnField, Enums.Directions.right);
        SetAttackableCells(abilityOwner.PositionOnField, Enums.Directions.left);
    }
    private void SetAttackableCells(Vector2 pos, Enums.Directions direction)
    {
        int newI = (int)pos.x;
        int newJ = (int)pos.y;

        for (int i = 0; i < abilityData.range; i++)
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

            if (cell.transform.childCount > 1)
            {
                Character character;
                character = cell.GetComponentInChildren<Character>();

                if (character == null)
                {
                    objectsOnCells.Add(cell.transform.GetChild(0).gameObject);
                }
                else
                {
                    if (character is KostilEnemy)
                    {
                        objectsOnCells.Add(character.gameObject);
                    }
                    if (character is WallEnemyCharacter)
                    {
                        objectsOnCells.Add(character.gameObject);
                    }
                }
                
            }

        }
    }
}
