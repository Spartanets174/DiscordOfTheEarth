using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BookOfCardsController : CardController
{
    public List<CharacterCard> CharacterUserCards => m_PlayerManager.AllUserCharCards;

    public List<CardSupport> SupportUserCards => m_PlayerManager.AllUserSupportCards;

    public List<CharacterCard> CharacterDeckUserCards => m_PlayerManager.DeckUserCharCards;

    public List<CardSupport> SupportDeckUserCards => m_PlayerManager.DeckUserSupportCards;


    public bool CanEquipCharacterCard(CharacterCard card)
    {
        if(card == null) return false;
        int count = 0;
        int max=5;
        for (int j = 0; j < m_PlayerManager.DeckUserCharCards.Count; j++)
        {
            if (m_PlayerManager.DeckUserCharCards[j].Class == card.Class)
            {
                count++;
            }
        }
        if (card.Class == Enums.Classes.Маг || card.Class == Enums.Classes.Лучник)
        {
            max = 2;
        }
        if (card.Class == Enums.Classes.Кавалерия)
        {
            max = 1;
        }
        if (count < max&& m_PlayerManager.DeckUserCharCards.Count<5)
        {
            m_PlayerManager.DeckUserCharCards.Add(card);
            m_PlayerManager.AllUserCharCards.Remove(card);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanEquipSupportCard(CardSupport card)
    {
        if (card == null) return false;

        if (m_PlayerManager.DeckUserSupportCards.Count < 7)
        {
            m_PlayerManager.DeckUserSupportCards.Add(card);
            m_PlayerManager.AllUserSupportCards.Remove(card);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveCardFromDeck(Card card)
    {
        if (card == null) return;

        if (card is CharacterCard)
        {
            m_PlayerManager.DeckUserCharCards.Remove((CharacterCard)card);
            m_PlayerManager.AllUserCharCards.Add((CharacterCard)card);
        }
        if (card is CardSupport)
        {
            m_PlayerManager.DeckUserSupportCards.Remove((CardSupport)card);
            m_PlayerManager.AllUserSupportCards.Add((CardSupport)card);
        }
    }
}
