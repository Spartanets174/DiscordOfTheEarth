using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


public class EnemyTurn : State
{
    public EnemyTurn(BattleSystem battleSystem) : base(battleSystem)
    {

    }
    public override IEnumerator Start()
    {
        new WaitForSecondsRealtime(1f);

        BattleSystem.FieldController.TurnOnCells();
        BattleSystem.PointsOfAction.Value = 20;

        CheckEnemyTurnCountables();

        BattleSystem.EnemyController.AttackAllEnemiesStaticCharacters();

        BattleSystem.EnemyController.ResetEnemyCharacters();      
        
        yield break;
    }

    private void CheckEnemyTurnCountables()
    {
        List<ITurnCountable> enemyTurnCountables = new List<ITurnCountable>();
        foreach (var enemyTurnCountable in BattleSystem.EnemyTurnCountables)
        {
            enemyTurnCountables.Add(enemyTurnCountable.Key);
        }
        foreach (var key in enemyTurnCountables)
        {
            if (BattleSystem.EnemyTurnCountables[key] == 0)
            {
                key.ReturnToNormal();
                BattleSystem.EnemyTurnCountables.Remove(key);
                CheckEnemyTurnCountables();
                break;
            }
            else
            {
                BattleSystem.EnemyTurnCountables[key]--;
            }
        }
    }

    public override IEnumerator ChooseCharacter(GameObject character)
    {
        EnemyCharacter enemyCharacter = character.GetComponent<EnemyCharacter>();
        BattleSystem.EnemyController.SetCurrentEnemyChosenCharacter(enemyCharacter);
        yield break;
    }
    

    public override IEnumerator Move(GameObject cell)
    {
        Cell cellToMove = cell.GetComponent<Cell>();
        EnemyCharacter enemyCharacter = BattleSystem.EnemyController.CurrentEnemyCharacter;
        Cell currentCell = enemyCharacter.GetComponentInParent<Cell>();
        int moveCost = BattleSystem.FieldController.GetMoveCost(currentCell, cellToMove,BattleSystem.State, enemyCharacter);

        if (moveCost > BattleSystem.PointsOfAction.Value)
        {
            Debug.Log("Недостаточно очков действий");
            new WaitForSecondsRealtime(1f);
            yield break;
        }

        if (moveCost > enemyCharacter.Speed)
        {
            Debug.Log("Недостаточно скорости у персонажа");
            new WaitForSecondsRealtime(1f);
            yield break;
        }

        BattleSystem.PointsOfAction.Value -= moveCost;
        enemyCharacter.Move(moveCost, cellToMove.transform);

        if (BattleSystem.PointsOfAction.Value == 0)
        {
            BattleSystem.SetPlayerTurn();
        }
        yield break;
    }
    public override IEnumerator Attack(GameObject target)
    {
        /*Логика при атаке*/
        EnemyCharacter enemyCharacter = BattleSystem.EnemyController.CurrentEnemyCharacter;
        Character currentTarget = target.GetComponent<Character>();

        enemyCharacter.OnAttackInvoke();

        currentTarget.Damage(enemyCharacter);

        if (BattleSystem.PlayerController.PlayerCharactersObjects.Count == 0)
        {
            BattleSystem.SetLost();
        }
        float chance = UnityEngine.Random.Range(0f, 1f);
        if (chance > enemyCharacter.ChanceToFreeAttack)
        {
            BattleSystem.PointsOfAction.Value -= 2;
        }

        if (BattleSystem.PointsOfAction.Value == 0)
        {
            BattleSystem.SetPlayerTurn();
        }
        BattleSystem.FieldController.TurnOnCells();
        yield break;
    }

    public override IEnumerator UseSupportCard(GameObject cardSupport)
    {

        yield break;
    }

    public override IEnumerator UseAttackAbility(GameObject gameObject)
    {
        yield break;
    }
    public override IEnumerator UseDefensiveAbility(GameObject gameObject)
    {
        /*Логика при применении способности 2*/
        yield break;
    }
    public override IEnumerator UseBuffAbility(GameObject gameObject)
    {
        /*Логика при применении способности 3*/
        yield break;
    }
   
    public override IEnumerator UseItem()
    {
        /*Логика при применении предмета*/
        yield break;
    }
}