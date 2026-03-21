using System;
using System.Collections;
using UnityEngine;
public class Begin : State
{
    public Begin(BattleSystem battleSystem) : base(battleSystem)
    {
    }

    public override IEnumerator Start()
    {
        BattleSystem.gameLogCurrentText.Value = $"Начните расстановку юнитов.";        
       
        OnStateStarted += AddUnitStatementAction;
        OnStateCompleted += RemoveUnitStatementAction;
     
        BattleSystem.FieldController.TurnOffCells();
        OnStateStartedInvoke();
        yield break;
    }

    private void AddUnitStatementAction()
    {       
        BattleSystem.FieldController.InvokeActionOnField(AddOnCellsClick);
    }

    private void RemoveUnitStatementAction(State state)
    {
        BattleSystem.FieldController.InvokeActionOnField(RemoveOnCellsClick);
    }

    private void AddOnCellsClick(Cell cell)
    {
        cell.OnClick += BattleSystem.OnMoveButton;
    }

    private void RemoveOnCellsClick(Cell cell)
    {
        cell.OnClick -= BattleSystem.OnMoveButton;
    }

    public override IEnumerator ChooseCharacter(GameObject character)
    {
        GameCharacterCardDisplay cardDisplay = character.GetComponent<GameCharacterCardDisplay>();
        OnCharacterChosenInvoke(cardDisplay);
        BattleSystem.FieldController.TurnOffCells();
        BattleSystem.FieldController.InvokeActionOnField(SetActiveCells);
        yield break;
    }

    private void SetActiveCells(Cell cell)
    {
        if (cell.transform.childCount <=1 &&( cell.CellIndex.y == BattleSystem.FieldController.CellsOfFieled.GetLength(1) - 1 || cell.CellIndex.y == BattleSystem.FieldController.CellsOfFieled.GetLength(1) - 2))
        {
            cell.SetCellMovable();
        }
    }

    public override IEnumerator Move(GameObject cell)
    {
        if (cell.transform.childCount == 1)
        {
            GameCharacterCardDisplay cardDisplay = OnCharacterMovedInvoke();           
            if (cardDisplay!=null)
            {
                cardDisplay.IsCharacterSpawned = true;
                cardDisplay.SetCharacter(BattleSystem.PlayerController.InstasiatePlayerCharacter(cardDisplay.CurrentCharacterCard, cell.transform));               
            }
            else
            {
                Debug.LogError("Нет выбранной карты");
            }
        }       
        yield break;
    }
}
