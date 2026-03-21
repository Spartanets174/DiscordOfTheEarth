using System.Collections.Generic;
using System.Linq;

public class StaticEnemyCharacter : Enemy
{
    private List<Cell> m_cellsToAttack = new();
    public List<Cell> CellsToAttack
    {
        get => m_cellsToAttack;
        set => m_cellsToAttack = value;
    }

    public void AttackEnemyCharacters()
    {
        List<EnemyCharacter> enemyCharactersToAttack = new();
        foreach (var cell in CellsToAttack)
        {
            EnemyCharacter currentTarget = cell.GetComponentInChildren<EnemyCharacter>();
            if (currentTarget != null)
            {
                enemyCharactersToAttack.Add(currentTarget);
            }
        }
        if (enemyCharactersToAttack.Count != 0)
        {
            EnemyCharacter target = enemyCharactersToAttack.OrderBy(x => x.Health).ToList()[0];
            AttackCharacter(target);
        }
    }

    public void AttackPlayerCharacters()
    {
        List<PlayerCharacter> playerCharacterToAttack = new();
        foreach (var cell in CellsToAttack)
        {
            PlayerCharacter currentTarget = cell.GetComponentInChildren<PlayerCharacter>();
            if (currentTarget != null)
            {
                playerCharacterToAttack.Add(currentTarget);
            }
        }
        if (playerCharacterToAttack.Count != 0)
        {
            PlayerCharacter target = playerCharacterToAttack.OrderBy(x => x.Health).ToList()[0];
            AttackCharacter(target);
        }
    }

    public void AttackEnemyCharacter(EnemyCharacter currentTarget)
    {
        foreach (var cell in CellsToAttack)
        {
            EnemyCharacter enemyCharacter = cell.GetComponentInChildren<EnemyCharacter>();
            if (enemyCharacter == currentTarget)
            {
                AttackCharacter(currentTarget);
            }
        }
    }

    public void AttackPlayerCharacter(PlayerCharacter currentTarget)
    {
        foreach (var cell in CellsToAttack)
        {
            PlayerCharacter playerCharacter = cell.GetComponentInChildren<PlayerCharacter>();
            if (playerCharacter == currentTarget)
            {
                AttackCharacter(currentTarget);
            }
        }
    }

    private void AttackCharacter(Character currentTarget)
    {
        currentTarget.Damage(this);
    }

}
