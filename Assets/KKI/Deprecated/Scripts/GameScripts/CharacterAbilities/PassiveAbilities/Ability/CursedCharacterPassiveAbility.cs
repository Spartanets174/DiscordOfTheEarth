using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedCharacterPassiveAbility : BasePassiveCharacterAbility
{
    private CursedCharacterPassiveAbilityData abilityData;
    private List<CharacterChecker> charactersOnCells = new();
    public override void Init(BattleSystem battleSystem, Character owner)
    {
        base.Init(battleSystem, owner);
        this.abilityData = (CursedCharacterPassiveAbilityData)baseAbilityData;
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
        foreach (var item in charactersOnCells)
        {
            if (item.isPhysDefence)
            {
                item.character.PhysDefence += abilityData.physDefenceAmount;
            }
            if (item.isMagDefence)
            {
                item.character.MagDefence += abilityData.magDefenceAmount;
            }
            

        }
        GetCharactersOnCells();
        foreach (var item in charactersOnCells)
        {
            if (item.character.PhysDefence == 1)
            {
                item.isPhysDefence = false;
            }
            else
            {
                item.isPhysDefence = true;
            }
            if (item.character.MagDefence == 1)
            {
                item.isMagDefence = false;
            }
            else
            {
                item.isMagDefence = true;
            }
            item.character.PhysDefence -= abilityData.physDefenceAmount;
            item.character.MagDefence -= abilityData.magDefenceAmount;
        }
    }

    public override void AbilityEnd(Character character)
    {
        foreach (var enemyCharacter in battleSystem.EnemyController.EnemyCharObjects)
        {
            enemyCharacter.OnPositionOnFieldChanged -= AbilityStart;
        }
        abilityOwner.OnPositionOnFieldChanged -= AbilityStart;

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
                    enemy = cell.GetComponentInChildren<Enemy>();
                }
                else
                {
                    enemy = cell.GetComponentInChildren<PlayerCharacter>();
                }


                KostilEnemy kostilEnemy = cell.GetComponentInChildren<KostilEnemy>();

                if (enemy != null && enemy is not KostilEnemy)
                {
                    charactersOnCells.Add(new(enemy,false,false));
                }
                if (kostilEnemy != null)
                {
                    charactersOnCells.Add(new(kostilEnemy.WallEnemyCharacter, false, false));
                }
            }

        }
    }

    private class CharacterChecker
    {
        public Character character;
        public bool isPhysDefence;
        public bool isMagDefence;

        public CharacterChecker(Character character, bool isPhysDefence, bool isMagDefence)
        {
            this.character = character;
            this.isPhysDefence = isPhysDefence;
            this.isMagDefence = isMagDefence;
        }
    }
}
