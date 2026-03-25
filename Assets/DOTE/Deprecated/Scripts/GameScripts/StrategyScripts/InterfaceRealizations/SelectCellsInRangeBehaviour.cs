using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class SelectCellsInRangeBehaviour : ICardSelectable
{
    private BattleSystem battleSystem;
    public event Action OnSelected;
    public event Action OnCancelSelection;

    public Vector2 size;
    private Character chosenCharacter;
    private int range;
    private string m_selectCardTipText;
    private string m_keyCellsColor;
    private CompositeDisposable disposables = new();

    public Cell clickedCell;
    public List<Cell> highlightedCells = new();
    public string SelectCardTipText
    {
        get
        {
            return m_selectCardTipText;
        }
    }

    public SelectCellsInRangeBehaviour(string text, BattleSystem battleSystem, Character chosenCharacter, Vector2 size, int range, string keyCellsColor)
    {
        m_selectCardTipText = text + ". Для поворота области нажмите \"R\" на клавиатуре";
        this.battleSystem = battleSystem;
        this.chosenCharacter = chosenCharacter;
        this.size = size;
        this.range = range;
        m_keyCellsColor = keyCellsColor;
    }

    public void SelectCard()
    {
        Observable.EveryUpdate().Where(x => Input.GetKeyDown(KeyCode.R)).Subscribe(x =>
        {
            float temp = size.x;
            size.x = size.y;
            size.y = temp;

            OnHoverExit(clickedCell.gameObject);
            OnHoverEnter(clickedCell.gameObject);
            OnHover(clickedCell.gameObject);
        }).AddTo(disposables);

        Vector2 pos = chosenCharacter.PositionOnField;

        battleSystem.FieldController.InvokeActionOnField((x) =>
        {
            if (x.CellIndex.x >= pos.x - range && x.CellIndex.x <= pos.x + range && x.CellIndex.y >= pos.y - range && x.CellIndex.y <= pos.y + range)
            {
                Subscribe(x);
            }
            else
            {
                x.SetCellState(false);
            }
        });
    }
    public void OnSelectedInvoke(GameObject gameObject)
    {
        clickedCell = gameObject.GetComponent<Cell>();
        CancelSelection();
        OnSelected?.Invoke();
    }

    public void Subscribe(Cell cell)
    {
        cell.SetCellState(true);
        if (battleSystem.State is PlayerTurn)
        {
            cell.OnHoverEnter += OnHoverEnter;
            cell.OnHoverExit += OnHoverExit;
            cell.OnHover += OnHover;
        }
    }

    private void OnHover(GameObject @object)
    {
        battleSystem.FieldController.InvokeActionOnField(x =>
        {
            if (highlightedCells.Contains(x))
            {
                x.SetColor($"{m_keyCellsColor}");
            }
            else
            {
                x.SetColor("normal");
            }
        });
    }

    private void OnHoverExit(GameObject gameObject)
    {
        Cell currentCell = gameObject.GetComponent<Cell>();
        highlightedCells.Clear();
        currentCell.OnClick -= OnSelectedInvoke;
    }

    private void OnHoverEnter(GameObject gameObject)
    {
        if (gameObject == null) return;

        Cell currentCell = gameObject.GetComponent<Cell>();
        clickedCell = currentCell;

        Vector2 currentCellIndex = currentCell.CellIndex;

        float xStart = currentCellIndex.x - Mathf.Floor(size.x / 2);
        float xEnd = currentCellIndex.x + Mathf.Ceil(size.x / 2) - 1;
        float yStart = currentCellIndex.y - Mathf.Floor(size.y / 2);
        float yEnd = currentCellIndex.y + Mathf.Ceil(size.y / 2) - 1;

        currentCell.OnClick += OnSelectedInvoke;
        battleSystem.FieldController.InvokeActionOnField(x =>
        {
            Vector2 cellIndex = x.CellIndex;
            if (cellIndex.x >= xStart && cellIndex.x <= xEnd && cellIndex.y >= yStart && cellIndex.y <= yEnd)
            {
                highlightedCells.Add(x);
            }
        });
    }

    public void UnSubscribe(Cell cell)
    {
        cell.OnHoverEnter -= OnHoverEnter;
        cell.OnHoverExit -= OnHoverExit;
        cell.OnHover -= OnHover;
    }

    public void CancelSelection()
    {
        battleSystem.FieldController.InvokeActionOnField(x =>
        {
            x.SetCellState(true);
            x.SetColor("normal");
            UnSubscribe(x);
        });
        ClearDisposables();
        OnCancelSelection?.Invoke();
    }

    private void ClearDisposables()
    {
        disposables.Dispose();
        disposables.Clear();
        disposables = new();
    }
}