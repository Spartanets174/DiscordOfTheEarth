using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField]
    private GameObject healthBarPrefab;
    private bool m_isCurrentEnemyCharacter;
    public bool IsCurrentEnemyCharacter
    {
        get => m_isCurrentEnemyCharacter;
        set => m_isCurrentEnemyCharacter = value;
    }
    public override void SetData(CharacterCard card,  int currentIndex)
    {
        base.SetData(card, currentIndex);
        healthBar.SetHealth(card.health, 1);
        healthBar.SetMaxHealth(card.health);
        m_card = card;
        m_index = currentIndex;
    }
}
