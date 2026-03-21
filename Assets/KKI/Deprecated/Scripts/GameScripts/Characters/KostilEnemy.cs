using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KostilEnemy : StaticEnemyCharacter
{
    public WallEnemyCharacter WallEnemyCharacter;
    private void Start()
    {
        IsEnabled = true;
        CanBeDamaged = true;
    }
    public override bool Damage(Character chosenCharacter)
    {
        return WallEnemyCharacter.Damage(chosenCharacter);
    }
}
