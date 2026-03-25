using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController :CardController
{
    public List<CharacterCard> CharacterShopCards=> m_PlayerManager.AllShopCharCards;

    public List<CardSupport> SupportShopCards => m_PlayerManager.AllShopSupportCards;

    public bool CanBuyCharacterCard(CharacterCard card)
    {
        if (m_PlayerManager.Money >= card.Price)
        {
            m_PlayerManager.AllUserCharCards.Add(card);
            m_PlayerManager.AllShopCharCards.Remove(card);

            m_PlayerManager.Money -= card.Price;
            return true;
        }
        else
        {
            return false;
        }   
    }

    public bool CanBuySupportCard(CardSupport card)
    {
        if (m_PlayerManager.Money >= card.Price)
        {
            m_PlayerManager.AllUserSupportCards.Add(card);
            m_PlayerManager.AllShopSupportCards.Remove(card);
            m_PlayerManager.Money -= card.Price;
            return true;
        }
        else
        {
            return false;
        }
    }

}
