using System.Collections.Generic;
using UnityEngine;

public class SergeantMajorCharacterPassiveAbility : BasePassiveCharacterAbility
{
    private SergeantMajorCharacterPassiveAbilityData abilityData;
    private List<Character> charactersOnCells = new();
    public override void Init(BattleSystem battleSystem, Character owner)
    {
        base.Init(battleSystem, owner);
        this.abilityData = (SergeantMajorCharacterPassiveAbilityData)baseAbilityData;
        foreach (var character in battleSystem.EnemyController.EnemyCharObjects)
        {
            character.OnPositionOnFieldChanged += AbilityStart;
        }
        foreach (var character in battleSystem.PlayerController.PlayerCharactersObjects)
        {
            character.OnPositionOnFieldChanged += AbilityStart;
        }

        abilityOwner.OnDeath += AbilityEnd;
        AbilityStart(abilityOwner);
    }
    public override void AbilityStart(Character character)
    {
        foreach (var item in charactersOnCells)
        {
            item.PhysDefence -= abilityData.physDefenceAmount;
            item.MagDefence -= abilityData.magDefenceAmount;

        }
        GetCharactersOnCells();
        foreach (var item in charactersOnCells)
        {
            item.PhysDefence += abilityData.physDefenceAmount;
            item.MagDefence += abilityData.magDefenceAmount;
        }
    }

    public override void AbilityEnd(Character character)
    {
        foreach (var enemyCharacter in battleSystem.EnemyController.EnemyCharObjects)
        {
            enemyCharacter.OnPositionOnFieldChanged -= AbilityStart;
        }
        foreach (var item in battleSystem.PlayerController.PlayerCharactersObjects)
        {
            item.OnPositionOnFieldChanged -= AbilityStart;
        }

    }

    private void GetCharactersOnCells()
    {
        charactersOnCells.Clear();
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
                Character enemy;
                if (abilityOwner is PlayerCharacter)
                {
                    enemy = cell.GetComponentInChildren<PlayerCharacter>();
                }
                else
                {
                    enemy = cell.GetComponentInChildren<EnemyCharacter>();
                }

                if (enemy != null && enemy is not KostilEnemy)
                {
                    if (enemy.Race == abilityData.raceToBuff)
                    {
                        charactersOnCells.Add(enemy);
                    }

                }
            }

        }
    }
}
