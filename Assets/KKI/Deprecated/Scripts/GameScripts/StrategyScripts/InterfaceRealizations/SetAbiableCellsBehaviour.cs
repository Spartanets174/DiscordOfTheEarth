using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAbiableCellsBehaviour : ICharacterSelectable
{
    private BattleSystem battleSystem;
    private string m_selectCharacterTipText;
    public List<Cell> cellsToMove = new();

    public event Action OnSelectCharacter;

    public string SelectCharacterTipText => m_selectCharacterTipText;

    public SetAbiableCellsBehaviour(string text, BattleSystem battleSystem)
    {
        m_selectCharacterTipText = text;
        this.battleSystem = battleSystem;
    }

    public void SelectCharacter(GameObject gameObject)
    {
        battleSystem.FieldController.TurnOffCells();
        foreach (var cell in cellsToMove)
        {
            if (battleSystem.State is PlayerTurn) cell.SetColor("allowed");
            cell.SetCellState(true);
        }

        battleSystem.PlayerController.SetPlayerChosenState(false);
        battleSystem.PlayerController.CurrentPlayerCharacter.IsChosen = true;

        OnSelectCharacter?.Invoke();
    }
}
