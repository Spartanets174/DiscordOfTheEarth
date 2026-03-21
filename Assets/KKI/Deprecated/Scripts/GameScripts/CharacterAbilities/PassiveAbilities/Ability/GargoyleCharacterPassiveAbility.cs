using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GargoyleCharacterPassiveAbility : BasePassiveCharacterAbility, ITurnCountable
{
    private GargoyleCharacterPassiveAbilityData abilityData;

    private bool isUsed;

    private int m_turnCount;
    public int TurnCount { get => m_turnCount; set => m_turnCount = value; }

    private bool m_isBuff;
    public bool IsBuff { get => m_isBuff; }

    public event Action<ITurnCountable> OnReturnToNormal;

    public override void Init(BattleSystem battleSystem, Character owner)
    {
        base.Init(battleSystem, owner);
        this.abilityData = (GargoyleCharacterPassiveAbilityData)baseAbilityData;

        m_turnCount = abilityData.turnCount;
        m_isBuff = abilityData.isBuff;

        abilityOwner.OnDamaged += OnDamaged;
        abilityOwner.OnDeath += AbilityEnd;

    }

    private void OnDamaged(Character character, string arg2, float arg3)
    {
        if (abilityOwner.Health <= abilityOwner.MaxHealth*abilityData.healthAmountToInvoke)
        {
            AbilityStart(abilityOwner);
        }
    }

    public override void AbilityStart(Character character)
    {
        abilityOwner.IsFreezed = true;
        abilityOwner.CanDamage = false;
        abilityOwner.CanUseAbilities = false;
        abilityOwner.IgnorePhysDamage = true;

        if (IsBuff)
        {
            battleSystem.PlayerTurnCountables.Add(this, TurnCount);
            battleSystem.PlayerTurnCountables[this]--;
        }
        else
        {
            battleSystem.EnemyTurnCountables.Add(this, TurnCount);
        }
        MoveCharacter(abilityOwner);
        abilityOwner.PhysDefence += abilityData.physDefenceAmount;
    }

    public override void AbilityEnd(Character character)
    {
        ReturnToNormal();
        abilityOwner.OnDamaged -= OnDamaged;
        abilityOwner.OnDeath -= AbilityEnd;
    }

    public void ReturnToNormal()
    {
        abilityOwner.IsFreezed = false;
        abilityOwner.CanDamage = true;
        abilityOwner.CanUseAbilities = true;
        abilityOwner.IgnorePhysDamage = false;

        OnReturnToNormal?.Invoke(this);
    }

    private void MoveCharacter(Character character)
    {
        int newI = (int)character.PositionOnField.x;
        int newJ = (int)character.PositionOnField.y;

        List<Cell> cells = new();

        for (int i = 0; i <abilityData.range; i++)
        {
            switch (abilityData.direction)
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

        if (cells.Count == abilityData.range)
        {
            Cell currentCell = cells.Last();
            character.Move(0, currentCell.transform);
        }
    }
}