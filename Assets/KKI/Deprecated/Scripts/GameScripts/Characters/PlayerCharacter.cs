using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character
{
    private bool m_isCurrentPlayerCharacter;
    public bool IsCurrentPlayerCharacter
    {
        get => m_isCurrentPlayerCharacter;
        set => m_isCurrentPlayerCharacter = value;
    }
    
    public override void SetData(CharacterCard card, int index)
    {
        base.SetData(card, index);
        m_index = index;
        if (healthBar!=null)
        {
            healthBar.SetHealth(card.health, 1);
            healthBar.SetMaxHealth(card.health);
        }
        
        m_card = card;
    }
}
