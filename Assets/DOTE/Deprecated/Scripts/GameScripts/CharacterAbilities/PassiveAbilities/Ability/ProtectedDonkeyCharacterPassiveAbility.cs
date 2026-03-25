using System.Collections.Generic;
using System.Linq;

public class ProtectedDonkeyCharacterPassiveAbility : BasePassiveCharacterAbility
{
    private ProtectedDonkeyCharacterPassiveAbilityData abilityData;

    public override void Init(BattleSystem battleSystem, Character owner)
    {
        base.Init(battleSystem, owner);
        this.abilityData = (ProtectedDonkeyCharacterPassiveAbilityData)baseAbilityData;

        abilityOwner.OnAttack += OnAttack;
        abilityOwner.OnDeath += AbilityEnd;

    }

    private void OnAttack(Character character)
    {
        float chance = UnityEngine.Random.Range(0f, 1f);
        if (chance <= abilityData.chance)
        {
            AbilityStart(abilityOwner);
        }
    }

    public override void AbilityStart(Character character)
    {
        MoveCharacter(abilityOwner);
    }

    public override void AbilityEnd(Character character)
    {
        abilityOwner.OnAttack -= OnAttack;
        abilityOwner.OnDeath -= AbilityEnd;
    }

    private void MoveCharacter(Character character)
    {
        int newI = (int)character.PositionOnField.x;
        int newJ = (int)character.PositionOnField.y;

        List<Cell> cells = new();

        for (int i = 0; i < abilityData.range; i++)
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