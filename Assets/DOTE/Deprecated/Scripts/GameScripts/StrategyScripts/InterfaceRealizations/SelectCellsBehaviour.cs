using System;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class SelectCellsBehaviour : ICardSelectable
{
    private BattleSystem battleSystem;
    public event Action OnSelected;
    public event Action OnCancelSelection;

    public Vector2 range;
    private string m_selectCardTipText;
    private string m_keyCellsColor;
    private CompositeDisposable disposables=new();

    public Cell clickedCell;
    public List<Cell> highlightedCells = new();
    public string SelectCardTipText
    {
        get
        {
            return m_selectCardTipText;
        }
    }

    public SelectCellsBehaviour(string text, BattleSystem battleSystem, Vector2 range, string keyCellsColor)
    {
        m_selectCardTipText = text + ". Для поворота области нажмите \"R\" на клавиатуре";
        this.battleSystem = battleSystem;
        this.range = range;
        m_keyCellsColor = keyCellsColor;
    }

    public void SelectCard()
    {
        Observable.EveryUpdate().Where(x => Input.GetKeyDown(KeyCode.R)).Subscribe(x =>
        {
            float temp = range.x;
            range.x = range.y;
            range.y = temp;
 
            OnHoverExit(clickedCell.gameObject);
            OnHoverEnter(clickedCell.gameObject);
            OnHover(clickedCell.gameObject);
        }).AddTo(disposables);
        battleSystem.FieldController.InvokeActionOnField(Subscribe);
    }

    public void OnSelectedInvoke(GameObject gameObject)
    {
        clickedCell = gameObject.GetComponent<Cell>();
        ClearDisposables();
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

        float xStart = currentCellIndex.x - Mathf.Floor(range.x / 2);
        float xEnd = currentCellIndex.x + Mathf.Ceil(range.x / 2)-1;
        float yStart = currentCellIndex.y - Mathf.Floor(range.y / 2);
        float yEnd = currentCellIndex.y + Mathf.Ceil(range.y / 2)-1;

        currentCell.OnClick += OnSelectedInvoke;
        battleSystem.FieldController.InvokeActionOnField(x =>
        {          
            Vector2 cellIndex = x.CellIndex;
            if (cellIndex.x >= xStart && cellIndex.x<= xEnd && cellIndex.y >= yStart && cellIndex.y <= yEnd)
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
        battleSystem.FieldController.InvokeActionOnField(x=> {
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
