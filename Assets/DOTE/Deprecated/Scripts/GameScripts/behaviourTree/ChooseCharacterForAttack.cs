using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChooseCharacterForAttack : ActionTask
{
    [RequiredField]
    public BBParameter<BattleSystem> battleSystem;
    [RequiredField]
    public BBParameter<EnemyController> enemyController;
    public float secondsToRun = 1f;
    public CompactStatus finishStatus = CompactStatus.Success;

    private Dictionary<EnemyCharacter, List<Character>> enemyCharactersWithCharsToAttack = new();

    protected override void OnUpdate()
    {
        if (elapsedTime >= secondsToRun)
        {
            EndAction(finishStatus == CompactStatus.Success ? true : false);
        }
    }
    protected override void OnExecute()
    {
        base.OnExecute();
        enemyCharactersWithCharsToAttack.Clear();

        foreach (var enemyCharacter in enemyController.value.EnemyCharObjects)
        {
            SetEnemiesForAttack(enemyCharacter);
        }

        EnemyCharacter chosenEnemyCharacter = ChooseChar();

        if (chosenEnemyCharacter != null)
        {
            battleSystem.value.OnChooseCharacterButton(chosenEnemyCharacter.gameObject);
            Character characterToAttack = enemyCharactersWithCharsToAttack[chosenEnemyCharacter][Random.Range(0, enemyCharactersWithCharsToAttack[chosenEnemyCharacter].Count)];
            battleSystem.value.OnAttackButton(characterToAttack.gameObject);
            finishStatus = CompactStatus.Success;
        }
        else
        {
            EndAction(false);
        }
    }
    public EnemyCharacter ChooseChar()
    {
        EnemyCharacter enemyCharacter = null;

        List<EnemyCharacter> charactersWithEnemiesToAttack = new();
        foreach (var item in enemyCharactersWithCharsToAttack.Keys)
        {
            if (enemyCharactersWithCharsToAttack[item].Count > 0 && IsCharValid(item))
            {
                charactersWithEnemiesToAttack.Add(item);
            }
        }

        if (charactersWithEnemiesToAttack.Count != 0)
        {
            enemyCharacter = charactersWithEnemiesToAttack[Random.Range(0, charactersWithEnemiesToAttack.Count)];
        }

        return enemyCharacter;
    }
    private bool IsCharValid(EnemyCharacter character)
    {
        if (!character.IsAttackedOnTheMove && character.CanDamage)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private void SetEnemiesForAttack(EnemyCharacter character)
    {
        if (character.IsAttackedOnTheMove) return;

        SetAttackableCells(character.PositionOnField, Enums.Directions.top, character);
        SetAttackableCells(character.PositionOnField, Enums.Directions.bottom, character);
        SetAttackableCells(character.PositionOnField, Enums.Directions.right, character);
        SetAttackableCells(character.PositionOnField, Enums.Directions.left, character);
    }
    private void SetAttackableCells(Vector2 pos, Enums.Directions direction, EnemyCharacter character)
    {
        int newI = (int)pos.x;
        int newJ = (int)pos.y;

        for (int i = 0; i < character.Range; i++)
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

            Cell cell = battleSystem.value.FieldController.GetCell(newI, newJ);
            Character enemy = cell.GetComponentInChildren<Character>();
            KostilEnemy kostilEnemy = cell.GetComponentInChildren<KostilEnemy>();
            if (cell.transform.childCount > 1)
            {
                if (enemy != null && enemy is not KostilEnemy && enemy is not EnemyCharacter)
                {
                    enemyCharactersWithCharsToAttack.TryAdd(character, new List<Character>());
                    enemyCharactersWithCharsToAttack[character].Add(enemy);
                }
                if (kostilEnemy != null)
                {
                    enemyCharactersWithCharsToAttack.TryAdd(character, new List<Character>());
                    enemyCharactersWithCharsToAttack[character].Add(kostilEnemy.WallEnemyCharacter);
                }
                if (character.Class == Enums.Classes.Маг)
                {
                    continue;
                }
                else
                {
                    break;
                }

            }

        }
    }
}