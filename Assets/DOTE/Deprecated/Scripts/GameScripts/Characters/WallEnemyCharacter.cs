using UnityEngine;

public class WallEnemyCharacter : StaticEnemyCharacter
{
    [SerializeField]
    private bool canDamage;
    void Start()
    {
        IsEnabled = true;

        SetData(Card, -1);
        OnDamaged += AttackAttackedCharacter;
    }

    private void AttackAttackedCharacter(Character character, string name, float damage)
    {
        if (canDamage)
        {
            LastAttackedCharacter.Damage(LastDamageAmount * 0.3f, CharacterName);
        }

    }

    private void OnDestroy()
    {
        OnDamaged -= AttackAttackedCharacter;
    }
}
