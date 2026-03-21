using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChooseCharacterForMove : ActionTask
{
    [RequiredField]
    public BBParameter<BattleSystem> battleSystem;
    [RequiredField]
    public BBParameter<EnemyController> enemyController;
    public float secondsToRun = 1f;
    public CompactStatus finishStatus = CompactStatus.Success;

    private List<Cell> possibleCells;
    private List<EnemyCharacter> zeroSpeedCharacters = new();
    protected override void OnUpdate()
    {
        if (elapsedTime >= secondsToRun)
        {
            EndAction(finishStatus == CompactStatus.Success ? true : false);
        }
    }
    protected override void OnExecute()
    {
        EnemyCharacter chosenEnemyCharacter = ChooseChar();
        if (chosenEnemyCharacter==null )
        {
            EndAction(false);
            return;
        }

        battleSystem.value.OnChooseCharacterButton(chosenEnemyCharacter.gameObject);
        possibleCells = battleSystem.value.FieldController.GetCellsForMove(chosenEnemyCharacter, chosenEnemyCharacter.Speed);
        Cell cellToMove = GetCellToMove();
        if (cellToMove != null)
        {
            battleSystem.value.OnMoveButton(cellToMove.gameObject);
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

        bool isCharValid = false;

        while (!isCharValid)
        {
            enemyCharacter = enemyController.value.EnemyCharObjects[Random.Range(0, enemyController.value.EnemyCharObjects.Count)];
            if (zeroSpeedCharacters.Count == enemyController.value.EnemyCharObjects.Count)
            {
                break;
            }
            if (enemyCharacter.Speed>0)
            {
                isCharValid = true;
            }
            else
            {
                if (!zeroSpeedCharacters.Contains(enemyCharacter))
                {
                    zeroSpeedCharacters.Add(enemyCharacter);
                }
            }
        }

        return enemyCharacter;
    }
    private Cell GetCellToMove()
    {
        Cell currentCell = null;
        if (possibleCells.Count==0)
        {
            return null;
        }
        currentCell = possibleCells[Random.Range(0, possibleCells.Count)];
        return currentCell;
    }
}
