using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectCellsWithCharactersInRangeBehaviour : ICardSelectable
{
    public Cell clickedCell;
    public List<Character> charactersOnCells = new();
    public Dictionary<Enums.Directions,Character> charactersDirectionsOnCells = new();

    private BattleSystem battleSystem;
    private int range;
    private string materialKey;
    private string m_selectCardTipText;
    public Character chosenCharacter;
    private List<Cell> cells = new();


    public event Action OnSelected;
    public event Action OnCancelSelection;

    public string SelectCardTipText
    {
        get
        {
            return m_selectCardTipText;
        }
    }

    public SelectCellsWithCharactersInRangeBehaviour(string text, BattleSystem battleSystem, Character character, int range, string materialKey)
    {
        m_selectCardTipText = text;
        this.battleSystem = battleSystem;
        this.range = range;
        this.materialKey = materialKey;
        chosenCharacter = character;        
    }

    public void SelectCard()
    {
        SetEnemiesForAttack(chosenCharacter);
        OnSelectedInvoke(null);
    }

    private void SetEnemiesForAttack(Character character)
    {
        charactersOnCells.Clear();
        battleSystem.FieldController.TurnOffCells();
        charactersDirectionsOnCells.Clear();

        SetAttackableCells(character.PositionOnField, Enums.Directions.top, character);
        SetAttackableCells(character.PositionOnField, Enums.Directions.bottom, character);
        SetAttackableCells(character.PositionOnField, Enums.Directions.right, character);
        SetAttackableCells(character.PositionOnField, Enums.Directions.left, character);
    }
    private void SetAttackableCells(Vector2 pos, Enums.Directions direction, Character character)
    {
        int newI = (int)pos.x;
        int newJ = (int)pos.y;

        for (int i = 0; i < range; i++)
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
            Character enemy;
            if (materialKey == "allowed")
            {
                if (battleSystem.State is PlayerTurn)
                {
                    enemy = cell.GetComponentInChildren<PlayerCharacter>();
                }
                else
                {
                    enemy = cell.GetComponentInChildren<Enemy>();                  
                }
            }
            else
            {
                if (battleSystem.State is PlayerTurn)
                {
                    enemy = cell.GetComponentInChildren<Enemy>();
                }
                else
                {
                    enemy = cell.GetComponentInChildren<PlayerCharacter>();
                }
            }
            

            KostilEnemy kostilEnemy = cell.GetComponentInChildren<KostilEnemy>();
            if (cell.transform.childCount > 1)
            {
                
                if (enemy != null && enemy is not KostilEnemy)
                {
                    cell.SetColor(materialKey);
                    charactersOnCells.Add(enemy);
                    if (!charactersDirectionsOnCells.ContainsKey(direction))
                    {
                        charactersDirectionsOnCells.Add(direction, enemy);
                    }
                    
                }
                if (kostilEnemy != null)
                {
                    cell.SetColor(materialKey);
                    charactersOnCells.Add(kostilEnemy.WallEnemyCharacter);
                    if (!charactersDirectionsOnCells.ContainsKey(direction))
                    {
                        charactersDirectionsOnCells.Add(direction, enemy);
                    }
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

    public void OnSelectedInvoke(GameObject gameObject)
    {
        if (gameObject!=null)
        {
            clickedCell = gameObject.GetComponent<Cell>();
        }
        
        OnSelected?.Invoke();
    }



    public void CancelSelection()
    {
        cells.Clear();
        charactersDirectionsOnCells.Clear();
        battleSystem.FieldController.InvokeActionOnField(x =>
        {
            x.SetCellState(true);
            x.SetColor("normal");
        });
        OnCancelSelection?.Invoke();
    }
}
