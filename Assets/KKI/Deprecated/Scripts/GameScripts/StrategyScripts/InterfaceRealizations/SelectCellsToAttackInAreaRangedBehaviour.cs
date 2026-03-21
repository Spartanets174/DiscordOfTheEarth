using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SelectCellsToAttackInAreaRangedBehaviour : ICardSelectable
{
    public List<Cell> cells = new();
    private BattleSystem battleSystem;
    private Vector2 area;
    private string m_selectCardTipText;
    private Character chosenCharacter;
    private Enums.Directions direction;


    public event Action OnSelected;
    public event Action OnCancelSelection;

    public string SelectCardTipText
    {
        get
        {
            return m_selectCardTipText;
        }
    }

    public SelectCellsToAttackInAreaRangedBehaviour(BattleSystem battleSystem, Character character, Vector2 area, Enums.Directions direction)
    {
        this.battleSystem = battleSystem;
        this.area = area;
        chosenCharacter = character;
        this.direction = direction;
    }

    public void SelectCard()
    {
        SetCellsForAttack(chosenCharacter);
        OnSelectedInvoke(null);
    }

    private void SetCellsForAttack(Character character)
    {
        cells.Clear();


        switch (direction)
        {
            case Enums.Directions.top:
                SetTopCells(character);
                break;
            case Enums.Directions.bottom:
                SetBottomCells(character);
                break;
            case Enums.Directions.left:
                SetLeftCells(character);
                break;
            case Enums.Directions.right:
                SetRightCells(character);
                break;
            case Enums.Directions.all:
                SetTopCells(character);
                SetBottomCells(character);
                SetLeftCells(character);
                SetRightCells(character);
                break;
        }
    }

    private void SetTopCells(Character character)
    {
        Vector2 pos = character.PositionOnField;

        int localRange = (int)Mathf.Floor(area.x / 2);
        Debug.Log(localRange);
        battleSystem.FieldController.InvokeActionOnField((x =>
        {
            if (x.CellIndex.x >= pos.x - localRange && x.CellIndex.x <= pos.x + localRange && x.CellIndex.y < pos.y && x.CellIndex.y >= pos.y - area.y)
            {
                if (cells.Contains(x)) return;
                cells.Add(x);
            }
        }));
    }

    private void SetBottomCells(Character character)
    {
        Vector2 pos = character.PositionOnField;
        int localRange = (int)Mathf.Floor(area.x / 2);
        Debug.Log(localRange);
        battleSystem.FieldController.InvokeActionOnField((x =>
        {
            if (x.CellIndex.x >= pos.x - localRange && x.CellIndex.x <= pos.x + localRange && x.CellIndex.y > pos.y  && x.CellIndex.y <= pos.y + area.y)
            {
                if (cells.Contains(x)) return;
                cells.Add(x);
            }
        }));
    }

    private void SetLeftCells(Character character)
    {
        Vector2 pos = character.PositionOnField;
        int localRange = (int)Mathf.Floor(area.y / 2);
        Debug.Log(localRange);
        battleSystem.FieldController.InvokeActionOnField((x =>
        {
            if (x.CellIndex.x >= pos.x - area.x && x.CellIndex.x < pos.x  && x.CellIndex.y >= pos.y - localRange && x.CellIndex.y <= pos.y + localRange)
            {
                if (cells.Contains(x)) return;
                cells.Add(x);
            }
        }));
    }

    private void SetRightCells(Character character)
    {
        Vector2 pos = character.PositionOnField;
        int localRange = (int)Mathf.Floor(area.y / 2);
        Debug.Log(localRange);
        battleSystem.FieldController.InvokeActionOnField((x =>
        {
            if (x.CellIndex.x > pos.x && x.CellIndex.x <= pos.x + area.x && x.CellIndex.y >= pos.y - localRange && x.CellIndex.y <= pos.y + localRange)
            {
                if (cells.Contains(x)) return;
                cells.Add(x);
            }
        }));
    }
    public void OnSelectedInvoke(GameObject gameObject)
    {
        OnSelected?.Invoke();
    }



    public void CancelSelection()
    {
        cells.Clear();
        battleSystem.FieldController.InvokeActionOnField(x =>
        {
            x.SetCellState(true);
            x.SetColor("normal");
        });
        OnCancelSelection?.Invoke();
    }
}
