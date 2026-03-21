using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class UnskillfullStudentCharacterBuffAbility : BaseCharacterAbility
{

    private Character character;
    private UnskillfullStudentCharacterBuffAbilityData abilityData;
    private Dictionary<Character, Cell> cellToMoveForEachCharacter = new();

    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData baseCharacterAbility)
    {
        character = null;
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (UnskillfullStudentCharacterBuffAbilityData)baseCharacterAbility;
        SetCardSelectBehaviour(new SelectAllPlayerUnitsWithConditionBehaviour(abilityData.selectAbilityText, battleSystem, CanBeSelected));
        SetUseCardBehaviour(new MoveToCellBehaviour(battleSystem));

        m_cardSelectBehaviour.OnSelected += OnSelected;

        m_useCardBehaviour.OnCardUse += OnCardUse;

        m_cardSelectBehaviour.OnCancelSelection += OnCancelSelection;
    }



    private bool CanBeSelected(Character character)
    {
        if (character == abilityOwner)
        {
            return false;
        }

        int newI = (int)character.PositionOnField.x;
        int newJ = (int)character.PositionOnField.y;

        List<Cell> cells = new();

        for (int i = 0; i < abilityData.rangeToMove; i++)
        {
            switch (abilityData.directionToMove)
            {
                case Enums.Directions.top:
                    newJ--;
                    break;
                case Enums.Directions.bottom:
                    newJ++;
                    break;
                case Enums.Directions.right:
                    newI++;
                    break;
                case Enums.Directions.left:
                    newI--;
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
            if (cell.transform.childCount > 1)
            {
                break;
            }
            cells.Add(cell);


        }

        if (cells.Count > 0)
        {
            cellToMoveForEachCharacter.Add(character, cells.Last());
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnSelected()
    {
        if (battleSystem.State is PlayerTurn)
        {
            foreach (var playerCharacter in battleSystem.PlayerController.PlayerCharactersObjects)
            {
                playerCharacter.OnClick += OnClick;
            }
        }
    }



    private void OnCardUse()
    {
        if (battleSystem.State is PlayerTurn)
        {
            character = battleSystem.PlayerController.CurrentPlayerCharacter;
        }
        else
        {
            character = battleSystem.EnemyController.CurrentEnemyCharacter;
        }

        character.MaxSpeed += abilityData.speedAmount;
        character.InstantiateEffectOnCharacter(abilityData.useEffects["buff"]);

        Uncubscribe();
    }

    private void OnCancelSelection()
    {
        Uncubscribe();
    }

    private void Uncubscribe()
    {
        character = null;
        cellToMoveForEachCharacter.Clear();
        foreach (var playerCharacter in battleSystem.PlayerController.PlayerCharactersObjects)
        {
            playerCharacter.OnClick -= OnClick;
        }

    }

    private void OnClick(GameObject gameObject)
    {
        UseCard(cellToMoveForEachCharacter[gameObject.GetComponent<Character>()].gameObject);
    }
}
[Serializable]
public class UnskillfullStudentCharacterBuffAbilityData : BaseCharacterAbilityData
{
    public string selectAbilityText;

    public int speedAmount;

    public int rangeToMove;

    public Enums.Directions directionToMove;
}