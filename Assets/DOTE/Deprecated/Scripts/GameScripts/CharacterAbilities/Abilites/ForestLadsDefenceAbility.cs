using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ForestLadsDefenceAbility : BaseCharacterAbility
{
    private ForestLadsDefenceAbilityData abilityData;

    private List<Character> characters = new List<Character>();
    public override void Init(BattleSystem battleSystem, Character owner, BaseCharacterAbilityData characterAbilityData)
    {
        this.abilityOwner = owner;
        this.battleSystem = battleSystem;
        abilityData = (ForestLadsDefenceAbilityData)characterAbilityData;
        SetCardSelectBehaviour(new SelectAllPlayerUnitsBehaviour(abilityData.selectAbilityText, battleSystem));
        SetSelectCharacterBehaviour(new EmptySelectCharacterBehaviour(""));

        m_cardSelectBehaviour.OnCancelSelection += OnCancelSelection;
        m_cardSelectBehaviour.OnSelected += OnSelected;
        m_selectCharacterBehaviour.OnSelectCharacter += OnSelectCharacter;
    }
    private void OnSelected()
    {
        if (battleSystem.State is PlayerTurn)
        {
            foreach (var playerCharacter in battleSystem.PlayerController.PlayerCharactersObjects)
            {
                playerCharacter.OnClick += SelectCharacter;
            }
        }

    }
    private void OnSelectCharacter()
    {
        Character character;
        if (battleSystem.State is PlayerTurn)
        {
            character = battleSystem.PlayerController.CurrentPlayerCharacter;
            character.PhysDefence += abilityData.physDefenceAmount;
            character.MagDefence += abilityData.magDefenceAmount;
            character.InstantiateEffectOnCharacter(abilityData.useEffects["buff"]);

        }
        else
        {
            character = battleSystem.EnemyController.CurrentEnemyCharacter;
            character.PhysDefence += abilityData.physDefenceAmount;
            character.MagDefence += abilityData.magDefenceAmount;
            character.InstantiateEffectOnCharacter(abilityData.useEffects["buff"]);
        }

        characters.Add(GetNextCharacterByDirection(character.PositionOnField, Enums.Directions.top, abilityData.range));
        characters.Add(GetNextCharacterByDirection(character.PositionOnField, Enums.Directions.bottom, abilityData.range));
        characters.Add(GetNextCharacterByDirection(character.PositionOnField, Enums.Directions.right, abilityData.range));
        characters.Add(GetNextCharacterByDirection(character.PositionOnField, Enums.Directions.left, abilityData.range));

        characters = characters.Where(x => x != null).ToList();
        if (characters.Count > 0)
        {
            character.PhysDefence ++;
            character.MagDefence ++;
        }

        battleSystem.PlayerController.SetPlayerChosenState(false, x =>
        {
            x.OnClick -= SelectCharacter;
        });

        UseCard(null);
    }

    private void OnCancelSelection()
    {
        foreach (var playerCharacter in battleSystem.PlayerController.PlayerCharactersObjects)
        {
            playerCharacter.OnClick -= SelectCharacter;
        }
    }


    private Character GetNextCharacterByDirection(Vector2 pos, Enums.Directions direction, int localRange)
    {
        int newI = (int)pos.x;
        int newJ = (int)pos.y;

        for (int i = 0; i < localRange; i++)
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
            if (battleSystem.State is PlayerTurn)
            {
                enemy = cell.GetComponentInChildren<Enemy>();
            }
            else
            {
                enemy = cell.GetComponentInChildren<PlayerCharacter>();
            }


            KostilEnemy kostilEnemy = cell.GetComponentInChildren<KostilEnemy>();
            if (cell.transform.childCount > 1)
            {

                if (enemy != null && enemy is not KostilEnemy)
                {
                    return enemy;
                }
                if (kostilEnemy != null)
                {
                    return enemy;
                }
            }

        }
        return null;
    }
}
[Serializable]
public class ForestLadsDefenceAbilityData : BaseCharacterAbilityData
{
    public string selectAbilityText;

    public int range;

    public float physDefenceAmount;

    public float magDefenceAmount;
}