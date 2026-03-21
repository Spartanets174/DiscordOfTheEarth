using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldController : MonoBehaviour, ILoadable
{
    //Скрипт для создания игрового поля
    [SerializeField] private int width, height;
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private List<Cell> swampCellPrefabs;
    [SerializeField] private GameObject mountainPrefab;
    public Cell[,] CellsOfFieled;
    public void Init()
    {
        GenerateField();
    }

    private void GenerateField()
    {
        CellsOfFieled = new Cell[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Cell spawnedTile;
                if ((i == 2 && j == 2) || (i == 4 && j == 3) || (i == 2 && j == 7) || (i == 4 && j == 8))
                {
                    spawnedTile = Instantiate(swampCellPrefabs[UnityEngine.Random.Range(0, swampCellPrefabs.Count)], transform);
                    spawnedTile.transform.localPosition = new Vector3(j * -0.27f, spawnedTile.transform.localPosition.y, i * -0.27f);
                }
                else
                {
                    spawnedTile = Instantiate(cellPrefab, Vector3.zero, Quaternion.identity, transform);
                    spawnedTile.transform.localPosition = new Vector3(j * -0.27f, 0, i * -0.27f);
                }
                if ((j == 4 || j == 6) && i % 2 != 0)
                {
                    GameObject Mountain = Instantiate(mountainPrefab, Vector3.zero, Quaternion.identity, spawnedTile.transform);
                    Mountain.transform.localPosition = new Vector3(0, 0.5f, 0);
                }

                spawnedTile.SetCellMovable();                
                spawnedTile.SetCellState(true);
                spawnedTile.IsEnabled = true;
                spawnedTile.CellIndex = new Vector2(i, j);

                spawnedTile.name += $"[{spawnedTile.CellIndex.x},{spawnedTile.CellIndex.y}]";

                CellsOfFieled[i, j] = spawnedTile;
            }
        }
    }

    public void InvokeActionOnField(Action<Cell> action)
    {
        for (int i = 0; i < CellsOfFieled.GetLength(0); i++)
        {
            for (int j = 0; j < CellsOfFieled.GetLength(1); j++)
            {
                action?.Invoke(CellsOfFieled[i, j]);
            }
        }
    }

    public void TurnOffCells()
    {
        SetCellsState(false);
    }

    public void TurnOnCells()
    {
        SetCellsState(true);
    }

    private void SetCellsState(bool state)
    {
        foreach (var cell in CellsOfFieled)
        {
            cell.SetCellState(state);
            cell.SetColor("normal");
        }
    }

    public int GetMoveCost(Cell startCell, Cell endCell, State gameState, Character character)
    {
        int moveCost = 0;

        int x0 = (int)startCell.CellIndex.x;
        int y0 = (int)startCell.CellIndex.y;
        int x1 = (int)endCell.CellIndex.x;
        int y1 = (int)endCell.CellIndex.y;

        bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
        if (steep)
        {
            int t;
            t = x0; // swap x0 and y0
            x0 = y0;
            y0 = t;
            t = x1; // swap x1 and y1
            x1 = y1;
            y1 = t;
        }
        if (x0 > x1)
        {
            int t;
            t = x0; // swap x0 and x1
            x0 = x1;
            x1 = t;
            t = y0; // swap y0 and y1
            y0 = y1;
            y1 = t;
        }
        int dx = x1 - x0;
        int dy = Math.Abs(y1 - y0);
        int error = dx / 2;
        int ystep = (y0 < y1) ? 1 : -1;
        int y = y0;
        for (int x = x0; x <= x1; x++)
        {
            Cell cell = GetCell((steep ? y : x), (steep ? x : y));
            if (cell == startCell) continue;
            error = error - dy;
            if (error < 0)
            {
                y += ystep;
                error += dx;
            }
            if (character.IgnoreMoveCostTroughtSwamp && cell.IsSwamp)
            {
                continue;
            }
            if (character.IgnoreMoveCost && !cell.IsSwamp)
            {
                continue;
            }
            moveCost += gameState is PlayerTurn ? cell.TransitionCostPlayer : cell.TransitionCostEnemy;
        }
        return moveCost;
    }

    public Cell GetCell(int i, int j)
    {
        return CellsOfFieled[i, j];
    }


    private Cell GetCellForMove(int i, int j, Vector2 pos)
    {
        float newI = pos.x + i;
        float newJ = pos.y + j;
        if (newI >= 7 || newI < 0)
        {
            return null;
        }
        if (newJ >= 11 || newJ < 0)
        {
            return null;
        }
        if (GetCell((int)newI, (int)newJ).transform.childCount > 1)
        {
            return null;
        }

        return GetCell((int)newI, (int)newJ);
    }

    public List<Cell> GetCellsForMove(Character character, int speed)
    {
        List<Cell> cellsToMove = new();
        bool top = true;
        bool bottom = true;
        bool left = true;
        bool rigth = true;
        int topCount = speed;
        int bottomCount = speed;
        int leftCount = speed;
        int rigthCount = speed;
        for (int i = 1; i <= speed; i++)
        {
            Cell topCell = GetCellForMove(-i, 0, character.PositionOnField);
            Cell bottomCell = GetCellForMove(0, -i, character.PositionOnField);
            Cell leftCell = GetCellForMove(i, 0, character.PositionOnField);
            Cell rigtCell = GetCellForMove(0, i, character.PositionOnField);

            if (BattleSystem.Instance.State is PlayerTurn)
            {
                topCount -= topCell == null ? 0 : topCell.TransitionCostPlayer;
                bottomCount -= bottomCell == null ? 0 : bottomCell.TransitionCostPlayer;
                leftCount -= leftCell == null ? 0 : leftCell.TransitionCostPlayer;
                rigthCount -= rigtCell == null ? 0 : rigtCell.TransitionCostPlayer;

            }
            else
            {
                topCount -= topCell == null ? 0 : topCell.TransitionCostEnemy;
                bottomCount -= bottomCell == null ? 0 : bottomCell.TransitionCostEnemy;
                leftCount -= leftCell == null ? 0 : leftCell.TransitionCostEnemy;
                rigthCount -= rigtCell == null ? 0 : rigtCell.TransitionCostEnemy;
            }


            top = topCell != null && top && topCount >= 0;
            bottom = bottomCell != null && bottom && bottomCount >= 0;
            rigth = rigtCell != null && rigth && leftCount >= 0;
            left = leftCell != null && left && rigthCount >= 0;

            SetActiveCell(topCell, top, cellsToMove);
            SetActiveCell(bottomCell, bottom, cellsToMove);
            SetActiveCell(rigtCell, rigth, cellsToMove);
            SetActiveCell(leftCell, left, cellsToMove);
        }
        return cellsToMove;
    }

    private void SetActiveCell(Cell cell, bool isAllowed, List<Cell> cellsToMove)
    {
        if (cell != null && isAllowed)
        {
            if (BattleSystem.Instance.State is PlayerTurn)
            {
                cell.SetCellMovable();
            }
            cellsToMove.Add(cell);
        }
    }
}
